using EPiServer.Core.Routing;
using EPiServer.Core.Routing.Pipeline;
using EPiServer.Web;
using EPiServer.Web.Routing;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

public record AuthorRoute(AuthorProfileBlock Author, int Page);

public class AuthorProfiles(IContentLoader contentLoader, IUrlResolver urlResolver, SiteSettings siteSettings)
{
    public const string UrlSegment = "author";

    public AuthorProfileBlock? FindBySlug(string slug) => All().FirstOrDefault(author => author.Slug == slug);

    public string? GetUrl(BlogPostPage post) =>
        All().FirstOrDefault(author => author.Name == post.Author) is { Slug: { } slug } ? GetUrl(post.ParentLink, slug) : null;

    public string GetUrl(ContentReference blogList, string slug, int page = 1) =>
        $"{urlResolver.GetUrl(blogList)}{UrlSegment}/{slug}/{(page > 1 ? $"page-{page}/" : string.Empty)}";

    private IEnumerable<AuthorProfileBlock> All() =>
        contentLoader.GetDescendents(AuthorFolder())
            .Select(contentLoader.Get<IContent>)
            .OfType<AuthorProfileBlock>();

    private ContentReference AuthorFolder() =>
        siteSettings.Current?.AuthorProfileFolder is { } folder && !ContentReference.IsNullOrEmpty(folder)
            ? folder
            : SystemDefinition.Current.GlobalAssetsRoot;
}

public class AuthorPartialRouter(AuthorProfiles authors, IHttpContextAccessor httpContextAccessor) : IPartialRouter<BlogListPage, AuthorRoute>
{
    private const string PagePrefix = "page-";

    public object? RoutePartial(BlogListPage content, UrlResolverContext segmentContext)
    {
        var segments = segmentContext.RemainingSegments.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments is not [AuthorProfiles.UrlSegment, var slug, .. var rest]
            || authors.FindBySlug(slug) is not { } author
            || ParsePage(rest) is not { } page)
        {
            return null;
        }

        httpContextAccessor.HttpContext!.Items[typeof(AuthorRoute)] = new AuthorRoute(author, page);
        segmentContext.RemainingSegments = ReadOnlyMemory<char>.Empty;
        return content;
    }

    public PartialRouteData? GetPartialVirtualPath(AuthorRoute content, UrlGeneratorContext urlGeneratorContext) => null;

    private static int? ParsePage(string[] rest) => rest switch
    {
        [] => 1,
        [var segment] when segment.StartsWith(PagePrefix) && int.TryParse(segment[PagePrefix.Length..], out var page) && page > 1 => page,
        _ => null,
    };
}

public static class AuthorProfilesServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorProfiles(this IServiceCollection services) =>
        services
            .AddHttpContextAccessor()
            .AddSingleton<AuthorProfiles>()
            .AddSingleton<IPartialRouter, AuthorPartialRouter>();
}
