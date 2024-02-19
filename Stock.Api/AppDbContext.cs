using Microsoft.EntityFrameworkCore;

namespace Stock.Api
{
    public class AppDbContext : DbContext
    {
        public DbSet<Models.Stock> Stocks { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
   
}
