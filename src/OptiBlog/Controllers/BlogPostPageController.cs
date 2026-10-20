using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.Pages;

namespace OptiBlog.Controllers;

public class BlogPostPageController : PageController<BlogPostPage>
{
    public IActionResult Index(BlogPostPage currentPage) => View(currentPage);
}
