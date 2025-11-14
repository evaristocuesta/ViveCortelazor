using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public interface IAccommodationService
{
    List<AccommodationViewmodel> GetAccommodations(string language);
}
