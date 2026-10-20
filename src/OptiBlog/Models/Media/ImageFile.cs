using EPiServer.Framework.DataAnnotations;

namespace OptiBlog.Models.Media;

[ContentType(
    GUID = "f02ea1b0-6661-409c-8b6d-e9921dbd3e97",
    DisplayName = "Image",
    Description = "An uploaded image.",
    GroupName = ContentTypeGroupNames.Media)]
[MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,png,gif,webp,svg")]
public class ImageFile : ImageData
{
}
