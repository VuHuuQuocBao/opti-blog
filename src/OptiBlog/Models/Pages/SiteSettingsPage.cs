using System.ComponentModel.DataAnnotations;
using EPiServer.Web;
using OptiBlog.Models.Blocks;

namespace OptiBlog.Models.Pages;

[ContentType(
    GUID = "203aedd1-44cd-4b1e-b790-aac46a9522de",
    DisplayName = "Site settings",
    Description = "Settings for the whole site. Only one is allowed, directly under the start page.",
    GroupName = ContentTypeGroupNames.Site,
    Order = 100)]
[AvailableContentTypes(Availability.None)]
public class SiteSettingsPage : SitePageData
{
    [Required]
    [Display(
        Name = "Site name",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string? SiteName { get; set; }

    [UIHint(UIHint.Block)]
    [AllowedTypes(typeof(HeaderBlock))]
    [Display(
        Name = "Default header",
        Description = "Header used by pages that inherit it.",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual ContentReference? DefaultHeader { get; set; }

    [UIHint(UIHint.Block)]
    [AllowedTypes(typeof(FooterBlock))]
    [Display(
        Name = "Default footer",
        Description = "Footer used by pages that inherit it.",
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual ContentReference? DefaultFooter { get; set; }

    [UIHint(UIHint.AssetsFolder)]
    [Display(
        Name = "Author profile folder",
        Description = "The assets folder containing all author profile blocks.",
        GroupName = SystemTabNames.Content,
        Order = 40)]
    public virtual ContentReference? AuthorProfileFolder { get; set; }
}
