using System.Windows.Media.Imaging;

namespace MusicPlayer.Data.Objects
{
    public class Song
    {
        public int TrackNumber { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public Album Album { get; set; }
        public HashSet<string> Genres => Album?.Genres;
        public string Genre { get; set; }

        private string filePath;
        public string FilePath
        {
            get => filePath;
            set => filePath = value;
        }

        public BitmapImage Image => Album?.Image;
        public int Length { get; set; }
        public int Index { get; set; } 
        public string Lyrics { get; set; } = string.Empty;
    }
}
