using api.app.comment.entity;
using api.app.portfolio.entity;
using api.app.stock.entity;
using api.app.user.entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.app.db
{
  public class ApplicationDbContext : IdentityDbContext<AppUser>
  {
    public ApplicationDbContext(DbContextOptions options)
     : base(options)
    {

    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // primary key 지정
      builder.Entity<Portfolio>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));
      // 연관 관계 설정
      builder.Entity<Portfolio>()
      .HasOne(u => u.AppUser)
      .WithMany(u => u.Portfolios)
      .HasForeignKey(p => p.AppUserId);

      builder.Entity<Portfolio>()
      .HasOne(it => it.Stock)
      .WithMany(u => u.Portfolios)
      .HasForeignKey(p => p.StockId);


      List<IdentityRole> roles = new List<IdentityRole> {
        new IdentityRole {
          Name = "Admin",
          NormalizedName = "ADMIN"
        },
        new IdentityRole {
          Name = "User",
          NormalizedName = "USER"
        }

      };
      // 초기 데이터 집어넣음
      builder.Entity<IdentityRole>().HasData(roles);
    }
  }
}