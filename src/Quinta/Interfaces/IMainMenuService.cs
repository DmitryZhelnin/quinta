using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Input;
using Quinta.MainMenu;

namespace Quinta.Interfaces;

public interface IMainMenuService
{
    ReadOnlyObservableCollection<MenuItemViewModel> MenuItems { get; }

    void AddGlobalCommand(string commandPath,  string commandName, ICommand command, KeyGesture? hotKey = null);
}