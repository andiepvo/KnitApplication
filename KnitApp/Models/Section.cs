namespace KnitApp.Models;

public class Section
{
    public int Id { get; set; } //primary key
    public int PatternId { get; set; }
    public Pattern? Pattern { get; set; }

    public string Name { get; set; } = string.Empty;
    public int StartRow { get; set; }
    public int EndRow { get; set; }

}