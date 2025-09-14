using EventManagerMaui.Models;
using EventManagerMaui.Services;

namespace EventManagerMaui;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        RegisterButton.Clicked += OnRegisterClicked;
    }

    private async void OnRegisterClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(FullNameEntry.Text) ||
            string.IsNullOrWhiteSpace(EmailEntry.Text) ||
            string.IsNullOrWhiteSpace(UsernameEntry.Text) ||
            string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            await DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }
        var newUser = new User
        {
            FullName = FullNameEntry.Text,
            Email = EmailEntry.Text,
            Username = UsernameEntry.Text,
            Password = PasswordEntry.Text
        };
        string result = await DatabaseService.RegisterUserAsync(newUser);
        if (result == "Registration successful!")
        {
            await DisplayAlert("Success", result, "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", result, "OK");
        }
    }

    private async void OnLoginTapped(object? sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}