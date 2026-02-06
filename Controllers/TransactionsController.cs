using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// 取得所有交易
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetAll()
    {
        var transactions = await _transactionService.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    /// <summary>
    /// 取得特定股票的交易記錄
    /// </summary>
    [HttpGet("symbol/{symbol}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetBySymbol(string symbol)
    {
        var transactions = await _transactionService.GetTransactionsBySymbolAsync(symbol);
        return Ok(transactions);
    }

    /// <summary>
    /// 記錄買入交易
    /// </summary>
    [HttpPost("buy")]
    public async Task<ActionResult<Transaction>> RecordBuy([FromBody] BuyRequest request)
    {
        try
        {
            var transaction = await _transactionService.RecordBuyAsync(
                request.Symbol,
                request.Date,
                request.Quantity,
                request.Price,
                request.Fee);
            
            return CreatedAtAction(nameof(GetAll), new { id = transaction.TransactionId }, transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// 記錄賣出交易
    /// </summary>
    [HttpPost("sell")]
    public async Task<ActionResult<Transaction>> RecordSell([FromBody] SellRequest request)
    {
        try
        {
            var transaction = await _transactionService.RecordSellAsync(
                request.Symbol,
                request.Date,
                request.Quantity,
                request.Price,
                request.Fee,
                request.Tax);
            
            return CreatedAtAction(nameof(GetAll), new { id = transaction.TransactionId }, transaction);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

// DTOs
public record BuyRequest(string Symbol, DateTime Date, decimal Quantity, decimal Price, decimal Fee = 0);
public record SellRequest(string Symbol, DateTime Date, decimal Quantity, decimal Price, decimal Fee = 0, decimal Tax = 0);
