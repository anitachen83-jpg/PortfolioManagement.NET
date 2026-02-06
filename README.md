# Portfolio Management System

一個使用 .NET 8 和 Entity Framework Core 建構的完整投資組合管理系統。

## ✨ 功能特色

- ✅ **股票管理** - 完整的 CRUD 操作，支援股票與 ETF 分類
- ✅ **交易記錄** - 買入/賣出交易，自動計算手續費與交易稅
- ✅ **持倉追蹤** - 自動計算持倉數量與加權平均成本
- ✅ **股息記錄** - 記錄與統計股息收入
- ✅ **投資報表** - 投資組合摘要、已實現損益、績效分析
- ✅ **RESTful API** - 標準化 API 設計
- ✅ **Swagger 文檔** - 互動式 API 測試介面

## 🚀 一鍵啟動

### macOS / Linux
```bash
chmod +x start.sh
./start.sh
```

### Windows
```bash
start.bat
```

腳本會自動：
- 檢查 .NET SDK
- 還原 NuGet 套件
- 建立資料庫（如果不存在）
- 啟動應用程式

### 手動啟動

```bash
# 1. 還原套件
dotnet restore

# 2. 建立資料庫（首次執行）
dotnet ef migrations add InitialCreate
dotnet ef database update

# 3. 執行應用程式
dotnet run
```

## 📖 訪問應用程式

啟動後，可以透過以下網址訪問：

- **Swagger API 文檔**: https://localhost:5001/swagger
- **API 基礎 URL**: https://localhost:5001/api

## 🛠️ 技術棧

| 技術 | 版本 | 用途 |
|------|------|------|
| .NET | 8.0 | 應用程式框架 |
| ASP.NET Core | 8.0 | Web API 框架 |
| Entity Framework Core | 8.0 | ORM 資料存取 |
| SQLite | - | 預設資料庫 |
| Swagger/OpenAPI | - | API 文檔 |
| AutoMapper | 12.0 | 物件映射 |

## 📁 專案結構

```
PortfolioManagement.NET/
├── Models/                    # 資料模型（實體）
│   ├── Stock.cs              # 股票
│   ├── Transaction.cs        # 交易
│   ├── Holding.cs            # 持倉
│   └── Dividend.cs           # 股息
├── Data/                     # 資料庫
│   └── PortfolioDbContext.cs # EF Core 資料庫上下文
├── Repositories/             # 資料存取層（Repository Pattern）
│   ├── IRepository.cs        # 通用介面
│   ├── Repository.cs         # 通用實作
│   └── ...Repository.cs      # 各實體專用 Repository
├── Services/                 # 業務邏輯層
│   ├── StockService.cs       # 股票管理
│   ├── TransactionService.cs # 交易處理
│   ├── HoldingService.cs     # 持倉計算
│   ├── DividendService.cs    # 股息管理
│   └── ReportService.cs      # 報表生成
├── Controllers/              # API 控制器
│   ├── StocksController.cs
│   ├── TransactionsController.cs
│   ├── HoldingsController.cs
│   ├── DividendsController.cs
│   └── ReportsController.cs
├── Program.cs                # 應用程式入口
├── appsettings.json          # 配置檔案
└── PortfolioManagement.csproj # 專案檔
```

## 🔌 API 端點總覽

| 資源 | 端點 | 主要功能 |
|------|------|---------|
| **Stocks** | `/api/stocks` | 新增、查詢、更新、刪除股票 |
| **Transactions** | `/api/transactions` | 記錄買入/賣出交易 |
| **Holdings** | `/api/holdings` | 查詢持倉、重新計算 |
| **Dividends** | `/api/dividends` | 記錄股息、查詢統計 |
| **Reports** | `/api/reports` | 投資組合摘要、損益報表 |

## 📚 詳細文檔

| 文檔 | 說明 |
|------|------|
| [API_USAGE.md](API_USAGE.md) | 完整的 API 使用指南與範例 |
| [ARCHITECTURE.md](ARCHITECTURE.md) | 系統架構設計說明 |
| [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) | 詳細的部署與執行指南 |

## 💡 使用範例

### 1. 新增股票
```bash
curl -X POST https://localhost:5001/api/stocks \
  -H "Content-Type: application/json" \
  -d '{
    "symbol": "2330",
    "name": "台積電",
    "type": "股票"
  }'
```

### 2. 記錄買入交易
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

### 3. 查詢持倉
```bash
curl https://localhost:5001/api/holdings
```

### 4. 查看投資組合摘要
```bash
curl https://localhost:5001/api/reports/summary
```

## 🧪 使用 Swagger 測試

1. 啟動應用程式
2. 開啟瀏覽器訪問 https://localhost:5001/swagger
3. 在 Swagger UI 中測試各個 API 端點

## 🔐 資料庫設定

### SQLite（預設）
專案預設使用 SQLite，無需額外設定。資料庫檔案 `portfolio.db` 會自動建立在專案根目錄。

### 切換到 SQL Server
請參考 [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) 中的資料庫設定章節。

## 🎯 核心設計模式

- **Repository Pattern** - 資料存取層抽象化
- **Dependency Injection** - 鬆散耦合、易於測試
- **Service Layer** - 業務邏輯集中管理
- **RESTful API** - 統一的 API 設計規範

## 📊 資料流程

```
Client Request
    ↓
Controller (API 端點)
    ↓
Service (業務邏輯)
    ↓
Repository (資料存取)
    ↓
DbContext (Entity Framework)
    ↓
Database (SQLite/SQL Server)
```

## 🛠️ 開發工具建議

- **Visual Studio 2022** - 完整 IDE
- **Visual Studio Code** - 輕量編輯器
- **Rider** - JetBrains IDE
- **Postman** - API 測試工具

## ❓ 疑難排解

遇到問題？請查看 [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) 中的「常見問題排解」章節。

## 📞 支援

- 詳細文檔：查看專案內的 Markdown 文件
- .NET 官方文檔：https://docs.microsoft.com/dotnet/
- Entity Framework Core：https://docs.microsoft.com/ef/core/

## 📄 授權

MIT License
