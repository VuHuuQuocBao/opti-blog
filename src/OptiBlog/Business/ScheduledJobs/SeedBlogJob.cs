using EPiServer.DataAccess;
using EPiServer.Scheduler;
using EPiServer.Security;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business.ScheduledJobs;

[ScheduledJob(DisplayName = "Seed blog content", GUID = "7f308abf-6214-4c87-99d1-3de6093863b9")]
public class SeedBlogJob : ScheduledJobBase
{
    private const string BlogName = "batch-blog-list-page";
    private const int PostCount = 25;
    private static readonly string[] Authors = ["Jane Doe", "John Smith", "Maria Garcia"];
    private static readonly string[] Tags = ["Optimizely", " cms", "Search "];

    private readonly IContentRepository repository;
    private bool stopRequested;

    public SeedBlogJob(IContentRepository repository)
    {
        this.repository = repository;
        IsStoppable = true;
    }

    public override void Stop() => stopRequested = true;

    public override string Execute()
    {
        var startPage = repository.GetChildren<StartPage>(ContentReference.RootPage).First();
        var blogLink = repository.GetChildren<BlogListPage>(startPage.ContentLink).FirstOrDefault(page => page.Name == BlogName)?.ContentLink
            ?? Publish(repository.GetDefault<BlogListPage>(startPage.ContentLink, startPage.Language), BlogName);
        var existing = repository.GetChildren<BlogPostPage>(blogLink).Select(post => post.Name).ToHashSet();
        var created = 0;

        for (var number = 1; number <= PostCount && !stopRequested; number++)
        {
            var title = $"example-{number}";
            if (existing.Contains(title))
            {
                continue;
            }

            var post = repository.GetDefault<BlogPostPage>(blogLink, startPage.Language);
            post.Title = title;
            post.Author = Authors[number % Authors.Length];
            post.Tags = [Tags[number % Tags.Length]];
            post.PublishDate = DateTime.Today.AddDays(-number);
            Publish(post, title);
            OnStatusChanged($"Created {++created} posts...");
        }

        return $"{(stopRequested ? "Stopped. " : "")}Created {created} posts under \"{BlogName}\".";
    }

    private ContentReference Publish(PageData page, string name)
    {
        page.Name = name;
        return repository.Save(page, SaveAction.Publish, AccessLevel.NoAccess);
    }
}
