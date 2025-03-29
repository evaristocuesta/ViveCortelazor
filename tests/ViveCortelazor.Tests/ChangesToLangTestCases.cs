using Microsoft.Extensions.Hosting;
using System.Collections;
using ViveCortelazor.Services;

namespace ViveCortelazor.Tests;

internal class ChangesToLangTestCases : IEnumerable
{
    private readonly IContentService _contentService = new ContentService();

    public IEnumerator GetEnumerator()
    {
        var esPosts = _contentService
            .GetContentList("Blog", "es")
            .ToDictionary(k => k.Name, k => k);
        
        var enPosts = _contentService
            .GetContentList("Blog", "en")
            .ToDictionary(k => k.Name, k => k);

        foreach (var post in esPosts.Values)
        {
            yield return new TestCaseData($"es/blog/{post.Slug}", $"en/blog/{enPosts[post.Name].Slug}", $"lang-en");
        }

        foreach (var post in enPosts.Values)
        {
            yield return new TestCaseData($"en/blog/{post.Slug}", $"es/blog/{esPosts[post.Name].Slug}", $"lang-es");
        }

        var esPages = _contentService
            .GetContentList("Pages", "es")
            .ToDictionary(k => k.Name, k => k);

        var enPages = _contentService
            .GetContentList("Pages", "en")
            .ToDictionary(k => k.Name, k => k);

        foreach (var page in esPages.Values)
        {
            yield return new TestCaseData($"es/{page.Slug}", $"en/{enPages[page.Name].Slug}", $"lang-en");
        }

        foreach (var page in enPages.Values)
        {
            yield return new TestCaseData($"en/{page.Slug}", $"es/{esPages[page.Name].Slug}", $"lang-es");
        }
    }
}
