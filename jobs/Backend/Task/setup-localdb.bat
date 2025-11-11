@echo off
REM Batch script wrapper to run PowerShell setup script
REM Run this as Administrator

echo Running LocalDB setup...
echo.

powershell.exe -ExecutionPolicy Bypass -File "%~dp0setup-localdb.ps1" %*

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo Setup failed. Please run this script as Administrator.
    pause
    exit /b %ERRORLEVEL%
)

pause
