using EPiServer.Framework.Cache;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

public class SiteSettingsService(
    IContentLoader contentLoader,
    ISynchronizedObjectInstanceCache cache,
    IContentCacheKeyCreator cacheKeys)
{
    public SiteSettingsPage? Current
    {
        get
        {
            var startPage = ContentReference.StartPage;
            var cacheKey = $"OptiBlog:SiteSettings:{startPage.ID}";

            return cache.ReadThrough(
                cacheKey,
                () => FindPublishedSettings(startPage),
                () => new CacheEvictionPolicy([], [cacheKeys.VersionKey]));
        }
    }

    public HeaderBlock? GetHeader(IContent? page)
    {
        var options = GetOptions(page);
        if (options?.HideHeader == true)
        {
            return null;
        }

        return LoadBlock<HeaderBlock>(options?.HeaderOverride, Current?.DefaultHeader);
    }

    public FooterBlock? GetFooter(IContent? page)
    {
        var options = GetOptions(page);
        if (options?.HideFooter == true)
        {
            return null;
        }

        return LoadBlock<FooterBlock>(options?.FooterOverride, Current?.DefaultFooter);
    }

    private SiteSettingsPage? FindPublishedSettings(ContentReference startPage)
    {
        return contentLoader
            .GetChildren<SiteSettingsPage>(startPage)
            .FirstOrDefault(settings => settings.CheckPublishedStatus(PagePublishedStatus.Published));
    }

    private static HeaderFooterSettings? GetOptions(IContent? page)
    {
        var configurablePage = page as IHeaderFooterConfigurable;
        return configurablePage?.HeaderFooter;
    }

    private T? LoadBlock<T>(ContentReference? pageOverride, ContentReference? siteDefault) where T : BlockData
    {
        var blockLink = ContentReference.IsNullOrEmpty(pageOverride) ? siteDefault : pageOverride;
        if (ContentReference.IsNullOrEmpty(blockLink))
        {
            return null;
        }

        return contentLoader.TryGet<T>(blockLink, out var block) ? block : null;
    }

    public void PreventSecondSettingsPage(object? sender, ContentEventArgs e)
    {
        if (e.Content is not SiteSettingsPage newSettings)
        {
            return;
        }

        var settingsAlreadyExist = contentLoader.GetChildren<SiteSettingsPage>(newSettings.ParentLink).Any();
        if (settingsAlreadyExist)
        {
            e.CancelAction = true;
            e.CancelReason = "This site already has a Site settings page.";
        }
    }
}
