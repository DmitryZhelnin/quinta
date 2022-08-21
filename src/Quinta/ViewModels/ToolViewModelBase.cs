using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Dock.Model.ReactiveUI.Controls;
using ReactiveUI;

namespace Quinta.ViewModels;

public class ToolViewModelBase : Tool, IViewModel
{
    public ToolViewModelBase()
    {
        var canClose = this.WhenAnyValue(x => x.CanClose).Select(x => x);
        Close = ReactiveCommand.Create(() => { }, canClose).DisposeWith(Disposables);
    }

    public ReactiveCommand<Unit, Unit> Close { get; }
    
    public override bool OnClose()
    {
        DisposeInternals();
        return base.OnClose();
    }

    protected virtual void DisposeInternals()
    {
        Disposables.Clear();
    }
    
    protected internal CompositeDisposable Disposables { get; }  = new();
}