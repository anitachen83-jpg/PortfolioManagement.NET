@echo off
REM Portfolio Management System - 啟動腳本 (Windows)

echo ==========================================
echo Portfolio Management System
echo 啟動腳本 v1.0
echo ==========================================
echo.

REM 檢查 .NET SDK
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ❌ 錯誤: 找不到 .NET SDK
    echo 請先安裝 .NET 8 SDK: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

dotnet --version
echo.

REM 檢查是否需要還原套件
if not exist "obj\" (
    echo 📦 還原 NuGet 套件...
    dotnet restore
    echo.
)

REM 檢查是否需要建立資料庫
if not exist "portfolio.db" (
    echo 🗄️  建立資料庫...
    
    REM 檢查是否已安裝 EF Core 工具
    dotnet ef >nul 2>nul
    if %ERRORLEVEL% NEQ 0 (
        echo 安裝 Entity Framework Core 工具...
        dotnet tool install --global dotnet-ef
    )
    
    REM 建立遷移
    echo 建立資料庫遷移...
    dotnet ef migrations add InitialCreate
    
    REM 套用遷移
    echo 套用資料庫遷移...
    dotnet ef database update
    echo.
)

echo ==========================================
echo 🚀 啟動應用程式...
echo ==========================================
echo.
echo 📍 API 位址: https://localhost:5001/api
echo 📖 Swagger 文檔: https://localhost:5001/swagger
echo.
echo 按 Ctrl+C 停止應用程式
echo.

REM 啟動應用程式
dotnet run
