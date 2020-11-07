// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Primitives;
using PartsUnlimited.Areas.Admin;
using PartsUnlimited.Models;
using PartsUnlimited.Queries;
using PartsUnlimited.Recommendations;
using PartsUnlimited.Search;
using PartsUnlimited.Security;
using PartsUnlimited.Telemetry;
using PartsUnlimited.WebsiteConfiguration;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PartsUnlimited
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add EF services to the services container
            services.AddDbContext<PartsUnlimitedContext>(options => {
                options.UseLazyLoadingProxies();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                }
                else
                {
                    options.UseSqlite("Data Source=partsunlimited.db");
                }
            });

            // Associate IPartsUnlimitedContext and PartsUnlimitedContext with context
            services.AddTransient<IPartsUnlimitedContext, PartsUnlimitedContext>();

            // Add Identity services to the services container
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<PartsUnlimitedContext>()
            //    .AddDefaultTokenProviders();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdB2C"));

            AppBuilderLoginProviderExtensions.AddLoginProviders(services, new ConfigurationLoginProviders(Configuration.GetSection("Authentication")));

            // Configure admin policies
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(AdminConstants.Role,
                    authBuilder =>
                    {
                        authBuilder.RequireClaim(AdminConstants.ManageStore.Name, AdminConstants.ManageStore.Allowed);
                    });

            });

            // Add implementations
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddScoped<IOrdersQuery, OrdersQuery>();
            services.AddScoped<IRaincheckQuery, RaincheckQuery>();

            services.AddSingleton<ITelemetryProvider, EmptyTelemetryProvider>();
            services.AddScoped<IProductSearch, StringContainsProductSearch>();

            SetupRecommendationService(services);

            services.AddScoped<IWebsiteOptions>(p =>
            {
                var telemetry = p.GetRequiredService<ITelemetryProvider>();

                return new ConfigurationWebsiteOptions(Configuration.GetSection("WebsiteOptions"), telemetry);
            });

            services.AddScoped<IApplicationInsightsSettings>(p =>
            {
                return new ConfigurationApplicationInsightsSettings(Configuration.GetSection(ConfigurationPath.Combine("Keys", "ApplicationInsights")));
            });

            //services.AddApplicationInsightsTelemetry(Configuration);

            // We need access to these settings in a static extension method, so DI does not help us :(
            ContentDeliveryNetworkExtensions.Configuration = new ContentDeliveryNetworkConfiguration(Configuration.GetSection("CDN"));

            // Add MVC services to the services container
            services.AddMvc()
                .AddMicrosoftIdentityUI();

            services.AddCors(cors => cors.AddDefaultPolicy(new CorsPolicyBuilder("partsunlimitednetconf2020.b2clogin.com").AllowAnyOrigin().Build()));

            //Add InMemoryCache
            services.AddSingleton<IMemoryCache, MemoryCache>();

            // Add session related services.
            //services.AddCaching();
            services.AddSession();

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        private void SetupRecommendationService(IServiceCollection services)
        {
            var azureMlConfig = new AzureMLFrequentlyBoughtTogetherConfig(Configuration.GetSection(ConfigurationPath.Combine("Keys", "AzureMLFrequentlyBoughtTogether")));

            // If keys are not available for Azure ML recommendation service, register an empty recommendation engine
            if (string.IsNullOrEmpty(azureMlConfig.AccountKey) || string.IsNullOrEmpty(azureMlConfig.ModelName))
            {
                services.AddSingleton<IRecommendationEngine, EmptyRecommendationsEngine>();
            }
            else
            {
                services.AddSingleton<IAzureMLAuthenticatedHttpClient, AzureMLAuthenticatedHttpClient>();
                services.AddSingleton<IAzureMLFrequentlyBoughtTogetherConfig>(azureMlConfig);
                services.AddScoped<IRecommendationEngine, AzureMLFrequentlyBoughtTogetherRecommendationEngine>();
            }
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Development' or is not defined
        //The allowed values are Development,Staging and Production
        public void ConfigureDevelopment(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            //Display custom error page in production when error occurs
            //During development use the ErrorPage middleware to display error information in the browser
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();

            Configure(app, environment);
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Staging'
        //The allowed values are Development,Staging and Production
        public void ConfigureStaging(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseExceptionHandler("/Home/Error");
            Configure(app, environment);
        }

        //This method is invoked when ASPNETCORE_ENVIRONMENT is 'Production'
        //The allowed values are Development,Staging and Production
        public void ConfigureProduction(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            app.UseExceptionHandler("/Home/Error");
            Configure(app, environment);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // Configure Session.
            app.UseSession();

            app.MapWhen(p => File.Exists(Path.Combine(environment.WebRootPath, p.Request.Path.Value.TrimStart('/'))), sub =>
            {
                sub.Use((ctx, nxt) =>
                {
                    ctx.Response.Headers.Add("Cache-Control", new StringValues(new[] { "no-cache", "no-store", "must-revalidate" }));
                    return nxt();
                });

                sub.UseCors();
                sub.UseStaticFiles();
            });

            // Add static files to the request pipeline
            app.UseStaticFiles();

            app.UseRouting();

            // Add cookie-based authentication to the request pipeline
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute(
                    name: "areaRoute",
                    areaName: AdminConstants.Area,
                    pattern: $"/{AdminConstants.Area}/{{controller}}/{{action}}",
                    defaults: new { action = "Index" });
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}