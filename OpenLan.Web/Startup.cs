using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Diagnostics.Entity;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Routing;
using Microsoft.AspNet.Security;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;
using Microsoft.Framework.Logging.Console;
using OpenLan.Web.Models;

namespace OpenLan.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup()
        {
            // Setup configuration sources.
            Configuration = new Configuration()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
        }

        /// <summary>
        /// This method gets called by the runtime.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF services to the services container.
            services.AddEntityFramework(Configuration)
                .AddSqlServer()
                .AddDbContext<OpenLanContext>();

            // Add Identity services to the services container.
            services.AddIdentity<ApplicationUser, IdentityRole>(Configuration)
                .AddEntityFrameworkStores<OpenLanContext>()
                .AddDefaultTokenProviders()
                .AddMessageProvider<EmailMessageProvider>()
                .AddMessageProvider<SmsMessageProvider>();

            // Add MVC services to the services container.
            services.AddMvc();

            // Add all SignalR related services to IoC
            services.AddSignalR();

            // Configure Auth
            services.Configure<AuthorizationOptions>(options =>
            {
                options.AddPolicy("ManageTournaments", new AuthorizationPolicyBuilder().RequiresClaim("ManageTournaments", "Allowed").Build());
                options.AddPolicy("ManageProducts", new AuthorizationPolicyBuilder().RequiresClaim("ManageProducts", "Allowed").Build());
                options.AddPolicy("ManageTeams", new AuthorizationPolicyBuilder().RequiresClaim("ManageTeams", "Allowed").Build());
            });

        }

        //This method is invoked when ASPNET_ENV is 'Development' or is not defined
        //The allowed values are Development, Staging and Production
        public void ConfigureDevelopment(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            //Display custom error page in production when error occurs
            //During development use the ErrorPage middleware to display error information in the browser
            app.UseErrorPage(ErrorPageOptions.ShowAll);

            app.UseDatabaseErrorPage(DatabaseErrorPageOptions.ShowAll);

            // Add the runtime information page that can be used by developers
            // to see what packages are used by the application
            // default path is: /runtimeinfo
            app.UseRuntimeInfoPage();

            Configure(app);
        }

        public void Configure(IApplicationBuilder app)
        {
            // Configure SignalR
            app.UseSignalR();

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add cookie-based authentication to the request pipeline
            app.UseIdentity();

            ////app.UseFacebookAuthentication();

            ////app.UseGoogleAuthentication();

            ////app.UseTwitterAuthentication();

            // The MicrosoftAccount service has restrictions that prevent the use of http://localhost:5001/ for test applications.
            // As such, here is how to change this sample to uses http://ktesting.com:5001/ instead.

            // Edit the Project.json file and replace http://localhost:5001/ with http://ktesting.com:5001/.

            // From an admin command console first enter:
            // notepad C:\Windows\System32\drivers\etc\hosts
            // and add this to the file, save, and exit (and reboot?):
            // 127.0.0.1 ktesting.com

            // Then you can choose to run the app as admin (see below) or add the following ACL as admin:
            // netsh http add urlacl url=http://ktesting:5001/ user=[domain\user]

            // The sample app can then be run via:
            // k web
            ////app.UseMicrosoftAccountAuthentication();

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areaRoute",
                    template: "{area:exists}/{controller}/{action}",
                    defaults: new { action = "Index" });

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });

            //Populates the OpenLan sample data
            SampleData.InitializeOpenLanDatabaseAsync(app.ApplicationServices).Wait();
        }
    }
}
