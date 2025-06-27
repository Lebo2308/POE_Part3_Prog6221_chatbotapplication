namespace POE_Part3_Prog6221_chatbotapplication
{
    public class Chatbot
    {
        // Dictionary to store keywords and associated chatbot replies
        private Dictionary<string, List<string>> replies;
        // List of common words to ignore during keyword matching
        private List<string> ignore;
        // Stores the user's name 
        public string userName;

        // Constructor initializes ignore list and keyword-reply dictionary
        public Chatbot()
        {
        }//end of constructor
    }
}