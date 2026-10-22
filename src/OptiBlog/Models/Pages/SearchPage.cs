namespace OptiBlog.Models.Pages;

[ContentType(
    GUID = "004956e9-8db9-40fe-bd53-38507f43f894",
    DisplayName = "Search page",
    Description = "Site-wide search. Excluded from the search index.",
    GroupName = ContentTypeGroupNames.Site,
    Order = 90)]
[AvailableContentTypes(Availability.None, IncludeOn = [typeof(StartPage)])]
public class SearchPage : SitePageData
{
}
