FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/Infrastructure/Identity/ExtenFlow.Identity.Abstractions/ExtenFlow.Identity.Abstractions.csproj", "src/Infrastructure/Identity/ExtenFlow.Identity.Abstractions/"]
COPY ["src/Infrastructure/Identity/ExtenFlow.Identity.Actors.Abstractions/ExtenFlow.Identity.Actors.Abstractions.csproj", "src/Infrastructure/Identity/ExtenFlow.Identity.Actors.Abstractions/"]
COPY ["src/Infrastructure/Identity/ExtenFlow.Identity.Actors/ExtenFlow.Identity.Actors.csproj", "src/Infrastructure/Identity/ExtenFlow.Identity.Actors/"]
COPY ["src/Infrastructure/Messages/ExtenFlow.Domain.Abstractions/ExtenFlow.Domain.Abstractions.csproj", "src/Infrastructure/Messages/ExtenFlow.Domain.Abstractions/"]
COPY ["src/Services/ExtenFlow.Identity.StoreActors/ExtenFlow.Identity.StoreActors.csproj", "src/Services/ExtenFlow.Identity.StoreActors/"]

RUN dotnet restore "src/Services/ExtenFlow.Identity.StoreActors/ExtenFlow.Identity.StoreActors.csproj"
COPY . .
WORKDIR "/src/src/Services/ExtenFlow.Identity.StoreActors"
RUN dotnet build "ExtenFlow.Identity.StoreActors.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExtenFlow.Identity.StoreActors.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExtenFlow.Identity.StoreActors.dll"]