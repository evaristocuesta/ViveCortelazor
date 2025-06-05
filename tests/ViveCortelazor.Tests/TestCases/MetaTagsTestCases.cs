using System.Collections;
using ViveCortelazor.Services;

namespace ViveCortelazor.Tests.TestCases;

public class MetaTagsTestCases : IEnumerable
{
    private readonly string _baseUrl;

    private readonly IContentService _contentService = new ContentService();

    public MetaTagsTestCases()
    {
        _baseUrl = TestContext.Parameters["BaseUrl"] ?? string.Empty;
        _baseUrl = _baseUrl.EndsWith('/') ? _baseUrl[..^1] : _baseUrl;
    }

    public IEnumerator GetEnumerator()
    {
        yield return new TestCaseData(
            "",
            $"{_baseUrl}/",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es" },
                { "en", $"{_baseUrl}/en" }
            }
        );

        yield return new TestCaseData(
            "es",
            $"{_baseUrl}/es",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es" },
                { "en", $"{_baseUrl}/en" }
            }
        );

        yield return new TestCaseData(
            "en",
            $"{_baseUrl}/en",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es" },
                { "en", $"{_baseUrl}/en" }
            }
        );

        yield return new TestCaseData(
            "es/privacidad",
            $"{_baseUrl}/es/privacidad",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es/privacidad" },
                { "en", $"{_baseUrl}/en/privacy" }
            }
        );

        yield return new TestCaseData(
            "en/privacy",
            $"{_baseUrl}/en/privacy",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es/privacidad" },
                { "en", $"{_baseUrl}/en/privacy" }
            }
        );

        yield return new TestCaseData(
            "es/cookies",
            $"{_baseUrl}/es/cookies",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es/cookies" },
                { "en", $"{_baseUrl}/en/cookies" }
            }
        );

        yield return new TestCaseData(
            "en/cookies",
            $"{_baseUrl}/en/cookies",
            new Dictionary<string, string>
            {
                { "es", $"{_baseUrl}/es/cookies" },
                { "en", $"{_baseUrl}/en/cookies" }
            }
        );

        var esPosts = _contentService
            .GetContentList("Content/Blog", "es")
            .ToDictionary(k => k.Name, k => k);

        var enPosts = _contentService
            .GetContentList("Content/Blog", "en")
            .ToDictionary(k => k.Name, k => k);

        foreach (var post in esPosts.Values)
        {
            yield return new TestCaseData(
                $"es/blog/{post.Slug}",
                $"{_baseUrl}/es/blog/{post.Slug}",
                new Dictionary<string, string>
                {
                    { "es", $"{_baseUrl}/es/blog/{post.Slug}" },
                    { "en", $"{_baseUrl}/en/blog/{enPosts[post.Name].Slug}" }
                }
            );
        }

        foreach (var post in enPosts.Values)
        {
            yield return new TestCaseData(
                $"en/blog/{post.Slug}",
                $"{_baseUrl}/en/blog/{post.Slug}",
                new Dictionary<string, string>
                {
                    { "es", $"{_baseUrl}/es/blog/{esPosts[post.Name].Slug}" },
                    { "en", $"{_baseUrl}/en/blog/{post.Slug}" }
                }
            );
        }

        var esPages = _contentService
            .GetContentList("Content/Pages", "es")
            .ToDictionary(k => k.Name, k => k);

        var enPages = _contentService
            .GetContentList("Content/Pages", "en")
            .ToDictionary(k => k.Name, k => k);

        foreach (var page in esPages.Values)
        {
            yield return new TestCaseData(
                $"es/{page.Slug}",
                $"{_baseUrl}/es/{page.Slug}",
                new Dictionary<string, string>
                {
                    { "es", $"{_baseUrl}/es/{page.Slug}" },
                    { "en", $"{_baseUrl}/en/{enPages[page.Name].Slug}" }
                }
            );
        }

        foreach (var page in enPages.Values)
        {
            yield return new TestCaseData(
                $"en/{page.Slug}",
                $"{_baseUrl}/en/{page.Slug}",
                new Dictionary<string, string>
                {
                    { "es", $"{_baseUrl}/es/{esPages[page.Name].Slug}" },
                    { "en", $"{_baseUrl}/en/{page.Slug}" }
                }
            );
        }
    }
}
