using api.app.db;
using api.app.stock.dto;
using api.app.stock.entity;
using Microsoft.EntityFrameworkCore;

namespace api.app.stock.repository
{
  public class StockRepository : IStockRepository
  {
    private readonly ApplicationDbContext context;
    public StockRepository(ApplicationDbContext context)
    {
      this.context = context;
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
      await context.Stocks.AddAsync(stock);
      await context.SaveChangesAsync();
      return stock;
    }

    public Task<List<Stock>> FindAllAsync()
    {
      return context.Stocks.Include(it => it.Comments).ToListAsync();
    }

    public async Task<Stock?> FindByIdAsync(int id)
    {
      var stock = await context.Stocks.Include(it => it.Comments).FirstOrDefaultAsync(it => it.Id == id);
      return stock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto dto)
    {
      var stock = await context.Stocks.FindAsync(id);
      if (stock is null) return null;

      stock.Symbol = dto.Symbol;
      stock.CompanyName = dto.CompanyName;
      stock.Purchase = dto.Purchase;
      stock.LastDiv = dto.LastDiv;
      stock.Industry = dto.Industry;
      stock.MarketCap = dto.MarketCap;

      await context.SaveChangesAsync();
      return stock;
    }

    public async Task<Stock?> DeleteByIdAsync(int id)
    {
      var stock = await context.Stocks.FindAsync(id);
      if (stock is null) return null;

      context.Stocks.Remove(stock);
      await context.SaveChangesAsync();
      return stock;
    }

    public Task<bool> ExistsAsync(int id)
    {
      return context.Comments.AnyAsync(it => it.Id == id);
    }
  }
}