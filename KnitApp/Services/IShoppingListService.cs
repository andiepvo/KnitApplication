using KnitApp.Models;

namespace KnitApp.Services;

//Defines the contract for generating an aggregated shopping list from selected patterns
public interface IShoppingListService
{
    List<ShoppingListItemDto> GenerateShoppingList(List<Pattern> patterns);
}