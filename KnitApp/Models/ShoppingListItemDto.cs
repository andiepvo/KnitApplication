namespace KnitApp.Models;

//data class representing a calculated shopping list with the needed variables
public class ShoppingListItemDto
{
    public string YarnName { get; set; } = string.Empty;
    public double TotalQuantity { get; set; }
    public string Unit {get; set;} = string.Empty;
}