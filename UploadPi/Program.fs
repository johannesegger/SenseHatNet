open System
open System.IO
open System.Reactive.Disposables
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Text
open FSharp.Control.Reactive
open Logary
open Logary.Message
open Renci.SshNet

let (@@) a b = Path.Combine(a, b)

module Logging =
    open Hopac
    open Logary
    open Logary.Configuration
    open Logary.Targets
    
    let createLogger() =
        withLogaryManager "UploadPi" (
          withTargets [
            LiterateConsole.create (LiterateConsole.empty) "console"
          ] >>
          withRules [
            Rule.createForTarget "console"
          ]
        )
        |> run

type WatcherEvent =
    | Changed of string
    | Created of string
    | Deleted of string
    | Renamed of string * string

let createWatcher (factory: unit -> FileSystemWatcher) =
    let triggerSubject = new BehaviorSubject<_>(())
    triggerSubject
    |> Observable.map(fun () ->
        Observable.using
            (fun () ->
                let watcher = factory()
                watcher.EnableRaisingEvents <- true
                watcher
            )
            (fun watcher ->
                Observable.Create(new Func<IObserver<WatcherEvent>, IDisposable>(fun obs ->
                    let d = new CompositeDisposable()
                    
                    watcher.Error.Subscribe (fun e ->
                        // obs.OnError (e.GetException())
                        triggerSubject.OnNext(())
                    )
                    |> d.Add

                    watcher.Changed.Subscribe (fun e -> obs.OnNext (Changed e.Name))
                    |> d.Add
                    watcher.Created.Subscribe (fun e -> obs.OnNext (Created e.Name))
                    |> d.Add
                    watcher.Deleted.Subscribe (fun e -> obs.OnNext (Deleted e.Name))
                    |> d.Add
                    watcher.Renamed.Subscribe (fun e -> obs.OnNext (Renamed (e.OldName, e.Name)))
                    |> d.Add

                    d :> IDisposable
                ))
            )
    )
    |> Observable.switch

module Observable =
    open System.Security.Cryptography

    let throttleChanges sourceDir =
        let getContentHash file =
            File.ReadAllBytes file
            |> SHA256.Create().ComputeHash
            |> Encoding.Default.GetString

        Observable.groupBy (function
            | Changed path
            | Created path
            | Renamed (path, _)
            | Deleted path -> path
        )
        >> Observable.map (
            Observable.throttle (TimeSpan.FromMilliseconds 500.)
            >> Observable.choose (function
                | Changed path
                | Created path as x ->
                    try
                        let hash = sourceDir @@ path |> getContentHash
                        Some (x, hash)
                    with | :? IOException -> None
                | Renamed (path, _)
                | Deleted path as x ->
                    Some (x, Guid.NewGuid().ToString()) // trigger always
            )
            >> Observable.distinctUntilChangedKey snd
            >> Observable.map fst
        )
        >> Observable.mergeInner

[<EntryPoint>]
let main argv =
    if argv.Length < 6
    then
        eprintfn "Usage: UploadPi.exe <source-dir> <target-address> <username> <password> <target-dir> <target-executable>"
        1
    else
        let sourceDir = argv.[0]
        let targetAddress = argv.[1]
        let username = argv.[2]
        let password = argv.[3]
        let targetDir = argv.[4]
        let targetExecutable = argv.[5]

        use logary = Logging.createLogger()
        let logger =
            logary.getLogger (PointName [| "UploadPi"; "main" |])

        use auth = new PasswordAuthenticationMethod(username, password)
        let connectionInfo = ConnectionInfo(targetAddress, username, auth)

        let clientExecute (factory: unit -> #BaseClient) =
            ConnectedObject.create
                (fun () ->
                    let client = factory()
                    let x = client.HostKeyReceived.Subscribe (fun e -> e.CanTrust <- true) // Trust every host
                    client.Connect()
                    client
                )
                (fun client ->
                    Observable.Create client.ErrorOccurred.Subscribe
                    |> Observable.map ignore
                )

        let sshExecute = clientExecute (fun () -> new SshClient(connectionInfo))
        let scpExecute = clientExecute (fun () -> new ScpClient(connectionInfo))

        let escapeLinuxFilename (text: string) =
            text
                .Replace(Path.DirectorySeparatorChar, '/')
                .Replace(" ", "\\ ")

        let upload source target =
            eventX "Uploading {source} to {target}"
            >> setField "source" source
            >> setField "target" target
            |> logger.info

            try
                fun (client: SshClient) ->
                    Path.GetDirectoryName target
                    |> sprintf "mkdir -p %s"
                    |> client.RunCommand
                |> sshExecute
                |> ignore

                fun (client: ScpClient) ->
                    client.Upload(FileInfo source, target)
                |> scpExecute
                eventX "Upload succeeded"
                |> logger.info
            with e ->
                eventX "Upload failed: {exception}"
                >> setField "exception" e
                |> logger.error

        let runCommand command =
            eventX "Executing {command} ..."
            >> setField "command" command
            |> logger.info

            try
                fun (client: SshClient) ->
                    client.RunCommand command
                |> sshExecute
                |> function
                | result when result.ExitStatus = 0 ->
                    eventX "OK {result}"
                    >> setField "result" result.Result
                    |> logger.info
                | result ->
                    eventX "FAILED ({exitStatus}) {result}"
                    >> setField "exitStatus" result.ExitStatus
                    >> setField "result" result.Error
                    |> logger.info
            with e ->
                eventX "EXCEPTION {exception}"
                >> setField "exception" e
                |> logger.error

        let watcherFactory () =
            new FileSystemWatcher(sourceDir, IncludeSubdirectories = true)
        use x =
            createWatcher watcherFactory
            // exclude directories
            |> Observable.filter(function
                | Changed path
                | Created path
                | Renamed (_, path) -> File.Exists (sourceDir @@ path)
                | Deleted path -> true
            )
            |> Observable.throttleChanges sourceDir
            |> Observable.subscribe(function
                | Changed path 
                | Created path ->
                    upload (sourceDir @@ path) ((targetDir @@ path) |> escapeLinuxFilename)
                | Renamed (oldPath, newPath) ->
                    let oldTarget = (targetDir @@ oldPath) |> escapeLinuxFilename
                    let newTarget = (targetDir @@ newPath) |> escapeLinuxFilename
                    sprintf """mv %s %s""" oldTarget newTarget
                    |> runCommand
                | Deleted path ->
                    targetDir @@ path
                    |> escapeLinuxFilename
                    |> sprintf """rm -r %s"""
                    |> runCommand
            )
        eventX "Synchronization of local folder {source} with Pi folder {target} started."
        >> setField "source" sourceDir
        >> setField "target" targetDir
        |> logger.info
        printfn "Press enter to exit ..."
        Console.ReadLine() |> ignore

        0