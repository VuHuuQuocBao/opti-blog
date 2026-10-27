using System.ComponentModel.DataAnnotations;
using EPiServer.Web;

namespace OptiBlog.Models.Blocks;

public interface IHeaderFooterConfigurable
{
    HeaderFooterSettings? HeaderFooter { get; }
}

[ContentType(
    GUID = "d43743e9-fdc5-4a87-be2d-3cc635ed8953",
    DisplayName = "Header and footer settings",
    AvailableInEditMode = false)]
public class HeaderFooterSettings : BlockData
{
    [Display(
        Name = "Hide header",
        Order = 10)]
    public virtual bool HideHeader { get; set; }

    [UIHint(UIHint.Block)]
    [AllowedTypes(typeof(HeaderBlock))]
    [Display(
        Name = "Header override",
        Description = "Leave empty to use the site's default header.",
        Order = 20)]
    public virtual ContentReference? HeaderOverride { get; set; }

    [Display(
        Name = "Hide footer",
        Order = 30)]
    public virtual bool HideFooter { get; set; }

    [UIHint(UIHint.Block)]
    [AllowedTypes(typeof(FooterBlock))]
    [Display(
        Name = "Footer override",
        Description = "Leave empty to use the site's default footer.",
        Order = 40)]
    public virtual ContentReference? FooterOverride { get; set; }
}
