FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5110

ENV ASPNETCORE_URLS=http://+:5110

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["CitizenFileManagement.API/CitizenFileManagement.API.csproj", "CitizenFileManagement.API/"]
COPY ["Core/CitizenFileManagement.Core.Application/CitizenFileManagement.Core.Application.csproj", "Core/CitizenFileManagement.Core.Application/"]
COPY ["Core/CitizenFileManagement.Core.Domain/CitizenFileManagement.Core.Domain.csproj", "Core/CitizenFileManagement.Core.Domain/"]
COPY ["Infrastructure/CitizenFileManagement.Infrastructure.External/CitizenFileManagement.Infrastructure.External.csproj", "Infrastructure/CitizenFileManagement.Infrastructure.External/"]
COPY ["Infrastructure/CitizenFileManagement.Infrastructure.Persistence/CitizenFileManagement.Infrastructure.Persistence.csproj", "Infrastructure/CitizenFileManagement.Infrastructure.Persistence/"]
RUN dotnet restore "CitizenFileManagement.API/CitizenFileManagement.API.csproj"
COPY . .
WORKDIR "/src/CitizenFileManagement.API"
RUN dotnet build "CitizenFileManagement.API.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CitizenFileManagement.API.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CitizenFileManagement.API.dll"]


