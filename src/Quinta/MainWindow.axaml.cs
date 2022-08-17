using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Quinta.Interfaces;

namespace Quinta;

public partial class MainWindow : Window
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
}