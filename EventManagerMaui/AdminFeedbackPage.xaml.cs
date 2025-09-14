using EventManagerMaui.Models;
using EventManagerMaui.Services;
using System.Linq;

namespace EventManagerMaui;

public partial class AdminFeedbackPage : ContentPage
{
    private EventItem _eventItem;

    public AdminFeedbackPage(EventItem eventItem)
    {
        InitializeComponent();
        _eventItem = eventItem;
        BackButton.Clicked += OnBackClicked;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadFeedback();
    }

    private async Task LoadFeedback()
    {
        EventTitleLabel.Text = $"Feedback for: \"{_eventItem.Title}\"";

        // Fetch all feedback for this specific event
        var feedbackList = await DatabaseService.GetFeedbackForEventAsync(_eventItem.Id);

        if (feedbackList.Any())
        {
            // If feedback exists, calculate the average rating
            double average = feedbackList.Average(f => f.Rating);
            AverageRatingLabel.Text = average.ToString("0.0");
            TotalFeedbackLabel.Text = $"based on {feedbackList.Count} reviews";
        }
        else
        {
            // If there is no feedback, show default text
            AverageRatingLabel.Text = "N/A";
            TotalFeedbackLabel.Text = "No reviews yet";
        }

        // Display the list of feedback in the UI
        FeedbackCollectionView.ItemsSource = feedbackList;
    }

    private async void OnBackClicked(object? sender, System.EventArgs e)
    {
        await Navigation.PopAsync();
    }
}