# Étape 1 : Construire l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copier les fichiers nécessaires
COPY DLMS.sln ./
COPY DLMS.API/DLMS.API.csproj DLMS.API/
COPY DLMS_BUSINESS/DLMS_BUSINESS.csproj DLMS_BUSINESS/
COPY DLMS_DAL/DLMS_DAL.csproj DLMS_DAL/
COPY DLMS_MODELS/DLMS_MODELS.csproj DLMS_MODELS/

# Restaurer seulement DLMS.API et ses dépendances
WORKDIR /app/DLMS.API
RUN dotnet restore

# Copier le reste du code
WORKDIR /app
COPY . ./

# Compiler et publier l'application
WORKDIR /app/DLMS.API
RUN dotnet publish -c Release -o /out

# Étape 2 : Exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

# Définir l'environnement
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5002

ENTRYPOINT ["dotnet", "DLMS.API.dll"]