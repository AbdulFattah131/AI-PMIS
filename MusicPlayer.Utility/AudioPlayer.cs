using System.ComponentModel;
using System.Windows.Threading;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;

namespace MusicPlayer.Utility
{
    public class AudioPlayer :INotifyPropertyChanged
    {
        private IWavePlayer _player;
        private AudioFileReader _audioFile;
        private MMDevice _defaultDevice;
        private bool _isMuted;
        string _loadedFilePath;
        private WaveOutEvent _waveOutEvent;
        public bool IsPlaying => _player?.PlaybackState == PlaybackState.Playing;
        public event Action SongEnded;
        public AudioPlayer()
        {
            _player = new WaveOutEvent();
            _player.PlaybackStopped += Player_PlaybackStopped;
        }
        
        private void Player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (_audioFile != null &&
                _audioFile.Position >= _audioFile.Length - _audioFile.Length * 0.001)
            {
                SongEnded?.Invoke();
            }

        }

        public void InitializeVolume()
        {
            var enumerator = new MMDeviceEnumerator();
            _defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            _player.Volume = _defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar;
            _isMuted = _defaultDevice.AudioEndpointVolume.Mute;
        }

        float _fLastKnownVolume = 1.0f;
        public float SystemVolume
        {
            get => _player.Volume;
            set
            {
                _player.Volume = value;
                OnPropertyChanged(nameof(SystemVolume));
            }
        }

        public PlaybackState PlaybackState
        {
            get => _player.PlaybackState;
        }

        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                if (_isMuted)
                {
                    _fLastKnownVolume = SystemVolume;
                    SystemVolume = 0f;
                }
                else
                {
                    SystemVolume = _fLastKnownVolume;
                }
            }
        }
        public void Load(string filePath)
        {
            Stop();

            _audioFile = new AudioFileReader(filePath);
            _player.Init(_audioFile);
            _loadedFilePath = filePath;
        }

        public void TogglePlayPause()
        {
            if (IsPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        public void Pause()
        {
            _player?.Pause();
        }

        public void Play()
        {
            if (_player == null || _audioFile == null)
                return;

            if (_player.PlaybackState != PlaybackState.Playing)
            {
                _player.Play();
            }
        }

        public void Stop()
        {
            if (_player != null)
                _player.Stop();
            _audioFile?.Dispose();
            _audioFile = null!;
        }

        public void SetOutputDevice(int deviceNumber)
        {
            _player?.Stop();
            _player?.Dispose();

            _player = new WaveOutEvent() { DeviceNumber = deviceNumber };

            if (_audioFile != null)
            {
                _player.Init(_audioFile);
            }
        }
        public TimeSpan CurrentTime
        {
            get
            {
                if (_audioFile == null)
                    return TimeSpan.Zero;
                return _audioFile.CurrentTime;
            }
            set
            {
                if (_audioFile != null)
                    _audioFile.CurrentTime = value;
            }
        }

        public TimeSpan TotalTime
        {
            get
            {
                if (_audioFile == null)
                    return TimeSpan.Zero;
                return _audioFile.TotalTime;
            }
        }

        public void Seek(double progress)
        {
            if (_audioFile != null && progress >= 0 && progress <= 1)
            {
                _audioFile.CurrentTime = TimeSpan.FromSeconds(_audioFile.TotalTime.TotalSeconds * progress);
            }
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
