# escape=`

# 0.1.0
FROM microsoft/windowsservercore
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

RUN `
    # Install Chocolatey.
    $env:chocolateyUseWindowsCompression = 'false'; `
    iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex; `
    # Install Psake, .NET Developer Pack 4.6.2.
    choco install psake -y; `
    if ($LASTEXITCODE) { exit $LASTEXITCODE }; `
    New-Item C:\Build -ItemType Directory | Out-Null; `
    iwr 'https://download.microsoft.com/download/E/F/D/EFD52638-B804-4865-BB57-47F4B9C80269/NDP462-DevPack-KB3151934-ENU.exe' -OutFile 'C:\Build\NDP462-DevPack-KB3151934-ENU.exe'; `
    $result = Start-Process -NoNewWindow -Wait -PassThru -FilePath 'C:\Build\NDP462-DevPack-KB3151934-ENU.exe' -ArgumentList '/q', '/norestart'; `
    if ($result.ExitCode) { exit $result.ExitCode }; `
    Remove-Item C:\Build\*.exe;

RUN `
    # Install Microsoft Build Tools 2017.
    cinst visualstudio2017buildtools -y;

ENTRYPOINT cmd /C
WORKDIR C:/Build

COPY . C:/Build

ENV Version=0.0.0 `
    NugetVersion=0.0.0

COPY ./Scripts C:/Build/Scripts
COPY ./default.ps1 C:/Build/
