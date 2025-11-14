using System.Text.Json;
using ViveCortelazor.Models;

namespace ViveCortelazor.Services;

public class AccommodationService : IAccommodationService
{
    public List<AccommodationViewmodel> GetAccommodations(string language)
    {
        var jsonPath = $"Content/Accommodation/accommodation.{language}.json";
        var jsonString = File.ReadAllText(jsonPath);
        
        var accommodations = JsonSerializer.Deserialize<List<AccommodationViewmodel>>(
            jsonString, 
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            }
        );

        return accommodations ?? new List<AccommodationViewmodel>();
    }
}
