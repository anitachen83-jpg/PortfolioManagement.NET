using Microsoft.EntityFrameworkCore;
using PortfolioManagement.Data;
using PortfolioManagement.Repositories;
using PortfolioManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// 配置資料庫
var databaseProvider = builder.Configuration["AppSettings:DatabaseProvider"];
if (databaseProvider == "SqlServer")
{
    builder.Services.AddDbContext<PortfolioDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
}
else
{
    builder.Services.AddDbContext<PortfolioDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// 註冊 Repositories
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IHoldingRepository, HoldingRepository>();
builder.Services.AddScoped<IDividendRepository, DividendRepository>();

// 註冊 Services
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IHoldingService, HoldingService>();
builder.Services.AddScoped<IDividendService, DividendService>();
builder.Services.AddScoped<IReportService, ReportService>();

// 註冊 AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// 加入 Controllers
builder.Services.AddControllers();

// 加入 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() 
    { 
        Title = "Portfolio Management API", 
        Version = "v1",
        Description = "投資組合管理系統 RESTful API",
        Contact = new() 
        { 
            Name = "Portfolio Manager",
            Email = "support@portfolio.com"
        }
    });
});

// 加入 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 自動執行資料庫遷移
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    dbContext.Database.EnsureCreated();
    // 或使用遷移：dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio Management API v1");
        c.RoutePrefix = string.Empty; // 設定 Swagger UI 為根路徑
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

Console.WriteLine("=================================================================");
Console.WriteLine("🚀 Portfolio Management API 已啟動");
Console.WriteLine("=================================================================");
Console.WriteLine($"📍 API URL: {app.Urls.FirstOrDefault() ?? "https://localhost:5001"}");
Console.WriteLine($"📖 Swagger UI: {app.Urls.FirstOrDefault() ?? "https://localhost:5001"}/swagger");
Console.WriteLine($"💾 Database: {databaseProvider}");
Console.WriteLine("=================================================================");

app.Run();