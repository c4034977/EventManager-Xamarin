using EventManagerMaui.Models;
using EventManagerMaui.Services;

namespace EventManagerMaui;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        SignInButton.Clicked += OnSignInClicked;
        var registerLabel = this.FindByName<Label>("RegisterLabel");
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnRegisterTapped;
        registerLabel.GestureRecognizers.Add(tapGesture);
    }

    private async void OnSignInClicked(object? sender, System.EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(UsernameEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "Username and password are required.", "OK");
            return;
        }
        var user = await DatabaseService.LoginUserAsync(UsernameEntry.Text, PasswordEntry.Text);
        if (user != null)
        {
            await Navigation.PushAsync(new UserDashboardPage(user));
        }
        else
        {
            await DisplayAlert("Login Failed", "Invalid username or password.", "Try Again");
        }
    }

    private async void OnRegisterTapped(object? sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }

    private async void OnBackTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}