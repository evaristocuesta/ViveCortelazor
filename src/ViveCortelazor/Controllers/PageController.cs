using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class PageController(IContentService contentService) : Controller
{
    public IActionResult Page(string page)
    {
        var viewModel = contentService.GetContent(
            "Pages",
            page,
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
    }
}
