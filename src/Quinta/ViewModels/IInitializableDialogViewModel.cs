namespace Quinta.ViewModels;

public interface IInitializableDialogViewModel<in T> : IDialogViewModel, IInitializableViewModel<T>
{
}