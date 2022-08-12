using Quinta.Interfaces;
using Quinta.ShowOptions;

namespace Quinta;

public static class UiStarter
{
    public static IShell Start<TStartWindow>(IBootstrapper bootstrapper, UiShowStartWindowOptions? options = null)
        where TStartWindow : class 
    {
        var shell = bootstrapper.Init();
        shell.ShowStartView<TStartWindow>(options);
        return shell;
    }
}