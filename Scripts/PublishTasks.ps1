Properties `
{
    $Version = $null
    $NugetVersion = $null
}

$root = $PSScriptRoot
$src = Resolve-Path "$root\..\Source"
$tools = Resolve-Path "$root\..\Tools"
$releases = "$root\..\Releases"

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
    Update-VariablesInFile "$src\TimesheetParser\TimesheetParser.nuspec" @{Version=$NugetVersion}
    Exec { &"$tools\nuget.exe" 'Pack' "$src\TimesheetParser\TimesheetParser.nuspec" }
}

Task release-app -depends pack-app `
{
    Exec { &"$src\packages\squirrel.windows.*\tools\Squirrel.exe" --releasify "TimesheetParser.$NugetVersion.nupkg" }

    Import-Module "${env:ProgramFiles(x86)}\AWS Tools\PowerShell\AWSPowerShell\AWSPowerShell.psd1"

    Set-AWSCredentials -ProfileName TimesheetParser
    
    UploadFile "$releases\RELEASES"
    UploadFile "$releases\Setup.exe"
    UploadFile "$releases\Setup.msi"
    
    Get-Item "$releases\TimesheetParser-$NugetVersion-*.nupkg" | % { UploadFile $_.FullName }
}

function UploadFile([string] $FileName)
{
    $file = Get-Item $FileName
    # FIXME: AWS SDK adds bucket name to URL (virtual host mode).
    #Write-S3Object -BucketName 'timesheet-parser' -Key $file.Name -File $file -EndpointUrl 'https://s3.saritasa.io'
}
