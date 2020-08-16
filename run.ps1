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

Function Invoke-DockerCompose {
    $extraArgs = $args
    Exec { docker-compose @extraArgs }
}

try {
    Push-Location $PSScriptRoot
    Invoke-DockerCompose up -d
}
finally {
    Pop-Location
}