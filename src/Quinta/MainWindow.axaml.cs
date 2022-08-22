using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Quinta.Interfaces;

namespace Quinta;

public partial class MainWindow : Window, IGlobalKeyBindingService
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    public MainWindow(IShell shell) : this()
    {
        Shell = shell;
        DataContext = Shell;
#if DEBUG
        this.AttachDevTools();
#endif
    }
    
    public IShell Shell { get; }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void AddKeyBinding(KeyGesture hotKey, ICommand command)
    {
        if (KeyBindings.Any(x => x.Gesture == hotKey))
        {
            return;
        }

        KeyBindings.Add(new KeyBinding
        {
            Gesture = hotKey,
            Command = command
        });
    }

    public void RemoveKeyBinding(KeyGesture hotKey)
    {
        var keyBinding = KeyBindings.FirstOrDefault(x => x.Gesture == hotKey);
        if (keyBinding is not null)
        {
            KeyBindings.Remove(keyBinding);
        }
    }
}