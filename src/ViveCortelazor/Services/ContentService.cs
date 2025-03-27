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

        string textFilePath = Path.Combine(directory, content, $"text.{lang}.md");

        if (!File.Exists(textFilePath))
        {
            return ContentViewModel.ContentError;
        }

        var text = File.ReadAllText(textFilePath);
        viewModel.Text = text;

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

        return contentList;
    }
}
