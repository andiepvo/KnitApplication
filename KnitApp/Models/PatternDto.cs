namespace KnitApp.Models;

//material is added because it makes sense to make it visible in json as weel because a pattern have it
public record MaterialDto(int Id, string MaterialName, int Quantity, string Unit, string? ColorOfYarn);

public record PatternDto(
    int Id,
    string Name,
    PatternType PatternType,
    CraftType CraftType,
    string? Description,
    string? Instructions,
    DateTime CreatedOn,
    List<MaterialDto> Materials,
    string? InstructionsPdf
);