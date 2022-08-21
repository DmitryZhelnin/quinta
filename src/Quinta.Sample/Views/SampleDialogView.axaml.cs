using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Quinta.Sample.Views;

public partial class SampleDialogView : UserControl
{
    public SampleDialogView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}