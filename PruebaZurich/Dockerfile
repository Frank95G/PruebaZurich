# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Instala las herramientas EF Core
RUN dotnet tool install --global dotnet-ef

# 1. Copia solo el archivo del proyecto y restaura dependencias
COPY ["PruebaZurich.csproj", "."]
RUN dotnet restore "PruebaZurich.csproj"


# 2. Copia el resto y construye
COPY . .
RUN dotnet publish "PruebaZurich.csproj" -c Release -o /app/publish

# Etapa final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5000

# Copia solo lo necesario desde la etapa de construcción
COPY --from=build /app/publish .
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT ["dotnet", "PruebaZurich.dll"]