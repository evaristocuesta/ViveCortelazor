using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public interface IContentService
{
    ContentViewModel GetContent(string directory, string content, string lang);
}
