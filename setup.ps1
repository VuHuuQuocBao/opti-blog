# Installs the prerequisites (.NET 10 SDK, SQL Server LocalDB) if missing, then starts the site.
# Usage (PowerShell as Administrator):  .\setup.ps1
$ErrorActionPreference = 'Stop'

$ProjectPath = Join-Path $PSScriptRoot 'src\OptiBlog'
$SiteUrl = 'http://localhost:5000'
$LocalDbInstance = 'MSSQLLocalDB'
$SqlExpressBootstrapperUrl = 'https://download.microsoft.com/download/29654887-7cde-4397-bba3-d7f087970845/SQL2022-SSEI-Expr.exe'

function Assert-Administrator {
    $principal = New-Object Security.Principal.WindowsPrincipal([Security.Principal.WindowsIdentity]::GetCurrent())
    if (-not $principal.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
        throw 'Run this script in PowerShell as Administrator (needed to install prerequisites).'
    }
}

function Update-SessionPath {
    $machine = [Environment]::GetEnvironmentVariable('Path', 'Machine')
    $user = [Environment]::GetEnvironmentVariable('Path', 'User')
    $env:Path = "$machine;$user"
}

function Test-DotNet10Sdk {
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) { return $false }
    return [bool](dotnet --list-sdks | Select-String -Pattern '^10\.')
}

function Install-DotNet10Sdk {
    if (Test-DotNet10Sdk) {
        Write-Host '.NET 10 SDK found.'
        return
    }
    if (-not (Get-Command winget -ErrorAction SilentlyContinue)) {
        throw 'winget is not available. Install the .NET 10 SDK from https://dotnet.microsoft.com/download and run again.'
    }
    Write-Host 'Installing .NET 10 SDK...'
    winget install --id Microsoft.DotNet.SDK.10 --exact --silent --accept-package-agreements --accept-source-agreements
    Update-SessionPath
    if (-not (Test-DotNet10Sdk)) { throw '.NET 10 SDK installation failed.' }
}

function Install-LocalDb {
    if (Get-Command sqllocaldb -ErrorAction SilentlyContinue) {
        Write-Host 'SQL Server LocalDB found.'
        return
    }
    $downloadFolder = Join-Path $env:TEMP 'optiblog-localdb'
    New-Item -ItemType Directory -Force -Path $downloadFolder | Out-Null
    $bootstrapper = Join-Path $downloadFolder 'SQL2022-SSEI-Expr.exe'

    Write-Host 'Downloading SQL Server LocalDB...'
    Invoke-WebRequest -Uri $SqlExpressBootstrapperUrl -OutFile $bootstrapper -UseBasicParsing
    Start-Process -FilePath $bootstrapper -ArgumentList '/ACTION=Download', "/MEDIAPATH=$downloadFolder", '/MEDIATYPE=LocalDB', '/QUIET' -Wait

    $msi = Get-ChildItem -Path $downloadFolder -Filter 'SqlLocalDB.msi' -Recurse | Select-Object -First 1
    if (-not $msi) { throw 'LocalDB download failed (SqlLocalDB.msi not found).' }

    Write-Host 'Installing SQL Server LocalDB...'
    Start-Process -FilePath 'msiexec.exe' -ArgumentList '/i', "`"$($msi.FullName)`"", '/qn', 'IACCEPTSQLLOCALDBLICENSETERMS=YES' -Wait
    Update-SessionPath
    if (-not (Get-Command sqllocaldb -ErrorAction SilentlyContinue)) { throw 'LocalDB installation failed.' }
}

function Start-LocalDbInstance {
    $instances = sqllocaldb info
    if ($instances -notcontains $LocalDbInstance) { sqllocaldb create $LocalDbInstance | Out-Null }
    sqllocaldb start $LocalDbInstance | Out-Null
    Write-Host "LocalDB instance '$LocalDbInstance' is running."
}

function Test-GraphSettings {
    if (-not (Test-Path (Join-Path $ProjectPath 'appsettings.Local.json'))) {
        Write-Warning 'src\OptiBlog\appsettings.Local.json not found: the site runs, but search needs the Optimizely Graph keys (see README).'
    }
}

function Start-Site {
    Write-Host "Starting the site at $SiteUrl (CMS: $SiteUrl/optimizely/cms). Press Ctrl+C to stop."
    $env:ASPNETCORE_ENVIRONMENT = 'Development'
    dotnet run --project $ProjectPath --urls $SiteUrl
}

Assert-Administrator
Install-DotNet10Sdk
Install-LocalDb
Start-LocalDbInstance
Test-GraphSettings
Start-Site
