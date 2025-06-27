using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace POE_Part3_Prog6221_chatbotapplication
{
    // This class handles creating and storing reminders with descriptions and due dates
    public class get_reminder
    {
        // Lists to store task descriptions and their associated reminder dates
        private List<string> descriptions = new List<string>();
        private List<string> dates = new List<string>();

        //Validates whether the user input(task description) is not empty or whitespace.
        public string validate_input(string user_input)
        {
            return string.IsNullOrWhiteSpace(user_input) ? "Please add a task" : "found";
        }

        public string get_days(string input)
        {
            // Remove all non-digit characters
            string digits = Regex.Replace(input, @"[^\d]", "");
            return string.IsNullOrEmpty(digits) || digits == "0" ? "today" : digits;
        }

        // Saves a reminder for today if the day input is "today"
        public string today_date(string description, string day)
        {
            // Get today's date in yyyy-MM-dd format
            if (day != "today") return "error";

            string date = DateTime.Now.Date.ToString("yyyy-MM-dd");
            descriptions.Add(description);
            dates.Add(date);
            return "Nice, will remind you today.";
        }

        // Adds a future reminder by parsing the number of days from the input.
        public string get_remindDate(string description, string day)
        {
            if (!int.TryParse(day, out int days)) return "error";
            // Calculate future date and add the reminder
            DateTime futureDate = DateTime.Now.Date.AddDays(days);
            string formattedDate = futureDate.ToString("yyyy-MM-dd");

            descriptions.Add(description);
            dates.Add(formattedDate);
            return "done";
        }

        // Returns all stored reminders, indicating which ones are due today.
        public string get_remind()
        {
            DateTime today = DateTime.Now.Date;
            string todayStr = today.ToString("yyyy-MM-dd");

            string result = "";
            // Loop through all saved reminders
            for (int i = 0; i < dates.Count; i++)
            {
                // Add a "DUE TODAY" prefix if reminder is due today
                string prefix = dates[i] == todayStr ? "DUE TODAY: " : "";
                result += $"{prefix}{descriptions[i]} ({dates[i]})\n";
            }
            return result;
        }

    }//end of class
}//end of namespace