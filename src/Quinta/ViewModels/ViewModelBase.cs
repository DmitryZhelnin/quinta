using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Dock.Model.ReactiveUI.Core;
using ReactiveUI;

namespace Quinta.ViewModels;

public abstract class ViewModelBase : DockableBase, IViewModel
{
    protected ViewModelBase()
    {
        var canClose = this.WhenAnyValue(x => x.CanClose).Select(x => x);
        Close = ReactiveCommand
            .Create(() => { }, canClose)
            .DisposeWith(Disposables);
    }

    public ReactiveCommand<Unit, Unit> Close { get; }

    protected virtual void DisposeInternals()
    {
        Disposables.Clear();
    }

    public override bool OnClose()
    {
        DisposeInternals();
        return base.OnClose();
    }

    protected internal CompositeDisposable Disposables { get; }  = new();
}