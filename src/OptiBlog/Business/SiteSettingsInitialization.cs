using EPiServer.Framework;
using EPiServer.Framework.Initialization;

namespace OptiBlog.Business;

[InitializableModule]
[ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
public class SiteSettingsInitialization : IInitializableModule
{
    public void Initialize(InitializationEngine context)
    {
        var siteSettings = context.Services.GetRequiredService<SiteSettingsService>();
        context.Services.GetRequiredService<IContentEvents>().CreatingContent += siteSettings.PreventSecondSettingsPage;
    }

    public void Uninitialize(InitializationEngine context)
    {
        var siteSettings = context.Services.GetRequiredService<SiteSettingsService>();
        context.Services.GetRequiredService<IContentEvents>().CreatingContent -= siteSettings.PreventSecondSettingsPage;
    }
}
