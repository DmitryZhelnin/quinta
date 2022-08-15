using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Dock.Model.Core;
using Microsoft.Extensions.DependencyInjection;
using Quinta.DockFactories;
using Quinta.Extensions;
using Quinta.Interfaces;
using Quinta.ShowOptions;
using Quinta.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Quinta;

public class Shell : ReactiveObject, IShell
{
    [Reactive] public string Title { get; set; }
    [Reactive] public WindowIcon? Icon { get; set; }
    public IServiceProvider ServiceProvider { get; set; }
    public IFactory DockFactory { get; set; }
    [Reactive] public IDock Layout { get; set; }
    public Window MainWindow { get; set; }

    public void ShowStartView<TStartWindow>(UiShowStartWindowOptions? options) where TStartWindow : class
    {
        Title = options?.Title ?? "";
        if (options?.IconSource is not null)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var iconStream = assets.Open(new Uri(options.IconSource));
            Icon = new WindowIcon(iconStream);
        }

        var window = ServiceProvider.GetRequiredService<TStartWindow>();
        MainWindow = window as Window ?? throw new InvalidCastException($"{window.GetType()} is not a window");
        DockFactory = options?.DockFactory ?? new DefaultDockFactory(MainWindow.DataContext!);
        Layout = DockFactory.CreateLayout() ?? throw new InvalidOperationException();
        DockFactory.InitLayout(Layout);
    }

    public void ShowView<TViewModel>(ViewRequest? viewRequest, UiShowOptions? options)
        where TViewModel : class, IViewModel
    {
        ShowInContainer(DefaultDockFactory.Documents,
            container => container.GetRequiredService<TViewModel>(),
            viewRequest,
            options);
    }

    public void ShowTool<TViewModel>(ViewRequest? viewRequest = null, UiShowOptions? options = null)
        where TViewModel : class, IViewModel
    {
        ShowInContainer(DefaultDockFactory.Tools,
            container => container.GetRequiredService<TViewModel>(),
            viewRequest,
            options);
    }

    public void ShowInContainer(string containerName, Func<IServiceProvider, IViewModel> viewModelFactory,
        ViewRequest? viewRequest = null, UiShowOptions? options = null)
    {
        var container = DockFactory.GetDockable<IDock>(containerName);
        if (container is null)
        {
            throw new ArgumentException($"Container {containerName} not found");
        }

        var dockable = container.FindByViewRequest(viewRequest);

        if (dockable is null)
        {
            var viewModel = viewModelFactory(ServiceProvider);
            if (options != null && viewModel is IConfigurableViewModel configurable)
                configurable.Configure(options);

            DockFactory.AddDockable(container, viewModel);

            AddClosingByRequest(viewModel);
            // InitializeView(view, viewRequest);
            dockable = viewModel;
        }

        DockFactory.SetActiveDockable(dockable);
        DockFactory.SetFocusedDockable(container, dockable);
    }

    private void AddClosingByRequest(IViewModel viewModel)
    {
        //TODO: base closable type
        switch (viewModel)
        {
            case DocumentViewModelBase documentViewModel:
                Observable
                    .FromEventPattern(x => documentViewModel.CloseQuery += x,
                        x => documentViewModel.CloseQuery -= x)
                    .Subscribe(x => DockFactory.CloseDockable(viewModel))
                    .DisposeWith(documentViewModel.Disposables);
                break;
            case ToolViewModelBase toolViewModel:
                Observable
                    .FromEventPattern(x => toolViewModel.CloseQuery += x,
                        x => toolViewModel.CloseQuery -= x)
                    .Subscribe(x => DockFactory.CloseDockable(viewModel))
                    .DisposeWith(toolViewModel.Disposables);
                break;
        }
    }
}