﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace ViveCortelazor.Extensions;

public static class UrlExtensions
{
    public static string? RouteUrlLang(this IUrlHelper urlHelper, string? routeName, string? language)
    {
        if (string.IsNullOrEmpty(routeName))
        {
            return urlHelper.RouteUrl("default");
        }

        string? url = urlHelper.RouteUrl($"{routeName}-{language}", new { lang = language });
        return url;
    }

    public static string RouteUrlLang(this IUrlHelper urlHelper, string? routeName, ViewContext viewContext)
    {
        var lang = viewContext.RouteData.Values["lang"]?.ToString();
        var url = RouteUrlLang(urlHelper, routeName, lang);

        if (string.IsNullOrEmpty(url))
        {
            return string.Empty;
        }

        return url;
    }

    public static string Canonical(this IUrlHelper urlHelper, string? routeName, string? language)
    {
        var url = $"https://www.vivecortelazor.es{RouteUrlLang(urlHelper, routeName, language)}";
        return url;
    }
}
