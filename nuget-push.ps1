Param(
    [Parameter(Mandatory=$true)][string]$packagePath,
    [Parameter(Mandatory=$true)][string]$apiKey
)
dotnet nuget push $packagePath -s https://api.nuget.org/v3/index.json -k $apiKey