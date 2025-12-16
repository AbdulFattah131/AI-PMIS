using System.Collections.ObjectModel;
using System.Diagnostics;
using MusicPlayer.Data.Objects;
using NAudio.Wave;

namespace MusicPlayer.Utility
{
    public class TagReader
    {
        private static TagReader _instance; // tagreader instance
        public static TagReader Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new TagReader();

                return _instance;
            }
        }

        private Dictionary<string, Album> m_dictAlbums = new Dictionary<string, Album>(); // dictionary of albums

        public Song ReadSongFromFilePath(string stfilePath)
        {
            Song song = null;

            using (TagLib.File file = TagLib.File.Create(stfilePath))
            {
                // --- SAFE TAG EXTRACTION ---
                string title = string.IsNullOrWhiteSpace(file.Tag.Title)
                                        ? Path.GetFileNameWithoutExtension(stfilePath)
                                        : file.Tag.Title;

                string albumTitle = string.IsNullOrWhiteSpace(file.Tag.Album)
                                        ? "Unknown Album"
                                        : file.Tag.Album;

                string artist = (file.Tag.Performers != null && file.Tag.Performers.Length > 0)
                                        ? string.Join(", ", file.Tag.Performers)
                                        : "Unknown Artist";

                string albumArtist = (file.Tag.AlbumArtists != null && file.Tag.AlbumArtists.Length > 0)
                                        ? string.Join(", ", file.Tag.AlbumArtists)
                                        : "Unknown Artist";

                string genre = (file.Tag.Genres != null && file.Tag.Genres.Length > 0)
                                        ? string.Join(", ", file.Tag.Genres)
                                        : "Unknown";

                int year = (file.Tag.Year > 0) ? (int)file.Tag.Year : 0;

                // SAFE PICTURE EXTRACTION
                byte[] imageData = null;
                if (file.Tag.Pictures != null && file.Tag.Pictures.Length > 0)
                {
                    try { imageData = file.Tag.Pictures[0].Data.Data; } catch { }
                }

                // --- CREATE ALBUM IF NOT EXISTS ---
                if (!m_dictAlbums.ContainsKey(albumTitle))
                {
                    Album album = new Album()
                    {
                        Title = albumTitle,
                        Artist = albumArtist,
                        Year = year,
                        ImageData = imageData,
                    };

                    if (file.Tag.Genres != null)
                        foreach (string g in file.Tag.Genres)
                            album.Genres.Add(g);

                    m_dictAlbums.Add(albumTitle, album);
                }

                // --- CREATE SONG ---
                song = new Song
                {
                    Title = title,
                    Artist = artist,
                    Album = m_dictAlbums[albumTitle],
                    Genre = genre,
                    TrackNumber = (int)file.Tag.Track,
                    FilePath = stfilePath,
                };

                if (!string.IsNullOrWhiteSpace(file.Tag.Lyrics))
                    song.Lyrics = file.Tag.Lyrics;

                // --- ADD SONG TO ALBUM ---
                m_dictAlbums[albumTitle].Songs.Add(song);
            }

            return song;
        }

        public void Reset()
        {
            m_dictAlbums.Clear();
        }
        public ObservableCollection<Song> ReadSongsFromFilePaths(IList<string> lstFilePaths)
        {
            ObservableCollection<Song> lstSongs = new ObservableCollection<Song>();

            // add each song
            foreach (string stFilePath in lstFilePaths)
            {
                Song temp = ReadSongFromFilePath(stFilePath);

                using (var audioFileReader = new AudioFileReader(stFilePath))
                {
                    TimeSpan duration = audioFileReader.TotalTime;
                    temp.Length = duration.Hours*3600 + duration.Minutes*60 + duration.Seconds;
                }

                if (temp != null)
                {
                    lstSongs.Add(temp);
                }
            }
         
            return lstSongs;
        }
        public IEnumerable<Album> GetAlbums()
        {
            return m_dictAlbums.Values;
        }
        private TagReader()
        {

        }
    }
}
