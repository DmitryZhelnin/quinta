using System.Reactive.Disposables;
using Dock.Model.ReactiveUI.Controls;

namespace Quinta.ViewModels;

public class ToolViewModelBase : Tool, IViewModel
{
    internal event EventHandler? CloseQuery;

    public void Close()
    {
        CloseQuery?.Invoke(this, EventArgs.Empty);
    }

    public override bool OnClose()
    {
        DisposeInternals();
        return base.OnClose();
    }

    protected virtual void DisposeInternals()
    {
        Disposables.Clear();
        CloseQuery = null;
    }
    
    protected internal CompositeDisposable Disposables { get; }  = new();
}