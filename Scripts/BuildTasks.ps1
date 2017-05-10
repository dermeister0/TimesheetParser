$root = $PSScriptRoot
$src = Resolve-Path "$root\..\Source"
$tools = Resolve-Path "$root\..\Tools"

Task pre-build `
{
    Initialize-MSBuild

    Invoke-NugetRestore "$src\TimesheetParser.sln"
}

Task build -depends build-app, copy-plugins -description "* Build project and copy plugins." `
{
}

Task build-app -depends pre-build `
{
    $env:HVMajor = Exec { GitVersion.exe /showvariable Major }
    $env:HVMinor = Exec { GitVersion.exe /showvariable Minor }
    $env:HVPatch = Exec { GitVersion.exe /showvariable Patch }
    $env:HVChangeset = (Exec { GitVersion.exe /showvariable Sha }).SubString(0, 7)
    Exec { &"$src\packages\Heavysoft.VersionGenerator.*\tools\HeavysoftVersion.ps1" }

    Invoke-ProjectBuild "$src\TimesheetParser.sln" -Configuration $Configuration -Target 'Restore;Build'
}

Task copy-plugins `
{
    $pluginsDir = "$src\TimesheetParser\bin\$Configuration\Plugins"
    New-Item -ItemType Directory $pluginsDir -ErrorAction SilentlyContinue
    Copy-Item "$src\Plugins\netstandard1.4\*" -Include *.dll, *.pdb $pluginsDir -Force
}

Task clean `
{
    Exec { git clean -xdf -e packages/ -e nuget.exe }
}
