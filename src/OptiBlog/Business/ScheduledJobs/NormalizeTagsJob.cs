using EPiServer.DataAccess;
using EPiServer.Scheduler;
using EPiServer.Security;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business.ScheduledJobs;

[ScheduledJob(DisplayName = "Normalize blog tags", GUID = "a1bd0f81-0328-463e-9274-7dfbf9ddb5c3")]
public class NormalizeTagsJob : ScheduledJobBase
{
    private readonly IContentRepository repository;
    private bool stopRequested;

    public NormalizeTagsJob(IContentRepository repository)
    {
        this.repository = repository;
        IsStoppable = true;
    }

    public override void Stop() => stopRequested = true;

    public override string Execute()
    {
        var updated = new List<string>();

        foreach (var link in repository.GetDescendents(ContentReference.RootPage))
        {
            if (stopRequested)
            {
                break;
            }

            if (!repository.TryGet<BlogPostPage>(link, out var post) || post.Tags is null)
            {
                continue;
            }

            var tags = post.Tags.Select(tag => tag.Trim().ToLowerInvariant()).ToList();
            if (tags.SequenceEqual(post.Tags))
            {
                continue;
            }

            var writablePost = (BlogPostPage)post.CreateWritableClone();
            writablePost.Tags = tags;
            repository.Save(writablePost, SaveAction.Publish, AccessLevel.NoAccess);
            updated.Add(post.Name);
            OnStatusChanged($"Updated {updated.Count} posts...");
        }

        return $"{(stopRequested ? "Stopped. " : "")}Updated {updated.Count} posts: {string.Join(", ", updated)}";
    }
}
