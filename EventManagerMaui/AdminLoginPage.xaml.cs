namespace EventManagerMaui;

public partial class AdminLoginPage : ContentPage
{
    private const string AdminUsername = "admin";
    private const string AdminPassword = "password123";

    public AdminLoginPage()
    {
        InitializeComponent();
        SignInButton.Clicked += OnSignInClicked;
    }

    private async void OnSignInClicked(object? sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        if (username == AdminUsername && password == AdminPassword)
        {
            await Navigation.PushAsync(new AdminDashboardPage());
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password.", "Try Again");
        }
    }

    private async void OnBackTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}