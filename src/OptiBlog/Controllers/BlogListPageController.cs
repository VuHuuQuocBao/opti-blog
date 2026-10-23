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
        if (HttpContext.Items[typeof(AuthorRoute)] is AuthorRoute authorRoute)
        {
            return await Author(currentPage, authorRoute, cancellationToken);
        }

        var result = await graphClient.SearchBlogPostsAsync(currentPage, search, cancellationToken);

        return View(new BlogListPageViewModel(currentPage, search, result));
    }

    private async Task<IActionResult> Author(BlogListPage currentPage, AuthorRoute route, CancellationToken cancellationToken)
    {
        var search = new BlogSearchQuery(null, null, null, false, true, route.Page, route.Author.Name);
        var result = await graphClient.SearchBlogPostsAsync(currentPage, search, cancellationToken);
        var model = new AuthorPageViewModel(currentPage, route.Author, result, route.Page);

        return route.Page > 1 && route.Page > model.TotalPages ? NotFound() : View(nameof(Author), model);
    }
}
