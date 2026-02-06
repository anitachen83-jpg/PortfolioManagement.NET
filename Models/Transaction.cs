using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioManagement.Models;

/// <summary>
/// 交易實體類別
/// </summary>
public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Symbol { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Type { get; set; } = string.Empty; // 買入、賣出

    [Required]
    public DateTime TransactionDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Quantity { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal Fee { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; } = 0;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // 導航屬性
    [ForeignKey("Symbol")]
    public virtual Stock Stock { get; set; } = null!;

    // 計算屬性
    [NotMapped]
    public decimal SubTotal => Quantity * Price;
}
