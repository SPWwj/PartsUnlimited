using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PartsUnlimited.Shared;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PartsUnlimited.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddHttpClient<PublicCartClient>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddHttpClient<CartClient>(c => c.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped<ShoppingCartNotificationService>();

            builder.Services.AddMsalAuthentication(options =>
            {
                var auth = options.ProviderOptions.Authentication;
                auth.Authority = "https://partsunlimitednetconf2020.b2clogin.com/partsunlimitednetconf2020.onmicrosoft.com/b2c_1_susi/";
                auth.ClientId = "3308a8fd-34ef-4b1a-ba55-4fa0a5b72d5d";
                auth.ValidateAuthority = false;
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://PartsUnlimitedNetConf2020.onmicrosoft.com/770baddd-a0b0-4b8f-b9bb-681500425d2f/Cart.Read");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("https://PartsUnlimitedNetConf2020.onmicrosoft.com/770baddd-a0b0-4b8f-b9bb-681500425d2f/Cart.Edit");
            });

            await builder.Build().RunAsync();
        }
    }
}
