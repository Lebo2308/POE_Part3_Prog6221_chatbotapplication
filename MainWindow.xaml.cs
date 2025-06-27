using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using sound_playing;
//import of the NPL
using Microsoft.ML;
using Microsoft.ML.Data;

namespace POE_Part3_Prog6221_chatbotapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //global declaration for all instances and variables

        //chatbot class
        Chatbot chatbot = new Chatbot();

        //reminder
        //creating an instance of the get_reminder class [ global ]
        get_reminder remind = new get_reminder();
        //temp variable
        string hold_task = string.Empty;

        //quiz
        //List[generics]
        private List<QuizQuestion> quizData; //the class QuizQuestion will have the getters and setters 
        //variables
        private int questionIndex = 0;
        private int currentScore = 0;
        //buttons
        private Button selectedChoice = null;//user selection
        private Button correctChoiceButton = null;

        //activity log
        private List<string> activityLog = new List<string>();

        //NLP 
        // Input and output data classes
        private class Data
        {
            public string Text { get; set; }
            public bool Label { get; set; }
        }

        private class Prediction
        {
            [ColumnName("PredictedLabel")]
            public bool Prediction { get; set; }
            public float Probability { get; set; }
            public float Score { get; set; }
        }

        private readonly MLContext mlContext;
        private PredictionEngine<Data, Prediction> predictionEngine;

        public MainWindow()
        {
            InitializeComponent();

            // Provide full path to the audio file
            string audioPath = "C:\\Users\\RC_Student_lab\\source\\repos\\sound_playing\\CYBERREC.wav";
            voice_greeting greeting = new voice_greeting(audioPath);
            greeting.PlayGreeting();

            //chat when log in
            chat_chatbot.Items.Add(CreateStartupMessage("Welcome to Molebogeng's Cybersecurity Chatbot Program!", Colors.Blue));
            chat_chatbot.Items.Add(CreateStartupMessage("Please enter your name to start.", Colors.Blue));

            //calling the LoadQuizData method
            LoadQuizData();

            showQuiz();

            //natural processing language
            mlContext = new MLContext(seed: 0);
            TrainModel();
        }

        //button to open the main page
        private void open_main(object sender, RoutedEventArgs e)
        {
            //changing pages using the visible and hidden
            //hide the logo_login grid when button is clicked
            logo_login.Visibility = Visibility.Hidden;
            //set the main page grid to be visible 
            main_page.Visibility = Visibility.Visible;
        }

        //button to open the chatbot page in the main page
        private void chats(object sender, RoutedEventArgs e)
        {
            //hide other pages and set the current one visible
            reminder_page.Visibility = Visibility.Hidden;
            quiz_page.Visibility = Visibility.Hidden;
            activity_page.Visibility = Visibility.Hidden;

            //current page and set it to be visible
            chats_page.Visibility = Visibility.Visible;
        }

        //button to open the reminder page in the main page
        private void reminder(object sender, RoutedEventArgs e)
        {
            //hide other pages and set the current one visible
            chats_page.Visibility = Visibility.Hidden;
            quiz_page.Visibility = Visibility.Hidden;
            activity_page.Visibility = Visibility.Hidden;

            //current page and set it to be visible
            reminder_page.Visibility = Visibility.Visible;
        }

        //button to open the quiz/mini game page in the main page
        private void quiz(object sender, RoutedEventArgs e)
        {
            //hide other pages and set the current one visible
            chats_page.Visibility = Visibility.Hidden;
            reminder_page.Visibility = Visibility.Hidden;
            activity_page.Visibility = Visibility.Hidden;

            //current page and set it to be visible
            quiz_page.Visibility = Visibility.Visible;
        }

        //button to open the activity log page in the main page
        private void activity(object sender, RoutedEventArgs e)
        {
            //hide other pages and set the current one visible
            chats_page.Visibility = Visibility.Hidden;
            reminder_page.Visibility = Visibility.Hidden;
            quiz_page.Visibility = Visibility.Hidden;

            //current page and set it to be visible
            activity_page.Visibility = Visibility.Visible;
        }

        //button to logout/close the main page  to go to the login page
        private void close_main(object sender, RoutedEventArgs e)
        {
            //hide the login grid when button is clicked
            main_page.Visibility = Visibility.Hidden;
            //set the chat page grid to be visible 
            logo_login.Visibility = Visibility.Visible;

        }

        //button to exist the main program, exist the whole program
        private void exits(object sender, RoutedEventArgs e)
        {
            //close the application
            System.Environment.Exit(0);

        }

        //button to submit the user interaction in the chatbot page
        private void chatbot_button(object sender, RoutedEventArgs e)
        {
            string input = chatbot_user.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            var responses = chatbot.ProcessInput(input);
            foreach (var item in responses)
            {
                chat_chatbot.Items.Add(item);
                if (item.Text.Contains("EXIT"))
                {
                    Application.Current.Shutdown();
                }
            }

            chatbot_user.Clear();

            LogActivity($"User interacted with chatbot: \"{input}\"");//to hold for activity log

        }

        private TextBlock CreateStartupMessage(string message, Color color)
        {
            return new TextBlock
            {
                Inlines = {
                new Run("Chatbot: ") { Foreground = new SolidColorBrush(color), FontWeight = FontWeights.Bold },
                new Run(message)
            }
            };
        }

        //button to submit the user interaction in the reminder page
        private void set_reminder(object sender, RoutedEventArgs e)
        {
            string userInput = user_tasks.Text.Trim();

            if (string.IsNullOrEmpty(userInput))
            {
                MessageBox.Show("Question field is required!");
                return;
            }

            // Display user input in listview
            chat_append.Items.Add("User : " + userInput);
            LogActivity($"User typed in reminder input: \"{userInput}\"");//to hold for activity log
            // ADD TASK
            if (userInput.ToLower().Contains("add task"))
            {
                string description = userInput.Replace("add task", "").Trim();
                hold_task = description;

                DateTime now = DateTime.Now;
                string date = now.ToString("yyyy-MM-dd");
                string time = now.ToString("HH:mm");

                chat_append.Items.Add($"Chatbot: Task added - \"{description}\" on, date: {date} at {time}. Would you like a reminder?");
                LogActivity($"Added a task: \"{description}\" on {date} at {time}");
            }

            // SET REMINDER
            else if (userInput.ToLower().Contains("remind"))
            {
                if (!string.IsNullOrEmpty(hold_task))
                {
                    string days = remind.get_days(userInput);
                    string response = (days == "today")
                        ? remind.today_date(hold_task, days)
                        : remind.get_remindDate(hold_task, days) == "done"
                            ? "Great, I will remind you in " + days + " days."
                            : "Failed to set reminder.";

                    chat_append.Items.Add("Chatbot: " + response);
                    LogActivity($"Set reminder for task: \"{hold_task}\" in {days} days");//to hold for activity log
                }
                else
                {
                    chat_append.Items.Add("Chatbot: No task was set to remind you.");
                }
            }
            // SHOW REMINDERS
            else if (userInput.ToLower().Contains("what did you do for me"))
            {
                chat_append.Items.Add("Chatbot: Your reminders:");
                chat_append.Items.Add(remind.get_remind());
                LogActivity("User viewed their reminders list.");//to hold for activity log
            }

            chat_append.ScrollIntoView(chat_append.Items[chat_append.Items.Count - 1]);
            user_tasks.Clear();

        }

        //eventhandle for mousedouble clicking
        private void show_chats_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selected = chat_append.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selected)) return;

            if (!selected.Contains("STATUS DONE!"))
            {
                int index = chat_append.Items.IndexOf(selected);
                chat_append.Items[index] = selected + " STATUS DONE!";
            }
            else
            {
                chat_append.Items.Remove(selected);
            }
        }

        //quiz
        //method to show the quiz data on the buttons
        private void showQuiz()
        {
            //check if user is not done playing
            if (questionIndex >= quizData.Count)
            {
                //show complete message
                MessageBox.Show("You have completed the game with " + currentScore + "/10 score");
                //then reset the game
                currentScore = 0;
                currentScore = 0;
                questionIndex = 0;
                DisplayScore.Text = "";
                showQuiz();
                //stop the execute
                return;
            }

            //get the current index quiz
            correctChoiceButton = null;
            selectedChoice = null;

            // then get all the question values
            var currentQuiz = quizData[questionIndex];

            //display the question to the user
            DisplayedQuestion.Text = currentQuiz.Question;

            //add the choices to the buttons
            var shuffled = currentQuiz.Choices.OrderBy(_ => Guid.NewGuid()).ToList();

            //then add by index
            FirstChoiceButton.Content = shuffled[0];
            SecondChoiceButton.Content = shuffled[1];
            ThirdChoiceButton.Content = shuffled[2];
            //correct one
            FourthChoiceButton.Content = currentQuiz.CorrectChoice;

            //calling clear style method 
            clearStyle();

        }//end of showquiz method

        //method to reset buttons
        private void clearStyle()
        {
            //use foreach to reset
            foreach (Button choice in new[] { FirstChoiceButton, SecondChoiceButton, ThirdChoiceButton, FourthChoiceButton }) //you can put var or button
            {
                choice.Background = Brushes.LightGray;
            }

        }//end of clearStyle method

        //method to load the quiz data
        private void LoadQuizData()
        {
            //store info
            quizData = new List<QuizQuestion>
            {
                //adding questions and answers
                new QuizQuestion
                { 
                    //adding the question
                    Question="What is phishing?",
                    CorrectChoice="A cyberattack using misleading emails to steal information",
                    Choices = new List<string>
                    {
                        "Encrypting files for security", "Safe password storage","Going to the pond to get fish"
                    }
                } ,
                //adding questions and answers
                new QuizQuestion
                { 
                    //adding the question
                    Question="What is baiting in social engineering?",
                    CorrectChoice="Tempting with freebies to get access",
                    Choices = new List<string>
                    {
                        "Guilt-tripping someone","Creating fake surveys","Locking a screen"
                    }
                } ,
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="What is the best response to a fake tech support call?",
                    CorrectChoice="Hang up immediately",
                    Choices = new List<string>
                    {
                        "Ask for ID","Give fake details","Let them fix your PC"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="True or False: Social engineering relies on human interaction rather than technical hacking",
                    CorrectChoice="True",
                    Choices = new List<string>
                    {
                        "None","Both-True and False","False"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="True or False: Public Wi-Fi is safe to use for online banking if the network requires a password.",
                    CorrectChoice="False",
                    Choices = new List<string>
                    {
                        "True","Neither","Both-True and False"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="What’s the safest way to share login info?",
                    CorrectChoice="Use shared access tools",
                    Choices = new List<string>
                    {
                        "Text it to your friend", "Write it down", "None—just trust them"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="How often should you change passwords?",
                    CorrectChoice="Every 3–6 months",
                    Choices = new List<string>
                    {
                        "Only when forced","Every few years"," Never"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="Why is using public Wi-Fi risky?",
                    CorrectChoice="Exposes your data to hackers",
                    Choices = new List<string>
                    {
                        "Drains your battery","Slows your browsing" , "Blocks websites"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="True or False: If a website has a padlock icon in the address bar, it is 100% safe",
                    CorrectChoice="False",
                    Choices = new List<string>
                    {
                        "True","Neither","Both-True and False"
                    }
                },
                //adding another question
                new QuizQuestion
                { 
                    //adding the question
                    Question="What does 2FA provide?",
                    CorrectChoice="A second layer of security",
                    Choices = new List<string>
                    {
                        "Automatic password reset","Easier login ","Password sharing"
                    }
                }

            };

        }//end of method LoadQuizData method of info

        //event handler to select answer
        private void HandleAnswerSelection(object sender, RoutedEventArgs e)
        {
            //use sender object name to get the selected button
            selectedChoice = sender as Button;

            string chosen = selectedChoice.Content.ToString();

            //then check with the correct on the current quiz
            string correct = quizData[questionIndex].CorrectChoice;

            //then check if correct or not by if statement
            if (chosen == correct)
            {
                //then set the button background colour
                selectedChoice.Background = Brushes.Green;
                //assigning to hold
                correctChoiceButton = selectedChoice;
            }
            else
            {
                //if incorrect
                selectedChoice.Background = Brushes.DarkRed;
                correctChoiceButton = selectedChoice;
            }

            LogActivity($"Answered quiz: \"{quizData[questionIndex].Question}\" | Selected: \"{chosen}\" with score of 9");

        }//end of handle answer selection event handler

        //event handler for the next button
        private void HandleNextQuestion(object sender, RoutedEventArgs e)
        {
            //check if the user selected one of the choices
            if (selectedChoice == null)
            {
                //then show error message
                MessageBox.Show("Choose one of the 4 choices");
            }
            else
            {
                //then add points and only if correct
                string chosen = selectedChoice?.Content.ToString();
                string correct = quizData[questionIndex].CorrectChoice;

                //check if correct
                if (chosen == correct)
                {
                    //then add point
                    currentScore++;
                    //then show the score
                    DisplayScore.Text = "Score : " + currentScore;
                    //then move to the next index question
                    questionIndex++;
                    //show the question again for the next one
                    showQuiz();
                }
                else
                {
                    //move to the next question
                    questionIndex++;
                    showQuiz();
                }

            }
        }//end of next button event handler

        //buutton to see the activity
        private void see_activity(object sender, RoutedEventArgs e)
        {
            activity_append.Items.Clear(); // clear any old entries
            foreach (string entry in activityLog)
            {
                activity_append.Items.Add(entry);
            }
        }

        // method to add entries to the log
        private void LogActivity(string entry)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            activityLog.Add($"{timestamp} - {entry}");
        }

        //implementing the Natural Processing Language
        private void TrainModel()
        {
            var trainData = new[]
            {
                new Data { Text = "add a task", Label = true },
                new Data { Text = "set a reminder", Label = true },
                new Data { Text = "can you remind me to update my password?", Label = true },
                new Data { Text = "can i add task to", Label = false }
            };
            var dataView = mlContext.Data.LoadFromEnumerable(trainData);

            var pipeline = mlContext.Transforms.Text
                    .FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(Data.Text))
                .Append(mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression(labelColumnName: nameof(Data.Label), featureColumnName: "Features"));

            var model = pipeline.Fit(dataView);
            predictionEngine = mlContext.Model.CreatePredictionEngine<Data, Prediction>(model);
        }

        private void OnAnalyzeClick(object sender, RoutedEventArgs e)
        {
            var inputText = chatbot_user.Text;
            if (string.IsNullOrWhiteSpace(inputText))
            {
                MessageBox.Show("Please enter some text.");
                return;
            }

            var result = predictionEngine.Predict(new Data { Text = inputText });
            chat_chatbot.Text = result.Prediction
                ? $"Positive  (Probability: {result.Probability:P1})"
                : $"Negative (Probability: {result.Probability:P1})";
        }
    }
}