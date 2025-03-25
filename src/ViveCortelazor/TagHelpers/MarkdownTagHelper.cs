using Microsoft.AspNetCore.Razor.TagHelpers;
using ViveCortelazor.Services;

namespace ViveCortelazor.TagHelpers;

public class MarkdownTagHelper : TagHelper
{
    private readonly IMarkdownService _markdownService;

    public MarkdownTagHelper(IMarkdownService markdownService)
    {
        _markdownService = markdownService;
    }

    public string Content { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = null;
        output.Content.SetHtmlContent(_markdownService.ToHtml(Content));
    }
}
