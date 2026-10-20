using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

public static class BlogPostPageExtensions
{
    public static DateTime GetPublishDate(this BlogPostPage post) =>
        post.PublishDate ?? post.StartPublish ?? post.Created;

    public static string GetTitle(this BlogPostPage post) =>
        string.IsNullOrWhiteSpace(post.Title) ? post.Name : post.Title;
}
