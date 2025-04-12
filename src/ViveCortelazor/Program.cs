using AspNetStatic;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using ViveCortelazor.Extensions;
using ViveCortelazor.Options;
using ViveCortelazor.Pipelines;
using ViveCortelazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<WebSettings>(
    builder.Configuration.GetSection("WebSettings"));

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
builder.Services.AddSingleton<ISitemapService, SitemapService>();

StaticWebAssetsLoader.UseStaticWebAssets(
    builder.Environment,
    builder.Configuration);

var app = builder.Build();

Console.WriteLine($"ENV: {app.Environment.EnvironmentName}");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/es/error/{0}");

app.UseHttpsRedirection();
app.MapStaticAssets();

app.UsePathBase(basePath);
app.UseRouting();

app.UseAuthorization();

app.AddControllerRoutes();

if (args.HasExitWhenDoneArg())
{
    app.ConfigureAspNetStatic(basePath, outputPath);
    app.GenerateStaticContent(outputPath);
}

await app.RunAsync();
