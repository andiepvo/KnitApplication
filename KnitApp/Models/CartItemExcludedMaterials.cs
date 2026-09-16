namespace KnitApp.Models;

public class CartItemExcludedMaterial
{
    public int Id { get; set; }

    public int CartItemId { get; set; }
    public CartItem CartItem { get; set; } = null!;

    public int MaterialId { get; set; }
    public Material Material { get; set; } = null!;
}