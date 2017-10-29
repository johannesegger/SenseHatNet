Param(
    [Parameter(Mandatory=$true)][string]$sourceDir,    
    [Parameter(Mandatory=$true)][string]$targetUser,
    [Parameter(Mandatory=$true)][string]$targetHost,
    [Parameter(Mandatory=$true)][string]$targetDir
)

$imageName = "ssh-sync"
$absSourceDir = Resolve-Path $sourceDir

Push-Location $PSScriptRoot

Write-Output "# Building docker image"
docker build -t $imageName .

Write-Output "# Starting docker container to sync changes"
docker run --rm -it `
    -v "${absSourceDir}:/usr/src/app" `
    -v "${pwd}\..\generate-ssh-cert\out:/usr/src/ssh" `
    --env targetUser=$targetUser `
    --env targetHost=$targetHost `
    --env targetDir=$targetDir `
    $imageName

Pop-Location
