using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Web.Routing;
using Optimizely.Cms.DependencyInjection;
using Optimizely.Graph.DependencyInjection;
using OptiBlog.Business.Editor;
using OptiBlog.Business.Rendering;
using OptiBlog.Business.Validation;

// Git-ignored file holding machine-specific secrets such as the Optimizely Graph keys.
const string LocalSettingsFile = "appsettings.Local.json";

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config => config.AddJsonFile(LocalSettingsFile, optional: true, reloadOnChange: true))
    .ConfigureCmsDefaults()
    .ConfigureWebHostDefaults(webBuilder => webBuilder
        .ConfigureServices((context, services) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(context.HostingEnvironment.ContentRootPath, "App_Data"));

                services.Configure<DataAccessOptions>(options => options.CreateDatabaseSchema = true);
            }

            services
                .AddCmsAspNetIdentity<ApplicationUser>()
                .AddCms()
                .AddAdminUserRegistration()
                .AddEmbeddedLocalization<Program>();
             
            services.AddCmsValidator<BlogPostPageParentValidator>();
            // Task 2
            services.AddCustomTinyMceStyles();
            services.AddBlogDisplayOptions();

            if (!string.IsNullOrWhiteSpace(context.Configuration["Optimizely:ContentGraph:AppKey"]))
            {
                services.AddContentGraph();
                services.AddGraphContentClient();
            }
        })
        .Configure((context, app) =>
        {
            if (context.HostingEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithReExecute("/not-found.html");
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapContent());
        }))
    .Build()
    .Run();
