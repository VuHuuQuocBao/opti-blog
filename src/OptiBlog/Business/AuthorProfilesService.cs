using EPiServer.Core.Routing;
using EPiServer.Core.Routing.Pipeline;
using EPiServer.Web;
using EPiServer.Web.Routing;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business;

public record AuthorRoute(AuthorProfileBlock Author, int Page);

public class AuthorProfilesService(IContentLoader contentLoader, IUrlResolver urlResolver, SiteSettingsService siteSettings)
{
    public const string UrlSegment = "author";

    public AuthorProfileBlock? FindBySlug(string slug) => All().FirstOrDefault(author => author.Slug == slug);

    public string? GetUrl(BlogPostPage post)
    {
        var author = All().FirstOrDefault(author => author.Name == post.Author);

        if (author?.Slug is not { } slug)
            return null;

        return GetUrl(post.ParentLink, slug);
    }

    public string GetUrl(ContentReference blogList, string slug, int page = 1)
    {
        var url = $"{urlResolver.GetUrl(blogList)}{UrlSegment}/{slug}/";

        if (page > 1)
            url += $"page-{page}/";

        return url;
    }

    private IEnumerable<AuthorProfileBlock> All() =>
        contentLoader.GetDescendents(AuthorFolder())
            .Select(contentLoader.Get<IContent>)
            .OfType<AuthorProfileBlock>();

    private ContentReference AuthorFolder()
    {
        var folder = siteSettings.Current?.AuthorProfileFolder;

        if (folder != null && !ContentReference.IsNullOrEmpty(folder))
            return folder;

        return SystemDefinition.Current.GlobalAssetsRoot;
    }
}

public class AuthorPartialRouter(AuthorProfilesService authors, IHttpContextAccessor httpContextAccessor) : IPartialRouter<BlogListPage, AuthorRoute>
{
    private const string PagePrefix = "page-";

    public object? RoutePartial(BlogListPage content, UrlResolverContext segmentContext)
    {
        var segments = segmentContext.RemainingSegments.ToString().Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments is not [AuthorProfilesService.UrlSegment, var slug, .. var rest]
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
            .AddSingleton<AuthorProfilesService>()
            .AddSingleton<IPartialRouter, AuthorPartialRouter>();
}
