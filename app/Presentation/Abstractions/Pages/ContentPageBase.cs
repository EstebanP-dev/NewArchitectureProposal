using Presentation.Abstractions.Mvvm;

namespace Presentation.Abstractions.Pages;

public class ContentPageBase : ContentPage, IPageBase
{
    private bool _isLoaded = false;
    public IViewModelBase ViewModel { get; }

    public ContentPageBase(IViewModelBase viewModel)
    {
        ViewModel = viewModel;
        ViewModel?.SetParent(this);
        BindingContext = viewModel;
    }

    public ContentPageBase()
    {
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        ViewModel?.NavigateTo();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        ViewModel?.NavigateFrom();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (!_isLoaded)
        {
            _isLoaded = true;
            ViewModel?.Initialize();
        }
    }
}
