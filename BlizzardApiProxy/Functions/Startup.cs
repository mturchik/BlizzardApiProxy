using BlizzardApiProxy.Functions;
using BlizzardApiProxy.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace BlizzardApiProxy.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddSingleton<TokenService>()
            .AddScoped<ProfileService>();
    }
}
