using Microsoft.EntityFrameworkCore;
using PortfolioManagement.Models;

namespace PortfolioManagement.Data;

/// <summary>
/// 投資組合資料庫上下文
/// </summary>
public class PortfolioDbContext : DbContext
{
    public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
        : base(options)
    {
    }

    // DbSet 定義
    public DbSet<Stock> Stocks { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<Holding> Holdings { get; set; } = null!;
    public DbSet<Dividend> Dividends { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Stock 實體配置
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Symbol);
            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Type);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Transaction 實體配置
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId);
            entity.HasIndex(e => e.Symbol);
            entity.HasIndex(e => e.TransactionDate);
            entity.HasIndex(e => e.Type);
            
            entity.HasOne(e => e.Stock)
                .WithMany(s => s.Transactions)
                .HasForeignKey(e => e.Symbol)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Holding 實體配置
        modelBuilder.Entity<Holding>(entity =>
        {
            entity.HasKey(e => e.HoldingId);
            entity.HasIndex(e => e.Symbol).IsUnique();
            
            entity.HasOne(e => e.Stock)
                .WithMany(s => s.Holdings)
                .HasForeignKey(e => e.Symbol)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Dividend 實體配置
        modelBuilder.Entity<Dividend>(entity =>
        {
            entity.HasKey(e => e.DividendId);
            entity.HasIndex(e => e.Symbol);
            entity.HasIndex(e => e.ExDividendDate);
            
            entity.HasOne(e => e.Stock)
                .WithMany(s => s.Dividends)
                .HasForeignKey(e => e.Symbol)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // 種子資料（可選）
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // 可以在這裡加入初始資料
        modelBuilder.Entity<Stock>().HasData(
            new Stock 
            { 
                Symbol = "2330", 
                Name = "台積電", 
                Type = "股票",
                Market = "台股",
                Industry = "半導體",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            },
            new Stock 
            { 
                Symbol = "0050", 
                Name = "元大台灣50", 
                Type = "ETF",
                Market = "台股",
                Industry = "指數型ETF",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            }
        );
    }
}