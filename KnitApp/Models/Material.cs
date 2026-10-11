using System.ComponentModel.DataAnnotations;

namespace KnitApp.Models;

public class Material
{
    public int Id { get; set; } //primary key
    public int PatternId { get; set; } //fremmednøkkel
    public Pattern? Pattern { get; set; }

    [Required(ErrorMessage = "Yarn name is required.")]
    public string MaterialName { get; set; } = string.Empty; //important,always have a value

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; } = string.Empty; //how many skeins
    public string? ColorOfYarn { get; set; }
    
}