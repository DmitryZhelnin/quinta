using Microsoft.Extensions.DependencyInjection;
using Quinta.Sample.ViewModels;

namespace Quinta.Sample;

public class Bootstrapper : BootstrapperBase
{
    protected override void ConfigureContainer(IServiceCollection services)
    {
        services.AddSingleton<StartPageViewModel>();
        services.AddTransient<SampleToolViewModel>();
        services.AddTransient<SampleDialogViewModel>();
        base.ConfigureContainer(services);
    }
}