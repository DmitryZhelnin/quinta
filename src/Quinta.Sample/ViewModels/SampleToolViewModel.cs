using Quinta.ViewModels;

namespace Quinta.Sample.ViewModels;

public class SampleToolViewModel : ToolViewModelBase
{
    public SampleToolViewModel()
    {
        Title = "Sample Tool";
        CanClose = true;
    }

    public string Text => "This is Sample Tool";
}