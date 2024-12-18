using Microsoft.EntityFrameworkCore;

namespace FirstFunction.models;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Personal> Products { get; set; }
}
