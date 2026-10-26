using System.ComponentModel.DataAnnotations;
using OptiBlog.Business;

namespace OptiBlog.Models.Pages;

[ContentType(
    GUID = "7b759cca-15ff-465a-ac59-bd06e0c90561",
    DisplayName = "Start page",
    Description = "The home page of a site.",
    GroupName = ContentTypeGroupNames.Site,
    Order = 10)]
[AvailableContentTypes(Availability.Specific, Include = [typeof(BlogListPage), typeof(SiteSettingsPage)])]
public class StartPage : SitePageData, IHeaderFooterConfigurable
{
    [Display(Name = "Header and footer", GroupName = SystemTabNames.Settings)]
    public virtual HeaderFooterSettings? HeaderFooter { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Heading",
        Description = "Main heading. Falls back to the page name.",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string? Heading { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Main content area",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual ContentArea? MainContentArea { get; set; }
}
