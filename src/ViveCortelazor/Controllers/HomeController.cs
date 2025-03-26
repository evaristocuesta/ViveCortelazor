using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using ViveCortelazor.Models;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class HomeController : Controller
{
    private readonly IContentService _contentService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IContentService contentService,
        ILogger<HomeController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Page(string page)
    {
        var viewModel = _contentService.GetContent(
            "Pages", 
            page, 
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
    }

    public IActionResult Blog(int page = 1)
    {
        return View();
    }

    public IActionResult Post(string post)
    {
        var viewModel = _contentService.GetContent(
            "Blog",
            post,
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
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
