Param(
    [Parameter(Mandatory=$true)][string]$targetUser,
    [Parameter(Mandatory=$true)][string]$targetHost
)

$imageName = "generate-ssh-cert"

Push-Location $PSScriptRoot

Write-Output "# Building docker image"
docker build -t $imageName .

Write-Output "# Starting docker container to interactively generate SSH certificate"
docker run --rm -it `
    --env targetUser=$targetUser `
    --env targetHost=$targetHost `
    -v "${pwd}\out:/usr/src/app" `
    $imageName

Pop-Location
