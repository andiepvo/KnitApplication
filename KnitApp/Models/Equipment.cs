namespace KnitApp.Models;

public class Equipment
{
    public int Id { get; set; } //primary key
    public int PatternId { get; set; }
    public Pattern Pattern { get; set; } = null!;
    public string EquipmentType { get; set; } = string.Empty;
    public double? Size { get; set; }
    public int? Length { get; set; }
}