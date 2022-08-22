using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Quinta.MainMenu;

public class MenuItemViewModel : ReactiveObject
{
    [Reactive] public string Header { get; set; } = default!;
    [Reactive] public ICommand? Command { get; set; }
    [Reactive] public object? CommandParameter { get; set; }
    public ObservableCollection<MenuItemViewModel> Items { get; } = new();
    [Reactive] public KeyGesture? InputGesture { get; set; }
}