Push-Location $PSScriptRoot

$imageName = "rtimulib"

Write-Output "# Building docker image"
docker build -f .\Dockerfile -t $imageName ..

$baseDir = Split-Path -Parent $pwd

Write-Output "# Starting docker container to compile RTIMULibWrapper for ARM"
docker run `
    --rm `
    -e CXX=arm-linux-gnueabihf-g++ `
    -v "$baseDir\RTIMULibWrapper:/opt/RTIMULibWrapper" `
    -v "$baseDir\Sense\Native:/opt/bin" `
    -v "$baseDir\Sense\Native\arm:/opt/bin" `
    $imageName

Write-Output "# Starting docker container to compile RTIMULibWrapper for AArch64"
docker run `
    --rm `
    -e CXX=aarch64-linux-gnu-g++ `
    -v "$baseDir\RTIMULibWrapper:/opt/RTIMULibWrapper" `
    -v "$baseDir\Sense\Native\aarch64:/opt/bin" `
    $imageName

Pop-Location
