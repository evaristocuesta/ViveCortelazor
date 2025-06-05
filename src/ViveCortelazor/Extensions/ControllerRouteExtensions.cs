using ViveCortelazor.Services;

namespace ViveCortelazor.Extensions;

public static class ControllerRouteExtensions
{
    public static void AddControllerRoutes(this WebApplication app)
    {
        app.MapControllerRoute(
            name: "sitemap",
            pattern: "sitemap.xml",
            defaults: new { controller = "Sitemap", action = "Index" });

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
            defaults: new { lang = "es", controller = "Home", action = "Home" },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: "Error",
            pattern: "es/error/{statusCode}",
            defaults: new { lang = "es", controller = "Home", action = "Error", statusCode = 0 },
            constraints: new { lang = @"(\w{2})" });

        var contentService = app.Services.GetRequiredService<IContentService>();

        AddPages(app, contentService);

        AddBlog(app, contentService);
    }

    private static void AddBlog(WebApplication app, IContentService contentService)
    {
        app.MapControllerRoute(
            name: $"es/blog",
            pattern: $"es/blog",
            defaults: new { lang = "es", controller = "Blog", action = "Blog", pageNumber = 1 },
            constraints: new { lang = @"(\w{2})" });

        app.MapControllerRoute(
            name: $"en/blog",
            pattern: $"en/blog",
            defaults: new { lang = "en", controller = "Blog", action = "Blog", pageNumber = 1 },
            constraints: new { lang = @"(\w{2})" });

        var posts = contentService.GetContentList("Content/Blog", "es");

        foreach (var post in posts)
        {
            app.MapControllerRoute(
                name: $"es/blog/{post.Name}",
                pattern: $"es/blog/{post.Slug}",
                defaults: new { lang = "es", controller = "Blog", action = "Post", post = post.Name },
                constraints: new { lang = @"(\w{2})" });
        }

        posts = contentService.GetContentList("Content/Blog", "en");

        foreach (var post in posts)
        {
            app.MapControllerRoute(
                name: $"en/blog/{post.Name}",
                pattern: $"en/blog/{post.Slug}",
                defaults: new { lang = "en", controller = "Blog", action = "Post", post = post.Name },
                constraints: new { lang = @"(\w{2})" });
        }

        int numPagesPosts = (int)Math.Ceiling((double)posts.Count / 10);

        foreach (var page in Enumerable.Range(1, numPagesPosts))
        {
            app.MapControllerRoute(
                name: $"es/blog/{page}",
                pattern: $"es/blog/{page}",
                defaults: new { lang = "es", controller = "Blog", action = "Blog", pageNumber = page },
                constraints: new { lang = @"(\w{2})" });

            app.MapControllerRoute(
                name: $"en/blog/{page}",
                pattern: $"en/blog/{page}",
                defaults: new { lang = "en", controller = "Blog", action = "Blog", pageNumber = page },
                constraints: new { lang = @"(\w{2})" });
        }
    }

    private static void AddPages(WebApplication app, IContentService contentService)
    {
        var pages = contentService.GetContentList("Content/Pages", "es");

        foreach (var page in pages)
        {
            app.MapControllerRoute(
                name: $"es/{page.Name}",
                pattern: $"es/{page.Slug}",
                defaults: new { lang = "es", controller = "Page", action = "Page", page = page.Name },
                constraints: new { lang = @"(\w{2})" });
        }

        pages = contentService.GetContentList("Content/Pages", "en");

        foreach (var page in pages)
        {
            app.MapControllerRoute(
                name: $"en/{page.Name}",
                pattern: $"en/{page.Slug}",
                defaults: new { lang = "en", controller = "Page", action = "Page", page = page.Name },
                constraints: new { lang = @"(\w{2})" });
        }
    }
}
