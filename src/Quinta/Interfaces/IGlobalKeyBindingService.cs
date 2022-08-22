using System.Windows.Input;
using Avalonia.Input;

namespace Quinta.Interfaces;

public interface IGlobalKeyBindingService
{
    void AddKeyBinding(KeyGesture hotKey, ICommand command);
    void RemoveKeyBinding(KeyGesture hotKey);
}