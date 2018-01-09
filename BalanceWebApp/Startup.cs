using System;
using BalanceWebApp.Model;
using BalanceWebApp.Model.Dao.Dapper;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace BalanceWebApp
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {                
                options.ClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
                options.ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
                options.Authority = Environment.GetEnvironmentVariable("AUTHORITY");
                // options.ClientId = "{clientId}";
                // options.ClientSecret = "{clientSecret}";
                // options.Authority = "https://dev-149130.oktapreview.com/oauth2/default";
                options.CallbackPath = "/authorization-code/callback";
                options.ResponseType = "code";
                options.SaveTokens = true;
                options.UseTokenLifetime = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name"
                };
            });            


            // Add framework services.
            services.AddLogging();
            services.AddOptions();
            services.AddMvc();

            services.Configure<AppSettings>(x => Configuration.GetSection("AppSettings").Bind(x));
            
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            // Data repositories
            services.AddSingleton<ConnectionFactory, ConnectionFactory>();
            services.AddSingleton<AccountTypeDao, AccountTypeDao>();
            services.AddSingleton<ProviderDao, ProviderDao>();
            services.AddSingleton<TransactionTypeDao, TransactionTypeDao>();
            services.AddSingleton<AccountDao, AccountDao>();
            services.AddSingleton<TransactionDao, TransactionDao>();

            // Services
            services.AddSingleton<AccountTypeService, AccountTypeService>();
            services.AddSingleton<ProviderService, ProviderService>();
            services.AddSingleton<TransactionTypeService, TransactionTypeService>();
            services.AddSingleton<AccountService, AccountService>();
            services.AddSingleton<TransactionService, TransactionService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
