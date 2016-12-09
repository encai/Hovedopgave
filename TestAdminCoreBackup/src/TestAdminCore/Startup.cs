using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TestAdminCore.Data;
using TestAdminCore.Models;
using TestAdminCore.Services;

namespace TestAdminCore
{
    public class Startup : ExtCore.WebApplication.Startup
    {
        //Base og serviceprovider er fra extcore
        public Startup(IHostingEnvironment env, IServiceProvider ServiceProvider)
            : base(ServiceProvider)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

            //Fra extCore
            this.configurationRoot = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        //sat override ind for at extcore virker
        public override void ConfigureServices(IServiceCollection services)
        {
            //tilføjet fra extcore
            base.ConfigureServices(services);

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<AdministratorSeedData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

        /*
         * Originalt var linien
          public async void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, AdministratorSeedData seeder) 
         * 
         */

            //sat override ind for at extcore virker
        public override void Configure(IApplicationBuilder app)
        {
            /*
             * Originalt 
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
             
             */
            this.serviceProvider.GetService<ILoggerFactory>().AddConsole(Configuration.GetSection("logging"));
            this.serviceProvider.GetService<ILoggerFactory>().AddDebug();

            if (this.serviceProvider.GetService<IHostingEnvironment>().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            base.Configure(app);
           
        }
    }
}
