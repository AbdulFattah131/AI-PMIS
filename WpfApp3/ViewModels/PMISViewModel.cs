using System.Collections.ObjectModel;
using System.ComponentModel;
using MusicPlayer.Data.Objects;
using MusicPlayer.Utility;
using NAudio.Wave;
using System.Windows.Threading;
using MusicPlayer.UIComponents.Constants;

namespace MusicPlayer.UIComponents.ViewModels
{
    public class MusicPlayerCache : INotifyPropertyChanged
    {
        #region Music Player Collections
        internal void Clear()
        {
            Player.Stop();
            IsPlaying = false;

            CurrentSong = null;
            SelectedAlbum = null;

            PlaybackQueue.Clear();

            Songs?.Clear();
            AllSongs.Clear();
            Albums.Clear();
            OnPropertyChanged("");
        }
        private ObservableCollection<Song> _lstSongs; // collection of all songs
        public ObservableCollection<Song> Songs
        {
            get => _lstSongs;
            set => _lstSongs = value;
        }

        private ObservableCollection<Song> _obAllSongs;
        public ObservableCollection<Song> AllSongs
        {
            get => _obAllSongs;
            set => _obAllSongs = value;
        }

        public ObservableCollection<Album> Albums { get; set; }  // albums collection
        public int CurrentIndex
        {
            get; private set;
        }

        private ObservableCollection<Song> _ocPlaybackQueue;
        public ObservableCollection<Song> PlaybackQueue
        {
            get => _ocPlaybackQueue;
            set
            {
                _ocPlaybackQueue = value;
                OnPropertyChanged(nameof(PlaybackQueue));
            }
        }

        private ENMusicPlayerMode _currentMode = ENMusicPlayerMode.Albums;
        public ENMusicPlayerMode CurrentMode
        {
            get => _currentMode;
            set
            {
                _currentMode = value;
                
                switch (CurrentMode)
                {
                    case ENMusicPlayerMode.Songs:
                        PlaybackQueue = new ObservableCollection<Song>(AllSongs);
                        break;
                    case ENMusicPlayerMode.Albums:
                        break;
                }

                OnPropertyChanged(nameof(CurrentMode));
            }
        }
        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            private set
            {
                if (_isPlaying == value) return;
                _isPlaying = value;
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        private Album _selectedAlbum; // album navigation
        private Album _previousSelectedAlbum;
        public Album SelectedAlbum
        {
            get => _selectedAlbum;
            set
            {
                if (_selectedAlbum != null)
                    _previousSelectedAlbum = _selectedAlbum;

                _selectedAlbum = value;
                Stop();
                
                CurrentSong = null;

                if (_selectedAlbum != null)
                {
                    CurrentMode = ENMusicPlayerMode.Albums;
                    PlaybackQueue = new ObservableCollection<Song>(SelectedAlbum.Songs);
                }

                if (PlaybackQueue.Count > 0)
                {
                    CurrentIndex = 0;
                    CurrentSong = PlaybackQueue[CurrentIndex];
                }
                
                OnPropertyChanged(nameof(SelectedAlbum));
            }
        }

        private Song _currentSong; // currently selected song
        public Song CurrentSong
        {
            get => _currentSong;
            set
            {
                if (_currentSong == value)
                    return;

                Stop();
                CurrentPosition = 0;
                _currentSong = value;

                if (value == null)
                    return;

                CurrentIndex = PlaybackQueue.IndexOf(_currentSong);

                if (_currentSong != null)
                    Player.Load(_currentSong.FilePath);
                
                OnPropertyChanged(nameof(CurrentSong));
                OnPropertyChanged(nameof(IsPlaying));
            }
        }

        #endregion

        #region Playback Functionality

        // Audio Player 

        private AudioPlayer _player;
        public AudioPlayer Player
        {
            get
            {
                if (_player is null)
                    _player = new();

                return _player;
            }
        }

        private AudioPlayer _audioFile;
        public AudioPlayer AudioFile
        {
            get
            {
                if (_audioFile is null)
                    _audioFile = new();

                return _audioFile;
            }
        }

        public PlaybackState PlaybackState
        {
            get => Player.PlaybackState;
        }

        public AudioPlayer MMDevice;

        private readonly DispatcherTimer _timer;

        // Playback Control Buttons

        public void PlayFromPlaylist()
        {
            if (PlaybackQueue == null || PlaybackQueue.Count == 0)
                return;
            CurrentIndex = 0;
            CurrentSong = PlaybackQueue[CurrentIndex];
        }

        private string _loadedFilePath;
        public void PlayPause()
        {
            if (CurrentSong == null && PlaybackQueue == null || PlaybackQueue.Count == 0 && CurrentIndex < 0 || CurrentIndex >= PlaybackQueue.Count)
                return;

            if (_loadedFilePath != CurrentSong.FilePath)
            {
                Player.Load(CurrentSong.FilePath);
                _loadedFilePath = CurrentSong.FilePath;
            }


            if (IsPlaying)
                Player.Pause();
            else
                Play();
        }
        private void Play()
        {
            IsPlaying = true;
            Player.Seek(CurrentPosition / Player.TotalTime.TotalSeconds);
            Player.Play();
        }

        private void Stop()
        {
            IsPlaying = false;
            Player.Stop();
        }
        public void Next()
        {
            if (PlaybackQueue == null || PlaybackQueue.Count == 0)
                return;
            
            bool wasPlaying = IsPlaying;
            Stop();

            CurrentIndex++;

            if (CurrentIndex == PlaybackQueue.Count)
                CurrentIndex = 0;

            CurrentSong = PlaybackQueue[CurrentIndex];
            Play();

        }

        private ENMusicPlayerRepeatMode _repeatMode = ENMusicPlayerRepeatMode.None;
        public ENMusicPlayerRepeatMode RepeatMode
        {
            get => _repeatMode;
            set
            {
                _repeatMode = value;
            }
        }
        public void OnSongEnded()
        {

            CurrentPosition = 0;

            switch (RepeatMode)
            {
                case ENMusicPlayerRepeatMode.RepeatList:
                    IsPlaying = false;
                    CurrentIndex = (CurrentIndex + 1) % PlaybackQueue.Count;
                    CurrentSong = PlaybackQueue[CurrentIndex];
                    Player.Load(CurrentSong.FilePath);
                    Play();
                    IsPlaying = true;
                    break;

                case ENMusicPlayerRepeatMode.RepeatSong:
                    Play();
                    break;

                case ENMusicPlayerRepeatMode.None:
                    if (CurrentIndex + 1 < PlaybackQueue.Count)
                    {
                        CurrentIndex++;
                        CurrentSong = PlaybackQueue[CurrentIndex];
                        Player.Load(CurrentSong.FilePath);
                        Play();
                    }
                    else
                    {
                        IsPlaying = false;
                        CurrentSong = PlaybackQueue[0];
                        Player.Load(CurrentSong.FilePath);
                    }
                    break;
            }
        }
        public void Previous()
        {
            if (PlaybackQueue == null || PlaybackQueue.Count == 0)
                return;

            bool wasPlaying = IsPlaying;
            Stop();

            CurrentIndex--;

            if (CurrentIndex < 0)
                CurrentIndex = PlaybackQueue.Count - 1;

            CurrentSong = PlaybackQueue[CurrentIndex];
            Play();
        }

        public void Shuffle()
        {
            if (PlaybackQueue == null || PlaybackQueue.Count == 0)
                return;

            Random random = new Random();

            Song[] _arrTempQueue = PlaybackQueue.ToArray();
            random.Shuffle(_arrTempQueue);
            PlaybackQueue = new ObservableCollection<Song>(_arrTempQueue);
            CurrentIndex = PlaybackQueue.IndexOf(CurrentSong);
        }

        #endregion

        #region Slider (Music Seek Bar)

        private bool _isUserDragging;
        public bool IsUserDragging
        {
            get => _isUserDragging;
            set
            {
                _isUserDragging = value;
                OnPropertyChanged(nameof(IsUserDragging));
            }
        }

        private int _currentPosition;
        public int CurrentPosition
        {
            get => _currentPosition;
            set
            {
                if (_currentPosition != value)
                {
                    _currentPosition = value;

                    if (_isUserDragging)
                    {
                        double progress = _currentPosition / Player.TotalTime.TotalSeconds;
                        Player.Seek(progress);
                    }

                    OnPropertyChanged(nameof(CurrentPosition));
                }
            }
        }
        #endregion

        public MusicPlayerCache()
        {
            TagReader.Instance.Reset();

            // timer
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();

            StartClock(); // Initial update

            Player.SongEnded += OnSongEnded;
            OnPropertyChanged("");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            StartClock();
        }
        private DispatcherTimer _clockTimer;
        public void StartClock()
        {
            _clockTimer = new DispatcherTimer();
            _clockTimer.Interval = TimeSpan.FromSeconds(1);
            _clockTimer.Tick += (s, e) =>
            {
                DateTime now = DateTime.Now;
                CurrentTime = now.ToString("hh:mm tt");       // 12-hour format
                CurrentDate = now.ToString("dddd, dd MMM yyyy");
                DayNightIcon = now.Hour >= 6 && now.Hour < 18 ? "☀️" : "🌙";
            };
            _clockTimer.Start();
        }

      
        private string _currentTime;
        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        private string _dayNightIcon;
        public string DayNightIcon
        {
            get => _dayNightIcon;
            private set
            {
                _dayNightIcon = value;
                OnPropertyChanged(nameof(DayNightIcon));
            }
        }
        private string _currentDate;
        public string CurrentDate
        {
            get => _currentDate;
            set
            {
                _currentDate = value;
                OnPropertyChanged(nameof(CurrentDate));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}

