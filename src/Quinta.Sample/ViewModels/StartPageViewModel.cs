using Quinta.ViewModels;

namespace Quinta.Sample.ViewModels;

public class StartPageViewModel : DocumentViewModelBase
{
    public StartPageViewModel()
    {
        Title = "Start Page";
        CanClose = false;
    }

    public string Text => "This is Start Page";
}