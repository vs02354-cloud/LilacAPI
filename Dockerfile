# Multi-stage Docker build for ASP.NET Core 9 Web API
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files for layer caching
COPY ["LilacTechSys.sln", "./"]
COPY ["src/LilacTechSys.Domain/LilacTechSys.Domain.csproj", "src/LilacTechSys.Domain/"]
COPY ["src/LilacTechSys.Application/LilacTechSys.Application.csproj", "src/LilacTechSys.Application/"]
COPY ["src/LilacTechSys.Infrastructure/LilacTechSys.Infrastructure.csproj", "src/LilacTechSys.Infrastructure/"]
COPY ["src/LilacTechSys.Api/LilacTechSys.Api.csproj", "src/LilacTechSys.Api/"]
COPY ["tests/LilacTechSys.Tests/LilacTechSys.Tests.csproj", "tests/LilacTechSys.Tests/"]

# Restore dependencies
RUN dotnet restore "LilacTechSys.sln"

# Copy source and publish
COPY . .
WORKDIR "/src/src/LilacTechSys.Api"
RUN dotnet publish "LilacTechSys.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000
ENV PORT=10000
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LilacTechSys.Api.dll"]
