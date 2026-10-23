using Optimizely.Graph.Cms.Query;
using Optimizely.Graph.Cms.Query.Abstractions;
using Optimizely.Graph.Cms.Query.Implementation;
using OptiBlog.Models.Pages;
using static Optimizely.Graph.Cms.Query.Linq.Request.GraphQueryFilter;

namespace OptiBlog.Business;

public record BlogSearchQuery(string? Q, string? Tag, string? Month, bool Quick, bool Newest, int Page = 1, string? Author = null);

public record BlogSearchResult(IReadOnlyList<BlogPostPage> Posts, int Total, List<FacetValue> Tags, List<FacetValue> Months);

public static class BlogSearch
{
    public const int QuickReadMinutes = 3;
    private const int RecentDays = 90;
    private const int RecentBoost = 5;
    private const string FeaturedTag = "optimizely";
    private const int FeaturedTagBoost = 3;

    public static async Task<BlogSearchResult> SearchBlogPostsAsync(
        this IGraphContentClient graphClient,
        BlogListPage blogList,
        BlogSearchQuery search,
        CancellationToken cancellationToken)
    {
        var blogListId = blogList.ContentLink.ID;
        var posts = graphClient.QueryContent<BlogPostPage>();
        IContentQuery<BlogPostPage> query = string.IsNullOrWhiteSpace(search.Q)
            ? posts
            : posts.SearchFor(search.Q)
                .UsingField(post => post.Title!)
                .UsingField(post => post.Summary!)
                .UsingField(post => post.PlainTextBody())
                .WithPinned();

        query = query.Where(post => post.BlogListId() == blogListId);

        query = query.Where(post =>
            Boost(post.AgeInDays() <= RecentDays, RecentBoost)
            || Boost(post.Tags!.Contains(FeaturedTag), FeaturedTagBoost)
            || post.BlogListId() == blogListId);

        if (search.Author is { } author)
        {
            query = query.Where(post => post.Author == author);
        }

        if (search.Quick)
            query = query.Where(post => post.ReadingTime() <= QuickReadMinutes);

        var faceted = query
            .Facet(post => post.Tags!, search.Tag is null ? null : [search.Tag])
            .Facet(post => post.PublishMonth(), search.Month is null ? null : [search.Month]);

        if (search.Newest || string.IsNullOrWhiteSpace(search.Q))
            faceted = faceted.OrderBy(post => post.PublishDate, OrderDirection.Descending);

        var result = await faceted
            .Skip((search.Page - 1) * blogList.PageSize)
            .Limit(blogList.PageSize)
            .IncludeTotal()
            .WithDisplayFilters()
            .GetAsContentAsync(cancellationToken);

        return new BlogSearchResult(
            result.ToList(),
            result.Total ?? 0,
            result.Facets!.GetFacet(post => post.Tags).ToList(),
            result.Facets.GetFacet(post => post.PublishMonth()).OrderByDescending(month => month.Name).ToList());
    }
}
