using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public interface IContentService
{
    ContentViewModel GetContent(string directory, string content, string lang);
    IReadOnlyList<ContentViewModel> GetContentList(string contentDirectory, string lang);
    PagedList<ContentViewModel> GetPagedContentList(string contentDirectory, string lang, int page = 1, int pageSize = 10);
}
