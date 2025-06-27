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

namespace POE_Part3_Prog6221_chatbotapplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Provide full path to the audio file
            string audioPath = "C:\\Users\\RC_Student_lab\\source\\repos\\sound_playing\\CYBERREC.wav";
            voice_greeting greeting = new voice_greeting(audioPath);
            greeting.PlayGreeting();
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
    }
}