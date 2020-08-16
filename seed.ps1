#Requires -Version 7.0
using namespace System

function exec {
    [CmdletBinding()]
    param(
        [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
        [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = "Error executing command: " + $cmd
    )

    & $cmd
    if ($lastexitcode -ne 0) {
        throw ("Exec: " + $errorMessage)
    }
}

Push-Location $PSScriptRoot/Bugtracker/src/Deploy
Try {
    Exec { dotnet run -- "Server=localhost; Port=5432; Database=dev; Username=postgres; Password=postgres123;" --drop }
}
Finally {
    Pop-Location
}
