using System.Reactive;
using ReactiveUI;

namespace Quinta.ViewModels;

public interface IDialogViewModel : IDisposable
{
    ReactiveCommand<Unit, Unit> Close { get; }
}