using EPiServer.Validation;
using OptiBlog.Models.Pages;

namespace OptiBlog.Business.Validation;

public class BlogPostPageParentValidator(IContentLoader contentLoader) : IValidate<BlogPostPage>
{
    public IEnumerable<ValidationError> Validate(BlogPostPage instance)
    {
        if (ContentReference.IsNullOrEmpty(instance.ParentLink))
        {
            return [];
        }

        var options = new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() };
        if (contentLoader.TryGet<IContent>(instance.ParentLink, options, out var parent) && parent is BlogListPage)
        {
            return [];
        }

        return
        [
            new ValidationError
            {
                ErrorMessage = "A blog post can only be created under a blog list page.",
                Severity = ValidationErrorSeverity.Error,
                ValidationType = ValidationErrorType.StorageValidation,
            },
        ];
    }
}
