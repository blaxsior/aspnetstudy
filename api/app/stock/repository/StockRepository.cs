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

    public async Task<List<Stock>> FindAllAsync(ListStockQuery queries)
    {
      var query = context.Stocks.AsQueryable();

      // 일반적인 표준 API와 연동되서 쿼리가 생성됨
      if (!string.IsNullOrWhiteSpace(queries.Symbol))
        query = query.Where(x => x.Symbol.Contains(queries.Symbol));
      if (!string.IsNullOrWhiteSpace(queries.CompanyName))
        query = query.Where(x => x.CompanyName.Contains(queries.CompanyName));

      // 리팩토링 방법 고민
      if (!string.IsNullOrWhiteSpace(queries.SortBy))
      {
        query = (queries.SortBy.ToLower(), queries.IsDesc) switch
        {
          ("symbol", false) => query.OrderBy(it => it.Symbol),
          ("symbol", true) => query.OrderByDescending(it => it.Symbol),
          ("companyname", false) => query.OrderBy(it => it.CompanyName),
          ("companyname", true) => query.OrderByDescending(it => it.CompanyName),
          _ => query
        };

      }
      return await query.Include(it => it.Comments).ToListAsync();
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