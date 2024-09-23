using api.app.portfolio.repository;
using api.app.stock.repository;
using api.app.user.entity;
using api.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.app.portfolio.controller {
  [Route("api/portfolio")]
  [ApiController]
  public class PortfolioController: ControllerBase {
    private readonly UserManager<AppUser> userManager;
    private readonly IPortfolioRepository portfolioRepository;
    public PortfolioController(
      UserManager<AppUser> userManager,
      IPortfolioRepository portfolioRepository
      )
    {
      this.userManager = userManager;
      this.portfolioRepository = portfolioRepository;
    }

    [HttpGet()]
    [Authorize]
    public async Task<IActionResult> GetPortfolio() {
      var username = User.GetUsername();
      var appUser = await userManager.FindByNameAsync(username);
      var portfolio = await portfolioRepository.GetUserPortfolio(appUser);

      return Ok(portfolio);
    }
  }
}