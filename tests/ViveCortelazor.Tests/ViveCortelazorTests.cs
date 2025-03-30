using ViveCortelazor.Tests.TestCases;

namespace ViveCortelazor.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class ViveCortelazorTests : PageTest
{
    private string _baseUrl = string.Empty;

    [Test]
    [TestCase("", "Vive Cortelazor - Sierra de Aracena")]
    [TestCase("es", "Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en", "Vive Cortelazor - Sierra de Aracena")]
    [TestCase("es/privacidad", "Política de privacidad - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en/privacy", "Privacy policy - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("es/cookies", "Política de Cookies - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en/cookies", "Cookies policy - Vive Cortelazor - Sierra de Aracena")]
    [TestCaseSource(typeof(HasTitleAsyncTestCases))]
    public async Task HasTitleAsync(string url, string title)
    {
        await Page.GotoAsync(url);

        // Expect a title 
        await Expect(Page).ToHaveTitleAsync(title);
    }

    [Test]
    [TestCaseSource(typeof(MetaTagsTestCases))]
    public async Task VerifyMetaTagsAsync(string url, string expectedCanonical, Dictionary<string, string> expectedAlternates)
    {
        await Page.GotoAsync(url);

        // Verify canonical link
        var canonicalLink = await Page.GetAttributeAsync("link[rel='canonical']", "href");
        Assert.That(canonicalLink, Is.EqualTo(expectedCanonical), $"Canonical link mismatch for {url}");

        // Verify alternate links
        foreach (var alternate in expectedAlternates)
        {
            var alternateLink = await Page.GetAttributeAsync($"link[rel='alternate'][hreflang='{alternate.Key}']", "href");
            Assert.That(alternateLink, Is.EqualTo(alternate.Value), $"Alternate link mismatch for {url} and hreflang {alternate.Key}");
        }
    }

    [TestCase("", "en", "lang-en")]
    [TestCase("es", "en", "lang-en")]
    [TestCase("en", "es", "lang-es")]
    [TestCase("es/privacidad", "en/privacy", "lang-en")]
    [TestCase("en/privacy", "es/privacidad", "lang-es")]
    [TestCase("es/cookies", "en/cookies", "lang-en")]
    [TestCase("en/cookies", "es/cookies", "lang-es")]
    [TestCaseSource(typeof(ChangesToLangTestCases))]
    public async Task ChangesToLangFromOffcanvasAsync(string origin, string target, string lang)
    {
        await Page.GotoAsync(origin);

        await Page.GetByLabel("Toggle navigation").ClickAsync();
        await Page.Locator("id=languages").ClickAsync();
        await Page.Locator($"id={lang}").ClickAsync();

        // Expect a url
        await Expect(Page).ToHaveURLAsync(new Regex($"{_baseUrl}{target}\\/?$"));
    }

    [TestCase("es/privacidad", "en/privacy", "lang-en")]
    [TestCase("en/privacy", "es/privacidad", "lang-es")]
    [TestCase("es/cookies", "en/cookies", "lang-en")]
    [TestCase("en/cookies", "es/cookies", "lang-es")]
    [TestCaseSource(typeof(ChangesToLangTestCases))]
    public async Task ChangesToLangFromHorizontalMenuAsync(string origin, string target, string lang)
    {
        await Page.GotoAsync(origin);

        await Page.Locator("id=languages-horizontal-menu").ClickAsync();
        await Page.Locator($"id=languages-horizontal-menu-{lang}").ClickAsync();

        // Expect a url
        await Expect(Page).ToHaveURLAsync(new Regex($"{_baseUrl}{target}\\/?$"));
    }

    [TestCase("en")]
    [TestCase("es")]
    [TestCase("es/privacidad")]
    [TestCase("en/privacy")]
    [TestCase("es/cookies")]
    [TestCase("en/cookies")]
    [TestCaseSource(typeof(VerifyAllTestCases))]
    public async Task VerifyAllImagesExist(string pageUrl)
    {
        await Page.GotoAsync(pageUrl);

        var images = await Page.QuerySelectorAllAsync("img");

        var srcs = (await Task.WhenAll(images.Select(async link => await link.GetAttributeAsync("src"))))
            .Where(href => !string.IsNullOrEmpty(href))
            .Distinct()
            .ToList();

        using var httpClient = new HttpClient();

        foreach (var src in srcs)
        {
            if (!string.IsNullOrEmpty(src))
            {
                // Ensure the URL is absolute
                var imageUrl = new Uri(new Uri(_baseUrl), src).ToString();

                // Make the HTTP request and check if the link responds correctly
                var response = await httpClient.GetAsync(imageUrl);
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"{imageUrl} does not exist");
            }
        }
    }

    [TestCase("en")]
    [TestCase("es")]
    [TestCase("es/privacidad")]
    [TestCase("en/privacy")]
    [TestCase("es/cookies")]
    [TestCase("en/cookies")]
    [TestCaseSource(typeof(VerifyAllTestCases))]
    public async Task VerifyAllLinksWork(string pageUrl)
    {
        await Page.GotoAsync(pageUrl);

        var links = await Page.QuerySelectorAllAsync("a");

        var hrefs = (await Task.WhenAll(links.Select(async link => await link.GetAttributeAsync("href"))))
            .Where(href => !string.IsNullOrEmpty(href))
            .Distinct()
            .ToList();


        using var httpClient = new HttpClient();

        foreach (var href in hrefs)
        {
            if (!string.IsNullOrEmpty(href) &&
                !href.StartsWith('#') &&
                !href.StartsWith("javascript") &&
                !href.StartsWith("mailto"))
            {
                // Ensure the URL is absolute
                var linkUrl = new Uri(new Uri(_baseUrl), href).ToString();

                try
                {
                    // Make the HTTP request and check if the link responds correctly
                    var response = await httpClient.GetAsync(linkUrl);
                    Assert.That((int)response.StatusCode, Is.EqualTo(200), $"{linkUrl} is broken (Status code: {response.StatusCode}).");
                }
                catch (Exception ex)
                {
                    Assert.Fail($"{linkUrl} is broken: {ex.Message}");
                }
            }
        }
    }

    public override BrowserNewContextOptions ContextOptions()
    {
        _baseUrl = TestContext.Parameters["BaseUrl"] ?? string.Empty;

        return new BrowserNewContextOptions()
        {
            BaseURL = _baseUrl
        };
    }
}
