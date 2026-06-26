@echo off
:: ─────────────────────────────────────────────────────────────────────────────
:: Hotel Reservation System — Build & Run Script (Windows)
:: Usage: build-and-run.bat
:: ─────────────────────────────────────────────────────────────────────────────

setlocal EnableDelayedExpansion

:: Move to the script's directory
cd /d "%~dp0"

:: ── Read ports from .env (basic parsing) ──────────────────────────────────────
set FRONTEND_PORT=80
set API_PORT=8080

if exist .env (
    for /f "usebackq tokens=1,* delims==" %%A in (".env") do (
        set "line=%%A"
        if "!line:~0,1!" NEQ "#" (
            if "%%A"=="FRONTEND_PORT" set FRONTEND_PORT=%%B
            if "%%A"=="API_PORT"      set API_PORT=%%B
        )
    )
)

:: ── Banner ────────────────────────────────────────────────────────────────────
echo.
echo +----------------------------------------------------------+
echo ^|        Hotel Reservation System — Docker Setup           ^|
echo +----------------------------------------------------------+
echo.

:: ── Pre-flight check ──────────────────────────────────────────────────────────
where docker >nul 2>&1
if errorlevel 1 (
    echo [ERROR] Docker is not installed or not in PATH. Please install Docker Desktop.
    exit /b 1
)

docker compose version >nul 2>&1
if errorlevel 1 (
    set COMPOSE=docker-compose
) else (
    set COMPOSE=docker compose
)

:: ── Build images ──────────────────────────────────────────────────────────────
echo [*] Building Docker images (this may take a few minutes on first run)...
%COMPOSE% build --pull
if errorlevel 1 (
    echo [ERROR] Image build failed. Check the output above.
    exit /b 1
)

:: ── Start containers ──────────────────────────────────────────────────────────
echo.
echo [*] Starting containers...
%COMPOSE% up -d
if errorlevel 1 (
    echo [ERROR] Failed to start containers. Check the output above.
    exit /b 1
)

:: ── Brief pause for services to initialise ────────────────────────────────────
echo.
echo [*] Waiting for services to initialise (30 seconds)...
timeout /t 30 /nobreak >nul

:: ── Success message ───────────────────────────────────────────────────────────
echo.
echo +----------------------------------------------------------+
echo ^|  [OK] Hotel Reservation System is up and running!        ^|
echo ^|                                                          ^|
echo ^|  Frontend  :  http://localhost:%FRONTEND_PORT%                   ^|
echo ^|  API       :  http://localhost:%API_PORT%                  ^|
echo ^|                                                          ^|
echo ^|  Admin credentials:  admin / Admin@123                   ^|
echo ^|                                                          ^|
echo ^|  To stop   :  docker compose down                        ^|
echo ^|  To rebuild:  docker compose build --no-cache            ^|
echo +----------------------------------------------------------+
echo.

endlocal
pause
