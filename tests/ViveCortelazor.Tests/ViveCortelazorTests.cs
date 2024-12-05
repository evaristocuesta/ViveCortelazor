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
    public async Task HasTitleAsync(string url, string title)
    {
        await Page.GotoAsync(url);

        // Expect a title 
        await Expect(Page).ToHaveTitleAsync(title);
    }

    [TestCase("", "en", "lang-en")]
    [TestCase("es", "en", "lang-en")]
    [TestCase("en", "es", "lang-es")]
    public async Task ChangesToLangAsync(string origin, string target, string lang)
    {
        await Page.GotoAsync(origin);

        await Page.GetByLabel("Toggle navigation").ClickAsync();
        await Page.Locator("id=languages").ClickAsync();
        await Page.Locator($"id={lang}").ClickAsync();

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
