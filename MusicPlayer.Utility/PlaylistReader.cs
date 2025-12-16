using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class PlaylistReader
    {
        private static PlaylistReader _Instance;
        public static PlaylistReader Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PlaylistReader();
                return _Instance;
            }
        }

        private PlaylistReader() { }

        public List<Playlist> GetPlaylists()
        {
            string folderPath = "./";
            string filePath = $"{folderPath}/Playlists.xml";

            if (!File.Exists(filePath))
                return new List<Playlist>();

            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(List<Playlist>));
                return (List<Playlist>)serializer.Deserialize(fs);
            }
        }
    }
}
