using System.ComponentModel.DataAnnotations;
using EPiServer.Security;

namespace OptiBlog.Models;

[GroupDefinitions]
public static class GroupNames
{
    [Display(Name = "Post details", Order = 20)]
    public const string PostDetails = "PostDetails";

    [Display(Name = "Listing", Order = 20)]
    public const string Listing = "Listing";

    [Display(Name = "Media", Order = 30)]
    public const string Media = "Media";

    [Display(Name = "Related", Order = 40)]
    public const string Related = "Related";

    [Display(Name = "SEO", Order = 80)]
    public const string Seo = "SEO";

    [Display(Name = "Editorial", Order = 90)]
    [RequiredAccess(AccessLevel.Edit)]
    public const string Editorial = "Editorial";
}
