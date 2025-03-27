using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace ViveCortelazor.Extensions;

public static class UrlExtensions
{
    public static string Canonical(this IUrlHelper urlHelper, RouteData routeData, string? language)
    {
        var routeParams = new RouteValueDictionary(routeData.Values)
        {
            ["lang"] = language
        };

        var url = $"https://www.vivecortelazor.es{urlHelper.RouteUrl(routeParams)}";
        return url;
    }
}
