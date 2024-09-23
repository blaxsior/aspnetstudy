using System.ComponentModel.DataAnnotations.Schema;
using api.app.stock.entity;
using api.app.user.entity;

namespace api.app.portfolio.entity
{
  [Table("Portfolios")]
  public class Portfolio
  {
    public string AppUserId {get; set;}
    public int StockId {get; set;}

    public AppUser AppUser {get; set;}
    public Stock  Stock {get; set;}
  }
}