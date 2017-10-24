Param(
    [string]$version
)
Push-Location $PSScriptRoot
dotnet pack Sense /p:PackageVersion=$version -o $PSScriptRoot
Pop-Location