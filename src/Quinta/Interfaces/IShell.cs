using Avalonia.Controls;
using Dock.Model.Core;
using Quinta.ShowOptions;
using Quinta.ViewModels;

namespace Quinta.Interfaces;

public interface IShell
{
    string Title { get; set; }

    WindowIcon? Icon { get; set; }

    IServiceProvider ServiceProvider { get; set; }

    IFactory DockFactory { get; set; }

    public IDock Layout { get; set; }

    Window MainWindow { get; set; }

    IMainMenuService MainMenuService { get; }

    IDialogService DialogService { get; }

    IViewModel? ActiveViewModel { get; set; }

    IEnumerable<DocumentViewModelBase> Documents { get; }

    bool ShowStartView<TStartWindow>(UiShowStartWindowOptions? options) where TStartWindow : class;

    Task ShowView<TViewModel>(ViewRequest? viewRequest = null,
        UiShowOptions? options = null)
        where TViewModel : class, IViewModel;

    Task ShowView<TViewModel, TInitParameter>(TInitParameter initParameter,
        ViewRequest? viewRequest = null,
        UiShowOptions? options = null)
        where TViewModel : class, IViewModel, IInitializableViewModel<TInitParameter>;

    Task ShowTool<TViewModel>(ViewRequest? viewRequest = null,
        UiShowOptions? options = null)
        where TViewModel : class, IViewModel;

    Task ShowInContainer(
        string containerName,
        Func<IServiceProvider, Task<IViewModel>> viewModelFactory,
        ViewRequest? viewRequest = null,
        UiShowOptions? options = null);
}
