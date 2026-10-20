using System.ComponentModel.DataAnnotations;
using OptiBlog.Models.Pages;

namespace OptiBlog.Models.Blocks;

[ContentType(
    GUID = "c2e1166b-31bc-49ba-8c11-8b334c4a11b2",
    DisplayName = "Blog teaser",
    Description = "Promotes a single blog post.",
    GroupName = ContentTypeGroupNames.Blog,
    Order = 30)]
public class BlogTeaserBlock : BlockData
{
    [Required]
    [AllowedTypes(typeof(BlogPostPage))]
    [Display(
        Name = "Post",
        Description = "The blog post to promote.",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual ContentReference? Post { get; set; }

    [CultureSpecific]
    [StringLength(50)]
    [Display(
        Name = "Kicker",
        Description = "Short label shown above the title, for example \"Editor's pick\".",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual string? Kicker { get; set; }
}
