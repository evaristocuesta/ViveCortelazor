namespace ViveCortelazor.Models;

public class PageViewModel
{
    public required string Title { get; set; }
    public string? Text { get; set; }
    public required string Keywords { get; set; }
    public required string Description { get; set; }
    public required string Image { get; set; }
    public required string ImageWidth { get; set; }
    public required string ImageHeight { get; set; }
    public required string Robots { get; set; }
}
