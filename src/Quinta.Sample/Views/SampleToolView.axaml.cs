using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Quinta.Sample.Views;

public partial class SampleToolView : UserControl
{
    public const string SampleToolId = "SampleTool";
    
    public SampleToolView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}