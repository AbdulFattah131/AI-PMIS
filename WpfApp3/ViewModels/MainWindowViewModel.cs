using System.ComponentModel;
using System.Windows.Media;
using MusicPlayer.Data.Objects;
using MusicPlayer.Utility;
using System.IO;
using System.Xml.Serialization;
using MusicPlayer.UIComponents.ViewModels;
using System.Collections.ObjectModel;

namespace MusicPlayer.UIComponents.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MusicPlayerCache MusicPlayerCache { get; set; }

        private static MainWindowViewModel _instance;
        public static MainWindowViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainWindowViewModel();
                return _instance;
            }
        }
        public ThemeDesignerViewModel ThemeDesignerViewModel => ThemeDesignerViewModel.Instance;
        public string WindowTitle
        {
            get;
            set;
        }

        private Theme _currentTheme;
        public Theme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    OnPropertyChanged(nameof(CurrentTheme));
                }
            }
        }
        public Settings Settings
        {
            get; set;
        }

        private Theme defaultTheme;
        public Theme DefaultTheme
        {
            get => defaultTheme;
            set => DefaultTheme = value;
        }

        private Theme defaultTheme2;
        public Theme DefaultTheme2
        {
            get => defaultTheme2;
            set
            {
                if (defaultTheme2 != value)
                {
                    defaultTheme2 = value;
                    OnPropertyChanged(nameof(DefaultTheme2));
                }
            }
        }

        public MainWindowViewModel()
        {
            WindowTitle = "Music Player";
            defaultTheme = new Theme()
            {

                Name = "Lavender",
                WindowTextForeground = new ThemeColor(38, 38, 38), // Overall Text
                WindowHeadingForeground = new ThemeColor(57, 59, 64), // Center Text Titles
                WindowPrimaryAnchor = new ThemeColor(114, 91, 164), // Icons for the Toggle Buttons
                SearchBoxBackground = new ThemeColor(Brushes.White), // Search Box
                WindowPrimarySlider = new ThemeColor(142, 125, 183), // Sliders
                WindowPrimary = new ThemeColor(197, 178, 239), // New Playlist Button
                WindowTitleForeground = new ThemeColor(Brushes.White), // Title Bar Text
                WindowContentBackground = new ThemeColor(219, 218, 234), // Music Player Background
                ComplementGray = new ThemeColor(101, 97, 107), // Playback Controls : Repeat, Previous, Next, Shuffle
                CurrentSongArtistForeground = new ThemeColor(Brushes.Gray), // Artist Names
                PlayerControlBackground = new ThemeColor(231, 234, 240), // Panels
                TitleBarBackground = new ThemeColor(46, 44, 54), // Title Bar Background
                ListBoxHover = new ThemeColor(248, 248, 255), // List Box Hover
                ListBoxSelected = new ThemeColor(Brushes.White), // List Box Selected
                LeftToggleHover = new ThemeColor(239, 241, 245), // Toggle Buttons Hover
                LeftToggleSelected = new ThemeColor(197, 178, 239), // Toggle Buttons Selected
                PlayPauseBackground = new ThemeColor(114, 91, 164), // Playback Controls : Play, Pause
                GenresTextForeground = new ThemeColor(Brushes.Black), // Text for Genres
                NowPlayingForeground = new ThemeColor(114, 91, 164), // Now Playing Icon
                NewPlaylistHover = new ThemeColor(102, 102, 102), // New Playlist Button on Hover
                PlaylistsToggleSelected = new ThemeColor(140, 140, 140), // Playlists Button when Selected
            };

            defaultTheme2 = new Theme()
            {

                Name = "Lavender Dark",
                WindowTextForeground = new ThemeColor(238, 238, 247),
                WindowHeadingForeground = new ThemeColor(227, 227, 242),
                WindowPrimaryAnchor = new ThemeColor(197, 178, 239),
                SearchBoxBackground = new ThemeColor(106, 103, 131),
                WindowPrimarySlider = new ThemeColor(197, 178, 239),
                WindowPrimary = new ThemeColor(197, 178, 239),
                WindowTitleForeground = new ThemeColor(Brushes.White),
                WindowContentBackground = new ThemeColor(31, 31, 45),
                ComplementGray = new ThemeColor(237, 235, 250),
                CurrentSongArtistForeground = new ThemeColor(Brushes.LightGray),
                PlayerControlBackground = new ThemeColor(42, 42, 56),
                TitleBarBackground = new ThemeColor(46, 44, 54),
                ListBoxHover = new ThemeColor(92, 90, 114),
                ListBoxSelected = new ThemeColor(106, 103, 131),
                LeftToggleHover = new ThemeColor(92, 90, 114),
                LeftToggleSelected = new ThemeColor(106, 103, 131),
                PlayPauseBackground = new ThemeColor(197, 178, 239),
                GenresTextForeground = new ThemeColor(Brushes.Black),
                NowPlayingForeground = new ThemeColor(197, 178, 239),
                NewPlaylistHover = new ThemeColor(114, 91, 164),
                PlaylistsToggleSelected = new ThemeColor(114, 91, 164),
            };

            CurrentTheme = defaultTheme;
            Settings = new Settings();
            Settings = SettingsReader.Instance.ReadFromFile("./Settings.xml");
            FileScanner.Instance.UpdateFilePath(Settings.MusicLibraryPath);
            Init();
        }

        public void Init()
        {
            MusicPlayerCache = new MusicPlayerCache();
            OnPropertyChanged("");
        }

        public void SwitchTheme()
        {
            if (CurrentTheme.Name == "Lavender")
            {
                CurrentTheme = DefaultTheme2;
            }
            else
            {
                CurrentTheme = DefaultTheme;
            }
        }

        public void SaveTheme(string path)
        {
            var serializer = new XmlSerializer(typeof(Theme));
            using (var writer = new StreamWriter(path))
            {
                serializer.Serialize(writer, CurrentTheme);
            }
        }

        public void LoadTheme(string path)
        {
            if (!File.Exists(path)) return;

            var serializer = new XmlSerializer(typeof(Theme));
            using (var reader = new StreamReader(path))
            {
                var theme = (Theme)serializer.Deserialize(reader);
                if (theme != null)
                    CurrentTheme = theme;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}