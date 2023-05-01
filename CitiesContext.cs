using Microsoft.EntityFrameworkCore;

namespace PogodaCityBot;

public partial class CitiesContext : DbContext
{
    public CitiesContext()
    {
    }

    public CitiesContext(DbContextOptions<CitiesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CityId> CityIds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=cities.db");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
