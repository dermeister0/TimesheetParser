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
    if ($LASTEXITCODE) { exit $LASTEXITCODE };

RUN cinst visualstudio2017community visualstudio2017-workload-manageddesktop -y

ENTRYPOINT cmd /C
WORKDIR C:/Build

COPY . C:/Build

ENV Version=0.0.0 `
    NugetVersion=0.0.0 `
    Changeset=0000000

COPY ./Scripts C:/Build/Scripts
COPY ./default.ps1 C:/Build/
