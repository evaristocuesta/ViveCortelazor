using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using ViveCortelazor.Pipelines;
using AspNetStatic;
using ViveCortelazor.Services;
using ViveCortelazor.Extensions;

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

if (args.HasExitWhenDoneArg())
{
    builder.Services.AddSingleton<IStaticResourcesInfoProvider>(new StaticResourcesInfoProvider());
}

builder.Services.AddSingleton<IMarkdownService, MarkdownService>();
builder.Services.AddSingleton<IContentService, ContentService>();

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

app.AddControllerRoutes();

if (args.HasExitWhenDoneArg())
{
    app.ConfigureAspNetStatic(basePath, outputPath);

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
