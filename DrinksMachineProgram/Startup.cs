using DrinksMachineProgram.Authentication;
using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DrinksMachineProgram
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            ProductsBL.Instance.Create(new Product
            {
                Name = "Coke",
                Cost = 25,
                QuantityAvailable = 5
            });

            ProductsBL.Instance.Create(new Product
            {
                Name = "Pepsi",
                Cost = 36,
                QuantityAvailable = 15
            });

            ProductsBL.Instance.Create(new Product
            {
                Name = "Soda",
                Cost = 45,
                QuantityAvailable = 3
            });


            CoinsBL.Instance.Create(new Coin
            {
                Name = "Cent",
                Value = 0.01m,
                QuantityAvailable = 100
            });

            CoinsBL.Instance.Create(new Coin
            {
                Name = "Nickels",
                Value = 0.05m,
                QuantityAvailable = 10
            });

            CoinsBL.Instance.Create(new Coin
            {
                Name = "Dime",
                Value = 0.1m,
                QuantityAvailable = 5
            });

            CoinsBL.Instance.Create(new Coin
            {
                Name = "Quarter",
                Value = 0.25m,
                QuantityAvailable = 25
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddControllersWithViews();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ReturnUrlParameter = "returnUrl";
                options.AccessDeniedPath = "/Common/DeniedAcces";
                options.LoginPath = "/Login/Login";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser()
                        .RequireAssertion(context => context.User.HasClaim(ClaimTypes.Role, "Admin"))
                        .Build();
                });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<ILoginManager, LoginManager>();
            services.AddScoped<IUserSession, UserSession>();

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
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
