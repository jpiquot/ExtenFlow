# ExtenFlow Enterprise Microservices 

ExtenFlow consists of two distinct projects:

- __ExtenFlow Core Framework__: An application framework for building modular, distributed applications on ASP.NET Core.
- __ExtenFlow Marketplace__: A Peer-to-peer marketplace, built on top of the ExtenFlow Core Framework.

[![Join the chat at https://gitter.im/ExtenFlow/community](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ExtenFlow/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Apache 2.0 License](https://img.shields.io/badge/license-Apache--2--Clause-blue.svg)](LICENSE)
[![Documentation](https://readthedocs.org/projects/extenflow/badge/)](https://extenflow.readthedocs.io/en/latest/)


## Build Status

Stable (master): 

[![NuGet](https://img.shields.io/nuget/v/ExtenFlow.Messages.Abstractions.svg)](https://www.nuget.org/packages/ExtenFlow.Messages.Abstractions) [![Join the chat at https://gitter.im/ExtenFlow/community](https://badges.gitter.im/ExtenFlow/community.svg)](https://gitter.im/ExtenFlow/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)


## Status

### 0.0.1 Alpha

The software can't be used. Features are under development.

Here is a more detailed [roadmap](https://github.com/jpiquot/ExtenFlow/wiki/Roadmap).

## Getting Started

- Clone the repository using the command `git clone https://github.com/jpiquot/ExtenFlow.git` and checkout the `master` branch.

### Command line

- Install the latest version of the .NET Core SDK from this page <https://www.microsoft.com/net/download/core>
- Install the latest version of the [Dapr](https://dapr.io/) distributed application runtime from this page <https://dapr.io/>
- Next, navigate to `D:\ExtenFlow\ExtenFlow\src\` or wherever your folder is on the commandline in Administrator mode.
- Call `dapr dotnet run`.
- Then open the `http://localhost:5000` URL in your browser.

### Visual Studio

- Download Visual Studio 2019 (any edition) from https://www.visualstudio.com/downloads/
- Open `ExtenFlow.sln` and wait for Visual Studio to restore all Nuget packages
- Ensure `ExtenFlow.Marketplace` is the startup project and run it


### Documentation

The documentation can be accessed here: <https://docs.extenflow.net/>