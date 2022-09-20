namespace Quinta.ViewModels;

public abstract class InitializableDialogViewModelBase<TParameter> : DialogViewModelBase,
    IInitializableDialogViewModel<TParameter>
{
    protected InitializableDialogViewModelBase()
    {
    }

    public abstract Task InitializeAsync(TParameter initializeParameter);
}