using EPiServer.Web;
using EPiServer.Web.Mvc.Html;

namespace OptiBlog.Business.Rendering;

public static class DisplayOptionTags
{
    public const string Full = "full";
    public const string Half = "half";
    public const string Card = "card";
}

public static class ContentAreaServiceCollectionExtensions
{
    public static IServiceCollection AddBlogDisplayOptions(this IServiceCollection services) =>
        services
            .Configure<DisplayOptions>(options => options
                .Add(DisplayOptionTags.Full, "Full width", DisplayOptionTags.Full)
                .Add(DisplayOptionTags.Half, "Half width", DisplayOptionTags.Half)
                .Add(DisplayOptionTags.Card, "Card", DisplayOptionTags.Card))
            .AddTransient<ContentAreaRenderer, BlogContentAreaRenderer>();
}
