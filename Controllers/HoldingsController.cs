using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Models;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HoldingsController : ControllerBase
{
    private readonly IHoldingService _holdingService;

    public HoldingsController(IHoldingService holdingService)
    {
        _holdingService = holdingService;
    }

    /// <summary>
    /// 取得所有持倉
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Holding>>> GetAll()
    {
        var holdings = await _holdingService.GetAllHoldingsAsync();
        return Ok(holdings);
    }

    /// <summary>
    /// 取得特定股票的持倉
    /// </summary>
    [HttpGet("{symbol}")]
    public async Task<ActionResult<Holding>> Get(string symbol)
    {
        var holding = await _holdingService.GetHoldingAsync(symbol);
        if (holding == null)
            return NotFound($"找不到股票 {symbol} 的持倉");
        
        return Ok(holding);
    }

    /// <summary>
    /// 重新計算所有持倉
    /// </summary>
    [HttpPost("recalculate")]
    public async Task<ActionResult> RecalculateAll()
    {
        await _holdingService.RecalculateAllHoldingsAsync();
        return Ok(new { message = "持倉已重新計算" });
    }
}
