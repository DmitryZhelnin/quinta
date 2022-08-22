using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using Quinta.Interfaces;
using Quinta.ShowOptions;
using Quinta.ViewModels;

namespace Quinta.Dialogs;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ShowDialogAsync<TViewModel>(UiShowDialogOptions options) where TViewModel : IDialogViewModel
    {
        var disposable = new CompositeDisposable();
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        var dialog = new DialogWindow
        {
            Title = options.Title,
            DataContext = viewModel
        };

        viewModel.Close
            .Subscribe(_ => dialog.Close())
            .DisposeWith(disposable);

        Observable
            .FromEventPattern(x => dialog.Closed += x, x => dialog.Closed -= x)
            .Subscribe(_ => viewModel.Dispose())
            .DisposeWith(disposable);
        
        if (!string.IsNullOrWhiteSpace(options.IconSource))
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var iconStream = assets!.Open(new Uri(options.IconSource));
            dialog.Icon = new WindowIcon(iconStream);
        }

        var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
        await dialog.ShowDialog(mainWindow);
        disposable.Dispose();
    }
}