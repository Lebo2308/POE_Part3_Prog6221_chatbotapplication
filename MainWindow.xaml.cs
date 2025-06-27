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
    }
}