using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Input;
using DynamicData;
using Quinta.Interfaces;
using ReactiveUI;

namespace Quinta.MainMenu;

public class MainMenuService : IMainMenuService
{
    private readonly SourceCache<MenuItemViewModel, string> _menuItemsSource = new(vm => vm.Header);
    private readonly ReadOnlyObservableCollection<MenuItemViewModel> _menuItems;

    public MainMenuService()
    {
        _menuItemsSource
            .Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _menuItems)
            .Subscribe();
    }

    public ReadOnlyObservableCollection<MenuItemViewModel> MenuItems => _menuItems;


    // TODO: HotKey bug https://github.com/AvaloniaUI/Avalonia/issues/2441
    public void AddGlobalCommand(string commandPath, string commandName, ICommand command, KeyGesture? hotKey = null)
    {
        var parent = ProcessCommandPath(commandPath);
        if (parent.Items.Any(x => x.Header == commandName))
        {
            throw new Exception($"Duplicate command name: {commandName}");
        }

        var item = CreateMenuItem(commandName, parent);
        item.Command = command;
        item.HotKey = hotKey;
    }

    private MenuItemViewModel ProcessCommandPath(string commandPath)
    {
        var names = commandPath.Split('$', StringSplitOptions.RemoveEmptyEntries);
        if (names.Length == 0)
        {
            throw new Exception("Bad command path");
        }

        var rootName = names[0];
        var optional = _menuItemsSource.Lookup(rootName);
        MenuItemViewModel? item;
        if (optional.HasValue)
        {
            item = optional.Value;
        }
        else
        {
            item = CreateMenuItem(rootName);
            _menuItemsSource.AddOrUpdate(item);
        }

        var parent = item;
        for (var i = 1; i < names.Length; i++)
        {
            var name = names[i];
            item = parent.Items.FirstOrDefault(x => x.Header == name) ?? CreateMenuItem(name, parent);
            parent = item;
        }

        return item;
    }

    private static MenuItemViewModel CreateMenuItem(string name, MenuItemViewModel? parent = null)
    {
        var item = new MenuItemViewModel { Header = name };
        parent?.Items.Add(item);
        return item;
    }
}