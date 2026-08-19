namespace KnitApp.Services;

using KnitApp.Models;

public interface IYarnCatalogService
{
    Task<List<YarnCatalog>> GetAllAsync();

}