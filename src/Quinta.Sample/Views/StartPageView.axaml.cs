using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Quinta.ShowOptions;
using Quinta.ViewModels;

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

    public IViewModel ViewModel { get; set; }
    
    public void Configure(UiShowOptions options)
    {
        ViewModel.Title = options.Title;
    }
}