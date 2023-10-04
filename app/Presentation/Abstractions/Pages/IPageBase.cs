using Presentation.Abstractions.Dependencies;
using Presentation.Abstractions.Mvvm;

namespace Presentation.Abstractions.Pages;

public interface IPageBase : ITransientDependency
{
    IViewModelBase ViewModel { get; }
}
