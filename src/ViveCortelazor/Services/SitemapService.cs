using System.Xml.Linq;

namespace ViveCortelazor.Services;

public class SitemapService(IContentService contentService) : ISitemapService
{
    private const string BASE_URL = "https://www.vivecortelazor.es";

    public XDocument GetSitemap()
    {
        var urls = AddHomeRoutes();
        urls.AddRange(AddPages());
        urls.AddRange(AddBlog());

        return new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement(XName.Get("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9"),
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                urls.Select(url => new XElement("url",
                    new XElement("loc", url.Loc),
                    new XElement("lastmod", url.LastMod.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                    new XElement("changefreq", url.ChangeFreq),
                    new XElement("priority", url.Priority)
                ))
            )
        );
    }

    record SitemapUrl(string Loc, DateTime LastMod, string ChangeFreq, double Priority)
    {
        public string Loc { get; set; } = Loc;
        public DateTime LastMod { get; set; } = LastMod;
        public string ChangeFreq { get; set; } = ChangeFreq;
        public double Priority { get; set; } = Priority;
    }

    private static List<SitemapUrl> AddHomeRoutes()
    {
        return
        [
            new($"{BASE_URL}/en", DateTime.UtcNow, "monthly", 1.0),
            new($"{BASE_URL}/es", DateTime.UtcNow, "monthly", 1.0)
        ];
    }

    private List<SitemapUrl> AddPages()
    {
        List<SitemapUrl> sitemapUrls = [];

        var pages = contentService.GetContentList("Pages", "es");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/es/{page.Slug}", DateTime.UtcNow, "monthly", 0.8));
        }

        pages = contentService.GetContentList("Pages", "en");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/en/{page.Slug}", DateTime.UtcNow, "monthly", 0.8));
        }

        return sitemapUrls;
    }

    private List<SitemapUrl> AddBlog()
    {
        var sitemapUrls = new List<SitemapUrl>
        {
            new($"{BASE_URL}/en/blog", DateTime.UtcNow, "monthly", 0.7),
            new($"{BASE_URL}/es/blog", DateTime.UtcNow, "monthly", 0.7)
        };

        var posts = contentService.GetContentList("Blog", "es");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new($"{BASE_URL}/es/blog/{post.Slug}", DateTime.UtcNow, "monthly", 0.7));
        }

        posts = contentService.GetContentList("Blog", "en");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new($"{BASE_URL}/en/blog/{post.Slug}", DateTime.UtcNow, "monthly", 0.7));
        }

        int numPagesPosts = (int)Math.Ceiling((double)posts.Count / 10);

        foreach (var page in Enumerable.Range(2, numPagesPosts - 1))
        {
            sitemapUrls.Add(new($"{BASE_URL}/es/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
            sitemapUrls.Add(new($"{BASE_URL}/en/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
        }

        return sitemapUrls;
    }
}
