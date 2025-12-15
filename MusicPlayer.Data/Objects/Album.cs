using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Media.Imaging;

namespace MusicPlayer.Data.Objects
{
    public class Album : INotifyPropertyChanged
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public byte[] ImageData { get; set; }
        public BitmapImage Image
        {
            get
            {
                if (ImageData == null)
                    return null;

                using (var stream = new MemoryStream(ImageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze(); // Freeze for thread safety
                    return bitmap;
                }
            }
        }
        public int ReleaseDate { get; set; }

        private HashSet<string> _hsGenres = new();
        public HashSet<string> Genres
        {
                get => _hsGenres;
        }
        
        public int Year { get; set; }   
        public ObservableCollection<Song> Songs { get; set; } = new ObservableCollection<Song>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

