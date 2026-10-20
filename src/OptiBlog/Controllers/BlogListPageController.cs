using EPiServer.Filters;
using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Business;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Controllers;

public class BlogListPageController(IContentLoader contentLoader) : PageController<BlogListPage>
{
    public IActionResult Index(BlogListPage currentPage, int page = 1)
    {
        var pageSize = currentPage.PageSize > 0 ? currentPage.PageSize : BlogListPage.DefaultPageSize;

        var posts = FilterForVisitor
            .Filter(contentLoader.GetChildren<BlogPostPage>(currentPage.ContentLink))
            .OfType<BlogPostPage>()
            .OrderByDescending(post => post.GetPublishDate())
            .ToList();

        var totalPages = Math.Max(1, (int)Math.Ceiling(posts.Count / (double)pageSize));
        var pageNumber = Math.Clamp(page, 1, totalPages);
        var pagePosts = posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return View(new BlogListPageViewModel(currentPage, pagePosts, pageNumber, totalPages));
    }
}
