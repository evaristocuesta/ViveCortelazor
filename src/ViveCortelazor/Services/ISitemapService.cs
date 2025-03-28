using System.Xml.Linq;

namespace ViveCortelazor.Services;

public interface ISitemapService
{
    XDocument GetSitemap();
}
