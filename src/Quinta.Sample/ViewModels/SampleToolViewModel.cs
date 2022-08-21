using System.Reactive;
using Quinta.ShowOptions;
using Quinta.ViewModels;
using ReactiveUI;

namespace Quinta.Sample.ViewModels;

public class SampleToolViewModel : ToolViewModelBase, IConfigurableViewModel
{
    public SampleToolViewModel()
    {
        Title = "Sample Tool";
        CanClose = true;
        ChangeTitle = ReactiveCommand.Create(() => { Title = "Changed Title"; });
    }

    public string Text => "This is Sample Tool";
    
    public void Configure(UiShowOptions options)
    {
        Title = options.Title;
    }

    public ReactiveCommand<Unit, Unit> ChangeTitle { get; }
}