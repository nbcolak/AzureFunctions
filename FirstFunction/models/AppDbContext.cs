using Microsoft.EntityFrameworkCore;

namespace FirstFunction.models;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Product { get; set; }
}
