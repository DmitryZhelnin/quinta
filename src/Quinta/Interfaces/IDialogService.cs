using Quinta.ShowOptions;

namespace Quinta.Interfaces;

public interface IDialogService
{
    //TODO: add dialog result

    Task ShowDialogAsync<TViewModel>(UiShowDialogOptions options) where TViewModel : notnull;
}