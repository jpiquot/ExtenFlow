# ExtenFlow Enterprise Microservices 

ExtenFlow consists of two distinct projects:

- __ExtenFlow Core Framework__: An application framework for building modular, distributed applications on ASP.NET Core.
- __ExtenFlow Marketplace__: A Peer-to-peer marketplace, built on top of the ExtenFlow Core Framework.

[![Join the chat at https://gitter.im/ExtenFlow/community](https://badges.gitter.im/ExtenFlow/community.svg)](https://gitter.im/ExtenFlow/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
[![Apache 2.0 License](https://img.shields.io/badge/license-Apache--2--Clause-blue.svg)](LICENSE)
[![Documentation](https://readthedocs.org/projects/extenflow/badge/)](https://extenflow.readthedocs.io/en/latest/)


## Build Status

Stable : 

[![NuGet](https://img.shields.io/nuget/v/ExtenFlow.Messages.Abstractions.svg)](https://www.nuget.org/packages/ExtenFlow.Messages.Abstractions) 

Latest: 

[![ExtenFlow.Messages.Abstractions package in ExtenFlow feed in Azure Artifacts](https://fiveforty.feeds.visualstudio.com/809ce676-89cb-41a2-9b29-11fb2f3e8eac/_apis/public/Packaging/Feeds/0b32c8be-a69d-4b95-94d4-68ab53649474/Packages/95720f60-409f-4453-b69c-4af804ff9066/Badge)](https://fiveforty.visualstudio.com/ExtenFlow/_packaging?_a=package&feed=0b32c8be-a69d-4b95-94d4-68ab53649474&package=95720f60-409f-4453-b69c-4af804ff9066&preferRelease=false)

## Status

### 0.1.0 Alpha

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

The documentation can be accessed here: <https://readthedocs.org/projects/extenflow>