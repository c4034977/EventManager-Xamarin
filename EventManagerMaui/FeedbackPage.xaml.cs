using EventManagerMaui.Models;
using EventManagerMaui.Services;
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace EventManagerMaui
{
    public partial class FeedbackPage : ContentPage
    {
        private EventItem _eventItem;
        private User _currentUser;
        private int _currentRating = 0;

        public FeedbackPage(EventItem eventItem, User user)
        {
            InitializeComponent();
            _eventItem = eventItem;
            _currentUser = user;

            EventTitleLabel.Text = $"Feedback for: {_eventItem.Title}";
            SubmitButton.Clicked += OnSubmitClicked;
            BackButton.Clicked += OnBackClicked;
            CreateStarRating(5);
        }

        private void CreateStarRating(int maxRating)
        {
            for (int i = 1; i <= maxRating; i++)
            {
                var starButton = new Button
                {
                    Text = "☆", // Empty star
                    FontSize = 36,
                    BackgroundColor = Colors.Transparent,
                    BorderColor = Colors.Transparent,
                    TextColor = Colors.Gray,
                    CommandParameter = i
                };
                starButton.Clicked += OnStarClicked;
                StarRatingLayout.Children.Add(starButton);
            }
        }

        private void OnStarClicked(object? sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                _currentRating = (int)button.CommandParameter;
                UpdateStarAppearance();
            }
        }

        private void UpdateStarAppearance()
        {
            for (int i = 0; i < StarRatingLayout.Children.Count; i++)
            {
                var starButton = StarRatingLayout.Children[i] as Button;
                if (starButton != null)
                {
                    if (i < _currentRating)
                    {
                        starButton.Text = "★"; // Filled star
                        starButton.TextColor = Colors.Gold;
                    }
                    else
                    {
                        starButton.Text = "☆"; // Empty star
                        starButton.TextColor = Colors.Gray;
                    }
                }
            }
        }

        private async void OnSubmitClicked(object? sender, EventArgs e)
        {
            if (_currentRating == 0)
            {
                await DisplayAlert("Error", "Please select a star rating.", "OK");
                return;
            }

            var feedback = new Feedback
            {
                UserId = _currentUser.Id,
                EventId = _eventItem.Id,
                Rating = _currentRating,
                Comment = CommentEditor.Text ?? ""
            };

            // --- THE FIX IS HERE ---
            // This line is now uncommented and will save the feedback.
            await DatabaseService.AddFeedbackAsync(feedback);

            await DisplayAlert("Success", "Thank you for your feedback!", "OK");
            await Navigation.PopAsync(); // Go back to the user dashboard
        }

        private async void OnBackClicked(object? sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}