using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class PageController : Controller
{
    private readonly IContentService _contentService;

    public PageController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IActionResult Page(string page)
    {
        var viewModel = _contentService.GetContent(
            "Pages",
            page,
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
    }
}
