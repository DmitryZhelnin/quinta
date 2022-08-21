using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Quinta.Sample.ViewModels;
using Quinta.Sample.Views;
using Quinta.ShowOptions;
using ReactiveUI;

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
                var shell = UiStarter.Start<MainWindow>(new Bootstrapper(), options);
                desktop.MainWindow = shell.MainWindow;

                shell.ShowView<StartPageViewModel>(
                    viewRequest: new ViewRequest { ViewId = StartPageView.StartPageId },
                    options: new UiShowOptions { Title = "Start Page", CanClose = false });

                shell.ShowTool<SampleToolViewModel>(new ViewRequest { ViewId = SampleToolView.SampleToolId });
                shell.ShowTool<SampleToolViewModel>(options: new UiShowOptions { Title = "Second Tools" });

                shell.MainMenuService.AddGlobalCommand(
                    commandPath: "_File$Open",
                    commandName: "Open...",
                    command: ReactiveCommand.Create(() => { }));

                shell.MainMenuService.AddGlobalCommand(
                    commandPath: "_File",
                    commandName: "E_xit",
                    command: ReactiveCommand.Create(() => { Environment.Exit(0); }),
                    hotKey: KeyGesture.Parse("Ctrl+Q"));
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}