using OptiBlog.Models.Pages;

namespace OptiBlog.Models.ViewModels;

public record BlogTeaserViewModel(BlogPostPage Post, string? Kicker = null)
{
    public const string ViewPath = "~/Views/Shared/_Teaser.cshtml";
}
