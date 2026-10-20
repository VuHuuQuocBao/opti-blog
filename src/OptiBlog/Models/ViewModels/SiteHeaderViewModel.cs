namespace OptiBlog.Models.ViewModels;

public record SiteHeaderViewModel(string HomePageUrl, IReadOnlyList<MenuItem> Menu);

public record MenuItem(string Text, string Link);
