using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManagement.Models;

/// <summary>
/// 持倉實體類別
/// </summary>
public class Holding
{
    [Key]
    public int HoldingId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal AverageCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? CurrentPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MarketValue { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? UnrealizedPL { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal? UnrealizedPLPercent { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // 導航屬性
    [ForeignKey("Symbol")]
    public virtual Stock Stock { get; set; } = null!;
}
