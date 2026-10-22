using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using Optimizely.Graph.Cms.Query;
using OptiBlog.Business;
using OptiBlog.Models.Pages;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Controllers;

public class SearchPageController(IGraphContentClient graphClient, IContentLoader contentLoader) : PageController<SearchPage>
{
    public async Task<IActionResult> Index(SearchPage currentPage, [FromQuery] SiteSearchQuery search, CancellationToken cancellationToken)
    {
        var siteKey = contentLoader.Get<StartPage>(ContentReference.StartPage).ContentGuid.ToString("N");
        var result = await graphClient.SearchSiteAsync(siteKey, search, cancellationToken);

        return View(new SearchPageViewModel(currentPage, search, result));
    }
}
