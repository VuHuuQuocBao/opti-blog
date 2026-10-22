using EPiServer.Framework.DataAnnotations;

namespace OptiBlog.Models.Media;

[ContentType(
    GUID = "752651a3-c39f-4881-893b-c20aa0677f90",
    DisplayName = "Document",
    Description = "An uploaded PDF or Word document.",
    GroupName = ContentTypeGroupNames.Media)]
[MediaDescriptor(ExtensionString = "pdf,doc,docx")]
public class DocumentFile : MediaData
{
}

[ContentType(
    GUID = "fc133141-0d01-4975-a56b-41733c800578",
    DisplayName = "Video",
    Description = "An uploaded video.",
    GroupName = ContentTypeGroupNames.Media)]
[MediaDescriptor(ExtensionString = "mp4,webm,mov")]
public class VideoFile : VideoData
{
}
