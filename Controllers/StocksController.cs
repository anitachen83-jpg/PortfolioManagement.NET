using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;
    private readonly ILogger<StocksController> _logger;

    public StocksController(IStockService stockService, ILogger<StocksController> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    /// <summary>
    /// 取得所有股票
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetAll()
    {
        var stocks = await _stockService.GetAllStocksAsync();
        return Ok(stocks);
    }

    /// <summary>
    /// 依股票代號取得股票
    /// </summary>
    [HttpGet("{symbol}")]
    public async Task<ActionResult<Stock>> Get(string symbol)
    {
        var stock = await _stockService.GetStockAsync(symbol);
        if (stock == null)
            return NotFound($"找不到股票代號 {symbol}");
        
        return Ok(stock);
    }

    /// <summary>
    /// 依類型搜尋股票
    /// </summary>
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Stock>>> GetByType(string type)
    {
        var stocks = await _stockService.GetStocksByTypeAsync(type);
        return Ok(stocks);
    }

    /// <summary>
    /// 搜尋股票
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Stock>>> Search([FromQuery] string keyword)
    {
        var stocks = await _stockService.SearchStocksAsync(keyword);
        return Ok(stocks);
    }

    /// <summary>
    /// 新增股票
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Stock>> Create([FromBody] Stock stock)
    {
        try
        {
            var created = await _stockService.CreateStockAsync(stock);
            return CreatedAtAction(nameof(Get), new { symbol = created.Symbol }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// 更新股票
    /// </summary>
    [HttpPut("{symbol}")]
    public async Task<ActionResult<Stock>> Update(string symbol, [FromBody] Stock stock)
    {
        if (symbol != stock.Symbol)
            return BadRequest("股票代號不符");

        try
        {
            var updated = await _stockService.UpdateStockAsync(stock);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// 刪除股票
    /// </summary>
    [HttpDelete("{symbol}")]
    public async Task<ActionResult> Delete(string symbol)
    {
        try
        {
            await _stockService.DeleteStockAsync(symbol);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
