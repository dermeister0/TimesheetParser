# escape=`

FROM microsoft/windowsservercore
SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop';"]

RUN `
    # Install Chocolatey.
    $env:chocolateyUseWindowsCompression = 'false'; `
    iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex; `
    # Install Psake, .NET Developer Pack 4.6.2.
    choco install psake netfx-4.6.2-devpack -y;

RUN `
    # Install Microsoft Build Tools 2017.
    cinst visualstudio2017buildtools -y;

COPY . C:\Build
