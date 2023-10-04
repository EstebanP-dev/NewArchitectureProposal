using Acr.UserDialogs;
using Presentation.Abstractions.Dependencies;

namespace Presentation.Abstractions.Mvvm;

public interface IViewModelBase : ITransientDependency
{
    void Initialize();
    void NavigateTo();
    void NavigateFrom();
    void SetParent(Element parent);
}
