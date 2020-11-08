Param(
    [Parameter(Mandatory=$true)][string]$version
)
Push-Location $PSScriptRoot
.\RTIMUExt\build.ps1
dotnet pack Sense /p:PackageVersion=$version -o $PSScriptRoot
Pop-Location