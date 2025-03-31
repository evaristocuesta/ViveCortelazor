using Microsoft.Extensions.Options;
using System.Xml.Linq;
using ViveCortelazor.Options;

namespace ViveCortelazor.Services;

public class SitemapService(IContentService contentService, IOptions<WebSettings> options) : ISitemapService
{
    private readonly string _host = options.Value.Host;

    public XDocument GetSitemap()
    {
        var urls = AddHomeRoutes();
        urls.AddRange(AddPages());
        urls.AddRange(AddBlog());

        return new XDocument(
            new XDeclaration("1.0", "utf-8", "yes"),
            new XElement("urlset",
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

    private List<SitemapUrl> AddHomeRoutes()
    {
        return
        [
            new($"{_host}/en", DateTime.UtcNow, "monthly", 1.0),
            new($"{_host}/es", DateTime.UtcNow, "monthly", 1.0)
        ];
    }

    private List<SitemapUrl> AddPages()
    {
        List<SitemapUrl> sitemapUrls = [];

        var pages = contentService.GetContentList("Content/Pages", "es");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{_host}/es/{page.Slug}", DateTime.UtcNow, "monthly", 0.8));
        }

        pages = contentService.GetContentList("Content/Pages", "en");

        foreach (var page in pages)
        {
            sitemapUrls.Add(new SitemapUrl($"{_host}/en/{page.Slug}", DateTime.UtcNow, "monthly", 0.8));
        }

        return sitemapUrls;
    }

    private List<SitemapUrl> AddBlog()
    {
        List<SitemapUrl> sitemapUrls = new();

        var posts = contentService.GetContentList("Content/Blog", "es");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new($"{_host}/es/blog/{post.Slug}", DateTime.UtcNow, "monthly", 0.7));
        }

        posts = contentService.GetContentList("Content/Blog", "en");

        foreach (var post in posts)
        {
            sitemapUrls.Add(new($"{_host}/en/blog/{post.Slug}", DateTime.UtcNow, "monthly", 0.7));
        }

        sitemapUrls.Add(new($"{_host}/en/blog", DateTime.UtcNow, "monthly", 0.7));
        sitemapUrls.Add(new($"{_host}/es/blog", DateTime.UtcNow, "monthly", 0.7));

        int numPagesPosts = (int)Math.Ceiling((double)posts.Count / 10);

        foreach (var page in Enumerable.Range(2, numPagesPosts - 1))
        {
            sitemapUrls.Add(new($"{_host}/es/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
            sitemapUrls.Add(new($"{_host}/en/blog/{page}", DateTime.UtcNow, "monthly", 0.6));
        }

        return sitemapUrls;
    }
}
