# 如何上傳完整源代碼到 GitHub

目前 repository 已包含基礎架構和文檔，但還缺少部分源代碼檔案。

## 方法一：使用 Git 命令列（推薦）

### 步驟：

1. **Clone repository 到本地**
```bash
git clone https://github.com/anitachen83-jpg/PortfolioManagement.NET.git
cd PortfolioManagement.NET
```

2. **複製所有源代碼檔案**
   - 將本地的所有 `.cs` 檔案複製到對應的資料夾中
   - Controllers/
   - Models/
   - Services/
   - Repositories/
   - DTOs/

3. **提交並推送到 GitHub**
```bash
git add .
git commit -m "Add complete source code"
git push origin main
```

## 方法二：使用 GitHub 網頁介面

1. 訪問 https://github.com/anitachen83-jpg/PortfolioManagement.NET
2. 點擊 "Add file" > "Upload files"
3. 將整個專案資料夾拖放到頁面中
4. 填寫 commit 訊息並提交

## 方法三：使用 GitHub Desktop

1. 下載並安裝 [GitHub Desktop](https://desktop.github.com/)
2. Clone repository
3. 複製所有檔案到本地 repository 資料夾
4. Commit 並 Push

## 還需要上傳的檔案清單

### Services 資料夾 (6 個檔案)
- StockService.cs
- TransactionService.cs
- HoldingService.cs
- DividendService.cs
- ReportService.cs
- IStockService.cs 等介面檔案

### Repositories 資料夾 (8 個檔案)
- IRepository.cs
- Repository.cs
- IStockRepository.cs
- StockRepository.cs
- ITransactionRepository.cs
- TransactionRepository.cs
- IHoldingRepository.cs
- HoldingRepository.cs

### DTOs 資料夾 (所有 DTO 檔案)
- StockDto.cs
- TransactionDto.cs
- HoldingDto.cs
- DividendDto.cs
- 等等...

## 驗證上傳是否完整

上傳完成後，確保專案結構如下：

```
PortfolioManagement.NET/
├── Controllers/          ✅ (部分已上傳)
├── Models/              ✅ (部分已上傳)
├── Services/            ⚠️ (需要上傳)
├── Repositories/        ⚠️ (需要上傳)
├── DTOs/                ⚠️ (需要上傳)
├── Data/                ✅ (已上傳)
├── README.md            ✅
├── API_USAGE.md         ✅
├── ARCHITECTURE.md      ✅
├── DEPLOYMENT_GUIDE.md  ✅
├── start.sh             ✅
├── start.bat            ✅
└── appsettings.json     ✅
```

## 需要協助？

如果在上傳過程中遇到問題，可以：
1. 查看 [GitHub 文檔](https://docs.github.com/)
2. 使用 Git 命令列進行批量上傳（最高效）
3. 確保所有檔案都在正確的資料夾結構中
