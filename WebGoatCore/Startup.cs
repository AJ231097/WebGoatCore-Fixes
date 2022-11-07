using WebGoatCore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebGoatCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            var execDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            var builder = new ConfigurationBuilder();

            var dic = new Dictionary<string, string>
            {
                {Constants.WEBGOAT_ROOT, execDirectory},
            };
            builder.AddInMemoryCollection(dic);
            builder.AddConfiguration(configuration);
            Configuration = builder.Build();

            env.WebRootFileProvider = new CompositeFileProvider(
                env.WebRootFileProvider, new PhysicalFileProvider(Path.Combine(execDirectory, "wwwroot"))
            );

            NorthwindContext.Initialize(this.Configuration, env);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            services.AddDbContext<NorthwindContext>(options =>
                options.UseSqlite(NorthwindContext.ConnString)
                    .UseLazyLoadingProxies(),
                ServiceLifetime.Scoped);

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NorthwindContext>()
                .AddDefaultTokenProviders();
            services.AddAntiforgery(options =>
            {
                options.FormFieldName = "AntiForgeryFieldName";
                options.HeaderName = "AntiForgeryHeaderName";
                options.Cookie.Name = "AntiForgeryCookieName";
            });
            services.AddControllersWithViews(options =>
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 2;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            });

            // Set the duration of token expiry to 24hrs
            services.Configure<DataProtectionTokenProviderOptions>(opt => opt.TokenLifespan = TimeSpan.FromHours(24));

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = false;
                options.IdleTimeout = TimeSpan.FromHours(1);
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            services.AddScoped<CustomerRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<BlogEntryRepository>();
            services.AddScoped<BlogResponseRepository>();
            services.AddScoped<ShipperRepository>();
            services.AddScoped<SupplierRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<CategoryRepository>();

            // TODO: Enable this to use Argon 2 as default hasher algorithm
            // services.AddScoped<IPasswordHasher<IdentityUser>, Argon2Hasher<IdentityUser>>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
            });
        }
    }
}
