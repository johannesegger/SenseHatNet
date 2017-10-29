# SenseHatNet
.NET Core API for Raspberry Pi Sense HAT running on Linux.

## Development
Unfortunately [you can't develop .NET Core applications directly on the Raspberry Pi](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md), but you can still *run* apps that are built for Linux ARM devices.  
There are several scripts that simplify development:

* [`SSHSync\generate-ssh-cert\run.ps1`](SSHSync\generate-ssh-cert\run.ps1) generates an SSH certificate and uploads the public key to the Pi via SSH. You only have to do that initially (and every time you loose the generated key).
* [`publish.ps1`](publish.ps1) builds the C# part of the app
* [`RTIMULib-dev/build.ps1`](RTIMULib-dev/build.ps1) builds the C++ part of the app using a docker container
* [`SSHSync\ssh-sync\run.ps1`](SSHSync\ssh-sync\run.ps1) deploys the app to the Pi
