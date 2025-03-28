using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViveCortelazor.Models;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class HomeController : Controller
{
    private readonly IContentService _contentService;

    public HomeController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IActionResult Index()
    {
        var posts = _contentService
            .GetContentList("Blog", ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es")
            .Take(3);
        
        return View(posts);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Cookies()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
