using System.Text.Json.Serialization;
using Optimizely.Graph.Cms.Query;
using Optimizely.Graph.Cms.Query.Abstractions;
using Optimizely.Graph.Cms.Query.Attributes;
using Optimizely.Graph.Cms.Query.Filtering;
using Optimizely.Graph.Cms.Query.Linq.Request;
using OptiBlog.Models.Media;
using OptiBlog.Models.Pages;
using static Optimizely.Graph.Cms.Query.Linq.Request.GraphQueryFilter;

namespace OptiBlog.Business;

public record SiteSearchQuery(string? Q, string? Type, int Page = 1);

public record SiteSearchHit(string Title, string Url, string? Excerpt);

public record SiteSearchResult(IReadOnlyList<SiteSearchHit> Hits, int Total);

public class _Content
{
    [GraphName("_metadata")]
    [JsonPropertyName("_metadata")]
    public SiteContentMetadata Metadata { get; set; } = new();
}

public class SiteContentMetadata
{
    [GraphName("key")]
    public string Key { get; set; } = string.Empty;

    [GraphName("displayName")]
    public string DisplayName { get; set; } = string.Empty;

    [GraphName("url")]
    public SiteContentUrl Url { get; set; } = new();

    [GraphName("types")]
    public IList<string> Types { get; set; } = [];

    [GraphName("path")]
    public IList<string> Path { get; set; } = [];
}

public class SiteContentUrl
{
    [GraphName("default")]
    public string Default { get; set; } = string.Empty;
}

public static class SiteSearchService
{
    public const int PageSize = 10;
    private const string PageType = "_Page";
    private const string PostType = nameof(BlogPostPage);
    private const string DocumentType = nameof(DocumentFile);
    private const int TitleBoost = 5;
    private const int SummaryBoost = 3;
    private const int ContentBoost = 1;
    private const int DocumentContentBoost = 2;

    public static async Task<SiteSearchResult> SearchSiteAsync(
        this IGraphContentClient graphClient,
        string siteKey,
        SiteSearchQuery search,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(search.Q))
        {
            return new SiteSearchResult([], 0);
        }

        var text = search.Q;
        var title = new GraphQlField<string>("_metadata.displayName");
        var fulltext = new GraphQlField<string>("_fulltext");
        var summaryKeys = await graphClient.FindSummaryMatchesAsync(text, cancellationToken);

        var query = graphClient.Query<_Content>()
            .SearchFor(text, "mark")
            .WithPinned()
            .WithExcerpts()
            .Where(content => content.Metadata.Path.ContainsAny(siteKey) | content.Metadata.Types.ContainsAny(DocumentType))
            .Where(content => Boost(title.Match(text), TitleBoost)
                || (Boost(fulltext.Match(text), ContentBoost) && !content.Metadata.Types.Contains(DocumentType))
                || (Boost(fulltext.Match(text), DocumentContentBoost) && content.Metadata.Types.Contains(DocumentType))
                || (Boost(fulltext.Match(text), SummaryBoost) && summaryKeys.Contains(content.Metadata.Key)));

        query = search.Type switch
        {
            "pages" => query.Where(content => content.Metadata.Types.ContainsAny(PageType) & !content.Metadata.Types.ContainsAny(PostType)),
            "posts" => query.Where(content => content.Metadata.Types.ContainsAny(PostType)),
            "documents" => query.Where(content => content.Metadata.Types.ContainsAny(DocumentType)),
            _ => query,
        };

        var result = await query
            .Skip((search.Page - 1) * PageSize)
            .Limit(PageSize)
            .IncludeTotal()
            .WithDisplayFilters()
            .Fields<_Content>(content => content.Metadata.DisplayName, content => content.Metadata.Url.Default)
            .GetAsync(cancellationToken);

        var hits = result.Items
            .Select(content => new SiteSearchHit(
                content.Metadata.DisplayName,
                content.Metadata.Url.Default,
                result.Excerpts?.TryGetValue(content, out var excerpts) == true ? excerpts[0] : null))
            .ToList();

        return new SiteSearchResult(hits, result.Total ?? 0);
    }

    // _Content has no Summary field, so find the posts whose summary matches; "none" keeps the key list non-empty.
    private static async Task<List<string>> FindSummaryMatchesAsync(this IGraphContentClient graphClient, string text, CancellationToken cancellationToken)
    {
        var posts = await graphClient.QueryContent<BlogPostPage>()
            .SearchFor(text)
            .UsingField(post => post.Summary!)
            .GetAsContentAsync(cancellationToken);

        return [.. posts.Select(post => post.ContentGuid.ToString("N")).DefaultIfEmpty("none")];
    }
}
