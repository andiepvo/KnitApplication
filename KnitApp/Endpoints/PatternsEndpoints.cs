using Microsoft.AspNetCore.Routing;
using KnitApp.Services;
using KnitApp.Models;

namespace KnitApp.Endpoints;

public static class PatternsEndpoints
{
    public static void MapPatternsEndpoints(this IEndpointRouteBuilder app)
    {
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
    }

    private static PatternDto MapToDto(Pattern p) => new(
        p.Id, p.Name, p.PatternType, p.CraftType, p.Description, p.Instructions, p.CreatedOn,
        p.Materials.Select(m => new MaterialDto(m.Id, m.MaterialName, m.Quantity, m.Unit, m.ColorOfYarn)).ToList(),
        p.Equipment.Select(eq => new EquipmentDto(eq.Id, eq.EquipmentType, eq.Size, eq.Length)).ToList(),
        p.InstructionsPdf
    );
}