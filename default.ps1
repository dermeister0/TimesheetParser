Framework 4.6
$InformationPreference = 'Continue'
$env:PSModulePath += ";$PSScriptRoot\Scripts\Modules"

. .\scripts\Saritasa.PsakeTasks.ps1

. .\scripts\BuildTasks.ps1

Properties `
{
    $Configuration = 'Debug'
}
