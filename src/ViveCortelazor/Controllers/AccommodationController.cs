using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;

public class AccommodationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
