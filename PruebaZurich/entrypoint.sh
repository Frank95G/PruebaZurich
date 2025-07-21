#!/bin/bash

# Configuración de variables
DB_SERVER="db"
DB_PORT=1433
DB_USER="sa"
DB_PASSWORD="$SA_PASSWORD"  # Se pasa desde docker-compose
TIMEOUT=30

# 1. Esperar a que SQL Server esté listo
echo "Esperando a que SQL Server esté disponible..."
/wait-for-it.sh $DB_SERVER:$DB_PORT -t $TIMEOUT -- echo "SQL Server está listo"

# 2. Ejecutar script de inicialización (opcional)
echo "Ejecutando script de inicialización de BD..."
/opt/mssql-tools/bin/sqlcmd -S $DB_SERVER -U $DB_USER -P $DB_PASSWORD -d master -i /app/ZurichDB/init.sql

# 3. Ejecutar migraciones de Entity Framework Core (si aplica)
# echo "Aplicando migraciones de EF Core..."
# dotnet ef database update

# 4. Iniciar la aplicación .NET Core
echo "Iniciando aplicación .NET..."
exec dotnet PruebaZurich.Api.dll