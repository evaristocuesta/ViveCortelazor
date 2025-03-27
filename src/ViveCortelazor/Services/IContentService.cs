using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public interface IContentService
{
    ContentViewModel GetContent(string directory, string content, string lang);
    IReadOnlyList<ContentViewModel> GetContentList(string contentDirectory, string lang);
}
