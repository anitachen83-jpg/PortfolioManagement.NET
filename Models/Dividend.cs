using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManagement.Models;

/// <summary>
/// 股息實體類別
/// </summary>
public class Dividend
{
    [Key]
    public int DividendId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    public DateTime ExDividendDate { get; set; } // 除息日

    public DateTime? PaymentDate { get; set; } // 發放日

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal DividendPerShare { get; set; } // 每股股息

    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; } // 持有股數

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalDividend { get; set; } // 總股息

    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; } = 0; // 股息稅

    [Column(TypeName = "decimal(18,2)")]
    public decimal NetDividend { get; set; } // 實收股息

    [MaxLength(50)]
    public string? DividendType { get; set; } // 現金股利、股票股利

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // 導航屬性
    [ForeignKey("Symbol")]
    public virtual Stock Stock { get; set; } = null!;
}
