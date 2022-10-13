using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Dock.Model.Core;
using Dock.Model.Core.Events;
using Dock.Model.ReactiveUI.Controls;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Quinta.DockFactories;
using Quinta.Extensions;
using Quinta.Interfaces;
using Quinta.ShowOptions;
using Quinta.ViewModels;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Quinta;

public class Shell : ReactiveObject, IShell
{
    private class DependencyInjectionContractResolver : DefaultContractResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public DependencyInjectionContractResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override JsonObjectContract CreateObjectContract(Type objectType)
        {
            var isService = _serviceProvider.GetService<IServiceProviderIsService>();
            if (isService is not null && isService.IsService(objectType))
            {
                var contract = base.CreateObjectContract(objectType);
                contract.DefaultCreator = () => _serviceProvider.GetService(objectType)!;
                return contract;
            }

            return base.CreateObjectContract(objectType);
        }
    }

    public Shell(IMainMenuService mainMenuService, IDialogService dialogService)
    {
        MainMenuService = mainMenuService;
        DialogService = dialogService;
    }

    [Reactive] public string Title { get; set; }

    [Reactive] public WindowIcon? Icon { get; set; }

    [Reactive] public IViewModel? ActiveViewModel { get; set; }

    public IServiceProvider ServiceProvider { get; set; }

    public IFactory DockFactory { get; set; }

    [Reactive] public IDock Layout { get; set; }

    public Window MainWindow { get; set; }

    public IMainMenuService MainMenuService { get; }

    public IDialogService DialogService { get; }

    public void ShowStartView<TStartWindow>(UiShowStartWindowOptions? options) where TStartWindow : class
    {
        JsonSerializerSettings jsonSettings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DependencyInjectionContractResolver(ServiceProvider)
        };

        Title = options?.Title ?? "";
        if (options?.IconSource is not null)
        {
            var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var iconStream = assets!.Open(new Uri(options.IconSource));
            Icon = new WindowIcon(iconStream);
        }

        var window = ServiceProvider.GetRequiredService<TStartWindow>();
        MainWindow = window as Window ?? throw new InvalidCastException($"{window.GetType()} is not a window");
        MainWindow.Closing += (_, _) =>
        {
            if (!string.IsNullOrWhiteSpace(options?.LayoutFilePath))
            {
                var json = JsonConvert.SerializeObject(Layout, jsonSettings);
                if (!string.IsNullOrEmpty(json))
                {
                    File.WriteAllText(options.LayoutFilePath, json);
                }
            }
        };

        DockFactory = options?.DockFactory ?? new DefaultDockFactory(MainWindow.DataContext!);
        Observable
            .FromEventPattern<FocusedDockableChangedEventArgs>(
                x => DockFactory.FocusedDockableChanged += x,
                x => DockFactory.FocusedDockableChanged -= x)
            .Subscribe(x =>
            {
                if (x.EventArgs.Dockable is IViewModel vm)
                    ActiveViewModel = vm;
            });

        InitLayout(options?.LayoutFilePath, jsonSettings);
    }

    public void ShowView<TViewModel>(ViewRequest? viewRequest, UiShowOptions? options)
        where TViewModel : class, IViewModel
    {
        ShowInContainer(DefaultDockFactory.Documents,
            container => container.GetRequiredService<TViewModel>(),
            viewRequest,
            options);
    }

    public void ShowTool<TViewModel>(ViewRequest? viewRequest = null, UiShowOptions? options = null)
        where TViewModel : class, IViewModel
    {
        ShowInContainer(DefaultDockFactory.Tools,
            container => container.GetRequiredService<TViewModel>(),
            viewRequest,
            options);
    }

    public void ShowInContainer(string containerName, Func<IServiceProvider, IViewModel> viewModelFactory,
        ViewRequest? viewRequest = null, UiShowOptions? options = null)
    {
        var container = DockFactory.GetDockable<IDock>(containerName);
        if (container is null)
        {
            throw new ArgumentException($"Container {containerName} not found");
        }

        var dockable = container.FindByViewRequest(viewRequest);

        if (dockable is null)
        {
            var viewModel = viewModelFactory(ServiceProvider);
            viewModel.Id = viewRequest?.ViewId ?? "";
            if (options != null && viewModel is IConfigurableViewModel configurable)
                configurable.Configure(options);

            DockFactory.AddDockable(container, viewModel);

            AddClosingByCommand(viewModel);
            // InitializeView(view, viewRequest);
            dockable = viewModel;
        }
        else
        {
            if (options != null && dockable is IConfigurableViewModel configurable)
                configurable.Configure(options);
        }

        DockFactory.SetActiveDockable(dockable);
        DockFactory.SetFocusedDockable(container, dockable);
    }

    private void AddClosingByCommand(IViewModel viewModel)
    {
        if (viewModel is not ViewModelBase vmb)
        {
            return;
        }

        vmb.Close
            .Subscribe(_ => DockFactory.CloseDockable(vmb))
            .DisposeWith(vmb.Disposables);
    }

    private void InitLayout(string? layoutFilePath, JsonSerializerSettings jsonSettings)
    {
        RootDock? rootDock = default;
        if (!string.IsNullOrWhiteSpace(layoutFilePath) && File.Exists(layoutFilePath))
        {
            try
            {
                var json = File.ReadAllText(layoutFilePath);
                if (!string.IsNullOrWhiteSpace(json))
                {

                    rootDock = JsonConvert.DeserializeObject<RootDock?>(json, jsonSettings);
                    if (rootDock is not null)
                    {
                        Layout = rootDock;
                    }
                }
            }
            catch
            {
                rootDock = null;
            }
        }

        if (rootDock is null)
        {
            Layout = DockFactory.CreateLayout() ?? throw new InvalidOperationException();
        }

        DockFactory.InitLayout(Layout);
    }
}
