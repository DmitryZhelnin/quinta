using System.Reactive;
using ReactiveUI;

namespace Quinta.ViewModels;

public abstract class DialogViewModelBase : ReactiveObject, IDialogViewModel
{
    protected DialogViewModelBase()
    {
        Close = ReactiveCommand.Create(() => { });
    }

    public ReactiveCommand<Unit, Unit> Close { get; }

    protected virtual void Dispose(bool disposing)
    {
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}