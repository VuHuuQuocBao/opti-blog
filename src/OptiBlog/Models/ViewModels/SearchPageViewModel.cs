using OptiBlog.Business;
using OptiBlog.Models.Pages;

namespace OptiBlog.Models.ViewModels;

public record SearchPageViewModel(SearchPage CurrentPage, SiteSearchQuery Search, SiteSearchResult Result)
{
    public int TotalPages => (int)Math.Ceiling(Result.Total / (double)SiteSearchService.PageSize);
    public bool HasPreviousPage => Search.Page > 1;
    public bool HasNextPage => Search.Page < TotalPages;
}
