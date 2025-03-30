using System.Collections;
using ViveCortelazor.Services;

namespace ViveCortelazor.Tests.TestCases;

public class MetaTagsTestCases : IEnumerable
{
    private readonly IContentService _contentService = new ContentService();

    public IEnumerator GetEnumerator()
    {
        yield return new TestCaseData(
            "",
            "https://www.vivecortelazor.es/es",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en"
                }
            }
        );

        yield return new TestCaseData(
            "es",
            "https://www.vivecortelazor.es/es",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en"
                }
            }
        );

        yield return new TestCaseData(
            "en",
            "https://www.vivecortelazor.es/en",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en"
                }
            }
        );

        yield return new TestCaseData(
            "es/privacidad",
            "https://www.vivecortelazor.es/es/privacidad",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es/privacidad"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en/privacy"
                }
            }
        );

        yield return new TestCaseData(
            "en/privacy",
            "https://www.vivecortelazor.es/en/privacy",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es/privacidad"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en/privacy"
                }
            }
        );

        yield return new TestCaseData(
            "es/cookies",
            "https://www.vivecortelazor.es/es/cookies",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es/cookies"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en/cookies"
                }
            }
        );

        yield return new TestCaseData(
            "en/cookies",
            "https://www.vivecortelazor.es/en/cookies",
            new Dictionary<string, string>
            {
                {
                    "es",
                    "https://www.vivecortelazor.es/es/cookies"
                },
                {
                    "en",
                    "https://www.vivecortelazor.es/en/cookies"
                }
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
                $"https://www.vivecortelazor.es/es/blog/{post.Slug}",
                new Dictionary<string, string>
                {
                    {
                        "es",
                        $"https://www.vivecortelazor.es/es/blog/{post.Slug}"
                    },
                    {
                        "en",
                        $"https://www.vivecortelazor.es/en/blog/{enPosts[post.Name].Slug}"
                    }
                }
            );
        }

        foreach (var post in enPosts.Values)
        {
            yield return new TestCaseData(
                $"en/blog/{post.Slug}",
                $"https://www.vivecortelazor.es/en/blog/{post.Slug}",
                new Dictionary<string, string>
                {
                    {
                        "es",
                        $"https://www.vivecortelazor.es/es/blog/{esPosts[post.Name].Slug}"
                    },
                    {
                        "en",
                        $"https://www.vivecortelazor.es/en/blog/{post.Slug}"
                    }
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
                $"https://www.vivecortelazor.es/es/{page.Slug}",
                new Dictionary<string, string>
                {
                    {
                        "es",
                        $"https://www.vivecortelazor.es/es/{page.Slug}"
                    },
                    {
                        "en",
                        $"https://www.vivecortelazor.es/en/{enPages[page.Name].Slug}"
                    }
                }
            );
        }

        foreach (var page in enPages.Values)
        {
            yield return new TestCaseData(
                $"en/{page.Slug}",
                $"https://www.vivecortelazor.es/en/{page.Slug}",
                new Dictionary<string, string>
                {
                    {
                        "es",
                        $"https://www.vivecortelazor.es/es/{esPages[page.Name].Slug}"
                    },
                    {
                        "en",
                        $"https://www.vivecortelazor.es/en/{page.Slug}"
                    }
                }
            );
        }
    }
}
