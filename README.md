# SenseHatNet

.NET Core API for Raspberry Pi Sense HAT running on Linux. Inspired by and partially ported from the [Sense HAT Python library](https://github.com/RPi-Distro/python-sense-hat).

[![image](https://img.shields.io/nuget/v/SenseHatNet.svg)](https://www.nuget.org/packages/SenseHatNet/)

Since .NET Core 2.1 we can build and run .NET applications directly on a RPi 3. Check out [this sample application](Sample/) on how to build and run a .NET application on a RPi 3 using Docker.

## Getting started

* Create a new console app (e.g. `dotnet new console -o SenseHatNetSample`).
* Install `SenseHatNet` from NuGet (e.g. navigate inside the project directory and run `dotnet add package SenseHatNet`).
* Explore namespace `Sense` (e.g. `Sense.Led.LedMatrix.ShowMessage("Hello World");`).
* Explore the [sample application](Sample/) on how to build and run the app.

## Acknowledgements

* Drawing text on the LED matrix uses [SCUMM-8px-unicode](https://fontstruct.com/fontstructions/show/1009353/scumm_8px_unicode) font.
