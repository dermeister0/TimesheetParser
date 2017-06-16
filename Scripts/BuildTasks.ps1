$root = $PSScriptRoot
$src = Resolve-Path "$root\..\Source"
$workspace = Resolve-Path "$root\.."

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

Task build-wpf -depends pre-build, build-saritasa-api `
{
    Invoke-ProjectBuild "$src\TimesheetParser\TimesheetParser.csproj" -Configuration $Configuration -Target 'Restore;Build'
}

Task build-saritasa-api `
{
    $crmWorkspace = "$workspace\CRM"
    if (!(Test-Path $crmWorkspace))
    {
        Write-Information 'CRM plugin is not found.'
        return
    }

    Push-Location $crmWorkspace
    Exec { psake build -properties "@{Configuration='$Configuration'}" }
    Copy-Item "$crmWorkspace\src\FarSaritasa\SaritasaApi\bin\$Configuration\netstandard1.4\*" -Include *.dll, *.pdb "$src\Plugins\netstandard1.4"
    Pop-Location
}

Task clean `
{
    Exec { git clean -xdf -e packages/ -e nuget.exe -e Releases/ }
}
