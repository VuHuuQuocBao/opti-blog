using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Templating;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OptiBlog.Business.Rendering;

public class BlogContentAreaRenderer(
    IContentRenderer contentRenderer,
    ITemplateResolver templateResolver,
    IContentAreaItemAttributeAssembler attributeAssembler,
    IContentRepository contentRepository,
    IContentAreaLoader contentAreaLoader,
    IContextModeResolver contextModeResolver,
    ContentAreaRenderingOptions renderingOptions,
    IModelMetadataProvider metadataProvider,
    IModelTemplateTagResolver templateTagResolver,
    IPrincipalAccessor principalAccessor,
    IEnumerable<IContentAreaItemsRenderingFilter> renderingFilters)
    : ContentAreaRenderer(
        contentRenderer,
        templateResolver,
        attributeAssembler,
        contentRepository,
        contentAreaLoader,
        contextModeResolver,
        renderingOptions,
        metadataProvider,
        templateTagResolver,
        principalAccessor,
        renderingFilters)
{
    private const string ItemModifierPrefix = "content-item--";

    protected override string GetContentAreaItemCssClass(IHtmlHelper htmlHelper, ContentAreaItem contentAreaItem)
    {
        var modifier = ItemModifierPrefix + ToLayoutTag(GetContentAreaItemTemplateTag(htmlHelper, contentAreaItem));
        var baseCssClass = base.GetContentAreaItemCssClass(htmlHelper, contentAreaItem);

        return string.IsNullOrWhiteSpace(baseCssClass) ? modifier : $"{baseCssClass} {modifier}";
    }

    private static string ToLayoutTag(string? templateTag) => templateTag switch
    {
        DisplayOptionTags.Half or DisplayOptionTags.Card => templateTag,
        _ => DisplayOptionTags.Full,
    };
}
