$root = $PSScriptRoot
$src = Resolve-Path "$root\..\Source"
$tools = Resolve-Path "$root\..\Tools"

Task pre-build `
{
    Initialize-MSBuild

    Invoke-NugetRestore "$src\TimesheetParser.sln"

    $values = $NugetVersion.Split('.')
    $env:HVMajor = $values[0]
    $env:HVMinor = $values[1]
    $env:HVPatch = $values[2]
    $env:HVChangeset = $Changeset

    if ($env:HVChangeset.Length -gt 7)
    {
        $env:HVChangeset = $env:HVChangeset.SubString(0, 7)
    }

    Exec { &"$src\packages\Heavysoft.VersionGenerator.*\tools\HeavysoftVersion.ps1" }
}

Task build -depends build-wpf -description "* Build projects." `
{
}

Task build-wpf -depends pre-build `
{
    Invoke-ProjectBuild "$src\TimesheetParser\TimesheetParser.csproj" -Configuration $Configuration -Target 'Restore;Build'
}

Task clean `
{
    Exec { git clean -xdf -e packages/ -e nuget.exe -e Releases/ }
}
