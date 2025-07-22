# Fase base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl net-tools # Añade estas líneas
USER $APP_UID
WORKDIR /app
EXPOSE 80

# Fase de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia SOLO el archivo .csproj primero (desde la raíz del proyecto)
COPY ["PruebaZurich.csproj", "."]
RUN dotnet restore "PruebaZurich.csproj"

# Copia TODO el resto del código
COPY . .

# Compilación (se ejecuta desde /src)
RUN dotnet build "PruebaZurich.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Fase de publicación
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "PruebaZurich.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Fase final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruebaZurich.dll"]