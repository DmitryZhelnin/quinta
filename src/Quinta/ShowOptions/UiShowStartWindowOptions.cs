using Dock.Model.Core;

namespace Quinta.ShowOptions;

public class UiShowStartWindowOptions : UiShowOptions
{
    public IFactory? DockFactory { get; set; }

    public string LayoutFilePath { get; set; }

    public string? IconSource { get; set; }
}