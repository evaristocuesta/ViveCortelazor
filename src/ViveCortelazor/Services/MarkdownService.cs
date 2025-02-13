using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using ViveCortelazor.Pipelines;

namespace ViveCortelazor.Services;

public class MarkdownService : IMarkdownService
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownService()
    {
        var builder = new MarkdownPipelineBuilder()
            .UseAdvancedExtensions()
            .UseBootstrap();

        _pipeline = builder.Build();

        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        _pipeline.Setup(renderer);

        //renderer.ObjectRenderers.RemoveAll(r => r is ListRenderer);
        //renderer.ObjectRenderers.Add(new CustomListRenderer());
    }

    public string ToHtml(string markdown)
    {
        var writer = new StringWriter();
        var renderer = new HtmlRenderer(writer);
        _pipeline.Setup(renderer);

        //if (!renderer.ObjectRenderers.Any(r => r is CustomListRenderer))
        //{
        //    renderer.ObjectRenderers.RemoveAll(r => r is ListRenderer);
        //    renderer.ObjectRenderers.Add(new CustomListRenderer());
        //}

        var document = Markdown.Parse(markdown, _pipeline);
        renderer.Render(document);
        return writer.ToString();
    }
}
