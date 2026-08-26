using KnitApp.Data;
using KnitApp.Models;
using KnitApp.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace KnitApp.Tests;

//if getallsync() actual get the materials and not only the recipes
public class PatternServiceTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly PatternService _service;

    public PatternServiceTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _service = new PatternService(_context);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }

    [Fact]
    public async Task AddAsync_SavesPatternToDatabase()
    {
        // Arrange
        var pattern = new Pattern { Name = "Test Sweater" };

        // Act
        await _service.AddAsync(pattern);

        // Assert
        var patterns = await _context.Patterns.ToListAsync();
        Assert.Single(patterns);
        Assert.Equal("Test Sweater", patterns[0].Name);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsPatternsWithMaterials()
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

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Single(result);
        Assert.Single(result[0].Materials);
        Assert.Equal("Merino Wool", result[0].Materials[0].MaterialName);
    }
    
    [Fact]
    public async Task GetByIdAsync_ReturnsPatternWithMaterials()
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

        _context.ChangeTracker.Clear();

        // Act
        var result = await _service.GetByIdAsync(pattern.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Sweater", result.Name);
        Assert.Single(result.Materials);
    }
    [Fact]
    public async Task UpdateAsync_ReplacesMaterialsCorrectly()
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

        _context.ChangeTracker.Clear();

        // Act 
        var toUpdate = await _service.GetByIdAsync(pattern.Id);
        toUpdate!.Materials = new List<Material>
        {
            new Material { MaterialName = "Alpaca", Quantity = 2, Unit = "Balls" }
        };
        await _service.UpdateAsync(toUpdate);

        _context.ChangeTracker.Clear();

        // Assert 
        var result = await _service.GetByIdAsync(pattern.Id);
        Assert.NotNull(result);
        Assert.Single(result.Materials);
        Assert.Equal("Alpaca", result.Materials[0].MaterialName);
    }
    [Fact]
    public async Task DeleteAsync_RemovesPatternFromDatabase()
    {
        // Arrange
        var pattern = new Pattern { Name = "Test Sweater" };
        _context.Patterns.Add(pattern);
        await _context.SaveChangesAsync();

        _context.ChangeTracker.Clear();

        // Act
        await _service.DeleteAsync(pattern.Id);

        // Assert
        var patterns = await _context.Patterns.ToListAsync();
        Assert.Empty(patterns);
    }
    
    //edge case 
    [Fact]
    public async Task DeleteAsync_NonExistentId_DoesNotThrow()
    {
        // Act & Assert - skal ikke kaste exception
        await _service.DeleteAsync(999);
    }
}