using Avalonia.Data;
using Dock.Avalonia.Controls;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.ReactiveUI;
using Dock.Model.ReactiveUI.Controls;

namespace Quinta.DockFactories;

public class DefaultDockFactory : Factory
{
    public const string Tools = "Tools";
    public const string Documents = "Documents";
    public const string MainLayout = "MainLayout";
    public const string Splitter = "Splitter";
    public const string Root = "Root";

    private IRootDock _rootDock;
    private IDocumentDock _documentDock;
    private IToolDock _toolDock;
    
    private readonly object _context;

    public DefaultDockFactory(object context)
    {
        _context = context;
    }

    public override IRootDock CreateLayout()
    {
        var toolDock = new ToolDock
        {
            Id = Tools,
            Title = Tools,
            ActiveDockable = null,
            IsCollapsable = false,
            CanFloat = false,
            Proportion = 0.25,
            VisibleDockables = CreateList<IDockable>(),
            Alignment = Alignment.Left,
            GripMode = GripMode.Visible
        };
        
        var documentDock = new DocumentDock
        {
            Id = Documents,
            Title = Documents,
            IsCollapsable = false,
            Proportion = double.NaN,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>()
        };
        
        var mainLayout = new ProportionalDock
        {
            Id = MainLayout,
            Title = MainLayout,
            IsCollapsable = false,
            Proportion = double.NaN,
            Orientation = Orientation.Horizontal,
            ActiveDockable = null,
            VisibleDockables = CreateList<IDockable>
            (
                toolDock,
                new ProportionalDockSplitter
                {
                    Id = Splitter,
                    Title = Splitter
                },
                documentDock
            )
        };

        var root = CreateRootDock();
        
        root.Id = Root;
        root.Title = Root;
        root.ActiveDockable = mainLayout;
        root.DefaultDockable = mainLayout;
        root.VisibleDockables = CreateList<IDockable>(mainLayout);

        _rootDock = root;
        _documentDock = documentDock;
        _toolDock = toolDock;

        return root;
    }

    public override void InitLayout(IDockable layout)
    {
         ContextLocator = new Dictionary<string, Func<object>>
        {
            [Tools] = () => _context,
            [Documents] = () => _context,
            [MainLayout] = () => _context,
            [Root] = () => _context,

        };

        DockableLocator = new Dictionary<string, Func<IDockable?>>
        {
            [Tools] = () => _toolDock,
            [Documents] = () => _documentDock,
            [Root] = () => _rootDock
        };

        HostWindowLocator = new Dictionary<string, Func<IHostWindow>>
        {
            [nameof(IDockWindow)] = () =>
            {
                var hostWindow = new HostWindow
                {
                    [!HostWindow.TitleProperty] = new Binding("ActiveDockable.Title")
                };
                return hostWindow;
            }
        };

        base.InitLayout(layout);
    }
}