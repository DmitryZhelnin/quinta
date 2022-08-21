using Quinta.ShowOptions;
using Quinta.ViewModels;

namespace Quinta.Sample.ViewModels;

public class StartPageViewModel : DocumentViewModelBase, IConfigurableViewModel
{
    public string Text => "This is Start Page";

    public void Configure(UiShowOptions options)
    {
        Title = options.Title;
        CanClose = options.CanClose;
        CanFloat = options.CanFloat;
    }
}