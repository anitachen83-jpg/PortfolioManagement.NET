using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManagement.Models;

/// <summary>
/// 股票實體類別
/// </summary>
public class Stock
{
    [Key]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string Type { get; set; } = "股票"; // 股票、ETF

    [MaxLength(50)]
    public string? Market { get; set; } // 市場：台股、美股

    [MaxLength(50)]
    public string? Industry { get; set; } // 產業分類

    public bool IsActive { get; set; } = true;

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // 導航屬性
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public virtual ICollection<Holding> Holdings { get; set; } = new List<Holding>();
    public virtual ICollection<Dividend> Dividends { get; set; } = new List<Dividend>();
}