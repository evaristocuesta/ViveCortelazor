using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;
public class BlogController : Controller
{
    private readonly IContentService _contentService;

    public BlogController(IContentService contentService)
    {
        _contentService = contentService;
    }

    public IActionResult Blog(int pageNumber = 1)
    {
        var posts = _contentService.GetPagedContentList(
            "Blog",
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es", 
            pageNumber);

        return View(posts);
    }

    public IActionResult Post(string post)
    {
        var viewModel = _contentService.GetContent(
            "Blog",
            post,
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(viewModel);
    }
}
