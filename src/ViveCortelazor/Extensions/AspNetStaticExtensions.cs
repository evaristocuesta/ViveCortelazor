using AspNetStatic;

namespace ViveCortelazor.Extensions;

public static class AspNetStaticExtensions
{
    public static IApplicationBuilder ConfigureAspNetStatic(
        this WebApplication app,
        string basePath,
        string outputPath)
    {
        if (app.Services.GetRequiredService<IStaticResourcesInfoProvider>() is not StaticResourcesInfoProvider staticResourcesProvider)
        {
            throw new InvalidOperationException("StaticResourcesInfoProvider is not registered");
        }

        app.UseEndpoints(endpoints => { });

        AddStaticResources(staticResourcesProvider, basePath);
        AddEndpoints(app, basePath, staticResourcesProvider);

        if (!Path.Exists(outputPath))
        {
            Console.WriteLine($"Creating directory {outputPath}");
            Directory.CreateDirectory(outputPath);
        }

        Console.WriteLine($"Generating static content in {outputPath}");

        app.GenerateStaticContent(outputPath,
            alwaysDefaultFile: true,
            exitWhenDone: true,
            dontUpdateLinks: true);

        return app;
    }

    private static void AddEndpoints(WebApplication app, string basePath, StaticResourcesInfoProvider staticResourcesProvider)
    {
        var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();

        var routeEndpoints = endpointDataSource.Endpoints
            .OfType<RouteEndpoint>()
            .Where(e => e.Metadata.OfType<RouteNameMetadata>().Any())
            .GroupBy(e => e.Metadata.OfType<RouteNameMetadata>().FirstOrDefault()?.RouteName)
            .Select(g => g.First())
            .Select(e => new
            {
                Route = e.RoutePattern.RawText,
                Name = e.Metadata.OfType<RouteNameMetadata>().FirstOrDefault()?.RouteName
            })
            .ToList();

        foreach (var route in routeEndpoints)
        {
            staticResourcesProvider.Add(new PageResource($"{basePath}{route.Route}"));
        }
    }

    private static void AddStaticResources(StaticResourcesInfoProvider staticResourcesProvider, string basePath)
    {
        staticResourcesProvider.Add(new CssResource($"{basePath}/ViveCortelazor.styles.css"));

        foreach (var file in Directory.GetFiles("wwwroot/", "*.*", SearchOption.AllDirectories))
        {
            var resource = ResourceFactory.CreateResource($"{basePath}{file.Replace("wwwroot", "").Replace(Path.DirectorySeparatorChar, '/')}");

            switch (resource)
            {
                case CssResource cssResource:
                    staticResourcesProvider.Add(cssResource);
                    break;
                case JsResource jsResource:
                    staticResourcesProvider.Add(jsResource);
                    break;
                case BinResource binResource:
                    staticResourcesProvider.Add(binResource);
                    break;
                case PageResource pageResource:
                    staticResourcesProvider.Add(pageResource);
                    break;
            }
        }
    }

    static class ResourceFactory
    {
        public static ResourceInfoBase CreateResource(string path)
        {
            if (path.EndsWith(".css"))
            {
                return new CssResource(path);
            }
            if (path.EndsWith(".js"))
            {
                return new JsResource(path);
            }
            if (path.EndsWith(".svg") || path.EndsWith(".jpg") || path.EndsWith(".png"))
            {
                return new BinResource(path);
            }

            return new PageResource(path);
        }
    }
}
