@echo off
setlocal
echo ========================================
echo   EJECUTANDO SERVICIOS LOGISTICOS - SEGUIMIENTO 2
echo ========================================
echo.
where dotnet >nul 2>nul
if errorlevel 1 (
    echo ERROR: .NET SDK no esta instalado o no esta en el PATH.
    pause
    exit /b 1
)

if not defined ASPNETCORE_ENVIRONMENT set ASPNETCORE_ENVIRONMENT=Development

echo [1/4] Restaurando paquetes...
dotnet restore
if errorlevel 1 goto error
echo.
echo [2/4] Compilando solucion en Release...
dotnet build --configuration Release --no-restore
if errorlevel 1 goto error
echo.
echo [3/4] Aplicando migraciones de base de datos (SQL Server)...
dotnet ef database update --project src\Servicios_Logisticos.Infrastructure --startup-project src\Servicios_Logisticos.Presentation
echo (Si no hay migraciones pendientes, este paso no hace cambios)
echo.
echo [4/4] Ejecutando aplicacion...
dotnet run --configuration Release --no-build --project src\Servicios_Logisticos.Presentation
echo.
echo ========================================
echo   EJECUCION FINALIZADA
echo ========================================
pause
exit /b 0
:error
echo.
echo ========================================
echo   OCURRIO UN ERROR DURANTE LA EJECUCION
echo ========================================
pause
exit /b 1
