module ConnectedObject

open System.Reactive.Subjects
open FSharp.Control.Reactive

let create initializer disconnectedObservableFactory =
    let stream = new BehaviorSubject<_>(None)
    let d =
        stream
        |> Observable.choose id
        |> Observable.map disconnectedObservableFactory
        |> Observable.switch
        |> Observable.subscribe (fun () -> stream.OnNext None)

    fun action ->
        match stream.Value with
        | Some o -> o
        | None ->
            let o = initializer()
            stream.OnNext (Some o)
            o
        |> action
