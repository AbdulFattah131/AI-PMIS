using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class PlaylistWriter
    {
        private static PlaylistWriter _Instance;
        public static PlaylistWriter Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new PlaylistWriter();
                return _Instance;
            }
        }

        private PlaylistWriter() { }

        public void WriteToFile(List<Song> playlist)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms);
                var serializer = new XmlSerializer(typeof(List<Song>));
                serializer.Serialize(writer, playlist);
                writer.Flush();

                string folderPath = "./";
                string filePath = $"{folderPath}/Playlists.xml";

                File.WriteAllBytes(filePath, ms.ToArray());
            }
        }
    }
}
