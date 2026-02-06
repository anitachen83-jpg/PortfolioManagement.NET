# Portfolio Management System - 部署與執行指南

## 📋 系統需求

### 必要環境
- **.NET 8.0 SDK** 或更高版本
- **作業系統**: Windows / macOS / Linux
- **資料庫**: SQLite（預設）或 SQL Server

### 安裝 .NET 8 SDK

**Windows / macOS / Linux:**
前往官方網站下載：https://dotnet.microsoft.com/download/dotnet/8.0

驗證安裝：
```bash
dotnet --version
# 應顯示 8.0.x 或更高版本
```

---

## 🚀 快速啟動（本地開發）

### 步驟 1: 下載專案檔案

將整個 `PortfolioManagement.NET` 資料夾下載到你的本地電腦。

### 步驟 2: 開啟終端機並進入專案目錄

```bash
cd /path/to/PortfolioManagement.NET
```

### 步驟 3: 還原 NuGet 套件

```bash
dotnet restore
```

這會下載所有必要的套件：
- Entity Framework Core 8.0
- ASP.NET Core 8.0
- Swagger/OpenAPI
- AutoMapper

### 步驟 4: 建立資料庫

```bash
# 建立初始資料庫遷移
dotnet ef migrations add InitialCreate

# 套用遷移，建立資料庫
dotnet ef database update
```

執行完成後，專案目錄會出現 `portfolio.db` 檔案（SQLite 資料庫）。

### 步驟 5: 執行應用程式

```bash
dotnet run
```

或使用監看模式（自動重新載入）：
```bash
dotnet watch run
```

### 步驟 6: 訪問應用程式

應用程式啟動後，會顯示類似以下訊息：
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
      Now listening on: http://localhost:5000
```

**Swagger UI（API 文檔）:**  
https://localhost:5001/swagger

**API 基礎 URL:**  
https://localhost:5001/api

---

## 🔧 開發環境設定

### 使用 Visual Studio 2022

1. 開啟 `PortfolioManagement.csproj`
2. Visual Studio 會自動還原套件
3. 按 F5 執行（或點選「偵錯」→「開始偵錯」）

### 使用 Visual Studio Code

1. 安裝 C# 擴充套件
2. 開啟專案資料夾
3. 按 F5 執行，或在終端機執行 `dotnet run`

### 使用 Rider

1. 開啟專案
2. Rider 會自動還原套件
3. 點選執行按鈕

---

## 📊 資料庫設定

### 使用 SQLite（預設）

專案預設使用 SQLite，不需額外設定。資料庫檔案會自動建立在專案根目錄。

**連線字串** (`appsettings.Development.json`):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=portfolio.db"
  }
}
```

### 切換到 SQL Server

如果想使用 SQL Server：

1. **修改 `appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PortfolioDB;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

2. **修改 `Program.cs`**:
```csharp
// 將 UseSqlite 改為 UseSqlServer
builder.Services.AddDbContext<PortfolioDbContext>(options =>
    options.UseSqlServer(connectionString));
```

3. **重新建立遷移**:
```bash
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## 🧪 測試 API

### 使用 Swagger UI

1. 啟動應用程式
2. 訪問 https://localhost:5001/swagger
3. 在 Swagger UI 中測試各個 API 端點

### 使用 cURL

**新增股票:**
```bash
curl -X POST https://localhost:5001/api/stocks \
  -H "Content-Type: application/json" \
  -d '{
    "symbol": "2330",
    "name": "台積電",
    "type": "股票"
  }'
```

**查詢所有股票:**
```bash
curl https://localhost:5001/api/stocks
```

**記錄買入交易:**
```bash
curl -X POST https://localhost:5001/api/transactions/buy \
  -H "Content-Type: application/json" \
  -d '{
    "symbol": "2330",
    "date": "2024-01-15",
    "quantity": 1000,
    "price": 580,
    "fee": 827
  }'
```

### 使用 Postman

1. 匯入 Swagger 定義：https://localhost:5001/swagger/v1/swagger.json
2. 自動生成所有 API 請求
3. 測試各個端點

---

## 🛠️ 常見問題排解

### 問題 1: 找不到 `dotnet` 命令

**解決方案:**  
安裝 .NET 8 SDK：https://dotnet.microsoft.com/download

### 問題 2: Entity Framework 工具未安裝

**錯誤訊息:**  
```
The Entity Framework tools version ... is older than that of the runtime ...
```

**解決方案:**
```bash
dotnet tool install --global dotnet-ef
# 或更新
dotnet tool update --global dotnet-ef
```

### 問題 3: 資料庫遷移失敗

**解決方案:**
```bash
# 刪除現有遷移
rm -rf Migrations/

# 重新建立
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 問題 4: 埠號已被佔用

**錯誤訊息:**  
```
Address already in use
```

**解決方案:**  
修改 `Properties/launchSettings.json` 中的埠號，或終止佔用該埠的程序。

### 問題 5: SSL 憑證錯誤

**解決方案:**
```bash
# 信任開發憑證
dotnet dev-certs https --trust
```

---

## 📦 生產環境部署

### Docker 部署

建立 `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "PortfolioManagement.dll"]
```

建立映像：
```bash
docker build -t portfolio-management .
docker run -p 5000:5000 portfolio-management
```

### Azure App Service

```bash
# 登入 Azure
az login

# 建立資源
az webapp up --name portfolio-management --runtime "DOTNET:8.0"
```

### Linux Server (Ubuntu)

```bash
# 安裝 .NET Runtime
wget https://dot.net/v1/dotnet-install.sh
bash dotnet-install.sh --channel 8.0

# 發布應用程式
dotnet publish -c Release -o /var/www/portfolio

# 設定 systemd 服務
sudo nano /etc/systemd/system/portfolio.service
```

---

## 📈 效能優化建議

### 1. 使用生產環境設定

```bash
dotnet run --configuration Release
```

### 2. 啟用快取

在 `Program.cs` 加入：
```csharp
builder.Services.AddResponseCaching();
app.UseResponseCaching();
```

### 3. 使用連線池

SQL Server 預設已啟用，SQLite 可設定：
```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=portfolio.db;Cache=Shared;Pooling=True"
}
```

### 4. 啟用壓縮

```csharp
builder.Services.AddResponseCompression();
app.UseResponseCompression();
```

---

## 🔐 安全性建議

### 1. 使用環境變數存放敏感資訊

```bash
export ConnectionStrings__DefaultConnection="your-connection-string"
```

### 2. 啟用 CORS（如有前端應用）

在 `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://your-frontend.com")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

app.UseCors("AllowFrontend");
```

### 3. 加入 JWT 驗證（多使用者支援）

安裝套件：
```bash
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
```

---

## 📞 支援與資源

**專案文檔:**
- README.md - 專案概述
- API_USAGE.md - API 使用指南
- ARCHITECTURE.md - 系統架構說明

**官方資源:**
- .NET 文檔: https://docs.microsoft.com/dotnet/
- Entity Framework Core: https://docs.microsoft.com/ef/core/
- ASP.NET Core: https://docs.microsoft.com/aspnet/core/

---

## ✅ 檢查清單

執行前請確認：

- [ ] 已安裝 .NET 8 SDK
- [ ] 已執行 `dotnet restore`
- [ ] 已建立資料庫遷移
- [ ] 已套用資料庫更新
- [ ] 可以訪問 Swagger UI

全部完成後，系統就可以正常運作了！
