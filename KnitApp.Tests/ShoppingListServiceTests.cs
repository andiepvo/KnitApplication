using KnitApp.Services;

namespace KnitApp.Tests;

using KnitApp.Models;

//Test: check if two similar yarntypes from two different recipes, are summing up correct
public class ShoppingListServiceTests
{
    [Fact]
    public void GenerateSummary_CombinesQuantities_SameYarnType()
    {
        var material1 = new Material
        {
            MaterialName = "Sandnes duo",
            Quantity = 5,
            Unit = "Skeins"
        };

        var pattern1 = new Pattern
        {
            Materials = new List<Material>{material1}
        };

        var material2 = new Material
        {
            MaterialName = "Sandnes duo",
            Quantity = 3,
            Unit = "Skeins"
        };
        
        var pattern2 = new Pattern
        {
            Materials = new List<Material>{material2} 
        };
        
        var patterns = new List<Pattern>{pattern1, pattern2};

        var service = new ShoppingListServices();
        
        var result = service.GenerateShoppingList(patterns);
        
        Assert.Single(result);
        Assert.Equal(8, result[0].TotalQuantity);
    }
    
    [Fact]
    public void GenerateSummary_CombinesQuantities_DifferentYarnType()
    {
        var material1 = new Material
        {
            MaterialName = "Sandnes Duo",
            Quantity = 5,
            Unit = "Skeins"
        };

        var pattern1 = new Pattern
        {
            Materials = new List<Material>{material1}
        };

        var material2 = new Material
        {
            MaterialName = "Sandnes Line",
            Quantity = 3,
            Unit = "Skeins"
        };
        
        var pattern2 = new Pattern
        {
            Materials = new List<Material>{material2} 
        };
        
        var patterns = new List<Pattern>{pattern1, pattern2};

        var service = new ShoppingListServices();
        
        var result = service.GenerateShoppingList(patterns);
        
        Assert.Equal(2, result.Count);
    }
}

