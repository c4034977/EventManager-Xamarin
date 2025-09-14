using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EventManagerMaui.Models;

namespace EventManagerMaui.Services
{
    public class DatabaseService
    {
        private static SQLiteAsyncConnection? _database;

        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), "EventManager.db3");
                    _database = new SQLiteAsyncConnection(dbPath);
                    _database.CreateTableAsync<User>().Wait();
                    _database.CreateTableAsync<EventItem>().Wait();
                    _database.CreateTableAsync<Registration>().Wait();
                    _database.CreateTableAsync<Feedback>().Wait();
                }
                return _database;
            }
        }

        // --- User Methods ---
        public static async Task<string> RegisterUserAsync(User user)
        {
            var existingUser = await Database.Table<User>().Where(u => u.Username == user.Username || u.Email == user.Email).FirstOrDefaultAsync();
            if (existingUser != null) { return "Username or email already exists."; }
            await Database.InsertAsync(user);
            return "Registration successful!";
        }

        public static async Task<User?> LoginUserAsync(string username, string password)
        {
            var user = await Database.Table<User>().Where(u => u.Username == username).FirstOrDefaultAsync();
            if (user != null && user.Password == password) { return user; }
            return null;
        }

        // --- Admin Methods ---
        public static Task<int> AddItemAsync(EventItem item)
        {
            return Database.InsertAsync(item);
        }

        public static Task<List<EventItem>> GetAllItemsAsync()
        {
            return Database.Table<EventItem>().OrderByDescending(i => i.Id).ToListAsync();
        }

        // --- THIS IS THE FIRST MISSING METHOD ---
        public static async Task<List<Feedback>> GetFeedbackForEventAsync(int eventId)
        {
            var feedbacks = await Database.Table<Feedback>().Where(f => f.EventId == eventId).ToListAsync();
            foreach (var feedback in feedbacks)
            {
                var user = await Database.Table<User>().Where(u => u.Id == feedback.UserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    feedback.Username = user.Username;
                }
            }
            return feedbacks;
        }

        // --- Stat Methods for Admin ---
        public static Task<int> GetTotalEventsCountAsync()
        {
            return Database.Table<EventItem>().Where(i => i.Type == "event").CountAsync();
        }
        public static Task<int> GetTotalUsersCountAsync()
        {
            return Database.Table<User>().CountAsync();
        }
        public static Task<int> GetTotalFeedbackCountAsync()
        {
            return Database.Table<Feedback>().CountAsync();
        }

        // --- User Dashboard Methods ---
        public static Task<List<EventItem>> GetEventsAsync()
        {
            return Database.Table<EventItem>().Where(i => i.Type == "event").OrderBy(i => i.EventDate).ToListAsync();
        }
        public static Task<List<Registration>> GetRegistrationsForUserAsync(int userId)
        {
            return Database.Table<Registration>().Where(r => r.UserId == userId).ToListAsync();
        }
        public static Task<int> RegisterForEventAsync(Registration registration)
        {
            return Database.InsertAsync(registration);
        }
        public static Task<List<Feedback>> GetFeedbackForUserAsync(int userId)
        {
            return Database.Table<Feedback>().Where(f => f.UserId == userId).ToListAsync();
        }

        // --- THIS IS THE SECOND MISSING METHOD ---
        public static Task<int> AddFeedbackAsync(Feedback feedback)
        {
            return Database.InsertAsync(feedback);
        }
    }
}