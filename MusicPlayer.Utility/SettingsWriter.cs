using System.Windows.Controls.Primitives;
using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class SettingsWriter
    {
        private static SettingsWriter _Instance;

        public static SettingsWriter Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance= new SettingsWriter();
                return _Instance;
            }  
        }
        private SettingsWriter()
        {

        }

        public void WriteToFile(Settings settings)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms);
                var serializer = new XmlSerializer(settings.GetType());
                serializer.Serialize(writer, settings);
                writer.Flush();

                string stFolderPath = "./";
                string stFilePath = $"{stFolderPath}/Settings.xml";

                //if the serialization succeed, rewrite the file.
                File.WriteAllBytes(stFilePath, ms.ToArray());
            }
        }
    }
}
