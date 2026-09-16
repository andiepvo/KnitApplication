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
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IPatternPdfService, PatternPdfService>();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//ser up swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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
    return patterns.Select(MapToDto);
});

patternsApi.MapGet("/{id:int}", async (int id, IPatternService service) =>
{
    var pattern = await service.GetByIdAsync(id);
    return pattern is null ? Results.NotFound() : Results.Ok(MapToDto(pattern));
});

patternsApi.MapPost("/", async (PatternInputDto input, IPatternService service) =>
{
    var pattern = new Pattern
    {
        Name = input.Name,
        CraftType = input.CraftType,
        PatternType = input.PatternType,
        Description = input.Description,
        Instructions = input.Instructions,
        InstructionsPdf = input.InstructionsPdf,
        CreatedOn = DateTime.UtcNow,
        Materials = input.Materials
            .Select(m => new Material { MaterialName = m.MaterialName, Quantity = m.Quantity, Unit = m.Unit, ColorOfYarn = m.ColorOfYarn })
            .ToList(),
        Equipment = input.Equipment
            .Select(eq => new Equipment { EquipmentType = eq.EquipmentType, Size = eq.Size, Length = eq.Length })
            .ToList()
    };

    var created = await service.AddAsync(pattern);
    return Results.Created($"/api/patterns/{created.Id}", MapToDto(created));
});

patternsApi.MapPut("/{id:int}", async (int id, PatternInputDto input, IPatternService service) =>
{
    var pattern = await service.GetByIdAsync(id);
    if (pattern is null)
        return Results.NotFound();

    pattern.Name = input.Name;
    pattern.CraftType = input.CraftType;
    pattern.PatternType = input.PatternType;
    pattern.Description = input.Description;
    pattern.Instructions = input.Instructions;
    pattern.InstructionsPdf = input.InstructionsPdf;
    pattern.Materials = input.Materials
        .Select(m => new Material { MaterialName = m.MaterialName, Quantity = m.Quantity, Unit = m.Unit, ColorOfYarn = m.ColorOfYarn })
        .ToList();
    pattern.Equipment = input.Equipment
        .Select(eq => new Equipment { EquipmentType = eq.EquipmentType, Size = eq.Size, Length = eq.Length })
        .ToList();

    await service.UpdateAsync(pattern);
    return Results.NoContent();
});

patternsApi.MapDelete("/{id:int}", async (int id, IPatternService service) =>
{
    var pattern = await service.GetByIdAsync(id);
    if (pattern is null)
        return Results.NotFound();

    await service.DeleteAsync(id);
    return Results.NoContent();
});

patternsApi.MapPost("/pdf", async (IFormFile file, IPatternPdfService service) =>
{
    await using var stream = file.OpenReadStream();
    var filePath = await service.SavePdfAsync(stream, file.FileName);
    return Results.Ok(new PdfUploadResultDto(filePath));
})
.DisableAntiforgery();

static PatternDto MapToDto(Pattern p) => new(
    p.Id, p.Name, p.PatternType, p.CraftType, p.Description, p.Instructions, p.CreatedOn,
    p.Materials.Select(m => new MaterialDto(m.Id, m.MaterialName, m.Quantity, m.Unit, m.ColorOfYarn)).ToList(),
    p.Equipment.Select(eq => new EquipmentDto(eq.Id, eq.EquipmentType, eq.Size, eq.Length)).ToList(),
    p.InstructionsPdf
);

app.Run();