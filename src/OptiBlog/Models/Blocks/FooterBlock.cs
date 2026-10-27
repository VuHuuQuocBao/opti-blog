using System.ComponentModel.DataAnnotations;

namespace OptiBlog.Models.Blocks;

[ContentType(
    GUID = "3a636a78-33b3-43ec-83b3-075333a46d8b",
    DisplayName = "Footer",
    Description = "Site footer.",
    GroupName = ContentTypeGroupNames.Site,
    Order = 20)]
public class FooterBlock : BlockData
{
    [CultureSpecific]
    [Display(
        Name = "Copyright text",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string? CopyrightText { get; set; }
}
