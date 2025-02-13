using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace ViveCortelazor.Pipelines;

public class CustomListRenderer : HtmlObjectRenderer<ListBlock>
{
    protected override void Write(HtmlRenderer renderer, ListBlock obj)
    {
        if (renderer.EnableHtmlForBlock)
        {
            string tag = obj.IsOrdered ? "ol" : "ul";
            renderer.Write($"<{tag} class=\"list-group\">").WriteLine();
            var savedImplicitParagraph = renderer.ImplicitParagraph;
            renderer.ImplicitParagraph = false;

            foreach (var item in obj)
            {
                var listItem = (ListItemBlock)item;
                renderer.Write("<li class=\"list-group-item\">").WriteLine();
                renderer.WriteChildren(listItem);
                renderer.Write("</li>").WriteLine();
            }

            renderer.ImplicitParagraph = savedImplicitParagraph;
            renderer.Write($"</{tag}>").WriteLine();
        }
        else
        {
            renderer.WriteChildren(obj);
        }
    }
}
