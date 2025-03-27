namespace ViveCortelazor.Models;

public class ContentViewModel
{
    public string Name { get; set; } = string.Empty;
    public required string Title { get; set; }
    public DateOnly Date { get; set; }
    public required string Slug { get; set; }
    public required string Language { get; set; }
    public string? Text { get; set; }
    public required string Keywords { get; set; }
    public required string Description { get; set; }
    public required string Image { get; set; }
    public required string ImageWidth { get; set; }
    public required string ImageHeight { get; set; }
    public required string Robots { get; set; }
    public bool HasError { get; set; } = false;

    public static ContentViewModel ContentError => new()
    {
        Name = "Error",
        Title = "Error",
        Date = DateOnly.FromDateTime(DateTime.UtcNow),
        Slug = "error",
        Language = "en",
        Text = "An error occurred.",
        Keywords = "error",
        Description = "An error occurred.",
        Image = "/images/error.png",
        ImageWidth = "1200",
        ImageHeight = "630",
        Robots = "noindex, nofollow",
        HasError = true
    };
}
