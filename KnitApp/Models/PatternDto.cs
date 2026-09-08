namespace KnitApp.Models;

//material is added because it makes sense to make it visible in JSON as well because a pattern have it
public record MaterialDto(int Id, string MaterialName, int Quantity, string Unit, string? ColorOfYarn);

public record EquipmentDto(int Id, string EquipmentType, double? Size, int? Length);

public record PatternDto(
    int Id,
    string Name,
    PatternType PatternType,
    CraftType CraftType,
    string? Description,
    string? Instructions,
    DateTime CreatedOn,
    List<MaterialDto> Materials,
    List<EquipmentDto> Equipment,
    string? InstructionsPdf
);

public record PdfUploadResultDto(string FilePath);

// Used for both create (POST) and update (PUT) — the editable shape of a pattern is the same either way
public record PatternInputDto(
    string Name,
    PatternType PatternType,
    CraftType CraftType,
    string? Description,
    string? Instructions,
    List<MaterialInputDto> Materials,
    List<EquipmentInputDto> Equipment,
    string? InstructionsPdf
);

public record MaterialInputDto(string MaterialName, int Quantity, string Unit, string? ColorOfYarn);

public record EquipmentInputDto(string EquipmentType, double? Size, int? Length);