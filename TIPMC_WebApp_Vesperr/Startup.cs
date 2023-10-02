using TIPMC_WebApp_Vesperr.Data;
using TIPMC_WebApp_Vesperr.Models;
using TIPMC_WebApp_Vesperr.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TIPMC_WebApp_Vesperr.Services.POS;
using TIPMC_WebApp_Vesperr.Hubs;
using System.Globalization;

namespace TIPMC_WebApp_Vesperr
{
    public class Startup
    {
        ////Manual
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        //Auto
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environments.Development}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
             //services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             //{
             //    options.Password.RequireDigit = false;
             //    options.Password.RequireLowercase = false;
             //    options.Password.RequireNonAlphanumeric = false;
             //    options.Password.RequireUppercase = false;
             //    options.Password.RequiredLength = 6;
             //})
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddTransient<IRepository, Repository>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddScoped<SignInManager<ApplicationUser>, AuditableSignInManager<ApplicationUser>>();

            var mvcBuilder = services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            services.Configure<CookieAuthenticationOptions>(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
            });

            services.AddControllersWithViews();
            services.AddSignalR();

            services.AddMvc();

            //services.AddAuthentication().AddFacebook(fb =>
            //{
            //    fb.AppId = "279781507964431";
            //    fb.AppSecret = "9cd72ce710659573f1c0d3fe4455e8e5";
            //});

            //services.AddAuthentication().AddGoogle(go =>
            //{
            //    go.ClientId = "688106088890-pov7l0qi48ep9nhb4idiihhig97mc97a.apps.googleusercontent.com";
            //    go.ClientSecret = "AZGG3CIboE3UAxf1AgdjX9pF";
            //});

            // services.AddAuthentication().AddGithub( gt =>
            //  {
            //        gt.ClientId = "428f22bee79c351e2c1a";
            //       gt.ClientSecret = "a755fb0d4a0c0e1950bddd442dd867de08819535";
            //      }

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            //CultureInfo.DefaultThreadCurrentCulture.NumberFormat.CurrencySymbol = "₱"; // Set PHP currency symbol
            //CultureInfo.DefaultThreadCurrentUICulture.NumberFormat.CurrencySymbol = "₱"; // Set PHP currency symbol
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("~/Home/Error/{0}");

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            //Add middleware here
            //app.UseMiddleware<RequestLoggingMiddleware>();


            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            //do not add middleware here (it wont be invoked)
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
            });


            //var logger = loggerFactory.CreateLogger(env.EnvironmentName);
            //app.Use(async (context, next) =>
            //{
            //    await next.Invoke();
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("ill be executed after the code above!");
            //    Debug.WriteLine("invoke thru await next.Invoke();");
            //});

            //Auto
            // Populate default user admin
            DataSeed.Seed(app.ApplicationServices).Wait();
        }
    }
}
