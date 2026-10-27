using System.ComponentModel.DataAnnotations;
using EPiServer.Web;
using OptiBlog.Models.Blocks;

namespace OptiBlog.Models.Pages;

[ContentType(
    GUID = "e40a2637-b6e0-4240-8cee-8d7b80c31eb2",
    DisplayName = "Blog post",
    Description = "A single blog post. Can only be created under a blog list page.",
    GroupName = ContentTypeGroupNames.Blog,
    Order = 20)]
[AvailableContentTypes(Availability.None)]
public class BlogPostPage : SitePageData, IHeaderFooterConfigurable
{
    [Display(Name = "Header and footer", GroupName = SystemTabNames.Settings)]
    public virtual HeaderFooterSettings? HeaderFooter { get; set; }

    [CultureSpecific]
    [Required]
    [StringLength(150)]
    [Display(
        Name = "Title",
        Description = "Post title shown as the main heading.",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    [IndexingType(IndexingType.Searchable)]
    public virtual string? Title { get; set; }

    [CultureSpecific]
    [StringLength(300)]
    [UIHint(UIHint.Textarea)]
    [Display(
        Name = "Summary",
        Description = "Plain text summary used in listings and teasers. Maximum 300 characters.",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    [IndexingType(IndexingType.Searchable)]
    public virtual string? Summary { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Main body",
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual XhtmlString? MainBody { get; set; }

    [Display(
        Name = "Publish date",
        Description = "Date shown on the post and used to order the blog list.",
        GroupName = GroupNames.PostDetails,
        Order = 10)]
    [IndexingType(IndexingType.Queryable)]
    public virtual DateTime? PublishDate { get; set; }

    [Display(
        Name = "Author",
        Description = "Name of the author.",
        GroupName = GroupNames.PostDetails,
        Order = 20)]
    [IndexingType(IndexingType.Queryable)]
    public virtual string? Author { get; set; }

    [Display(
        Name = "Tags",
        Description = "Keywords for the post.",
        GroupName = GroupNames.PostDetails,
        Order = 30)]
    [IndexingType(IndexingType.Queryable)]
    public virtual IList<string>? Tags { get; set; }

    [UIHint(UIHint.Image)]
    [Display(
        Name = "Hero image",
        Description = "Large image shown at the top of the post.",
        GroupName = GroupNames.Media,
        Order = 10)]
    public virtual ContentReference? HeroImage { get; set; }

    [AllowedTypes(typeof(BlogPostPage), typeof(BlogTeaserBlock))]
    [Display(
        Name = "Related posts",
        Description = "Other posts shown below this one.",
        GroupName = GroupNames.Related,
        Order = 10)]
    public virtual ContentArea? RelatedPosts { get; set; }

    [UIHint(UIHint.Textarea)]
    [Display(
        Name = "Internal notes",
        Description = "Notes for editors. Never shown on the website.",
        GroupName = GroupNames.Editorial,
        Order = 10)]
    public virtual string? InternalNotes { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);
        PublishDate = DateTime.Now;
    }
}
