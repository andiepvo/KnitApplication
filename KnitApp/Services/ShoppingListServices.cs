using KnitApp.Models;

namespace KnitApp.Services;

//
public class ShoppingListServices : IShoppingListService
{
    // Takes in a list of selected Pattern objects
    public List<ShoppingListItemDto> GenerateShoppingList(List<Pattern> patterns)
    {
        List<ShoppingListItemDto> result = (
            // Merges all materials across all the patterns into a single combined list
            from pattern in patterns
            from material in pattern.Materials
            // Groups the materials by yarn name, so identical yarn types end up together
            group material by material.MaterialName
            into materialGroup 
            // one aggregated shopping list row per group
            select new ShoppingListItemDto
            {
                YarnName = materialGroup.Key,
                TotalQuantity = materialGroup.Sum(material => material.Quantity),
                Unit = materialGroup.First().Unit
            }
        ).ToList();
        
        // Returns the resulting list
        return result;
        
    } 
    
}