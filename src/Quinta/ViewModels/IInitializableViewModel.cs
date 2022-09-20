namespace Quinta.ViewModels;

public interface IInitializableViewModel<in T>
{
    Task InitializeAsync(T initializeParameter);
}