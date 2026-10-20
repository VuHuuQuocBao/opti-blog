using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Data;
using EPiServer.DependencyInjection;
using EPiServer.Web.Routing;
using Optimizely.Cms.DependencyInjection;
using Optimizely.Graph.DependencyInjection;

Host.CreateDefaultBuilder(args)
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

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapContent());
        }))
    .Build()
    .Run();
