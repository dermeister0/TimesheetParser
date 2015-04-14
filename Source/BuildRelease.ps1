if ($PSVersionTable.PSVersion.Major -lt 3)
{
    throw "PowerShell 3 is required.`nhttp://www.microsoft.com/en-us/download/details.aspx?id=40855"
}

Import-Module "$PSScriptRoot\Scripts\BuildHelpers.psm1"

Initialize-BuildVariables
Invoke-NugetRestore "$PSScriptRoot\TimesheetParser.sln"

Invoke-SolutionBuild "$PSScriptRoot\TimesheetParser.sln" 'Release'
