namespace KnitApp.Models;

public class CartItem
{
    public int Id { get; set; }

    public int PatternId { get; set; }
    public Pattern Pattern { get; set; } = null!;

    public int Quantity { get; set; } = 1;
    
    public List<CartItemExcludedMaterial> ExcludedMaterials { get; set; } = new();
}