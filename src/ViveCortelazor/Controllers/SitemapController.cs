using Microsoft.AspNetCore.Mvc;
using System.Text;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;

public class SitemapController(ISitemapService sitemapService) : Controller
{
    [HttpGet()]
    public IActionResult Index()
    {
        var sitemap = sitemapService.GetSitemap();
        return Content(sitemap.ToString(), "application/xml", Encoding.UTF8);
    }
}
