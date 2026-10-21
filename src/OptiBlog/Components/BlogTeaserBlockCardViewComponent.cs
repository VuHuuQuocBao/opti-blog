using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Business.Rendering;
using OptiBlog.Models.Blocks;

namespace OptiBlog.Components;

[TemplateDescriptor(Tags = [DisplayOptionTags.Card], AvailableWithoutTag = false)]
public class BlogTeaserBlockCardViewComponent(IContentLoader contentLoader) : BlockComponent<BlogTeaserBlock>
{
    protected override IViewComponentResult InvokeComponent(BlogTeaserBlock currentContent) =>
        BlogTeaserBlockViewComponent.CreateViewModel(contentLoader, currentContent) is { } model
            ? View(model)
            : Content(string.Empty);
}
