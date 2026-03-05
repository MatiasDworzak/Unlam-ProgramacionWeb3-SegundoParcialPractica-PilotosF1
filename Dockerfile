# 1. SDK de .NET para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# 2. Copiar todo el contenido del repo
COPY . .

# 3. Restaurar dependencias de la solución completa
RUN dotnet restore "GestionF1.Web.sln"

# 4. Compilar y publicar específicamente el proyecto Web
WORKDIR /source/GestionF1.Web
RUN dotnet publish -c Release -o /app

# 5. Imagen de ejecución (más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .



# 7. Ejecutar el DLL de tu proyecto Web
ENTRYPOINT ["dotnet", "GestionF1.Web.dll"]