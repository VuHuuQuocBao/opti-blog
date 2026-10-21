using EPiServer.Cms.TinyMce.Core;

namespace OptiBlog.Business.Editor;

public static class TinyMceServiceCollectionExtensions
{
    private const string DefaultToolbarAdditions = "blockquote styles";
    private const string EditorContentCss = "/blog-landing-page.css";
    private static readonly object[] CustomStyleFormats =
    [
        new { title = "Lead Paragraph", selector = "p", classes = "lead" },
        new { title = "Primary Button", selector = "a", attributes = new { @class = "btn btn-primary" } },
        new { title = "Secondary Button", selector = "a", attributes = new { @class = "btn btn-secondary" } },
        new { title = "Pull Quote", selector = "blockquote", classes = "pull-quote" },
    ];

    public static IServiceCollection AddCustomTinyMceStyles(this IServiceCollection services) =>
        services.Configure<TinyMceConfiguration>(config => config.Default()
            .AppendToolbar(DefaultToolbarAdditions)
            .StyleFormats(CustomStyleFormats)
            .ContentCss(EditorContentCss));
}
