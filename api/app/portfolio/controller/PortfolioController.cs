using api.app.portfolio.entity;
using api.app.portfolio.repository;
using api.app.stock.repository;
using api.app.user.entity;
using api.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.app.portfolio.controller
{
  [Route("api/portfolio")]
  [ApiController]
  public class PortfolioController : ControllerBase
  {
    private readonly UserManager<AppUser> userManager;
    private readonly IPortfolioRepository portfolioRepository;
    private readonly IStockRepository stockRepository;
    public PortfolioController(
      UserManager<AppUser> userManager,
      IPortfolioRepository portfolioRepository,
      IStockRepository stockRepository
      )
    {
      this.userManager = userManager;
      this.portfolioRepository = portfolioRepository;
      this.stockRepository = stockRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPortfolio()
    {
      var username = User.GetUsername();
      var appUser = await userManager.FindByNameAsync(username);
      var portfolio = await portfolioRepository.GetUserPortfolio(appUser);

      return Ok(portfolio);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPortfolio(string symbol)
    {
      var username = User.GetUsername();
      var appUser = await userManager.FindByNameAsync(username);
      var stock = await stockRepository.FindBySymbolAsync(symbol);
      if (stock == null) return BadRequest("stock not found");

      var portfolio = await portfolioRepository.GetUserPortfolio(appUser);
      if (portfolio.Any(e => e.Symbol.ToLower() == symbol.ToLower())) return BadRequest("cannot add same stock");

      var portfolioModel = new Portfolio
      {
        StockId = stock.Id,
        AppUserId = appUser.Id
      };

      await portfolioRepository.CreateAsync(portfolioModel);

      if (portfolioModel == null)
      {
        return StatusCode(500, "could not create portfolio");
      }

      return Created();
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeletePortfolio(string symbol)
    {
      var username = User.GetUsername();
      var appUser = await userManager.FindByNameAsync(username);

      var portfolio = await portfolioRepository.GetUserPortfolio(appUser);

      var filteredStocks = portfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();

      if (filteredStocks.Count() == 1)
      {
        await portfolioRepository.DeleteAsync(filteredStocks[0].);
      }
      else
      {
        return BadRequest("Stock not in your portfolio");
      }

      return Ok();
    }
  }
}