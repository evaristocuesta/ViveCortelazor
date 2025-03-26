using System.Text.Json;
using ViveCortelazor.Models;

namespace ViveCortelazor.Extensions;

public static class ControllerRouteExtensions
{
    public static void AddControllerRoutes(this IEndpointRouteBuilder app)
    {
        app.MapControllerRoute(
            name: "en/index",
            pattern: "en",
            defaults: new { lang = "en", controller = "Home", action = "Index" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "es/index",
            pattern: "es",
            defaults: new { lang = "es", controller = "Home", action = "Index" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "en/privacy",
            pattern: "en/privacy",
            defaults: new { lang = "en", controller = "Home", action = "Privacy" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "es/privacy",
            pattern: "es/privacidad",
            defaults: new { lang = "es", controller = "Home", action = "Privacy" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "en/cookies",
            pattern: "en/cookies",
            defaults: new { lang = "en", controller = "Home", action = "Cookies" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "es/cookies",
            pattern: "es/cookies",
            defaults: new { lang = "es", controller = "Home", action = "Cookies" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "/",
            pattern: "/",
            defaults: new { lang = "es", controller = "Home", action = "Index" },
            constraints: new { lang = @"(\w{2})" });

        foreach (var directory in Directory.GetDirectories("Pages"))
        {
            foreach (var file in  Directory.GetFiles(directory, "data.*.json"))
            {
                var json = System.IO.File.ReadAllText(file);
                var viewModel = JsonSerializer.Deserialize<ContentViewModel>(json);

                if (viewModel is null)
                {
                    throw new InvalidOperationException($"Error deserializing {file}");
                }

                string pageName = directory.Replace($"Pages{Path.DirectorySeparatorChar}", "");

                app.MapControllerRoute(
                    name: $"{viewModel.Language}/{pageName}",
                    pattern: $"{viewModel.Language}/{viewModel.Slug}",
                    defaults: new { lang = viewModel.Language, controller = "Home", action = "Page", page = pageName },
                    constraints: new { lang = @"(\w{2})" });
            }
        }

        foreach (var directory in Directory.GetDirectories("Blog"))
        {
            foreach (var file in Directory.GetFiles(directory, "data.*.json"))
            {
                var json = System.IO.File.ReadAllText(file);
                var viewModel = JsonSerializer.Deserialize<ContentViewModel>(json);

                if (viewModel is null)
                {
                    throw new InvalidOperationException($"Error deserializing {file}");
                }

                string postName = directory.Replace($"Blog{Path.DirectorySeparatorChar}", "");

                app.MapControllerRoute(
                    name: $"{viewModel.Language}/blog/{postName}",
                    pattern: $"{viewModel.Language}/blog/{viewModel.Slug}",
                    defaults: new { lang = viewModel.Language, controller = "Home", action = "Post", post = postName },
                    constraints: new { lang = @"(\w{2})" });
            }
        }
    }
}
