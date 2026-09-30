using KnitApp.Models;

namespace KnitApp.Services;

//
public class ShoppingListServices : IShoppingListService
{
    // Takes in a list of selected Pattern objects
    public List<ShoppingListItemDto> GenerateShoppingList(List<Pattern> patterns)
    {
        List<ShoppingListItemDto> result = patterns
            // Merges all materials across all the patterns into a single combined list
            .SelectMany(pattern => pattern.Materials)
            // Groups the materials by yarn name (case-insensitive), so "Sandnes Duo"
            // and "sandnes duo" end up in the same group
            .GroupBy(material => material.MaterialName, StringComparer.OrdinalIgnoreCase)
            // one aggregated shopping list row per group
            .Select(materialGroup => new ShoppingListItemDto
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