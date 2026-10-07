using Microsoft.EntityFrameworkCore;
using KnitApp.Data;
using KnitApp.Services;

namespace KnitApp.Extensions;

public static class ServiceConfiguration
{
    public static void AddKnitAppServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IYarnCatalogService, YarnCatalogService>();
        services.AddScoped<IPatternService, PatternService>();
        services.AddScoped<IPatternImageService, PatternImageService>();
        services.AddScoped<IShoppingListService, ShoppingListServices>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IPatternPdfService, PatternPdfService>();

        services.AddHttpClient();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}