using System.Collections.ObjectModel;

namespace MusicPlayer.Utility
{
    //Singleton 
    public class FileScanner
    {
        // Scan and load all song File Paths into our project.

        private static FileScanner _instance;

        public static FileScanner Instance
        {
            get
            {
               if(_instance == null)
                    _instance = new FileScanner();

               return _instance;
            }
        }

        public string FilePath
        {
            get; private set;
        } = @"./Songs";

        FileScanner()
        {
           
        }

        public void UpdateFilePath(string stFilePath)
        {
            FilePath = stFilePath;
        }

        public ObservableCollection<string> ScanSongs()
        {
            ObservableCollection<string> lstSongFilePaths = new ObservableCollection<string>();

            try
            {
                string filePath = FilePath;
                var allowedExtensions = new[] { ".mp3", ".m4a" };
                List<string> files = Directory.GetFiles(filePath)
                                     .Where(file => allowedExtensions.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                                     .ToList();
                 
                lstSongFilePaths = new ObservableCollection<string>(files);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading files: {ex.Message}");
            }

            return lstSongFilePaths;
        }
    }
}
