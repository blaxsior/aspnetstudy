using api.app.portfolio.entity;
using api.app.stock.entity;
using api.app.user.entity;

namespace api.app.portfolio.repository
{
  public interface IPortfolioRepository
  {
    Task<List<Stock>> GetUserPortfolio(AppUser user);

    Task<Portfolio> CreateAsync(Portfolio portfolio);
    Task<Portfolio?> DeleteAsync(AppUser appUser, string symbol);
  }
}