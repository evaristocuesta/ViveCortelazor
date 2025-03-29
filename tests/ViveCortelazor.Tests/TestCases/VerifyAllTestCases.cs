using System.Collections;
using ViveCortelazor.Services;

namespace ViveCortelazor.Tests.TestCases;

internal class VerifyAllTestCases : IEnumerable
{
    private readonly IContentService _contentService = new ContentService();

    public IEnumerator GetEnumerator()
    {
        var posts = _contentService.GetContentList("Content/Blog", "es");

        foreach (var post in posts)
        {
            yield return new TestCaseData($"es/blog/{post.Slug}");
        }

        posts = _contentService.GetContentList("Content/Blog", "en");

        foreach (var post in posts)
        {
            yield return new TestCaseData($"en/blog/{post.Slug}");
        }

        var pages = _contentService.GetContentList("Content/Pages", "es");

        foreach (var page in pages)
        {
            yield return new TestCaseData($"es/{page.Slug}");
        }

        pages = _contentService.GetContentList("Content/Pages", "en");

        foreach (var page in pages)
        {
            yield return new TestCaseData($"en/{page.Slug}");
        }
    }
}
