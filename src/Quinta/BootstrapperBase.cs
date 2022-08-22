using Microsoft.Extensions.DependencyInjection;
using Quinta.Dialogs;
using Quinta.Interfaces;
using Quinta.MainMenu;

namespace Quinta;

public abstract class BootstrapperBase : IBootstrapper
{
    public IShell Init()
    {
        var serviceProvider = ConfigureContainer();
        var shell = serviceProvider.GetRequiredService<IShell>();
        shell.ServiceProvider = serviceProvider;
        return shell;
    }

    private IServiceProvider ConfigureContainer()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IShell, Shell>();
        services.AddSingleton<IMainMenuService, MainMenuService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<IGlobalKeyBindingService>(provider => provider.GetRequiredService<MainWindow>());

        ConfigureContainer(services);

        return services.BuildServiceProvider();
    }

    protected virtual void ConfigureContainer(IServiceCollection services)
    {
        
    }
}