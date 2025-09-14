using EventManagerMaui.Models;
using EventManagerMaui.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace EventManagerMaui;

public partial class UserDashboardPage : ContentPage
{
    public ObservableCollection<EventItem> Events { get; set; }
    private User _currentUser;

    public UserDashboardPage(User user)
    {
        // --- THE FIX IS HERE ---
        // This line was missing. It is essential for building the UI.
        InitializeComponent();

        _currentUser = user;
        Events = new ObservableCollection<EventItem>();
        EventsCollectionView.ItemsSource = Events;
        WelcomeLabel.Text = $"Welcome, {user.Username}!";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        var allEvents = await DatabaseService.GetEventsAsync();
        var userRegistrations = await DatabaseService.GetRegistrationsForUserAsync(_currentUser.Id);
        var userFeedback = await DatabaseService.GetFeedbackForUserAsync(_currentUser.Id);

        Events.Clear();

        foreach (var ev in allEvents)
        {
            ev.IsRegistered = userRegistrations.Any(r => r.EventId == ev.Id);
            ev.HasGivenFeedback = userFeedback.Any(f => f.EventId == ev.Id);
            Events.Add(ev);
        }
    }

    private async void OnRegisterClicked(object? sender, System.EventArgs e)
    {
        var button = sender as Button;
        var eventItem = button?.BindingContext as EventItem;
        if (eventItem != null)
        {
            var registration = new Registration
            {
                UserId = _currentUser.Id,
                EventId = eventItem.Id
            };
            await DatabaseService.RegisterForEventAsync(registration);
            await DisplayAlert("Success", "You have successfully registered for the event!", "OK");
            await LoadEvents();
        }
    }

    private async void OnLeaveFeedbackClicked(object? sender, System.EventArgs e)
    {
        var button = sender as Button;
        var eventItem = button?.BindingContext as EventItem;
        if (eventItem != null)
        {
            await Navigation.PushAsync(new FeedbackPage(eventItem, _currentUser));
        }
    }

    private async void OnLogoutClicked(object? sender, System.EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}