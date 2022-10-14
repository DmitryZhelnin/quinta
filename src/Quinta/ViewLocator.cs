using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Dock.Model.Core;
using ReactiveUI;

namespace Quinta;

public class ViewLocator : IDataTemplate
{
    private readonly Dictionary<Type, Type> _registry = new();

    public IControl Build(object data)
    {
        var viewModelType = data.GetType();
        if (_registry.TryGetValue(viewModelType, out var viewType))
        {
            return (Control)Activator.CreateInstance(viewType)!;
        }

        var name = viewModelType.AssemblyQualifiedName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type)!;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object data)
    {
        return data is ReactiveObject || data is IDockable;
    }

    public void Register<TViewModel, TView>() where TView : Control
    {
        _registry[typeof(TViewModel)] = typeof(TView);
    }
}
