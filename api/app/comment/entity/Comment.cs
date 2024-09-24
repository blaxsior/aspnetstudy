using System.ComponentModel.DataAnnotations.Schema;
using api.app.stock.entity;
using api.app.user.entity;
using Microsoft.EntityFrameworkCore;

namespace api.app.comment.entity
{
  [Table("Comments")]
  public class Comment
  {
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;

    public DateTime CreatedOn {get; set; } = DateTime.Now;
    public int? StockId {get; set;} // navigation property
    public Stock? Stock { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
  }
}