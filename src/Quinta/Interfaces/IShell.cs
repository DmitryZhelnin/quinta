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

    void ShowStartView<TStartWindow>(UiShowStartWindowOptions? options) where TStartWindow : class;

    void ShowView<TViewModel>(
        ViewRequest? viewRequest = null,
        UiShowOptions? options = null)
        where TViewModel : class, IViewModel;
    
    void ShowTool<TViewModel>(
        ViewRequest? viewRequest = null,
        UiShowOptions? options = null)
        where TViewModel : class, IViewModel;
    
    void ShowInContainer(
        string containerName,
        Func<IServiceProvider, IViewModel> viewModelFactory,
        ViewRequest? viewRequest = null,
        UiShowOptions? options = null);
}