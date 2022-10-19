using Material.Icons;

namespace Quinta.ViewModels;

public abstract class DocumentWithIconViewModelBase : DocumentViewModelBase
{
    public virtual MaterialIconKind? TitleIcon => null;
}