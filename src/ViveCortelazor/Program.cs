using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ViveCortelazor.Pipelines;
using AspNetStatic;
using ViveCortelazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var supportedCultures = new[]
{
    new CultureInfo("es"),
    new CultureInfo("en"),
};

var options = new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(culture: "es", uiCulture: "es"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

options.RequestCultureProviders =
[
    new RouteDataRequestCultureProvider() { Options = options, RouteDataStringKey = "lang" }
];

builder.Services.AddSingleton(options);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc(opts =>
{
    opts.Filters.Add(new MiddlewareFilterAttribute(typeof(LocalizationPipeline)));
})
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

var outputPath = args.Length >= 2 ? $"{args[1]}" : string.Empty;
var basePath = args.Length == 3 ? $"/{args[2]}" : string.Empty;

builder.Services.AddSingleton<IStaticResourcesInfoProvider>(
  new StaticResourcesInfoProvider(
    [
      new PageResource($"{basePath}/"),
      new PageResource($"{basePath}/es"),
      new PageResource($"{basePath}/en"),
      new PageResource($"{basePath}/es/historia"),
      new PageResource($"{basePath}/en/history"),
      new PageResource($"{basePath}/es/que-hacer"),
      new PageResource($"{basePath}/en/what-to-do"),
      new PageResource($"{basePath}/es/senderismo"),
      new PageResource($"{basePath}/en/hiking"),
      new PageResource($"{basePath}/es/privacidad"),
      new PageResource($"{basePath}/en/privacy"),
      new PageResource($"{basePath}/es/cookies"),
      new PageResource($"{basePath}/en/cookies"),
      new CssResource($"{basePath}/css/site.css?v=pAGv4ietcJNk_EwsQZ5BN9-K4MuNYS2a9wl4Jw-q9D0"),
      new CssResource($"{basePath}/ViveCortelazor.styles.css?v=QVIm3G0TQnz7jhf0QoO7Vxi4Cck3I2ZBcZUJUpvQ19o"),
      new JsResource($"{basePath}/js/site.js?v=hRQyftXiu1lLX2P9Ly9xa4gHJgLeR1uGN5qegUobtGo"),
      new BinResource($"{basePath}/images/fox.svg"),
      new BinResource($"{basePath}/images/cookie-color.svg"),
      new BinResource($"{basePath}/images/history/history-cortelazor.jpg"),
      new BinResource($"{basePath}/images/index/cortelazor-streets.jpg"),
      new BinResource($"{basePath}/images/index/cortelazor-trees.jpg"),
      new BinResource($"{basePath}/images/carousel/cortelazor1.jpg"),
      new BinResource($"{basePath}/images/carousel/cortelazor2.jpg"),
      new BinResource($"{basePath}/images/what-to-do/cortelazor.jpg"),
      new BinResource($"{basePath}/images/what-to-do/charco-malo.jpg"),
      new BinResource($"{basePath}/images/hiking/hiking.jpg"),
      new BinResource($"{basePath}/images/hiking/charco-malo.jpg"),
      new BinResource($"{basePath}/images/what-to-do/iglesia-nuestra-senora-remedios.jpg"),
      new PageResource($"{basePath}/sitemap.xml"),
      new PageResource($"{basePath}/robots.txt")]
    ));

builder.Services.AddSingleton<IMarkdownService, MarkdownService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UsePathBase(basePath);
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Index-en",
    pattern: "en",
    defaults: new { lang = "en", controller = "Home", action = "Index" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Index-es",
    pattern: "es",
    defaults: new { lang = "es", controller = "Home", action = "Index" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "History-en",
    pattern: "{lang=en}/history",
    defaults: new { lang = "en", controller = "Home", action = "History" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "History-es",
    pattern: "{lang=es}/historia",
    defaults: new { lang = "es", controller = "Home", action = "History" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "WhatToDo-en",
    pattern: "{lang=en}/what-to-do",
    defaults: new { lang = "en", controller = "Home", action = "WhatToDo" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "WhatToDo-es",
    pattern: "{lang=es}/que-hacer",
    defaults: new { lang = "es", controller = "Home", action = "WhatToDo" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Hiking-en",
    pattern: "{lang=en}/hiking",
    defaults: new { lang = "en", controller = "Home", action = "Hiking" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Hiking-es",
    pattern: "{lang=es}/senderismo",
    defaults: new { lang = "es", controller = "Home", action = "Hiking" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Privacy-en",
    pattern: "{lang=en}/privacy",
    defaults: new { lang = "en", controller = "Home", action = "Privacy" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Privacy-es",
    pattern: "{lang=es}/privacidad",
    defaults: new { lang = "es", controller = "Home", action = "Privacy" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Cookies-en",
    pattern: "{lang=en}/cookies",
    defaults: new { lang = "en", controller = "Home", action = "Cookies" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "Cookies-es",
    pattern: "{lang=es}/cookies",
    defaults: new { lang = "es", controller = "Home", action = "Cookies" },
    constraints: new { lang = @"(\w{2})" });

app.MapControllerRoute(
    name: "default",
    pattern: "{lang=es}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { lang = @"(\w{2})" });

if (args.HasExitWhenDoneArg())
{
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
}

await app.RunAsync();
