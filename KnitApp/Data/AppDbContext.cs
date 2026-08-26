namespace KnitApp.Data;
using KnitApp.Models;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}

    public DbSet<Pattern> Patterns => Set<Pattern>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Section> Sections => Set<Section>();
    
    public DbSet<YarnCatalog> YarnCatalogs {get; set;}
    
    public DbSet<PatternImage> PatternImages { get; set; }

}