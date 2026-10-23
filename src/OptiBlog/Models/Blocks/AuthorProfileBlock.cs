using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace OptiBlog.Models.Blocks;

[ContentType(
    GUID = "f411f21c-3091-481b-8d27-f81f7f5f81ba",
    DisplayName = "Author profile",
    Description = "An author. The slug gives the profile page its URL: /blog/author/{slug}.",
    GroupName = ContentTypeGroupNames.Blog,
    Order = 40)]
public class AuthorProfileBlock : BlockData
{
    [Required]
    [RegularExpression("^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "Use lower-case letters, digits and hyphens, e.g. jane-doe.")]
    [Display(Name = "Slug", Description = "URL part, e.g. jane-doe.", GroupName = SystemTabNames.Content, Order = 10)]
    public virtual string? Slug { get; set; }

    [Required]
    [Display(Name = "Name", Description = "Must match the Author field on the author's blog posts.", GroupName = SystemTabNames.Content, Order = 20)]
    public virtual string? Name { get; set; }

    [UIHint(UIHint.Textarea)]
    [Display(Name = "Bio", GroupName = SystemTabNames.Content, Order = 30)]
    public virtual string? Bio { get; set; }
}
