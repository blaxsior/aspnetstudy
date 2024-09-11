using api.app.comment.entity;
using api.app.stock.entity;
using Microsoft.EntityFrameworkCore;

namespace api.app.db
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions options)
     : base(options) { 
      
     }

     public DbSet<Stock> Stocks { get; set; }
     public DbSet<Comment> Comments { get; set; }
  }
}