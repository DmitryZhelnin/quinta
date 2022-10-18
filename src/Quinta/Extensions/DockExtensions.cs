using Dock.Model.Core;
using Quinta.ViewModels;

namespace Quinta.Extensions;

public static class DockExtensions
{
    public static IDockable? FindByViewRequest(this IDock dock, ViewRequest? request)
    {
        return request is null
            ? null
            : dock.VisibleDockables?.FirstOrDefault(x => x.Id == request.ViewId);
    }

    public static IDockable? FindById(this IDock dock, string id)
    {
        if (dock.VisibleDockables != null)
        {
            foreach (var child in dock.VisibleDockables)
            {
                if (child.Id == id)
                {
                    return child;
                }

                if (child is IDock childDock)
                {
                    var result = childDock.FindById(id);
                    if (result is not null)
                    {
                        return result;
                    }
                }
            }
        }

        return null;
    }

    public static IEnumerable<DocumentViewModelBase> GetAllDocuments(this IDock dock)
    {
        if (dock.VisibleDockables != null)
        {
            foreach (var dockable in dock.VisibleDockables)
            {
                if (dockable is DocumentViewModelBase document)
                {
                    yield return document;
                }

                if (dockable is IDock childDock)
                {
                    foreach (var childDocument in childDock.GetAllDocuments())
                    {
                        yield return childDocument;
                    }
                }
            }
        }
    }
}
