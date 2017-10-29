Push-Location $PSScriptRoot
dotnet publish -r linux-arm SenseTest
SSHSync\ssh-sync\run.ps1 SenseTest\bin\Debug\netcoreapp2.0\linux-arm\publish pi 192.168.0.101 /home/pi/sense-test
Pop-Location
