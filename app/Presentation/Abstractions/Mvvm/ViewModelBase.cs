using Acr.UserDialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace Presentation.Abstractions.Mvvm;

public abstract partial class ViewModelBase : ObservableObject, IViewModelBase
{
    protected IUserDialogs Dialogs { get; init; }

    [ObservableProperty]
    protected Element parent;

    [ObservableProperty]
    protected bool isBusy;

    public ViewModelBase()
    {
#if IOS || ANDROID
        Dialogs = UserDialogs.Instance;
#endif
    }

    public virtual void Initialize()
    {
    }

    public virtual void NavigateFrom()
    {
    }

    public virtual void NavigateTo()
    {
    }

    public void SetParent(Element parent)
    {
        Parent = parent;
    }
    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);

        switch (e.PropertyName)
        {
            case nameof(IsBusy):
                if (IsBusy)
                {
                    Dialogs.ShowLoading("Loading", MaskType.Black);
                }
                else
                {
                    Dialogs.HideLoading();
                }
                break;
        }
    }
}
