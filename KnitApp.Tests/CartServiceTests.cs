using KnitApp.Data;
using KnitApp.Models;
using KnitApp.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KnitApp.Tests;

public class CartServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly CartService _service;

    public CartServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _service = new CartService(_context);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async Task AddToCartAsync_SavesCartItemToDatabase()
    {
        var pattern = new Pattern { Name = "Test Sweater" };
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        await _service.AddToCartAsync(pattern.Id);

        var cartItems = await _context.CartItems.ToListAsync();
        Assert.Single(cartItems);
        Assert.Equal(pattern.Id, cartItems[0].PatternId);
        Assert.Equal(1, cartItems[0].Quantity);
    }

    [Fact]
    public async Task GetCartAsync_ReturnsCartItemsWithPattern()
    {
        var pattern = new Pattern { Name = "Test Sweater" };
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        _context.CartItems.Add(new CartItem { PatternId = pattern.Id, Quantity = 1 });
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();

        var result = await _service.GetCartAsync();

        Assert.Single(result);
        Assert.Equal("Test Sweater", result[0].Pattern.Name);
    }

    [Fact]
    public async Task UpdateQuantityAsync_UpdatesQuantity()
    {
        var pattern = new Pattern { Name = "Test Sweater" };
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        var cartItem = new CartItem { PatternId = pattern.Id, Quantity = 1 };
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();

        await _service.UpdateQuantityAsync(cartItem.Id, 3);

        _context.ChangeTracker.Clear();
        var updated = await _context.CartItems.FindAsync(cartItem.Id);
        Assert.NotNull(updated);
        Assert.Equal(3, updated.Quantity);
    }

    [Fact]
    public async Task UpdateQuantityAsync_NonExistentId_DoesNotThrow()
    {
        await _service.UpdateQuantityAsync(999, 5);
    }

    [Fact]
    public async Task RemoveFromCartAsync_RemovesCartItem()
    {
        var pattern = new Pattern { Name = "Test Sweater" };
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        var cartItem = new CartItem { PatternId = pattern.Id, Quantity = 1 };
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();

        await _service.RemoveFromCartAsync(cartItem.Id);

        var cartItems = await _context.CartItems.ToListAsync();
        Assert.Empty(cartItems);
    }

    [Fact]
    public async Task RemoveFromCartAsync_NonExistentId_DoesNotThrow()
    {
        await _service.RemoveFromCartAsync(123);
    }
    
    [Fact]
public async Task RemoveMaterialFromCartItemAsync_ExcludesMaterialFromCartItem()
{
    // Arrange
    var pattern = new Pattern
    {
        Name = "Test Sweater",
        Materials = new List<Material>
        {
            new Material { MaterialName = "Merino Wool", Quantity = 3, Unit = "Skeins" }
        }
    };
    _context.Patterns.Add(pattern);
    await _context.SaveChangesAsync();

    var cartItem = new CartItem { PatternId = pattern.Id, Quantity = 1 };
    _context.CartItems.Add(cartItem);
    await _context.SaveChangesAsync();

    _context.ChangeTracker.Clear();

    var materialId = pattern.Materials[0].Id;

    // Act
    await _service.RemoveMaterialFromCartItemAsync(cartItem.Id, materialId);

    // Assert
    var excluded = await _context.CartItemExcludedMaterials.ToListAsync();
    Assert.Single(excluded);
    Assert.Equal(cartItem.Id, excluded[0].CartItemId);
    Assert.Equal(materialId, excluded[0].MaterialId);
}

// edge case
[Fact]
public async Task RemoveMaterialFromCartItemAsync_CalledTwice_DoesNotCreateDuplicate()
{
    // Arrange
    var pattern = new Pattern
    {
        Name = "Test Sweater",
        Materials = new List<Material>
        {
            new Material { MaterialName = "Merino Wool", Quantity = 3, Unit = "Skeins" }
        }
    };
    _context.Patterns.Add(pattern);
    await _context.SaveChangesAsync();

    var cartItem = new CartItem { PatternId = pattern.Id, Quantity = 1 };
    _context.CartItems.Add(cartItem);
    await _context.SaveChangesAsync();

    _context.ChangeTracker.Clear();

    var materialId = pattern.Materials[0].Id;

    // Act
    await _service.RemoveMaterialFromCartItemAsync(cartItem.Id, materialId);
    await _service.RemoveMaterialFromCartItemAsync(cartItem.Id, materialId);

    // Assert
    var excluded = await _context.CartItemExcludedMaterials.ToListAsync();
    Assert.Single(excluded);
}

[Fact]
public async Task GetCartAsync_ReflectsExcludedMaterials()
{
    // Arrange
    var pattern = new Pattern
    {
        Name = "Test Sweater",
        Materials = new List<Material>
        {
            new Material { MaterialName = "Merino Wool", Quantity = 3, Unit = "Skeins" }
        }
    };
    _context.Patterns.Add(pattern);
    await _context.SaveChangesAsync();

    var cartItem = new CartItem { PatternId = pattern.Id, Quantity = 1 };
    _context.CartItems.Add(cartItem);
    await _context.SaveChangesAsync();

    _context.ChangeTracker.Clear();

    var materialId = pattern.Materials[0].Id;
    await _service.RemoveMaterialFromCartItemAsync(cartItem.Id, materialId);

    _context.ChangeTracker.Clear();

    // Act
    var result = await _service.GetCartAsync();

    // Assert
    Assert.Single(result);
    Assert.Single(result[0].ExcludedMaterials);
    Assert.Equal(materialId, result[0].ExcludedMaterials[0].MaterialId);
}
}