using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Quinta.Sample.Views;

public partial class StartPageView : UserControl
{
    public const string StartPageId = "StartPage";

    public StartPageView()
    {
        InitializeComponent();
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}