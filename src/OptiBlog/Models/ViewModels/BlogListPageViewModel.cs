using OptiBlog.Models.Pages;

namespace OptiBlog.Models.ViewModels;

public record BlogListPageViewModel(
    BlogListPage CurrentPage,
    IReadOnlyList<BlogPostPage> Posts,
    int PageNumber,
    int TotalPages)
{
    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
