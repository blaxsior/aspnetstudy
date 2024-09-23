using api.app.portfolio.entity;
using Microsoft.AspNetCore.Identity;

namespace api.app.user.entity
{
  public class AppUser : IdentityUser
  { 
    public List<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
  }
}