using Presentation.Abstractions.Pages;

namespace Presentation.Features.Auth.Login;

public partial class LoginPage : ContentPageBase
{
	public LoginPage(LoginViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
		EmailField.Focus();
	}
}