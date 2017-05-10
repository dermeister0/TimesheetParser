# Requires psake to run, see README.md for more details.

# Publish:
# Install AWS SDK for .NET:
# https://aws.amazon.com/sdk-for-net/
#
# Execute this statement before first publish:
# Set-AWSCredentials -AccessKey ******************** -SecretKey **************************************** -StoreAs TimesheetParser

Framework 4.6
$InformationPreference = 'Continue'
$env:PSModulePath += ";$PSScriptRoot\Scripts\Modules"

. .\Scripts\Saritasa.PsakeExtensions.ps1
. .\scripts\Saritasa.PsakeTasks.ps1

. .\scripts\BuildTasks.ps1
. .\scripts\PublishTasks.ps1

Properties `
{
    $Configuration = 'Debug'

    $Version = $null
    $NugetVersion = $null
}

TaskSetup `
{
    if (!$Version)
    {
        # 1.2.3-beta.1
        Expand-PsakeConfiguration @{ Version = Exec { GitVersion.exe /showvariable SemVer } }
    }

    if (!$NugetVersion)
    {
        # 1.2.3
        Expand-PsakeConfiguration @{ NugetVersion = Exec { GitVersion.exe /showvariable MajorMinorPatch } }
    }
}
