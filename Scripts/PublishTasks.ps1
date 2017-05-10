Properties `
{
    $Version = $null
    $NugetVersion = $null
}

$root = $PSScriptRoot
$src = Resolve-Path "$root\..\Source"
$tools = Resolve-Path "$root\..\Tools"

Task download-nuget `
{
    Install-NugetCli $tools
}

Task pack-plugininterfaces -depends download-nuget -description "* Pack Heavysoft.TimesheetParser.PluginInterfaces.nupkg." `
{
    Exec { &"$tools\nuget.exe" 'Pack' "$src\Heavysoft.TimesheetParser.PluginInterfaces\Heavysoft.TimesheetParser.PluginInterfaces.csproj" -Prop Configuration=Release }
}

Task pack-app -depends build, download-nuget `
{
    Exec { &"$tools\nuget.exe" 'Pack' "$src\TimesheetParser\TimesheetParser.nuspec" }
}

Task release-app -depends pack-app `
{
    Exec { &"$src\packages\squirrel.windows.*\tools\Squirrel.exe" --releasify "TimesheetParser.$NugetVersion.nupkg" }
}
