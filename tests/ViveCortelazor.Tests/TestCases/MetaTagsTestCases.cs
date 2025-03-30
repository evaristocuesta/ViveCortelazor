using System.Collections;

namespace ViveCortelazor.Tests.TestCases;

public class MetaTagsTestCases : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        yield return new object[] { "", "https://www.vivecortelazor.es/es", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es" }, { "en", "https://www.vivecortelazor.es/en" } } };
        yield return new object[] { "es", "https://www.vivecortelazor.es/es", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es" }, { "en", "https://www.vivecortelazor.es/en" } } };
        yield return new object[] { "en", "https://www.vivecortelazor.es/en", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es" }, { "en", "https://www.vivecortelazor.es/en" } } };
        yield return new object[] { "es/privacidad", "https://www.vivecortelazor.es/es/privacidad", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es/privacidad" }, { "en", "https://www.vivecortelazor.es/en/privacy" } } };
        yield return new object[] { "en/privacy", "https://www.vivecortelazor.es/en/privacy", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es/privacidad" }, { "en", "https://www.vivecortelazor.es/en/privacy" } } };
        yield return new object[] { "es/cookies", "https://www.vivecortelazor.es/es/cookies", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es/cookies" }, { "en", "https://www.vivecortelazor.es/en/cookies" } } };
        yield return new object[] { "en/cookies", "https://www.vivecortelazor.es/en/cookies", new Dictionary<string, string> { { "es", "https://www.vivecortelazor.es/es/cookies" }, { "en", "https://www.vivecortelazor.es/en/cookies" } } };
    }
}
