using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartsUnlimited.Models;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(PartsUnlimited.Functions.Startup))]

namespace PartsUnlimited.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<PartsUnlimitedContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
