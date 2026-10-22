using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Optimizely.Graph.Cms.Query;
using OptiBlog.Business;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Controllers;

public class BlogListPageController(IGraphContentClient graphClient) : PageController<BlogListPage>
{
    public async Task<IActionResult> Index(BlogListPage currentPage, [FromQuery] BlogSearchQuery search, CancellationToken cancellationToken)
    {
        var result = await graphClient.SearchBlogPostsAsync(currentPage, search, cancellationToken);

        return View(new BlogListPageViewModel(currentPage, search, result));
    }
}
