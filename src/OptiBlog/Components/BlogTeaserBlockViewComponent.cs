using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Components;

public class BlogTeaserBlockViewComponent(IContentLoader contentLoader) : BlockComponent<BlogTeaserBlock>
{
    protected override IViewComponentResult InvokeComponent(BlogTeaserBlock currentContent)
    {
        if (ContentReference.IsNullOrEmpty(currentContent.Post)
            || !contentLoader.TryGet<BlogPostPage>(currentContent.Post, out var post)
            || !FilterForVisitor.Filter([post]).Any())
        {
            return Content(string.Empty);
        }

        return View(new BlogTeaserViewModel(post, currentContent.Kicker));
    }
}
