using System.Collections;
using ViveCortelazor.Services;

namespace ViveCortelazor.Tests;

internal class VerifyAllTestCases : IEnumerable
{
    private readonly IContentService _contentService = new ContentService();

    public IEnumerator GetEnumerator()
    {
        var posts = _contentService.GetContentList("Blog", "es");

        foreach (var post in posts)
        {
            yield return new TestCaseData($"es/blog/{post.Slug}");
        }

        posts = _contentService.GetContentList("Blog", "en");

        foreach (var post in posts)
        {
            yield return new TestCaseData($"en/blog/{post.Slug}");
        }

        var pages = _contentService.GetContentList("Pages", "es");

        foreach (var page in pages)
        {
            yield return new TestCaseData($"es/{page.Slug}");
        }

        pages = _contentService.GetContentList("Pages", "en");

        foreach (var page in pages)
        {
            yield return new TestCaseData($"en/{page.Slug}");
        }
    }
}
