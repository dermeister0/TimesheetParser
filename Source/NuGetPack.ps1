if ($PSVersionTable.PSVersion.Major -lt 3)
{
    throw "PowerShell 3 is required.`nhttp://www.microsoft.com/en-us/download/details.aspx?id=40855"
}

Invoke-Expression "$PSScriptRoot\BuildRelease.ps1"

&"$PSScriptRoot\Scripts\nuget.exe" 'Pack' "$PSScriptRoot\Heavysoft.TimesheetParser.PluginInterfaces\Heavysoft.TimesheetParser.PluginInterfaces.csproj" -Prop Configuration=Release
