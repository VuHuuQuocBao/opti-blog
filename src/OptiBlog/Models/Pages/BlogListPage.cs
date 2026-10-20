using System.ComponentModel.DataAnnotations;

namespace OptiBlog.Models.Pages;

[ContentType(
    GUID = "e348c32e-cf38-4302-8d4f-e24f9f9050e0",
    DisplayName = "Blog list page",
    Description = "Lists the blog posts placed under it, newest first.",
    GroupName = ContentTypeGroupNames.Blog,
    Order = 10)]
[AvailableContentTypes(Availability.Specific, Include = [typeof(BlogPostPage)])]
public class BlogListPage : SitePageData
{
    public const int DefaultPageSize = 10;

    [CultureSpecific]
    [Display(
        Name = "Heading",
        Description = "Main heading. Falls back to the page name.",
        GroupName = SystemTabNames.Content,
        Order = 10)]
    public virtual string? Heading { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Intro",
        Description = "Short introduction shown above the post list.",
        GroupName = SystemTabNames.Content,
        Order = 20)]
    public virtual XhtmlString? Intro { get; set; }

    [CultureSpecific]
    [Display(
        Name = "Main content area",
        GroupName = SystemTabNames.Content,
        Order = 30)]
    public virtual ContentArea? MainContentArea { get; set; }

    [Range(1, 100)]
    [Display(
        Name = "Posts per page",
        Description = "How many posts to show on each page of the list.",
        GroupName = GroupNames.Listing,
        Order = 10)]
    public virtual int PageSize { get; set; }

    public override void SetDefaultValues(ContentType contentType)
    {
        base.SetDefaultValues(contentType);
        PageSize = DefaultPageSize;
    }
}
