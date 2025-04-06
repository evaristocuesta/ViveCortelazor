using System.Globalization;
using System.Text.Json;
using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public class ContentService : IContentService
{
    public ContentViewModel GetContent(string directory, string content, string lang)
    {
        string jsonFilePath = Path.Combine(directory, content, $"data.{lang}.json");

        if (!File.Exists(jsonFilePath))
        {
            return ContentViewModel.ContentError;
        }

        var json = File.ReadAllText(jsonFilePath);
        var viewModel = JsonSerializer.Deserialize<ContentViewModel>(json);

        if (viewModel is null)
        {
            return ContentViewModel.ContentError;
        }

        if (content.Length > 11 && 
            DateOnly.TryParse(content.AsSpan(0, 10), CultureInfo.InvariantCulture, out DateOnly date))
        {
            viewModel.Date = date;
        }

        string textFilePath = Path.Combine(directory, content, $"text.{lang}.md");

        if (!File.Exists(textFilePath))
        {
            return ContentViewModel.ContentError;
        }

        var text = File.ReadAllText(textFilePath);
        viewModel.Text = text;
        viewModel.Name = content;

        return viewModel;
    }

    public IReadOnlyList<ContentViewModel> GetContentList(string contentDirectory, string lang)
    {
        List<ContentViewModel> contentList = new();

        foreach (var directory in Directory.GetDirectories(contentDirectory))
        {
            ContentViewModel content = GetContent(
                contentDirectory,
                directory.Replace($"{contentDirectory}{Path.DirectorySeparatorChar}", string.Empty),
                lang);
            contentList.Add(content);
        }

        return contentList
            .OrderByDescending(c => c.Date)
            .ToList();
    }

    public PagedList<ContentViewModel> GetPagedContentList(string contentDirectory, string lang, int page = 1, int pageSize = 10)
    {
        var contentList = GetContentList(contentDirectory, lang);

        var totalCount = contentList.Count;

        var pagedContentList = contentList
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedList<ContentViewModel>(pagedContentList, page, pageSize, totalCount);
    }
}
