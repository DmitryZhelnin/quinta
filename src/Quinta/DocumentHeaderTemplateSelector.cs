using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Dock.Model.Controls;
using Quinta.ViewModels;

namespace Quinta;

public class DocumentHeaderTemplateSelector : IDataTemplate
{
    [Content]
    public Dictionary<Type, IDataTemplate> AvailableTemplates { get; } = new();

    public IControl Build(object data)
    {
        return data switch
        {
            DocumentWithIconViewModelBase => AvailableTemplates[typeof(DocumentWithIconViewModelBase)].Build(data),
            IDocument => AvailableTemplates[typeof(IDocument)].Build(data),
            _ => throw new ArgumentOutOfRangeException(nameof(data), "Unknown data type")
        };
    }

    public bool Match(object data)
    {
        return data is IDocument;
    }
}
