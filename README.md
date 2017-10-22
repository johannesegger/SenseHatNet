# SenseHatNet
.NET Core API for Raspberry Pi Sense HAT running on Raspbian.

## Development
You'll need three scripts for smooth development:

* [watch-upload.ps1](watch-upload.ps1)
  * Watches the build output directory for changes and uploads changed files to a connected Raspberry Pi via SSH.
  * Before running the script you have to open the script and set the parameters
* [publish-pi.ps1](publish-pi.ps1)
  * Builds the C# library
* [run-RTIMULib-dev.ps1](run-RTIMULib-dev.ps1)
  * Builds the C++ library using a docker image built with [build-RTIMULib-dev.ps1](build-RTIMULib-dev.ps1)
