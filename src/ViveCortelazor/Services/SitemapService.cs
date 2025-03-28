using System.Buffers.Text;
using System.Xml.Linq;

namespace ViveCortelazor.Services;

public class SitemapService : ISitemapService
{
    private const string BASE_URL = "https://www.vivecortelazor.es";
    private readonly IContentService _contentService;

    public SitemapService(IContentService contentService)
    {
        _contentService = contentService;
    }

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

    class SitemapUrl
    {
        public SitemapUrl(string loc, DateTime lastMod, string changeFreq, double priority)
        {
            Loc = loc;
            LastMod = lastMod;
            ChangeFreq = changeFreq;
            Priority = priority;
        }

        public string Loc { get; set; }
        public DateTime LastMod { get; set; }
        public string ChangeFreq { get; set; }
        public double Priority { get; set; }
    }

    private static List<SitemapUrl> AddHomeRoutes()
    {
        return new List<SitemapUrl>
        {
            new SitemapUrl($"{BASE_URL}/en", DateTime.UtcNow, "monthly", 1.0),
            new SitemapUrl($"{BASE_URL}/es", DateTime.UtcNow, "monthly", 1.0)
        };
    }

    private List<SitemapUrl> AddPages()
    {
        var sitemapUrls = new List<SitemapUrl>();

        var pages = _contentService.GetContentList("Pages", "es");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/es/{page.Slug}", page.Date.ToDateTime(TimeOnly.MinValue), "monthly", 0.8));
        }

        pages = _contentService.GetContentList("Pages", "en");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/en/{page.Slug}", page.Date.ToDateTime(TimeOnly.MinValue), "monthly", 0.8));
        }

        return sitemapUrls;
    }

    private List<SitemapUrl> AddBlog()
    {
        var sitemapUrls = new List<SitemapUrl>
        {
            new SitemapUrl($"{BASE_URL}/en/blog", DateTime.UtcNow, "monthly", 0.7),
            new SitemapUrl($"{BASE_URL}/es/blog", DateTime.UtcNow, "monthly", 0.7)
        };

        var posts = _contentService.GetContentList("Blog", "es");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/es/blog/{post.Slug}", post.Date.ToDateTime(TimeOnly.MinValue), "monthly", 0.7));
        }

        posts = _contentService.GetContentList("Blog", "en");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/en/blog/{post.Slug}", post.Date.ToDateTime(TimeOnly.MinValue), "monthly", 0.7));
        }

        int numPagesPosts = (int)Math.Ceiling((double)posts.Count / 10);

        foreach (var page in Enumerable.Range(2, numPagesPosts - 1))
        {
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/es/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
            sitemapUrls.Add(new SitemapUrl($"{BASE_URL}/en/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
        }

        return sitemapUrls;
    }
}
