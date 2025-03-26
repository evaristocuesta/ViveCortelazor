using System.Text.Json;
using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public class ContentService : IContentService
{
    public ContentViewModel GetContent(string directory, string content, string lang)
    {
        string jsonFilePath = Path.Combine(directory, content, $"data.{lang}.json");

        if (!System.IO.File.Exists(jsonFilePath))
        {
            return ContentViewModel.ContentError;
        }

        var json = System.IO.File.ReadAllText(jsonFilePath);
        var viewModel = JsonSerializer.Deserialize<ContentViewModel>(json);

        if (viewModel is null)
        {
            return ContentViewModel.ContentError;
        }

        string textFilePath = Path.Combine(directory, content, $"text.{lang}.md");

        if (!System.IO.File.Exists(textFilePath))
        {
            return ContentViewModel.ContentError;
        }

        var text = System.IO.File.ReadAllText(textFilePath);
        viewModel.Text = text;

        return viewModel;
    }
}
