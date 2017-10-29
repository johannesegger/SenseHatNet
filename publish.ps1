Param(
    [Parameter(Mandatory=$true)][string]$projectName
)

dotnet publish -r linux-arm $projectName
