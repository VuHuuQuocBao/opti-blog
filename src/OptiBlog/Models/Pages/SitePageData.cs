using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace OptiBlog.Models.Pages;

public abstract class SitePageData : PageData
{
    [CultureSpecific]
    [StringLength(70)]
    [Display(
        Name = "Meta title",
        Description = "Title shown in the browser tab and search engines. Falls back to the page name.",
        GroupName = GroupNames.Seo,
        Order = 10)]
    public virtual string? MetaTitle { get; set; }

    [CultureSpecific]
    [StringLength(160)]
    [UIHint(UIHint.Textarea)]
    [Display(
        Name = "Meta description",
        Description = "Short description for search engines.",
        GroupName = GroupNames.Seo,
        Order = 20)]
    public virtual string? MetaDescription { get; set; }
}
