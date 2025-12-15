using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class SettingsReader
    {
        public static SettingsReader _Instance;

        public static SettingsReader Instance
        {
            get 
            { 
                if (_Instance == null)
                    _Instance = new SettingsReader();
                return _Instance;
            }
        }
        private SettingsReader()
        {

        }
        public Settings ReadFromFile(string stFilePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            
            FileStream fs = new FileStream(stFilePath, FileMode.OpenOrCreate);
            TextReader reader = new StreamReader(fs);
            try
            {
                Settings i = (Settings)serializer.Deserialize(reader);
                return i;
            }
            catch (Exception e)
            {
                return new Settings();
            }
        }
    }
}
