using DrinksMachineProgram.Authentication;
using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json.Serialization;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

            MD5CryptoServiceProvider cryptoProvider = new MD5CryptoServiceProvider();

            UsersBL.Instance.Create(new User
            {
                UserName = "admin",
                FirstName = "Admin",
                LastName = "Admin",
                PasswordHash =  cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes("ZyxWv@98765"))
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

            services.AddMvc(options => options.Filters.Add(
                 new AuthorizeFilter(
                     new AuthorizationPolicyBuilder()
                         .RequireAuthenticatedUser()
                         .Build())))
             .AddJsonOptions(options => {
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
                 options.JsonSerializerOptions.DictionaryKeyPolicy = null;

             })
             .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


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

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
