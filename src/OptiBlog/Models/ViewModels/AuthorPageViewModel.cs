using OptiBlog.Business;
using OptiBlog.Models.Blocks;
using OptiBlog.Models.Pages;

namespace OptiBlog.Models.ViewModels;

public record AuthorPageViewModel(BlogListPage BlogList, AuthorProfileBlock Author, BlogSearchResult Result, int Page)
{
    public int TotalPages => (int)Math.Ceiling(Result.Total / (double)BlogList.PageSize);

    public bool HasPreviousPage => Page > 1;

    public bool HasNextPage => Page < TotalPages;
}
