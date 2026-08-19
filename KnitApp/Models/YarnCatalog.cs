namespace KnitApp.Models;

//a table with known yarn types, that people can search up
public class YarnCatalog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    //public string? DefaultUnit { get; set; }
}