using System.ComponentModel;
using MusicPlayer.Data.Objects;
using MusicPlayer.Utility;

namespace MusicPlayer.UIComponents.ViewModels
{
    public class ThemeDesignerViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel _mainWindowVM;

        private static ThemeDesignerViewModel _instance;

        public static ThemeDesignerViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ThemeDesignerViewModel();
                return _instance;
            }
        }
        public ThemeDesignerViewModel(MainWindowViewModel Instance)
        {
            CustomTheme = new Theme()
            {
                WindowTextForeground = new ThemeColor(), // Text
                WindowHeadingForeground = new ThemeColor(), // Center Text Titles
                WindowPrimaryAnchor = new ThemeColor(), // Icons for the Toggle Buttons
                SearchBoxBackground = new ThemeColor(), // Search Box
                WindowPrimarySlider = new ThemeColor(), // Sliders
                WindowPrimary = new ThemeColor(), // New Playlist Button
                WindowTitleForeground = new ThemeColor(), // Title Bar Text
                WindowContentBackground = new ThemeColor(), // Window Background
                ComplementGray = new ThemeColor(), // Playback Controls : Repeat, Previous, Next, Shuffle
                CurrentSongArtistForeground = new ThemeColor(), // Artist Names
                PlayerControlBackground = new ThemeColor(), // Panels
                TitleBarBackground = new ThemeColor(), // Title Bar Background
                ListBoxHover = new ThemeColor(), // List Box Hover
                ListBoxSelected = new ThemeColor(), // List Box Selected
                LeftToggleHover = new ThemeColor(), // Toggle Buttons Hover
                LeftToggleSelected = new ThemeColor(), // Toggle Buttons Selected
                PlayPauseBackground = new ThemeColor(), // Playback Controls : Play, Pause
                GenresTextForeground = new ThemeColor(), // Text for Genres
                NowPlayingForeground = new ThemeColor(), // Now Playing Icon
            };
        }
        public void ApplyTheme(Theme theme)
        {
            MainWindowViewModel.Instance.CurrentTheme = CustomTheme;
        }

        private Theme m_customTheme;
        public Theme CustomTheme
        {
            get { return m_customTheme; }
            set
            {
                m_customTheme = value;
                OnPropertyChanged(new PropertyChangedEventArgs(""));
            }
        }  

        public List<Theme> LoadedThemes
        {
            get
            {
                return ThemeReader.Instance.GetThemes();
            }
        }

        public ThemeDesignerViewModel()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }

    }
}
