Push-Location $PSScriptRoot

$imageName = "rtimulib"

Write-Output "# Building docker image"
docker build -f .\Dockerfile -t $imageName ..

Write-Output "# Starting docker container to compile RTIMULibWrapper"
$baseDir = Split-Path -Parent $pwd
docker run `
    --rm `
    -v "$baseDir\RTIMULibWrapper:/opt/RTIMULibWrapper" `
    -v "$baseDir\Sense\Native:/opt/bin" `
    $imageName

Pop-Location
    