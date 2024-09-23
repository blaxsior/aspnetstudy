using api.app.stock.entity;
using api.app.user.entity;

namespace api.app.portfolio.repository {
  public interface IPortfolioRepository {
    Task<List<Stock>> GetUserPortfolio(AppUser user);
  }
}