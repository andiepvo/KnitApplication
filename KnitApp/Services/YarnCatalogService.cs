namespace KnitApp.Services;

using KnitApp.Data;
using KnitApp.Models;
using Microsoft.EntityFrameworkCore;

public class YarnCatalogService :  IYarnCatalogService
{
    private readonly AppDbContext _context;

    public YarnCatalogService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<YarnCatalog>> GetAllAsync()
    {
        return await _context.YarnCatalogs.ToListAsync();
    }
    
}