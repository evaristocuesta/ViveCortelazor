using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class BlogController(IContentService contentService) : Controller
{

    public IActionResult Blog(int pageNumber = 1)
    {
        var posts = contentService.GetPagedContentList(
            "Blog",
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es",
            pageNumber);

        return View(posts);
    }

    public IActionResult Post(string post)
    {
        var viewModel = contentService.GetContent(
            "Blog",
            post,
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
    }
}
