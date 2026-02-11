@echo off
setlocal

cd /d "%~dp0"

REM ===== CONFIG (adjust folder + csproj names if needed) =====
set INFRA_PROJ=ContinuityServer.Infrastructure\ContinuityServer.Infrastructure.csproj
set API_PROJ=ContinuityServer.Api\ContinuityServer.Api.csproj
set TF=net8.0

if "%1"=="" (
  echo Usage: add-migration MigrationName
  exit /b 1
)

set NAME=%1

echo.
echo =========================================
echo Adding migration: %NAME%
echo =========================================
echo Using Infrastructure: %INFRA_PROJ%
echo Using Startup:        %API_PROJ%
echo.

dotnet ef migrations add %NAME% ^
  --project "%INFRA_PROJ%" ^
  --startup-project "%API_PROJ%" ^
  --framework %TF%

if errorlevel 1 (
  echo.
  echo Migration FAILED
  exit /b 1
)

echo.
echo =========================================
echo Updating database
echo =========================================
echo.

dotnet ef database update ^
  --project "%INFRA_PROJ%" ^
  --startup-project "%API_PROJ%" ^
  --framework %TF%

if errorlevel 1 (
  echo.
  echo Database update FAILED
  exit /b 1
)

echo.
echo DONE.
pause
