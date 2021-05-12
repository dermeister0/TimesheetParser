# Requires psake to run, see README.md for more details.

# Publish:
# Install AWS SDK for .NET:
# https://aws.amazon.com/sdk-for-net/
#
# Execute this statement before first publish:
# Set-AWSCredentials -AccessKey ******************** -SecretKey **************************************** -StoreAs TimesheetParser

Framework 4.6
$InformationPreference = 'Continue'
$env:PSModulePath += [IO.Path]::PathSeparator + [IO.Path]::Combine($PSScriptRoot, 'scripts', 'modules')

. .\scripts\Saritasa.PsakeExtensions.ps1
. .\scripts\Saritasa.BuildTasks.ps1
. .\scripts\Saritasa.PsakeTasks.ps1

. .\scripts\BuildTasks.ps1
. .\scripts\PublishTasks.ps1

Properties `
{
    $Environment = $env:Environment
    $SecretConfigPath = $env:SecretConfigPath
    $ReleaseDir = $env:ReleaseDir
}

TaskSetup `
{
    if (!$Environment)
    {
        Expand-PsakeConfiguration @{ Environment = 'Development' }
    }
    Import-PsakeConfigurationFile ".\Config.$Environment.ps1"
    Import-PsakeConfigurationFile $SecretConfigPath
}
