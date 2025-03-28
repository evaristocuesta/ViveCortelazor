using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml.Linq;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;

public class SitemapController : Controller
{
    private readonly ISitemapService _sitemapService;

    public SitemapController(ISitemapService sitemapService)
    {
        _sitemapService = sitemapService;
    }

    [HttpGet()]
    public IActionResult Index()
    {
        var sitemap = _sitemapService.GetSitemap();
        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }
}
