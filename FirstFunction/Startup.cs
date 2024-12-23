using System;
using FirstFunction.models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(FirstFunction.Startup))]

namespace FirstFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        //builder.Services.AddSingleton<IMyService, MyService>();
        var constr = Environment.GetEnvironmentVariable("SqlCon");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(constr);
        });
    }
}

