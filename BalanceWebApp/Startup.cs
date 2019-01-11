using System;
using System.Threading.Tasks;
using BalanceWebApp.Model;
using BalanceWebApp.Model.Dao;
using BalanceWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

                options.Events = ConfigureOpenIdConnectEvents();
                
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
            services.AddSingleton<IAccountDao, AccountDao>();
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
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        private static OpenIdConnectEvents ConfigureOpenIdConnectEvents()
        {
            return new OpenIdConnectEvents()
            {
                OnRedirectToIdentityProvider = context =>
                {
                    context.ProtocolMessage.RedirectUri =
                        TransformToHttpsInProduction(context.ProtocolMessage.RedirectUri);
                    
                    return Task.FromResult(0);
                },
                
                OnRedirectToIdentityProviderForSignOut = context =>
                {
                    context.ProtocolMessage.PostLogoutRedirectUri =
                        TransformToHttpsInProduction(context.ProtocolMessage.PostLogoutRedirectUri);

                    return Task.FromResult(0);
                }
            };
        }

        private static string TransformToHttpsInProduction(string input)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var isProduction = "Production".Equals(env) ? true : false;

            return isProduction ? input.ToLower().Replace("http", "https") : input;            
        }        
    }
}
