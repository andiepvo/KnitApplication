namespace KnitApp.Models;

//Represent a saved pattern and shows the different elements of the pattern should include
public class Pattern
{
    public int Id { get; set; } //the primary key for each recipe
    public string Name { get; set; } = string.Empty; //name of the recipe
    public PatternType PatternType { get; set; } //Which pattern type
    public CraftType CraftType { get; set; }
    public string? Description { get; set; } 
    public string? Instructions { get; set; } 
    public int WorkingRow { get; set; } //unsure if it is needed or can be removed
    public int? TotalRows { get; set; }
    public DateTime CreatedOn { get; set; } //when the user started on the project
    public List<Material> Materials { get; set; } = new(); //list of needed materials for the pattern 
    public List<Section> Sections { get; set; } = new(); //list of sections in the recipe, for example arm, yoke etc.










}