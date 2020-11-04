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

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddScoped<CartClient>();
            builder.Services.AddScoped<ShoppingCartNotificationService>();

            await builder.Build().RunAsync();
        }
    }
}
