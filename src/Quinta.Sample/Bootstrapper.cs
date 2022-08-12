using System;
using Microsoft.Extensions.DependencyInjection;
using Quinta.Interfaces;
using Quinta.Sample.ViewModels;
using Quinta.ViewModels;

namespace Quinta.Sample;

public class Bootstrapper : IBootstrapper
{
    public IShell Init()
    {
        var serviceProvider = ConfigureContainer();
        var shell = serviceProvider.GetRequiredService<IShell>();
        shell.ServiceProvider = serviceProvider;
        return shell;
    }

    private static IServiceProvider ConfigureContainer()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IShell, Shell>();
        services.AddSingleton<IRootWindow, MainWindow>();

        ConfigureSingleView<StartPageViewModel>(services);

        services.AddTransient<SampleToolViewModel>();

        return services.BuildServiceProvider();
    }
    
    private static void ConfigureSingleView<TViewModel>(IServiceCollection services)
        where TViewModel : class, IViewModel
    {
        services.AddSingleton<TViewModel>();
    }
}