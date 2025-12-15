using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace MusicPlayer.Data.Objects
{
    public class Theme : INotifyPropertyChanged
    {
        public string Name
        {
            get;
            set;
        }

        public ThemeColor WindowTextForeground
        {
            get;
            set;
        }

        public ThemeColor WindowPrimary
        {
            get;
            set;
        }
        public ThemeColor WindowPrimaryAnchor
        {
            get;
            set;
        }
        public ThemeColor WindowPrimarySlider
        {
            get;
            set;
        }
        public ThemeColor SearchBoxBackground
        {
            get;
            set;
        }
        public ThemeColor ListBoxHover
        {
            get;
            set;
        }
        public ThemeColor ListBoxSelected
        {
            get;
            set;
        }
        public ThemeColor LeftToggleHover
        {
            get;
            set;
        }
        public ThemeColor LeftToggleSelected
        {
            get;
            set;
        }
        public ThemeColor PlayPauseBackground
        {
            get;
            set;
        }
        public ThemeColor WindowContentBackground
        {
            get;
            set;
        }
        
        public ThemeColor WindowTitleForeground
        {
            get;
            set;
        }

        public ThemeColor WindowHeadingForeground
        {
            get;
            set;
        }

        public ThemeColor ListBoxItemForeground
        {
            get;
            set;
        }
        public ThemeColor TitleBarBackground
        {
            get;
            set;
        }
      
        public ThemeColor CurrentSongTitleForeground
        {
            get;
            set;
        }
        
        public ThemeColor CurrentSongArtistForeground
        {
            get;
            set;
        }
        public ThemeColor PlayerControlBackground
        {
            get;
            set;
        }
        public ThemeColor ComplementGray
        {
            get;
            set;
        }

        public ThemeColor GenresTextForeground
        {
            get;
            set;
        }

        public ThemeColor NowPlayingForeground
        {
            get;
            set;
        }
        public ThemeColor NewPlaylistHover
        {
            get;
            set;
        }
        public ThemeColor PlaylistsToggleSelected
        {
            get;
            set;
        }
        public Theme ()
        {
            
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
