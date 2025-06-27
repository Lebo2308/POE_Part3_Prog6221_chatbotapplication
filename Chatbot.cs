using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

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

        //Property to check if the user name has been set
        public bool IsNameSet => !string.IsNullOrEmpty(userName);

        //processing user interaction
        public List<TextBlock> ProcessInput(string input)
        {
            List<TextBlock> responses = new List<TextBlock>();

            // Trim input and return if empty
            input = input.Trim();
            if (string.IsNullOrEmpty(input)) return responses;

            // If username is not set yet, take current input as username
            if (!IsNameSet)
            {
                userName = input;
                responses.Add(CreateColoredMessage("User", userName, Colors.Green));
                responses.Add(CreateColoredMessage("Chatbot", $"Hello {userName}, you can ask me questions about cybersecurity.", Colors.Blue));
                return responses;
            }
            // Add the user's message to the conversation
            responses.Add(CreateColoredMessage(userName, input, Colors.Green));

            string lowerInput = input.ToLower();
            // Handle exit command
            if (lowerInput == "exit")
            {
                responses.Add(CreateColoredMessage("Chatbot", $"Goodbye {userName}! Thank you for using Molebogeng's Cybersecurity Chatbot Program.", Colors.Blue));
                responses.Add(CreateColoredMessage("SYSTEM", "EXIT", Colors.Red)); // Special marker
                return responses;
            }
            //condition to check if interested
            if (lowerInput.Contains("interest"))
            {
                responses.Add(CreateColoredMessage("Chatbot", "Got it! I've noted your interest.", Colors.Blue));
            }
            // Filter out common ignored words
            string[] words = lowerInput.Split(' ');
            List<string> filteredWords = words.Where(word => !ignore.Contains(word)).ToList();

            // Try to find matching replies from the dictionary
            List<string> matchedReplies = new List<string>();
            bool found = false;
            Random rand = new Random();

            foreach (string word in filteredWords)
            {
                if (replies.ContainsKey(word))
                {
                    // Add a random reply from the matched keyword
                    matchedReplies.Add(replies[word][rand.Next(replies[word].Count)]);
                    found = true;
                }
            }

            // Add matched replies to the response list
            if (found)
            {
                foreach (var res in matchedReplies)
                {
                    responses.Add(CreateColoredMessage("Chatbot", res, Colors.Blue));
                }
                responses.Add(CreateColoredMessage("Chatbot", "Do you have another question, or would you like to type 'exit' to end our chat?", Colors.Blue));
            }
            else
            {
                // Add matched replies to the response list-error message
                responses.Add(CreateColoredMessage("Chatbot", "I'm not sure I understand that. Can you rephrase or ask something related to security?", Colors.Blue));
            }

            return responses;
        }

        //Creates a colored TextBlock for displaying messages in the GUI
        private TextBlock CreateColoredMessage(string speaker, string message, Color nameColor)
        {
            TextBlock textBlock = new TextBlock();
            Run nameRun = new Run(speaker + ": ")
            {
                Foreground = new SolidColorBrush(nameColor),
                FontWeight = FontWeights.Bold
            };

            Run messageRun = new Run(message);
            textBlock.Inlines.Add(nameRun);
            textBlock.Inlines.Add(messageRun);
            return textBlock;
        }

        //Stores predefined replies associated with specific cybersecurity-related keywords.
        private void store_replies()
        {
            replies.Add("you", new List<string> { "I'm good, thanks and you?" });
            replies.Add("purpose", new List<string> {
                "My purpose is to simulate real-life scenarios where users might encounter cyber threats and provide guidance on avoiding common traps." });
            replies.Add("password", new List<string> {
                "Passwords need to be at least 12 characters long.",
                "Use a mix of uppercase, lowercase, numbers, and special characters.",
                "Enable two-factor authentication.",
                "Avoid public Wi-Fi and keep software up to date."
            });
            replies.Add("phishing", new List<string> {
                "Phishing tricks you into revealing personal info.",
                "Check for spelling errors and verify senders.",
                "Never click suspicious links."
            });
            replies.Add("safe browsing", new List<string> {
                "Use HTTPS websites.",
                "Use a VPN and antivirus software."
            });
            replies.Add("worried", new List<string> { "It's okay to feel that way. Let me know what tip you’d like to stay safe online." });
            replies.Add("curious", new List<string> { "I’m here to help you understand and give safety tips." });
            replies.Add("frustrated", new List<string> { "It’s normal to feel that way. Let me offer some online safety tips." });
            replies.Add("happy", new List<string> { "That’s great! Stay safe and cautious online." });
            replies.Add("sad", new List<string> { "Sorry to hear that. I’m here with helpful tips to keep you secure online." });
        }

        //Stores common filler words that should be ignored during keyword matching.
        private void store_ignore()
        {
            ignore.AddRange(new[] { "what", "is", "your", "how", "are", "can", "i", "sports", "tell", "me", "about", "more", "my", "name", "is" });
        }
    }
}