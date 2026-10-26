using System.ComponentModel.DataAnnotations;
using EPiServer.Framework.Cache;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using OptiBlog.Models;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

[ContentType(GUID = "203aedd1-44cd-4b1e-b790-aac46a9522de", DisplayName = "Site settings", GroupName = ContentTypeGroupNames.Site, Order = 100)]
[AvailableContentTypes(Availability.None)]
public class SiteSettingsPage : SitePageData
{
    [Required]
    [Display(Name = "Site name", Order = 10)]
    public virtual string? SiteName { get; set; }

    [UIHint(UIHint.Block), AllowedTypes(typeof(HeaderBlock))]
    [Display(Name = "Default header", Order = 20)]
    public virtual ContentReference? DefaultHeader { get; set; }

    [UIHint(UIHint.Block), AllowedTypes(typeof(FooterBlock))]
    [Display(Name = "Default footer", Order = 30)]
    public virtual ContentReference? DefaultFooter { get; set; }

    [UIHint(UIHint.AssetsFolder)]
    [Display(Name = "Author profile folder", Order = 40)]
    public virtual ContentReference? AuthorProfileFolder { get; set; }
}

[ContentType(GUID = "d6c22fcb-f74e-464a-a728-b800cab8f258", DisplayName = "Header", GroupName = ContentTypeGroupNames.Site, Order = 10)]
public class HeaderBlock : BlockData
{
    [UIHint(UIHint.Image)]
    public virtual ContentReference? Logo { get; set; }

    [Display(Name = "Navigation links")]
    public virtual LinkItemCollection? NavigationLinks { get; set; }
}

[ContentType(GUID = "3a636a78-33b3-43ec-83b3-075333a46d8b", DisplayName = "Footer", GroupName = ContentTypeGroupNames.Site, Order = 20)]
public class FooterBlock : BlockData
{
    [CultureSpecific]
    [Display(Name = "Copyright text")]
    public virtual string? CopyrightText { get; set; }
}

public interface IHeaderFooterConfigurable
{
    HeaderFooterSettings? HeaderFooter { get; }
}

[ContentType(GUID = "d43743e9-fdc5-4a87-be2d-3cc635ed8953", DisplayName = "Header and footer settings", AvailableInEditMode = false)]
public class HeaderFooterSettings : BlockData
{
    [Display(Name = "Hide header", Order = 10)]
    public virtual bool HideHeader { get; set; }

    [UIHint(UIHint.Block), AllowedTypes(typeof(HeaderBlock))]
    [Display(Name = "Header override", Order = 20)]
    public virtual ContentReference? HeaderOverride { get; set; }

    [Display(Name = "Hide footer", Order = 30)]
    public virtual bool HideFooter { get; set; }

    [UIHint(UIHint.Block), AllowedTypes(typeof(FooterBlock))]
    [Display(Name = "Footer override", Order = 40)]
    public virtual ContentReference? FooterOverride { get; set; }
}

public class SiteSettings(
    IContentLoader contentLoader,
    IContentEvents contentEvents,
    ISynchronizedObjectInstanceCache cache,
    IContentCacheKeyCreator cacheKeys) : IHostedService
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

    public Task StartAsync(CancellationToken cancellationToken)
    {
        contentEvents.CreatingContent += PreventSecondSettingsPage;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        contentEvents.CreatingContent -= PreventSecondSettingsPage;
        return Task.CompletedTask;
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

    private void PreventSecondSettingsPage(object? sender, ContentEventArgs e)
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
