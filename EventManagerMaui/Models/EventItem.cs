using SQLite;

namespace EventManagerMaui.Models
{
    public class EventItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public System.DateTime EventDate { get; set; }

        [Ignore]
        public bool IsRegistered { get; set; }
        [Ignore]
        public bool HasGivenFeedback { get; set; }
        public bool IsNotRegistered => !IsRegistered;
        public bool CanLeaveFeedback => IsRegistered && !HasGivenFeedback;
        public bool IsEvent => Type == "event";
    }
}