using Presentation.Abstractions.Pages;

namespace Presentation.Features.Main;

public partial class MainPage : ContentPageBase
{
    int count = 0;

    public MainPage()
        : base()
    {
        InitializeComponent();
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        //count++;

        //if (count == 1)
        //    CounterBtn.Text = $"Clicked {count} time";
        //else
        //    CounterBtn.Text = $"Clicked {count} times";

        //SemanticScreenReader.Announce(CounterBtn.Text);

        await Shell.Current.GoToAsync("/LoginPage");
    }
}