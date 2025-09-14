namespace EventManagerMaui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        GetStartedButton.Clicked += OnGetStartedClicked;
        AdminLoginButton.Clicked += OnAdminLoginClicked;
    }
    private async void OnGetStartedClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginPage());
    }
    private async void OnAdminLoginClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminLoginPage());
    }
}