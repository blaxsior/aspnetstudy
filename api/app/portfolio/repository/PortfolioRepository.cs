using api.app.db;
using api.app.stock.entity;
using api.app.user.entity;
using Microsoft.EntityFrameworkCore;

namespace api.app.portfolio.repository
{
  public class PortfolioRepository : IPortfolioRepository
  {
    private readonly ApplicationDbContext context;
    public PortfolioRepository(ApplicationDbContext context)
    {
      this.context = context;
    }

    public Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
      return context.Portfolios.Where(p => p.AppUserId == user.Id)
      .Select(p => new Stock
      {
        Id = p.StockId,
        Symbol = p.Stock.Symbol,
        CompanyName = p.Stock.CompanyName,
        Purchase = p.Stock.Purchase,
        LastDiv = p.Stock.LastDiv,
        Industry = p.Stock.Industry,
        MarketCap = p.Stock.MarketCap
      }).ToListAsync();
    }
  }
}