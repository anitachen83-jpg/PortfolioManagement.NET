using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DividendsController : ControllerBase
{
    private readonly IDividendService _dividendService;

    public DividendsController(IDividendService dividendService)
    {
        _dividendService = dividendService;
    }

    /// <summary>
    /// 取得特定股票的股息記錄
    /// </summary>
    [HttpGet("symbol/{symbol}")]
    public async Task<ActionResult<IEnumerable<Dividend>>> GetBySymbol(string symbol)
    {
        var dividends = await _dividendService.GetDividendsBySymbolAsync(symbol);
        return Ok(dividends);
    }

    /// <summary>
    /// 取得特定年度的股息記錄
    /// </summary>
    [HttpGet("year/{year}")]
    public async Task<ActionResult<IEnumerable<Dividend>>> GetByYear(int year)
    {
        var dividends = await _dividendService.GetDividendsByYearAsync(year);
        return Ok(dividends);
    }

    /// <summary>
    /// 記錄股息
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<Dividend>> Create([FromBody] Dividend dividend)
    {
        var created = await _dividendService.RecordDividendAsync(dividend);
        return CreatedAtAction(nameof(GetBySymbol), new { symbol = created.Symbol }, created);
    }
}
