using System.Collections.ObjectModel;
using System.ComponentModel;
using MusicPlayer.Data.Objects;
using MusicPlayer.Utility;

namespace MusicPlayer.UIComponents.ViewModels
{
    public class PlaylistsWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel _mainWindowVM;

        private static PlaylistsWindowViewModel _instance;
        public static PlaylistsWindowViewModel Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PlaylistsWindowViewModel();
                return _instance;
            }
        }
        public bool IsCreatingPlaylist { get; set; }
        public PlaylistsWindowViewModel(MainWindowViewModel mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
            TemporarySongs.CollectionChanged += (s, e) => SongCount = TemporarySongs.Count;
            LoadSavedPlaylists();

        }

        public PlaylistsWindowViewModel()
        {
            LoadSavedPlaylists();
        }

        private Playlist _newPlaylist;
        public Playlist NewPlaylist
        {
            get { return _newPlaylist; }
            set
            {
                _newPlaylist = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewPlaylist)));
            }
        }

        public ObservableCollection<Song> TemporarySongs { get; } = new ObservableCollection<Song>();

        private int _songCount;
        public int SongCount
        {
            get => _songCount;
            set
            {
                if (_songCount != value)
                {
                    _songCount = value;
                    OnPropertyChanged(new PropertyChangedEventArgs(nameof(SongCount)));
                }
            }
        }

        private List<Playlist> Playlists = new();
        public List<Playlist> LoadedPlaylists
        {
            get 
            { 
                return PlaylistReader.Instance.GetPlaylists();
            }
           
        }
        public ObservableCollection<Playlist> SavedPlaylists { get; } = new ObservableCollection<Playlist>();

        public void CreateNewPlaylist(string name)
        {
            NewPlaylist = new Playlist() { Name = name, PlaylistSongs = new List<Song>() }; 
            TemporarySongs.Clear();
        }
        public bool AddSongToTemporaryPlaylist(Song song)
        {
            if (song == null) return false;

            if (TemporarySongs.Any(s => s.Index == song.Index))
                return false;

            TemporarySongs.Add(song);
            return true;
        }
        public void RemoveSongFromTemporaryPlaylist(Song song)
        {
            if (song == null) return;
            TemporarySongs.Remove(song);
        }
        public void LoadSavedPlaylists()
        {
            var loaded = PlaylistReader.Instance.GetPlaylists();
            SavedPlaylists.Clear();
            foreach (var pl in loaded)
                SavedPlaylists.Add(pl);
        }

        public void PlayFromPlaylist(Playlist playlist)
        {
            if (playlist == null || playlist.PlaylistSongs.Count == 0)
                return;

            _mainWindowVM = MainWindowViewModel.Instance;
            _mainWindowVM.MusicPlayerCache.PlayFromPlaylist();
        }
        public void SaveTemporaryPlaylist()
        {
            if (NewPlaylist == null) return;

            NewPlaylist.PlaylistSongs = TemporarySongs.ToList();
            PlaylistWriter.Instance.WriteToFile(NewPlaylist.PlaylistSongs);

            SavedPlaylists.Add(NewPlaylist);

            TemporarySongs.Clear();
            NewPlaylist = null;
        }
        //public void RemoveSavedPlaylist(Playlist playlist)
        //{
        //    if (playlist == null) return;

        //    PlaylistWriter.Instance.DeletePlaylistFile(playlist.Name); // implement file deletion
        //    SavedPlaylists.Remove(playlist);
        //}
        public void AddSongToCurrentPlaylist(Song song)
        {
            if (NewPlaylist == null)
                return;

            NewPlaylist.PlaylistSongs.Add(song);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewPlaylist)));
        }

        public void RemoveSongFromCurrentPlaylist(Song song)
        {
            if (NewPlaylist == null)
                return;

            NewPlaylist.PlaylistSongs.Remove(song);
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewPlaylist)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
