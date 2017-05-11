Project Setup
=============

Here are steps you need to do to set up environment to be able to develop. You need following software installed:

- [Visual Studio 2017](https://www.visualstudio.com/downloads/)
- [psake](https://github.com/psake/psake)
- [Git](https://git-scm.com/)
- [GitVersion](https://gitversion.readthedocs.io/)

You can easily install most software with Chocolatey package manager. To do that run `PowerShell` as administrator and type commands:

```
PS> iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex
PS> choco install psake git gitversion.portable
```

Task Runner
-----------

Run `psake` in repository root directory to get help about main project tasks. Execute `psake -docs` or `psake -detailedDocs` to show all tasks.

Build and Deploy
================

Build solution:

```
psake build
```

Build installer:
```
psake build-installer
```

Build in Docker
===============

```
docker-compose run --rm installer
```
