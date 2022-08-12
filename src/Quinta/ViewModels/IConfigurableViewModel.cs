using Quinta.ShowOptions;

namespace Quinta.ViewModels;

public interface IConfigurableViewModel : IViewModel
{
    void Configure(UiShowOptions options);
}