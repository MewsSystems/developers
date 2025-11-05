@echo off
REM Batch script wrapper to run PowerShell Redis setup script
REM Run this as Administrator

echo Running Redis setup...
echo.

powershell.exe -ExecutionPolicy Bypass -File "%~dp0setup-redis.ps1" %*

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Setup failed. Please ensure:
    echo   1. Docker Desktop is installed and running
    echo   2. You have Administrator privileges (if needed)
    pause
    exit /b %ERRORLEVEL%
)

pause
