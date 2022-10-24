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

    public async Task ShowDialogAsync<TViewModel>(
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IDialogViewModel
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        await ShowDialogAsync(viewModel, options, callback);
    }

    public async Task ShowDialogAsync<TViewModel, TInitParameter>(
        TInitParameter parameter,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IInitializableDialogViewModel<TInitParameter>
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        await viewModel.InitializeAsync(parameter);
        await ShowDialogAsync(viewModel, options, callback);
    }

    public async Task ShowDialogAsync<TViewModel, TInitParameter>(
        TViewModel viewModel,
        TInitParameter parameter,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IInitializableDialogViewModel<TInitParameter>
    {
        await viewModel.InitializeAsync(parameter);
        await ShowDialogAsync(viewModel, options, callback);
    }

    public async Task ShowDialogAsync<TViewModel>(
        TViewModel viewModel,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IDialogViewModel
    {
        var disposable = new CompositeDisposable();
        var dialog = new DialogWindow
        {
            Title = options.Title,
            SizeToContent = options.SizeToContent,
            ShowInTaskbar = options.ShowInTaskbar,
            DataContext = viewModel
        };

        if (options.SizeToContent == SizeToContent.Manual)
        {
            dialog.Width = options.Width;
            dialog.Height = options.Height;
        }

        viewModel.Close
            .Subscribe(_ => dialog.Close())
            .DisposeWith(disposable);

        Observable
            .FromEventPattern(x => dialog.Closed += x, x => dialog.Closed -= x)
            .Subscribe(_ =>
            {
                callback?.Invoke(viewModel);
                viewModel.Dispose();
            })
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
