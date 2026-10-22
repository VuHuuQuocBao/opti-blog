using System.Net;
using System.Text.RegularExpressions;
using EPiServer.DataAbstraction;
using Optimizely.Graph.DependencyInjection;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

public static class BlogPostIndexing
{
    private const int WordsPerMinute = 200;
    private static readonly Regex HtmlTag = new("<[^>]+>", RegexOptions.Compiled);

    public static string PlainTextBody(this BlogPostPage post)
    {
        var html = post.MainBody?.ToInternalString() ?? string.Empty;
        var textWithoutTags = HtmlTag.Replace(html, " ");
        var decodedText = WebUtility.HtmlDecode(textWithoutTags);

        return decodedText.Trim();
    }

    public static int ReadingTime(this BlogPostPage post)
    {
        var words = post.PlainTextBody().Split([' ', '\n', '\r', '\t'], StringSplitOptions.RemoveEmptyEntries);
        var minutes = words.Length / WordsPerMinute;

        return Math.Max(1, minutes);
    }

    public static int AgeInDays(this BlogPostPage post) =>
        (DateTime.Today - post.GetPublishDate().Date).Days;

    public static IServiceCollection AddBlogGraphIndexing(this IServiceCollection services)
    {
        services.ConfigureGraphConventions(conventions =>
        {
            conventions.ExcludeContentType<ImageData>();
            conventions.ExcludeContentType<VideoData>();
            conventions.ExcludeContentType<SearchPage>();

            var posts = conventions.ForInstancesOf<BlogPostPage>();
            posts.IncludeField(post => post.ReadingTime(), IndexingType.Queryable);
            posts.IncludeField(post => post.PlainTextBody(), IndexingType.Searchable);
            posts.IncludeField(post => post.AgeInDays(), IndexingType.Queryable);
        });

        return services;
    }
}

