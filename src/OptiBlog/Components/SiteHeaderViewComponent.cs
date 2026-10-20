using EPiServer.Filters;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using OptiBlog.Models.ViewModels;

namespace OptiBlog.Components;

public class SiteHeaderViewComponent(IContentLoader contentLoader, IUrlResolver urlResolver) : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var startPageLink = ContentReference.StartPage;
        if (ContentReference.IsNullOrEmpty(startPageLink)
            || !contentLoader.TryGet<PageData>(startPageLink, out var startPage))
        {
            return Content(string.Empty);
        }

        var homePageUrl = urlResolver.GetUrl(startPage.ContentLink) ?? "/";

        var menu = FilterForVisitor
            .Filter(contentLoader.GetChildren<PageData>(startPageLink))
            .OfType<PageData>()
            .Where(page => page.VisibleInMenu)
            .Select(page => new MenuItem(page.Name, urlResolver.GetUrl(page.ContentLink)))
            .Prepend(new MenuItem(startPage.Name, homePageUrl))
            .ToList();

        return View(new SiteHeaderViewModel(homePageUrl, menu));
    }
}
