<#
.SYNOPSIS
    Applique la migration AddMultiPassReading sur la BDD DLMS.

.DESCRIPTION
    Deux modes disponibles :
      - Mode SQL  : execute le script SQL directement via sqlcmd (pas besoin de dotnet ef)
      - Mode EF   : utilise dotnet ef database update (necessite dotnet-ef installe)

.PARAMETER Mode
    "sql" (defaut) ou "ef"

.PARAMETER Server
    Serveur SQL Server (defaut: localhost)

.PARAMETER Database
    Nom de la base de donnees (obligatoire en mode sql)

.PARAMETER ConnectionString
    Connection string complete (alternative a Server+Database, mode sql uniquement)

.EXAMPLE
    # Mode SQL (recommande) — avec serveur et BDD
    .\apply-migration.ps1 -Mode sql -Server "10.60.8.100" -Database "DLMS_DB"

    # Mode SQL — avec connection string
    .\apply-migration.ps1 -Mode sql -ConnectionString "Server=10.60.8.100;Database=DLMS_DB;Trusted_Connection=True;TrustServerCertificate=True"

    # Mode EF (necessite dotnet-ef)
    .\apply-migration.ps1 -Mode ef
#>

param(
    [ValidateSet("sql", "ef")]
    [string]$Mode = "sql",

    [string]$Server = "localhost",
    [string]$Database = "",
    [string]$ConnectionString = ""
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$SolutionDir = Split-Path -Parent $ScriptDir

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Migration Multi-Pass Reading" -ForegroundColor Cyan
Write-Host "  Mode: $Mode" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

if ($Mode -eq "sql") {
    # ---- Mode SQL direct ----
    $SqlFile = Join-Path $ScriptDir "migrate-multipass.sql"

    if (-not (Test-Path $SqlFile)) {
        Write-Host "ERREUR: Script SQL introuvable: $SqlFile" -ForegroundColor Red
        exit 1
    }

    # Construire les parametres sqlcmd
    if ($ConnectionString -ne "") {
        Write-Host "Connexion via connection string..." -ForegroundColor Yellow
        $sqlcmdArgs = @("-S", ($ConnectionString -replace ".*Server=([^;]+).*", '$1'),
                        "-d", ($ConnectionString -replace ".*Database=([^;]+).*", '$1'),
                        "-i", $SqlFile,
                        "-E")  # Windows Authentication
    }
    elseif ($Database -ne "") {
        Write-Host "Connexion: $Server / $Database" -ForegroundColor Yellow
        $sqlcmdArgs = @("-S", $Server, "-d", $Database, "-i", $SqlFile, "-E")
    }
    else {
        # Lire depuis appsettings.json
        $appSettings = Join-Path $SolutionDir "DLMS.API" "appsettings.json"
        if (Test-Path $appSettings) {
            $config = Get-Content $appSettings | ConvertFrom-Json
            $cs = $config.ConnectionStrings.DefaultConnection
            if (-not $cs) { $cs = $config.ConnectionStrings.DLMSConnection }
            if ($cs) {
                Write-Host "Connection string lue depuis appsettings.json" -ForegroundColor Yellow
                $Server = if ($cs -match "Server=([^;]+)") { $Matches[1] } else { "localhost" }
                $Database = if ($cs -match "Database=([^;]+)") { $Matches[1] } else { "" }
                if ($Database -eq "") {
                    Write-Host "ERREUR: Impossible d'extraire le nom de la BDD depuis appsettings.json" -ForegroundColor Red
                    Write-Host "Utilisez: .\apply-migration.ps1 -Server <SERVEUR> -Database <BDD>" -ForegroundColor Yellow
                    exit 1
                }
                $sqlcmdArgs = @("-S", $Server, "-d", $Database, "-i", $SqlFile, "-E")
            }
        }
        else {
            Write-Host "ERREUR: Aucune BDD specifiee et appsettings.json introuvable." -ForegroundColor Red
            Write-Host "Utilisez: .\apply-migration.ps1 -Server <SERVEUR> -Database <BDD>" -ForegroundColor Yellow
            exit 1
        }
    }

    # Verifier que sqlcmd est disponible
    $sqlcmd = Get-Command sqlcmd -ErrorAction SilentlyContinue
    if (-not $sqlcmd) {
        Write-Host "ERREUR: sqlcmd introuvable. Installez SQL Server Command Line Utilities." -ForegroundColor Red
        Write-Host "  ou executez le script SQL manuellement dans SSMS: $SqlFile" -ForegroundColor Yellow
        exit 1
    }

    Write-Host "Execution de $SqlFile ..." -ForegroundColor Green
    & sqlcmd @sqlcmdArgs

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Migration appliquee avec succes." -ForegroundColor Green
    }
    else {
        Write-Host "ERREUR lors de l'execution du script SQL (code: $LASTEXITCODE)" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
elseif ($Mode -eq "ef") {
    # ---- Mode EF ----
    $dotnetEf = Get-Command dotnet-ef -ErrorAction SilentlyContinue
    if (-not $dotnetEf) {
        # Tenter comme outil local
        Write-Host "Installation de dotnet-ef en outil global..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-ef 2>$null
        if ($LASTEXITCODE -ne 0) {
            dotnet tool update --global dotnet-ef 2>$null
        }
    }

    $apiProject = Join-Path $SolutionDir "DLMS.API" "DLMS.API.csproj"
    $dalProject = Join-Path $SolutionDir "DLMS_DAL" "DLMS_DAL.csproj"

    if (-not (Test-Path $apiProject)) {
        Write-Host "ERREUR: Projet API introuvable: $apiProject" -ForegroundColor Red
        exit 1
    }

    Write-Host "Application de la migration via EF Core..." -ForegroundColor Green
    Push-Location (Join-Path $SolutionDir "DLMS.API")
    try {
        dotnet ef database update `
            --project $dalProject `
            --startup-project $apiProject `
            --verbose
    }
    finally {
        Pop-Location
    }

    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "Migration appliquee avec succes via EF Core." -ForegroundColor Green
    }
    else {
        Write-Host "ERREUR lors de la migration EF (code: $LASTEXITCODE)" -ForegroundColor Red
        exit $LASTEXITCODE
    }
}
