Push-Location $PSScriptRoot

$imageName = "rtimulib"

Write-Output "# Building docker image"
docker build -f .\Dockerfile -t $imageName ..

Write-Output "# Starting docker container to compile RTIMULibWrapper"
docker run `
    --rm `
    -v "${pwd}\..\RTIMULibWrapper:/opt/RTIMULibWrapper" `
    -v "${pwd}\..\Sense\Native:/opt/bin" `
    $imageName

Pop-Location
    