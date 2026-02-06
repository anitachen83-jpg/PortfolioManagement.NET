# Portfolio Management API 使用指南

## 快速開始

### 1. 安裝與執行

```bash
# 還原套件
dotnet restore

# 建立資料庫
dotnet ef migrations add InitialCreate
dotnet ef database update

# 執行應用程式
dotnet run

# API 文檔位址
https://localhost:5001/swagger
```

### 2. API 端點概覽

| 資源 | 端點 | 說明 |
|------|------|------|
| Stocks | `/api/stocks` | 股票管理 |
| Transactions | `/api/transactions` | 交易記錄 |
| Holdings | `/api/holdings` | 持倉管理 |
| Dividends | `/api/dividends` | 股息記錄 |
| Reports | `/api/reports` | 報表分析 |

---

## API 使用範例

### 📊 股票管理 (Stocks)

#### 新增股票
```bash
POST /api/stocks
Content-Type: application/json

{
  "symbol": "2330",
  "name": "台積電",
  "type": "股票",
  "market": "台股",
  "industry": "半導體"
}
```

#### 查詢所有股票
```bash
GET /api/stocks
```

#### 依代號查詢
```bash
GET /api/stocks/2330
```

#### 搜尋股票
```bash
GET /api/stocks/search?keyword=台積
```

#### 更新股票
```bash
PUT /api/stocks/2330
Content-Type: application/json

{
  "symbol": "2330",
  "name": "台積電",
  "type": "股票",
  "market": "台股",
  "industry": "半導體",
  "notes": "更新備註"
}
```

#### 刪除股票
```bash
DELETE /api/stocks/2330
```

---

### 💰 交易管理 (Transactions)

#### 記錄買入
```bash
POST /api/transactions/buy
Content-Type: application/json

{
  "symbol": "2330",
  "date": "2024-01-15T00:00:00",
  "quantity": 1000,
  "price": 580.0,
  "fee": 827.25
}
```

**計算說明：**
- 成交金額 = 數量 × 價格 = 1000 × 580 = 580,000
- 手續費 = 580,000 × 0.001425 = 826.5（無條件進位）
- 總成本 = 580,000 + 827 = 580,827

#### 記錄賣出
```bash
POST /api/transactions/sell
Content-Type: application/json

{
  "symbol": "2330",
  "date": "2024-02-15T00:00:00",
  "quantity": 500,
  "price": 620.0,
  "fee": 442.75,
  "tax": 930.0
}
```

**計算說明：**
- 成交金額 = 500 × 620 = 310,000
- 手續費 = 310,000 × 0.001425 = 441.75（無條件進位）
- 交易稅 = 310,000 × 0.003 = 930
- 實收金額 = 310,000 - 443 - 930 = 308,627

#### 查詢所有交易
```bash
GET /api/transactions
```

#### 查詢特定股票的交易
```bash
GET /api/transactions/symbol/2330
```

---

### 📦 持倉管理 (Holdings)

#### 查詢所有持倉
```bash
GET /api/holdings
```

**回應範例：**
```json
[
  {
    "holdingId": 1,
    "symbol": "2330",
    "quantity": 500,
    "averageCost": 580.0,
    "totalCost": 290,000,
    "stock": {
      "symbol": "2330",
      "name": "台積電"
    }
  }
]
```

#### 查詢特定股票持倉
```bash
GET /api/holdings/2330
```

#### 重新計算所有持倉
```bash
POST /api/holdings/recalculate
```

**用途：** 當交易記錄有變更時，重新計算持倉數量和平均成本

---

### 💵 股息管理 (Dividends)

#### 記錄股息
```bash
POST /api/dividends
Content-Type: application/json

{
  "symbol": "2330",
  "exDividendDate": "2024-06-15T00:00:00",
  "paymentDate": "2024-07-15T00:00:00",
  "dividendPerShare": 2.75,
  "quantity": 1000,
  "tax": 0,
  "dividendType": "現金股利"
}
```

**自動計算：**
- 總股息 = 每股股息 × 持有股數 = 2.75 × 1000 = 2,750
- 實收股息 = 總股息 - 稅金 = 2,750 - 0 = 2,750

#### 查詢特定股票的股息
```bash
GET /api/dividends/symbol/2330
```

#### 查詢特定年度股息
```bash
GET /api/dividends/year/2024
```

---

### 📈 報表分析 (Reports)

#### 投資組合摘要
```bash
GET /api/reports/summary
```

**回應範例：**
```json
{
  "totalHoldings": 5,
  "totalCost": 1500000,
  "holdings": [
    {
      "symbol": "2330",
      "stockName": "台積電",
      "quantity": 1000,
      "averageCost": 580.0,
      "totalCost": 580000
    }
  ]
}
```

#### 已實現損益
```bash
GET /api/reports/realized-pl
```

**回應範例：**
```json
{
  "totalBuyAmount": 2000000,
  "totalSellAmount": 1800000,
  "totalFee": 5000,
  "totalTax": 5400,
  "realizedPL": -210400,
  "roi": -10.52
}
```

#### 績效分析
```bash
GET /api/reports/performance
```

結合投資組合摘要和已實現損益的完整報表。

---

## 使用流程範例

### 完整投資流程

```bash
# 1. 新增股票
POST /api/stocks
{
  "symbol": "2330",
  "name": "台積電",
  "type": "股票"
}

# 2. 記錄買入
POST /api/transactions/buy
{
  "symbol": "2330",
  "date": "2024-01-15",
  "quantity": 1000,
  "price": 580,
  "fee": 827
}

# 3. 查看持倉
GET /api/holdings/2330

# 4. 記錄賣出
POST /api/transactions/sell
{
  "symbol": "2330",
  "date": "2024-02-15",
  "quantity": 500,
  "price": 620,
  "fee": 443,
  "tax": 930
}

# 5. 記錄股息
POST /api/dividends
{
  "symbol": "2330",
  "exDividendDate": "2024-06-15",
  "dividendPerShare": 2.75,
  "quantity": 500
}

# 6. 查看報表
GET /api/reports/summary
GET /api/reports/realized-pl
```

---

## 錯誤處理

API 使用標準 HTTP 狀態碼：

| 狀態碼 | 說明 |
|--------|------|
| 200 OK | 請求成功 |
| 201 Created | 資源建立成功 |
| 204 No Content | 刪除成功 |
| 400 Bad Request | 請求參數錯誤 |
| 404 Not Found | 資源不存在 |
| 500 Internal Server Error | 伺服器錯誤 |

**錯誤回應範例：**
```json
{
  "message": "找不到股票代號 9999"
}
```

---

## 開發工具

### Swagger UI
啟動應用程式後，訪問 `https://localhost:5001/swagger` 可以：
- 瀏覽所有 API 端點
- 測試 API 呼叫
- 查看請求/回應格式
- 下載 OpenAPI 規格

### cURL 範例
```bash
# 查詢所有股票
curl -X GET https://localhost:5001/api/stocks

# 新增股票
curl -X POST https://localhost:5001/api/stocks \
  -H "Content-Type: application/json" \
  -d '{"symbol":"2330","name":"台積電","type":"股票"}'
```

### Postman
匯入 OpenAPI 規格 (`https://localhost:5001/swagger/v1/swagger.json`) 到 Postman 自動產生所有 API 請求。

---

## 資料庫管理

### Entity Framework Core 指令

```bash
# 建立新的遷移
dotnet ef migrations add MigrationName

# 更新資料庫
dotnet ef database update

# 移除最後一個遷移
dotnet ef migrations remove

# 查看遷移清單
dotnet ef migrations list

# 產生 SQL 腳本
dotnet ef migrations script
```

### 重設資料庫
```bash
# 刪除資料庫
dotnet ef database drop

# 重新建立
dotnet ef database update
```

---

## 注意事項

1. **手續費與稅金**：
   - 台股買入手續費率：0.1425%
   - 台股賣出手續費率：0.1425%
   - 台股賣出證交稅：0.3%
   - API 允許自訂手續費和稅金

2. **持倉計算**：
   - 系統使用加權平均成本法
   - 每次交易後自動更新持倉
   - 可手動觸發重新計算

3. **日期格式**：
   - ISO 8601 格式：`2024-01-15T00:00:00`
   - 或簡化格式：`2024-01-15`

4. **小數精度**：
   - 價格：小數點後 2 位
   - 平均成本：小數點後 4 位
   - 數量：小數點後 2 位

---

## 授權

MIT License
