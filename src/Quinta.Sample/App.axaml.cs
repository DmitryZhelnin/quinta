using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Quinta.Interfaces;
using Quinta.Sample.ViewModels;
using Quinta.Sample.Views;
using Quinta.ShowOptions;

namespace Quinta.Sample
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var options = new UiShowStartWindowOptions
                {
                    Title = "Quinta Sample",
                    IconSource = "avares://Quinta.Sample/Assets/avalonia-logo.ico"
                };
                var shell = UiStarter.Start<IRootWindow>(new Bootstrapper(), options);
                desktop.MainWindow = shell.MainWindow;
                
                shell.ShowView<StartPageViewModel>(
                    viewRequest: new ViewRequest { ViewId = StartPageView.StartPageId },
                    options: new UiShowOptions { Title = "Start Page", CanClose = false });

                shell.ShowTool<SampleToolViewModel>(new ViewRequest { ViewId = SampleToolView.SampleToolId });
                shell.ShowTool<SampleToolViewModel>();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}