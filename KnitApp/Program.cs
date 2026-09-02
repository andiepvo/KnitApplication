using KnitApp.Components;
using KnitApp.Data;
using Microsoft.EntityFrameworkCore; 
using KnitApp.Services;
using KnitApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IYarnCatalogService, YarnCatalogService>();

builder.Services.AddScoped<IPatternService, PatternService>();
builder.Services.AddScoped<IPatternImageService, PatternImageService>();
builder.Services.AddScoped<IShoppingListService, ShoppingListServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

var patternsApi = app.MapGroup("/api/patterns");
patternsApi.MapGet("/", async (IPatternService service) =>
{
    var patterns = await service.GetAllAsync();
    return patterns.Select(p => new PatternDto(
        p.Id,
        p.Name,
        p.PatternType,
        p.CraftType,
        p.Description,
        p.Instructions,
        p.CreatedOn,
        p.Materials.Select(m => new MaterialDto(m.Id, m.MaterialName, m.Quantity, m.Unit, m.ColorOfYarn)).ToList(),
        p.InstructionsPdf
    ));
});

app.Run();