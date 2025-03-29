using Microsoft.AspNetCore.Mvc;

namespace ViveCortelazor.Extensions;

public static class UrlExtensions
{
    public static string Canonical(this IUrlHelper urlHelper, string host, RouteData routeData, string? language)
    {
        var routeParams = new RouteValueDictionary(routeData.Values)
        {
            ["lang"] = language
        };

        var url = $"{host}{urlHelper.RouteUrl(routeParams)}";
        return url;
    }
}
