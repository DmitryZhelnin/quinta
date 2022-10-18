using System.Collections;
using System.ComponentModel;
using System.Reactive.Linq;
using Dock.Model.Controls;
using DynamicData;
using DynamicData.Binding;
using Quinta.ShowOptions;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.Components.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Formatters;
using ReactiveUI.Validation.Formatters.Abstractions;

namespace Quinta.ViewModels;

public abstract class DocumentViewModelBase : ViewModelBase, IDocument, IConfigurableViewModel, IValidatableViewModel, INotifyDataErrorInfo
{
    private readonly HashSet<string> _mentionedPropertyNames = new();
    private readonly IValidationTextFormatter<string> _formatter;
    private bool _hasErrors;

    protected DocumentViewModelBase()
    {
        _formatter = SingleLineFormatter.Default;
        ValidationContext = new ValidationContext();
        ValidationContext.Validations
            .ToObservableChangeSet()
            .ToCollection()
            .Select(components => components
                .Select(component => component
                    .ValidationStatusChange
                    .Select(_ => component))
                .Merge()
                .StartWith(ValidationContext))
            .Switch()
            .Subscribe(OnValidationStatusChange);
    }

    /// <inheritdoc />
    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    /// <inheritdoc />
    public bool HasErrors
    {
        get => _hasErrors;
        private set => this.RaiseAndSetIfChanged(ref _hasErrors, value);
    }

    /// <inheritdoc />
    public ValidationContext ValidationContext { get; }

    /// <summary>
    /// Returns a collection of error messages, required by the INotifyDataErrorInfo interface.
    /// </summary>
    /// <param name="propertyName">Property to search error notifications for.</param>
    /// <returns>A list of error messages, usually strings.</returns>
    /// <inheritdoc />
    public virtual IEnumerable GetErrors(string? propertyName) =>
        propertyName is null || string.IsNullOrEmpty(propertyName)
            ? SelectInvalidPropertyValidations()
                .Select(state => _formatter.Format(state.Text ?? ValidationText.None))
                .ToArray()
            : SelectInvalidPropertyValidations()
                .Where(validation => validation.ContainsPropertyName(propertyName))
                .Select(state => _formatter.Format(state.Text ?? ValidationText.None))
                .ToArray();

    /// <summary>
    /// Raises the <see cref="ErrorsChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the validated property.</param>
    protected void RaiseErrorsChanged(string propertyName = "") =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));

    /// <summary>
    /// Selects validation components that are invalid.
    /// </summary>
    /// <returns>Returns the invalid property validations.</returns>
    private IEnumerable<IPropertyValidationComponent> SelectInvalidPropertyValidations() =>
        ValidationContext.Validations
            .OfType<IPropertyValidationComponent>()
            .Where(validation => !validation.IsValid);

    /// <summary>
    /// Updates the <see cref="HasErrors" /> property before raising the <see cref="ErrorsChanged" />
    /// event, and then raises the <see cref="ErrorsChanged" /> event. This behaviour is required by WPF, see:
    /// https://stackoverflow.com/questions/24518520/ui-not-calling-inotifydataerrorinfo-geterrors/24837028.
    /// </summary>
    /// <remarks>
    /// WPF doesn't understand string.Empty as an argument for the <see cref="ErrorsChanged"/>
    /// event, so we are sending <see cref="ErrorsChanged"/> notifications for every saved property.
    /// This is required for e.g. cases when a <see cref="IValidationComponent"/> is disposed and
    /// detached from the <see cref="ValidationContext"/>, and we'd like to mark all invalid
    /// properties as valid (because the thing that validates them no longer exists).
    /// </remarks>
    private void OnValidationStatusChange(IValidationComponent component)
    {
        HasErrors = !ValidationContext.GetIsValid();
        if (component is IPropertyValidationComponent propertyValidationComponent)
        {
            foreach (var propertyName in propertyValidationComponent.Properties)
            {
                RaiseErrorsChanged(propertyName);
                _mentionedPropertyNames.Add(propertyName);
            }
        }
        else
        {
            foreach (var propertyName in _mentionedPropertyNames)
            {
                RaiseErrorsChanged(propertyName);
            }
        }
    }

    public virtual void Configure(UiShowOptions options)
    {
        Title = options.Title;
        CanClose = options.CanClose;
        CanFloat = options.CanFloat;
    }
}
