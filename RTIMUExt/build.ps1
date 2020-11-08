Push-Location $PSScriptRoot

$imageName = "rtimulib"

Write-Output "# Building docker image"
docker build -f .\Dockerfile -t $imageName ..

Write-Output "# Starting docker container to compile RTIMUExt for ARM"
docker run `
    --rm `
    -e CXX=arm-linux-gnueabihf-g++ `
    -v "$pwd\src:/opt/RTIMUExt" `
    -v "$pwd\..\Sense\Native\arm:/opt/bin" `
    $imageName

Write-Output "# Starting docker container to compile RTIMUExt for AArch64"
docker run `
    --rm `
    -e CXX=aarch64-linux-gnu-g++ `
    -v "$pwd\src:/opt/RTIMUExt" `
    -v "$pwd\..\Sense\Native\aarch64:/opt/bin" `
    $imageName

Pop-Location
