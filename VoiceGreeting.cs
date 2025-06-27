using System;
using System.Windows.Media;

namespace sound_playing
{
    public class voice_greeting
    {
        private readonly string _audioFilePath;

        public voice_greeting(string audioFilePath)
        {
            _audioFilePath = audioFilePath;
        }

        public void PlayGreeting()
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(_audioFilePath, UriKind.RelativeOrAbsolute));
            player.Play();
        }
    }
}
