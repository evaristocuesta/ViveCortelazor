using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViveCortelazor.Models;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class HomeController(IContentService contentService) : Controller
{
    public IActionResult Index()
    {
        var posts = contentService
            .GetContentList("Content/Blog", ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es")
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
    public IActionResult Error(int statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }

        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
