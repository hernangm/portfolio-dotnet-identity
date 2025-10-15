FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Portfolio.Dotnet.Identity.Server/Portfolio.Dotnet.Identity.Server.csproj", "Portfolio.Dotnet.Identity.Server/"]
COPY ["Portfolio.Dotnet.Identity.Data/Portfolio.Dotnet.Identity.Data.csproj", "Portfolio.Dotnet.Identity.Data/"]
COPY ["Portfolio.Dotnet.Identity.Email/Portfolio.Dotnet.Identity.Email.csproj", "Portfolio.Dotnet.Identity.Email/"]
COPY ["Portfolio.Dotnet.Identity.Users/Portfolio.Dotnet.Identity.Users.csproj", "Portfolio.Dotnet.Identity.Users/"]
RUN dotnet restore "./Portfolio.Dotnet.Identity.Server/Portfolio.Dotnet.Identity.Server.csproj"
COPY . .
WORKDIR "/src/Portfolio.Dotnet.Identity.Server"
RUN dotnet build "./Portfolio.Dotnet.Identity.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Portfolio.Dotnet.Identity.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Portfolio.Dotnet.Identity.Server.dll"]
