namespace ViveCortelazor.Models;
public class IndexViewModel
{
    public IEnumerable<ContentViewModel> Posts { get; init; } = Array.Empty<ContentViewModel>();
    public string? Introduction1 { get; init; }
    public string? Introduction2 { get; init; }
    public string? Introduction3 { get; init; }
}