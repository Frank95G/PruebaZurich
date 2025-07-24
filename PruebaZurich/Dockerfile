# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl net-tools
USER $APP_UID
WORKDIR /app
EXPOSE 80

# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PruebaZurich.csproj", "."]
RUN dotnet restore "PruebaZurich.csproj"
COPY . .
RUN dotnet build "PruebaZurich.csproj" -c Release -o /app/build

# Fase de publicación
FROM build AS publish
RUN dotnet publish "PruebaZurich.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruebaZurich.dll"]
# ENTRYPOINT ["dotnet", "PruebaZurich.dll", "--urls", "http://0.0.0.0:80"]