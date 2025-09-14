using EventManagerMaui.Models;
using EventManagerMaui.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace EventManagerMaui;

public partial class AdminDashboardPage : ContentPage
{
    public ObservableCollection<EventItem> Events { get; set; }
    public ObservableCollection<EventItem> Tasks { get; set; }

    public AdminDashboardPage()
    {
        InitializeComponent();
        Events = new ObservableCollection<EventItem>();
        Tasks = new ObservableCollection<EventItem>();
        EventsCollectionView.ItemsSource = Events;
        TasksCollectionView.ItemsSource = Tasks;
        AddItemButton.Clicked += OnAddItemClicked;
        LogoutButton.Clicked += OnLogoutClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDashboardData();
    }

    private async Task LoadDashboardData()
    {
        // Load stats
        int eventCount = await DatabaseService.GetTotalEventsCountAsync();
        int userCount = await DatabaseService.GetTotalUsersCountAsync();
        int feedbackCount = await DatabaseService.GetTotalFeedbackCountAsync();

        TotalEventsLabel.Text = eventCount.ToString();
        TotalUsersLabel.Text = userCount.ToString();
        TotalFeedbackLabel.Text = feedbackCount.ToString();

        // Load items list
        var allItems = await DatabaseService.GetAllItemsAsync();

        Events.Clear();
        Tasks.Clear();

        foreach (var item in allItems)
        {
            if (item.Type == "event")
            {
                Events.Add(item);
            }
            else
            {
                Tasks.Add(item);
            }
        }
    }

    private async void OnAddItemClicked(object? sender, System.EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TitleEntry.Text) && TypePicker.SelectedIndex != -1)
        {
            var newItem = new EventItem
            {
                Title = TitleEntry.Text,
                Type = (TypePicker.SelectedItem.ToString() ?? "task").ToLower(),
                EventDate = System.DateTime.Now
            };

            await DatabaseService.AddItemAsync(newItem);
            await DisplayAlert("Success", "New item added successfully!", "OK");

            TitleEntry.Text = string.Empty;
            TypePicker.SelectedIndex = -1;
            await LoadDashboardData(); // Reload all data
        }
        else
        {
            await DisplayAlert("Error", "Please enter a title and select a type.", "OK");
        }
    }

    // --- NEW METHOD TO VIEW FEEDBACK ---
    private async void OnViewFeedbackClicked(object? sender, System.EventArgs e)
    {
        var button = sender as Button;
        var eventItem = button?.BindingContext as EventItem;
        if (eventItem != null)
        {
            await Navigation.PushAsync(new AdminFeedbackPage(eventItem));
        }
    }

    private async void OnLogoutClicked(object? sender, System.EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}