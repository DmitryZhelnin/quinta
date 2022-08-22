using Avalonia.Controls;

namespace Quinta.ShowOptions;

public class UiShowDialogOptions
{
    public string Title { get; set; } = "";

    public string? IconSource { get; set; }

    public SizeToContent SizeToContent { get; set; } = SizeToContent.Manual;

    public double Width { get; set; } = 640;
    
    public double Height { get; set; } = 480;

    public bool ShowInTaskbar { get; set; } = true;
}