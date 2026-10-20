using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Components;

public class BlogPostPageViewComponent : PartialContentComponent<BlogPostPage>
{
    protected override IViewComponentResult InvokeComponent(BlogPostPage currentContent) =>
        View(new BlogTeaserViewModel(currentContent));
}
