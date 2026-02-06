using Microsoft.AspNetCore.Mvc;
using PortfolioManagement.Services;

namespace PortfolioManagement.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// 取得投資組合摘要
    /// </summary>
    [HttpGet("summary")]
    public async Task<ActionResult> GetSummary()
    {
        var summary = await _reportService.GetPortfolioSummaryAsync();
        return Ok(summary);
    }

    /// <summary>
    /// 取得已實現損益
    /// </summary>
    [HttpGet("realized-pl")]
    public async Task<ActionResult> GetRealizedPL()
    {
        var realizedPL = await _reportService.GetRealizedPLAsync();
        return Ok(realizedPL);
    }

    /// <summary>
    /// 取得績效分析
    /// </summary>
    [HttpGet("performance")]
    public async Task<ActionResult> GetPerformance()
    {
        var performance = await _reportService.GetPerformanceAsync();
        return Ok(performance);
    }
}
