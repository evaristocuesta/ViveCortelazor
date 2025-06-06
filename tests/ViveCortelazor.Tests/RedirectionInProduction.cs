namespace ViveCortelazor.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RedirectionInProduction : PageTest
{
    [Test]
    [TestCase("https://www.vivecortelazor.es", "es-ES", "https://www.vivecortelazor.es/es")]
    [TestCase("https://www.vivecortelazor.es", "en-GB", "https://www.vivecortelazor.es/en")]
    [TestCase("https://www.vivecortelazor.es", "fr-FR", "https://www.vivecortelazor.es/es")]
    public async Task UserRedirectedTo(string url, string language, string urlExpected)
    {
        var context = await Browser.NewContextAsync(new()
        {
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Accept-Language"] = language
            }
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });

        Assert.That(page.Url, Is.EqualTo(urlExpected), $"Expected redirect to {urlExpected} for language {language}");

        await context.CloseAsync();
        await context.DisposeAsync();
    }

    [Test]
    [TestCase("https://www.vivecortelazor.es", "es-ES", "https://www.vivecortelazor.es/")]
    [TestCase("https://www.vivecortelazor.es", "en-GB", "https://www.vivecortelazor.es/")]
    [TestCase("https://www.vivecortelazor.es", "fr-FR", "https://www.vivecortelazor.es/")]
    public async Task BotRedirectedTo(string url, string language, string urlExpected)
    {
        var context = await Browser.NewContextAsync(new()
        {
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["Accept-Language"] = language,
                ["User-Agent"] = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)" // Simulating a bot user agent
            }
        });

        var page = await context.NewPageAsync();

        await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });

        Assert.That(page.Url, Is.EqualTo(urlExpected), $"Expected redirect to {urlExpected} for language {language}");

        await context.CloseAsync();
        await context.DisposeAsync();
    }

    [Test]
    [TestCase("https://www.vivecortelazor.es", "https://www.vivecortelazor.es/", "https://www.vivecortelazor.es/es", "https://www.vivecortelazor.es/en")]
    public async Task VerifyMetaTagsAsync(string url, string expectedCanonical, string expectedAlternateEs, string expectedAlternateEn)
    {
        var context = await Browser.NewContextAsync(new()
        {
            ExtraHTTPHeaders = new Dictionary<string, string>
            {
                ["User-Agent"] = "Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)" // Simulating a bot user agent
            }
        });

        var page = await context.NewPageAsync();
        await page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });

        // Verify canonical link
        var canonicalLink = await page.GetAttributeAsync("link[rel='canonical']", "href");
        Assert.That(canonicalLink, Is.EqualTo(expectedCanonical), $"Canonical link mismatch for {url}");

        // Verify alternate links
        var alternateLink = await page.GetAttributeAsync($"link[rel='alternate'][hreflang='es']", "href");
        Assert.That(alternateLink, Is.EqualTo(expectedAlternateEs), $"Alternate link mismatch for {url} and hreflang es");

        alternateLink = await page.GetAttributeAsync($"link[rel='alternate'][hreflang='en']", "href");
        Assert.That(alternateLink, Is.EqualTo(expectedAlternateEn), $"Alternate link mismatch for {url} and hreflang en");
    }
}
