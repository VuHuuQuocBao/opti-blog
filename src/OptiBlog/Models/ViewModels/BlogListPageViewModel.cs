using OptiBlog.Business;
using OptiBlog.Models.Pages;

namespace OptiBlog.Models.ViewModels;

public record BlogListPageViewModel(BlogListPage CurrentPage, BlogSearchQuery Search, BlogSearchResult Result)
{
    public int TotalPages => (int)Math.Ceiling(Result.Total / (double)CurrentPage.PageSize);
    public bool HasPreviousPage => Search.Page > 1;
    public bool HasNextPage => Search.Page < TotalPages;
}
