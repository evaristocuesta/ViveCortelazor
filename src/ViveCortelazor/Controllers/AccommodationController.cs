using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Services;

namespace ViveCortelazor.Controllers;

public class AccommodationController(IAccommodationService accommodationService) : Controller
{
    public IActionResult Index()
    {
        var accommodations = accommodationService.GetAccommodations(
            ControllerContext.RouteData.Values["lang"]?.ToString() ?? "es");

        return View(accommodations);
    }
}
