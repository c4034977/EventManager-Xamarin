using SQLite;

namespace EventManagerMaui.Models
{
    public class Registration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
    }
}