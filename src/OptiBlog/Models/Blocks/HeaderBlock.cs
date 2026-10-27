using System.ComponentModel.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;

namespace OptiBlog.Models.Blocks;

[ContentType(
    GUID = "d6c22fcb-f74e-464a-a728-b800cab8f258",
    DisplayName = "Header",
    Description = "Site header with logo and navigation.",
    GroupName = ContentTypeGroupNames.Site,
    Order = 10)]
public class HeaderBlock : BlockData
{
    [UIHint(UIHint.Image)]
    [Display(
        Name = "Logo",
        Description = "Shown instead of the site name when set.",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual ContentReference? Logo { get; set; }

    [Display(
        Name = "Navigation links",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual LinkItemCollection? NavigationLinks { get; set; }
}
