Param(
    [string]$packagePath,
    [string]$apiKey
)
dotnet nuget push $packagePath -s https://api.nuget.org/v3/index.json -k $apiKey