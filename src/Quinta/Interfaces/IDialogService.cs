using Quinta.ShowOptions;
using Quinta.ViewModels;

namespace Quinta.Interfaces;

public interface IDialogService
{
    Task ShowDialogAsync<TViewModel>(
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IDialogViewModel;

    Task ShowDialogAsync<TViewModel, TInitParameter>(
        TInitParameter parameter,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IInitializableDialogViewModel<TInitParameter>;

    Task ShowDialogAsync<TViewModel>(
        TViewModel viewModel,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IDialogViewModel;

    Task ShowDialogAsync<TViewModel, TInitParameter>(
        TViewModel viewModel,
        TInitParameter parameter,
        UiShowDialogOptions options,
        Action<TViewModel>? callback = null) where TViewModel : IInitializableDialogViewModel<TInitParameter>;
}
