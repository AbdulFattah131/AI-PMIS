using System.Diagnostics;
using System.Windows;
using MusicPlayer.Data.Objects;
using MusicPlayer.UIComponents.ViewModels;
using MusicPlayer.Utility;

namespace MusicPlayer.UIComponents
{
    /// <summary>
    /// Interaction logic for CreatePlaylist.xaml
    /// </summary>
    public partial class PlaylistsWindow : Window
    {
        public MainWindowViewModel m_vm;
        public PlaylistsWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
        
        private void btnSavePlaylist_Click(object sender, RoutedEventArgs e)
        {
            PlaylistWriter.Instance.WriteToFile(PlaylistsWindowViewModel.Instance.NewPlaylist.PlaylistSongs);
            MessageBox.Show("Playlist saved!");
        }

        public List<Song> NewPlaylistSongs { get; set; } = new List<Song>();

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedSong = plSongsListBox.SelectedItem as Song;
            if (selectedSong == null) return;
            
            if (!PlaylistsWindowViewModel.Instance.TemporarySongs.Any(s => s.TrackNumber == selectedSong.TrackNumber))
            {
                PlaylistsWindowViewModel.Instance.TemporarySongs.Add(selectedSong);

                // Select the song in the ListBox automatically
                plSongsListBox.SelectedItem = selectedSong;
                plSongsListBox.ScrollIntoView(selectedSong);

                Debug.WriteLine($"Added '{selectedSong.Title}' to temporary playlist. Total: {PlaylistsWindowViewModel.Instance.TemporarySongs.Count}");
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int index = plSongsListBox.SelectedIndex;
            if (index < 0) return;

            var selectedSong = plSongsListBox.SelectedItem as Song;
            if (selectedSong == null) return;

            PlaylistsWindowViewModel.Instance.TemporarySongs.Remove(selectedSong);
        }
        private void RefreshTemporaryListUI()
        {
            plSongsListBox.ItemsSource = null;
            plSongsListBox.ItemsSource = PlaylistsWindowViewModel.Instance.TemporarySongs;
        }
    }
}
