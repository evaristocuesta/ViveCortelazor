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
    [TestCase("es/historia", "Historia - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en/history", "History - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("es/privacidad", "Política de privacidad - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en/privacy", "Privacy policy - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("es/cookies", "Política de Cookies - Vive Cortelazor - Sierra de Aracena")]
    [TestCase("en/cookies", "Cookies policy - Vive Cortelazor - Sierra de Aracena")]
    public async Task HasTitleAsync(string url, string title)
    {
        await Page.GotoAsync(url);

        // Expect a title 
        await Expect(Page).ToHaveTitleAsync(title);
    }

    [TestCase("", "en", "lang-en")]
    [TestCase("es", "en", "lang-en")]
    [TestCase("en", "es", "lang-es")]
    [TestCase("es/historia", "en/history", "lang-en")]
    [TestCase("en/history", "es/historia", "lang-es")]
    [TestCase("es/privacidad", "en/privacy", "lang-en")]
    [TestCase("en/privacy", "es/privacidad", "lang-es")]
    [TestCase("es/cookies", "en/cookies", "lang-en")]
    [TestCase("en/cookies", "es/cookies", "lang-es")]
    public async Task ChangesToLangFromOffcanvasAsync(string origin, string target, string lang)
    {
        await Page.GotoAsync(origin);

        await Page.GetByLabel("Toggle navigation").ClickAsync();
        await Page.Locator("id=languages").ClickAsync();
        await Page.Locator($"id={lang}").ClickAsync();

        // Expect a url
        await Expect(Page).ToHaveURLAsync(new Regex($"{_baseUrl}{target}\\/?$"));
    }

    [TestCase("es/historia", "en/history", "lang-en")]
    [TestCase("en/history", "es/historia", "lang-es")]
    [TestCase("es/privacidad", "en/privacy", "lang-en")]
    [TestCase("en/privacy", "es/privacidad", "lang-es")]
    [TestCase("es/cookies", "en/cookies", "lang-en")]
    [TestCase("en/cookies", "es/cookies", "lang-es")]
    public async Task ChangesToLangFromHorizontalMenuAsync(string origin, string target, string lang)
    {
        await Page.GotoAsync(origin);

        await Page.Locator("id=languages-horizontal-menu").ClickAsync();
        await Page.Locator($"id=languages-horizontal-menu-{lang}").ClickAsync();

        // Expect a url
        await Expect(Page).ToHaveURLAsync(new Regex($"{_baseUrl}{target}\\/?$"));
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
