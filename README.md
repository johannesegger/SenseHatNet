# SenseHatNet
.NET Core API for Raspberry Pi Sense HAT running on Linux. Inspired by and partially ported from the [Sense HAT Python library](https://github.com/RPi-Distro/python-sense-hat).

[![image](https://img.shields.io/nuget/v/SenseHatNet.svg)](https://www.nuget.org/packages/SenseHatNet/)

We can't develop .NET Core applications directly on the Raspberry Pi because there is no SDK available, but we can still *run* apps that are built for Linux ARM devices if the target device has the .NET Core runtime installed. So we have to build our app on a development machine and then deploy it to the Pi.

## Getting started
* Follow the instructions [here](https://github.com/dotnet/core/blob/master/samples/RaspberryPiInstructions.md) to setup a "Hello World"-app that runs on the Pi.
* Install `SenseHatNet` from NuGet (e.g. `dotnet add package SenseHatNet`).
* Explore namespace `Sense` (e.g. `Sense.Led.LedMatrix.ShowMessage("Hello World");`)
* Publish the app as described in the article above (`dotnet publish -r linux-arm`).
* Deploy the app to your Pi (for automatic deployment inspect the scripts in [SSHSync](SSHSync)).

## Contribute
There are several scripts that simplify development:
* [`SSHSync/generate-ssh-cert/run.ps1`](SSHSync/generate-ssh-cert/run.ps1) generates an SSH certificate and uploads the public key to the Pi via SSH. You only have to do that initially (and every time you loose the generated key).
* [`publish.ps1`](publish.ps1) builds the C# part of the app.
* [`RTIMULib-dev/build.ps1`](RTIMULib-dev/build.ps1) builds the C++ part of the app using a docker container.
* [`SSHSync/ssh-sync/run.ps1`](SSHSync/ssh-sync/run.ps1) deploys the app to the Pi.

## Acknowledgements
* Drawing text on the LED matrix uses [SCUMM-8px-unicode](https://fontstruct.com/fontstructions/show/1009353/scumm_8px_unicode) font.
