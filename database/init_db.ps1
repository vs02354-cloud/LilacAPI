# Run this script to restore or re-create the LilacTechSys PostgreSQL database
param(
    [string]$HostName = "127.0.0.1",
    [string]$Port = "5432",
    [string]$Database = "lilactechsys",
    [string]$Username = "postgres",
    [string]$Password = "postgres"
)

$env:PGPASSWORD = $Password
$psqlPath = "C:\Program Files\PostgreSQL\16\bin\psql.exe"

if (!(Test-Path $psqlPath)) {
    $psqlCmd = Get-Command psql -ErrorAction SilentlyContinue
    if ($psqlCmd) {
        $psqlPath = $psqlCmd.Source
    } else {
        Write-Error "psql.exe not found. Please install PostgreSQL or add it to PATH."
        exit 1
    }
}

$sqlScript = Join-Path $PSScriptRoot "lilactechsys_database.sql"

Write-Host "Creating database '$Database' if it doesn't exist..."
& $psqlPath -h $HostName -p $Port -U $Username -d postgres -c "SELECT 1 FROM pg_database WHERE datname='$Database';" -t | ForEach-Object {
    if ($_ -match "1") {
        Write-Host "Database '$Database' already exists."
    } else {
        Write-Host "Creating database '$Database'..."
        & $psqlPath -h $HostName -p $Port -U $Username -d postgres -c "CREATE DATABASE $Database;"
    }
}

Write-Host "Executing SQL script: $sqlScript..."
& $psqlPath -h $HostName -p $Port -U $Username -d $Database -f $sqlScript

Write-Host "Database restore completed successfully!"
