using Quinta.ShowOptions;
using Quinta.ViewModels;

namespace Quinta.Interfaces;

public interface IDialogService
{
    //TODO: add dialog result
    Task ShowDialogAsync<TViewModel>(UiShowDialogOptions options) where TViewModel : IDialogViewModel;

    Task ShowDialogAsync<TViewModel, TInitParameter>(TInitParameter parameter, UiShowDialogOptions options)
        where TViewModel : IInitializableDialogViewModel<TInitParameter>;

    Task ShowDialogAsync<TViewModel>(TViewModel viewModel, UiShowDialogOptions options) where TViewModel : IDialogViewModel;
}