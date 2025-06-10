using Microsoft.AspNetCore.Mvc;
using ViveCortelazor.Models;

namespace ViveCortelazor.Controllers;

public class AccommodationController : Controller
{
    public IActionResult Index()
    {
        var accommodations = new List<AccommodationViewmodel>
        {
            new AccommodationViewmodel
            {
                Name = "Casa Piedras",
                Description = "Casa para 2 personas",
                Description2 = "1 habitación, 1 baño y cocina",
                Image = "/images/carousel/cortelazor1.jpg",
                Url = "https://www.lasierraviva.com/listing/700014676"
            },
            new AccommodationViewmodel
            {
                Name = "La Casa de mi Tía María",
                Description = "Casa para 3 personas",
                Description2 = "1 habitación, 1 cama doble, 1 sofá cama, 1 baño y cocina",
                Image = "/images/carousel/cortelazor2.jpg",
                Url = "https://www.lacasademitiamaria.com"
            },
            new AccommodationViewmodel
            {
                Name = "La Casa de mi Tía María",
                Description = "Casa para 3 personas",
                Description2 = "1 habitación, 1 cama doble, 1 sofá cama, 1 baño y cocina",
                Image = "/images/carousel/cortelazor2.jpg",
                Url = "https://www.lacasademitiamaria.com"
            },
            new AccommodationViewmodel
            {
                Name = "La Casa de mi Tía María",
                Description = "Casa para 3 personas",
                Description2 = "1 habitación, 1 cama doble, 1 sofá cama, 1 baño y cocina",
                Image = "/images/carousel/cortelazor2.jpg",
                Url = "https://www.lacasademitiamaria.com"
            },
            new AccommodationViewmodel
            {
                Name = "La Casa de mi Tía María",
                Description = "Casa para 3 personas",
                Description2 = "1 habitación, 1 cama doble, 1 sofá cama, 1 baño y cocina",
                Image = "/images/carousel/cortelazor2.jpg",
                Url = "https://www.lacasademitiamaria.com"
            },
            new AccommodationViewmodel
            {
                Name = "La Casa de mi Tía María",
                Description = "Casa para 3 personas",
                Description2 = "1 habitación, 1 cama doble, 1 sofá cama, 1 baño y cocina",
                Image = "/images/carousel/cortelazor2.jpg",
                Url = "https://www.lacasademitiamaria.com"
            },
        };

        return View(accommodations);
    }
}
