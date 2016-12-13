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
using Microsoft.AspNetCore.Identity;

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
        public override async void Configure(IApplicationBuilder app)
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

            this.seed(app);

                base.Configure(app);

        }

        public async void seed(IApplicationBuilder app)
        {
            var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
            var roleManager = app.ApplicationServices.GetService<RoleManager<IdentityRole>>();

            if(await roleManager.FindByNameAsync("Administrator") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            if (await roleManager.FindByNameAsync("Editor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Editor"));
            }

            if (await roleManager.FindByNameAsync("Employee") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Employee"));
            }

            if (await userManager.FindByEmailAsync("admin@test.dk") == null)
            {
                ApplicationUser administrator = new ApplicationUser()
                {
                    UserName = "admin@test.dk",
                    Email = "admin@test.dk",
                    FirstName = "Admin",
                    LastName = "Adminsen",
                    Phone = "12345678",
                    Address = "Adminvej 1",
                    Zipcode = "4000",
                    City = "Roskilde"
                };



                await userManager.CreateAsync(administrator, "!Test123");

                
                string[] names = new string[3] { "Administrator", "Editor", "Employee" };


                IdentityResult result = await userManager.AddToRolesAsync(administrator, names.ToArray<string>());
                
            }

            if (await userManager.FindByEmailAsync("Employee@test.dk") == null)
            {
                ApplicationUser employee = new ApplicationUser()
                {
                    UserName = "Employee@test.dk",
                    Email = "Employee@test.dk",
                    FirstName = "Employee",
                    LastName = "Something",
                    Phone = "12345678",
                    Address = "Plebvej 1",
                    Zipcode = "4000",
                    City = "Roskilde"
                };



                await userManager.CreateAsync(employee, "!Test123!");

                IdentityResult result = await userManager.AddToRoleAsync(employee, "Employee");
            }

            if (await userManager.FindByEmailAsync("editor@test.dk") == null)
            {
                ApplicationUser editor = new ApplicationUser()
                {
                    UserName = "editor@test.dk",
                    Email = "editor@test.dk",
                    FirstName = "editor",
                    LastName = "Something",
                    Phone = "12345678",
                    Address = "Plebvej 1",
                    Zipcode = "4000",
                    City = "Roskilde"
                };



                await userManager.CreateAsync(editor, "!Test123!");

                string[] names = new string[2] {"Editor", "Employee"};


                IdentityResult result = await userManager.AddToRolesAsync(editor, names.ToArray<string>());

            }
        }

    }
      

    }

