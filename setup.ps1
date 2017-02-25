# ===============================================================================
# 1. Performs a nuget restore
# 2. Runs msbuild.exe
# ===============================================================================

# Install nuget
Write-Host "Installing Nuget." -ForegroundColor Green
choco install nuget.commandline -y

param ([string] $configuration = "Release")

$ErrorActionPreference = "Stop"
$solutionFile      = "Accounts.sln"
$platform          = "Any CPU"
$msbuild           = "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe"

$root              = $PSScriptRoot;

if (!(Test-Path $msbuild))
{
	$msbuild = "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\msbuild.exe"
}

# NodeJS is needed for Gulp
Write-Host "Installing NodeJS/Gulp"-ForegroundColor Cyan
choco install nodejs -y

# Refresh the path vars for npm (Chocolatey 0.98+)
refreshenv

try
{
    pushd Accounts
    npm install
    npm install gulp -g

    # Refresh the path vars for Gulp
    refreshenv

    gulp -b ".\" --gulpfile "gulpfile.js" default
}
finally
{
    popd
}

# Nuget restore
Write-Host "Performing Nuget restore" -ForegroundColor Green
nuget restore $solutionFile

# Build the sln file
Write-Host "Building $solutionFile." -ForegroundColor Green

& $msbuild $solutionFile /p:Configuration=$configuration /p:Platform=$platform /target:Build /verbosity:minimal 
if ($LastExitCode -ne 0)
{
	throw "Building solution failed."
}
else
{
	Write-Host "  Building solution complete."-ForegroundColor Green
}

# run database install
Write-Host "Run Database install" -ForegroundColor Green

$dbup = "$root\Accounts.DbUp\bin\Debug\Accounts.DbUp.exe"

if(Test-Path $dbup)
{
	& $dbup 
}
else
{
	throw "Error, unable to find DbUp exe $dbup"
}



