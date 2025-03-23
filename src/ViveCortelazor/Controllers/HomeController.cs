using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using ViveCortelazor.Models;

namespace ViveCortelazor.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Page(string page)
    {
        var lang = ControllerContext.RouteData.Values["lang"];
        string jsonFilePath = Path.Combine("Pages", page, $"data.{lang}.json");

        if (!System.IO.File.Exists(jsonFilePath))
        {
            return NotFound();
        }

        var json = System.IO.File.ReadAllText(jsonFilePath);
        var viewModel = JsonSerializer.Deserialize<PageViewModel>(json);
        
        if (viewModel is null)
        {
            return NotFound();
        }

        string textFilePath = Path.Combine("Pages", page, $"text.{lang}.md");
        
        if (!System.IO.File.Exists(textFilePath))
        {
            return NotFound();
        }

        var text = System.IO.File.ReadAllText(textFilePath);
        viewModel.Text = text;

        return View("Page", viewModel);
    }

    public IActionResult Blog()
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
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
