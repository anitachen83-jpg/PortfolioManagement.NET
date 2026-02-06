# 系統架構文檔

## 概述

Portfolio Management System 是一個使用 .NET 8 和 Entity Framework Core 建構的投資組合管理系統，採用分層架構設計。

## 技術棧

- **.NET 8.0** - 最新的 .NET 框架
- **ASP.NET Core Web API** - RESTful API 框架
- **Entity Framework Core 8.0** - ORM 框架
- **SQLite / SQL Server** - 資料庫
- **Swagger / OpenAPI** - API 文檔

## 架構設計

### 分層架構

```
┌─────────────────────────────────────┐
│         Presentation Layer          │
│      (Controllers / API)            │
├─────────────────────────────────────┤
│         Business Logic Layer        │
│           (Services)                │
├─────────────────────────────────────┤
│        Data Access Layer            │
│         (Repositories)              │
├─────────────────────────────────────┤
│         Data Layer                  │
│    (EF Core / Database)             │
└─────────────────────────────────────┘
```

### 目錄結構

```
PortfolioManagement.NET/
├── Models/                 # 資料模型 (Entity 類別)
│   ├── Stock.cs           # 股票實體
│   ├── Transaction.cs     # 交易實體
│   ├── Holding.cs         # 持倉實體
│   └── Dividend.cs        # 股息實體
│
├── Data/                   # 資料庫上下文
│   └── PortfolioDbContext.cs
│
├── Repositories/           # 資料存取層
│   ├── IRepository.cs     # 通用 Repository 介面
│   ├── Repository.cs      # 通用 Repository 實作
│   ├── IStockRepository.cs
│   ├── StockRepository.cs
│   ├── ITransactionRepository.cs
│   ├── TransactionRepository.cs
│   ├── IHoldingRepository.cs
│   ├── HoldingRepository.cs
│   ├── IDividendRepository.cs
│   └── DividendRepository.cs
│
├── Services/               # 業務邏輯層
│   ├── IStockService.cs
│   ├── StockService.cs
│   ├── ITransactionService.cs
│   ├── TransactionService.cs
│   ├── IHoldingService.cs
│   ├── HoldingService.cs
│   ├── IDividendService.cs
│   ├── DividendService.cs
│   ├── IReportService.cs
│   └── ReportService.cs
│
├── Controllers/            # API 控制器
│   ├── StocksController.cs
│   ├── TransactionsController.cs
│   ├── HoldingsController.cs
│   ├── DividendsController.cs
│   └── ReportsController.cs
│
├── Program.cs             # 應用程式入口點
├── appsettings.json       # 配置檔案
└── PortfolioManagement.csproj
```

## 設計模式

### 1. Repository Pattern

**目的：** 將資料存取邏輯與業務邏輯分離

**實作：**
- `IRepository<T>` - 通用 Repository 介面
- `Repository<T>` - 通用 Repository 實作
- 特定 Repository (如 `IStockRepository`) 繼承通用介面並擴展特定方法

**優點：**
- 降低耦合度
- 易於測試（可使用 Mock）
- 集中管理資料存取邏輯

### 2. Dependency Injection

**實作位置：** `Program.cs`

```csharp
// 註冊 Repositories
builder.Services.AddScoped<IStockRepository, StockRepository>();

// 註冊 Services
builder.Services.AddScoped<IStockService, StockService>();
```

**優點：**
- 鬆散耦合
- 易於測試
- 方便替換實作

### 3. Service Layer Pattern

**目的：** 封裝業務邏輯，提供高階操作

**範例：** `TransactionService` 處理交易並自動更新持倉

```csharp
public async Task<Transaction> RecordBuyAsync(...)
{
    // 1. 記錄交易
    await _transactionRepository.AddAsync(transaction);
    
    // 2. 自動更新持倉
    await _holdingService.UpdateHoldingAsync(symbol);
    
    return transaction;
}
```

## 資料模型

### Entity Relationship Diagram

```
┌──────────────┐
│    Stock     │
│──────────────│
│ Symbol (PK)  │
│ Name         │
│ Type         │
└──────────────┘
       │
       │ 1:N
       ├──────────────┐
       │              │
       ▼              ▼
┌──────────────┐  ┌──────────────┐
│ Transaction  │  │   Holding    │
│──────────────│  │──────────────│
│ Id (PK)      │  │ Id (PK)      │
│ Symbol (FK)  │  │ Symbol (FK)  │
│ Type         │  │ Quantity     │
│ Quantity     │  │ AvgCost      │
│ Price        │  └──────────────┘
└──────────────┘
       │
       │ 1:N
       ▼
┌──────────────┐
│   Dividend   │
│──────────────│
│ Id (PK)      │
│ Symbol (FK)  │
│ Amount       │
└──────────────┘
```

### 核心實體

#### Stock（股票）
- **主鍵：** Symbol（股票代號）
- **用途：** 股票主檔資料
- **關聯：** 1 對多 Transactions, Holdings, Dividends

#### Transaction（交易）
- **主鍵：** TransactionId
- **類型：** 買入 / 賣出
- **關聯：** 多對 1 Stock

#### Holding（持倉）
- **主鍵：** HoldingId
- **計算：** 加權平均成本法
- **關聯：** 多對 1 Stock

#### Dividend（股息）
- **主鍵：** DividendId
- **用途：** 記錄股息發放
- **關聯：** 多對 1 Stock

## API 設計

### RESTful 原則

| 操作 | HTTP 方法 | 端點範例 | 說明 |
|------|----------|---------|------|
| 列表 | GET | `/api/stocks` | 取得所有資源 |
| 查詢 | GET | `/api/stocks/{id}` | 取得特定資源 |
| 建立 | POST | `/api/stocks` | 建立新資源 |
| 更新 | PUT | `/api/stocks/{id}` | 更新資源 |
| 刪除 | DELETE | `/api/stocks/{id}` | 刪除資源 |

### 回應格式

**成功回應：**
```json
{
  "symbol": "2330",
  "name": "台積電",
  "type": "股票"
}
```

**錯誤回應：**
```json
{
  "message": "找不到股票代號 9999"
}
```

## 資料庫設計

### Entity Framework Core

**DbContext：** `PortfolioDbContext`

**配置方式：**
1. Fluent API（在 `OnModelCreating` 中）
2. Data Annotations（在 Model 類別中）

**遷移管理：**
```bash
dotnet ef migrations add MigrationName
dotnet ef database update
```

### 索引策略

```csharp
entity.HasIndex(e => e.Symbol);        // 股票代號
entity.HasIndex(e => e.TransactionDate); // 交易日期
entity.HasIndex(e => e.Type);          // 交易類型
```

### 關聯設定

```csharp
entity.HasOne(e => e.Stock)
    .WithMany(s => s.Transactions)
    .HasForeignKey(e => e.Symbol)
    .OnDelete(DeleteBehavior.Cascade);
```

## 業務邏輯

### 持倉計算（加權平均成本法）

```
範例：
1. 買入 1000 股 @ 580 = 580,000
2. 買入 500 股 @ 600 = 300,000
3. 賣出 500 股

計算：
- 總成本 = 580,000 + 300,000 = 880,000
- 總股數 = 1500 股
- 平均成本 = 880,000 / 1500 = 586.67
- 賣出後剩餘 1000 股
- 剩餘成本 = 1000 × 586.67 = 586,670
```

### 交易費用計算

**買入：**
```
總成本 = (數量 × 價格) + 手續費
```

**賣出：**
```
實收金額 = (數量 × 價格) - 手續費 - 證交稅
```

**台股費率：**
- 手續費：0.1425%（券商可打折）
- 證交稅：0.3%

## 擴展性設計

### 支援多種資料庫

```csharp
if (databaseProvider == "SqlServer")
{
    options.UseSqlServer(connectionString);
}
else
{
    options.UseSqlite(connectionString);
}
```

### 易於新增功能

1. **新增實體：** 在 Models 中建立類別
2. **新增 Repository：** 實作 `IRepository<T>`
3. **新增 Service：** 實作業務邏輯
4. **新增 Controller：** 暴露 API 端點
5. **註冊服務：** 在 `Program.cs` 中註冊

## 安全性考量

### 建議實作

1. **驗證與授權**
   - 使用 JWT Token
   - 實作使用者認證

2. **輸入驗證**
   - 使用 Data Annotations
   - 自訂驗證邏輯

3. **錯誤處理**
   - 全域例外處理中介軟體
   - 不暴露敏感資訊

4. **API 限流**
   - 防止濫用
   - 保護系統資源

## 測試策略

### 單元測試

```csharp
[Fact]
public async Task RecordBuy_Should_CreateTransaction()
{
    // Arrange
    var service = new TransactionService(mockRepo, mockStock, mockHolding);
    
    // Act
    var result = await service.RecordBuyAsync("2330", date, 1000, 580, 827);
    
    // Assert
    Assert.NotNull(result);
    Assert.Equal(580827, result.TotalAmount);
}
```

### 整合測試

使用 In-Memory Database 測試完整流程。

## 效能優化

1. **使用 AsNoTracking**：查詢唯讀資料
2. **批次操作**：使用 `AddRangeAsync`
3. **適當的索引**：提升查詢效能
4. **連線池**：重用資料庫連線

## 部署

### 發佈應用程式

```bash
# 發佈為自包含應用程式
dotnet publish -c Release --self-contained -r win-x64

# 發佈為框架相依應用程式
dotnet publish -c Release
```

### Docker 支援

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "PortfolioManagement.dll"]
```

## 維護與監控

### 日誌記錄

使用內建的 `ILogger` 記錄：
- 交易操作
- 錯誤訊息
- 效能指標

### 健康檢查

實作健康檢查端點監控系統狀態。

---

## 授權

MIT License
