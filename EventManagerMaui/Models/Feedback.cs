using SQLite;

namespace EventManagerMaui.Models
{
    public class Feedback
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = "";

        // --- NEW HELPER PROPERTY FOR THE UI ---
        [Ignore] // This tells SQLite not to save this property in the database
        public string Username { get; set; } = "";
    }
}