using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Components;

public class BlogTeaserBlockViewComponent(IContentLoader contentLoader) : BlockComponent<BlogTeaserBlock>
{
    protected override IViewComponentResult InvokeComponent(BlogTeaserBlock currentContent) =>
        CreateViewModel(contentLoader, currentContent) is { } model ? View(BlogTeaserViewModel.ViewPath, model) : Content(string.Empty);

    internal static BlogTeaserViewModel? CreateViewModel(IContentLoader contentLoader, BlogTeaserBlock block)
    {
        if (ContentReference.IsNullOrEmpty(block.Post)
            || !contentLoader.TryGet<BlogPostPage>(block.Post, out var post)
            || !FilterForVisitor.Filter([post]).Any())
        {
            return null;
        }

        return new BlogTeaserViewModel(post, block.Kicker);
    }
}
