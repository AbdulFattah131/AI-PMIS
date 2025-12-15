 using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class ThemeWriter
    {
        private static ThemeWriter _Instance;

        public static ThemeWriter Instance
        {
            get
            {
                if(_Instance is null)
                    _Instance = new ThemeWriter();

                return _Instance;
            }
        }

        private ThemeWriter()
        {

        }

        public void WriteToFile(Theme theme) 
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var writer = new StreamWriter(ms);
                var serializer = new XmlSerializer(theme.GetType());
                serializer.Serialize(writer, theme);
                writer.Flush();

                string stFolderPath = "./Themes";

                if(!Directory.Exists(stFolderPath))
                    Directory.CreateDirectory(stFolderPath);

                string stFilePath = $"{stFolderPath}/{theme.Name}.xml";

                //if the serialization succeed, rewrite the file.
                File.WriteAllBytes(stFilePath, ms.ToArray());
            }
        }
    }
}
