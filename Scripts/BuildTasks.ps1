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
    $env:HVChangeset = (git 'rev-parse' 'HEAD').SubString(0, 7)

    Invoke-ProjectBuild "$src\TimesheetParser.sln" -Configuration $Configuration -Target 'Restore;Build'
}

Task copy-plugins `
{
    $pluginsDir = "$src\TimesheetParser\bin\$Configuration\Plugins"
    New-Item -ItemType Directory $pluginsDir -ErrorAction SilentlyContinue
    Copy-Item "$src\Plugins\netstandard1.4\*" $pluginsDir -Force
}

Task nuget-pack `
{
    Install-NugetCli $tools
    Exec { &"$tools\nuget.exe" 'Pack' "$src\Heavysoft.TimesheetParser.PluginInterfaces\Heavysoft.TimesheetParser.PluginInterfaces.csproj" -Prop Configuration=Release }
}
