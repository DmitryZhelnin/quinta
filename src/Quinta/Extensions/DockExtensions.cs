using Dock.Model.Core;

namespace Quinta.Extensions;

public static class DockExtensions
{
    public static IDockable? FindByViewRequest(this IDock dock, ViewRequest? request)
    {
        return request is null
            ? null
            : dock.VisibleDockables?.FirstOrDefault(x => x.Id == request.ViewId);
    }
}