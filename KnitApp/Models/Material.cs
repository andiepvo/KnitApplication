namespace KnitApp.Models;

public class Material
{
    public int Id { get; set; } //primary key
    public int PatternId { get; set; } //fremmednøkkel
    public Pattern? Pattern { get; set; }

    public string MaterialName { get; set; } = string.Empty; //important,always have a value
    public int Quantity { get; set; } 
    public string Unit { get; set; } = string.Empty; //how many skeins
    public string? ColorOfYarn { get; set; }
    
}