using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ViveCortelazor.Models;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class HomeController(IContentService contentService) : Controller
{
    public IActionResult Index()
    {
        var lang = ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es";

        var posts = contentService
            .GetContentList("Content/Blog", lang)
            .Take(3);

        var model = new IndexViewModel
        {
            Posts = posts,
            Introduction1 = ReadMarkdown("Content/Index", "introduction1", lang),
            Introduction2 = ReadMarkdown("Content/Index", "introduction2", lang),
            Introduction3 = ReadMarkdown("Content/Index", "introduction3", lang)
        };

        return View(model);
    }

    public IActionResult Home()
    {
        return View();
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

    private string? ReadMarkdown(string directory, string name, string lang)
    {
        string textFilePath = Path.Combine(directory, $"{name}.{lang}.md");
        return System.IO.File.Exists(textFilePath) ? System.IO.File.ReadAllText(textFilePath) : null;
    }
}
