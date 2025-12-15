using System.Xml.Serialization;
using MusicPlayer.Data.Objects;

namespace MusicPlayer.Utility
{
    public class ThemeReader
    {
        private static ThemeReader _Instance;

        const string stFolderPath = "./Themes";

        public static ThemeReader Instance
        {
            get
            {
                if (_Instance is null)
                    _Instance = new ThemeReader();
                return _Instance;
            }
        }

        private ThemeReader()
        {

        }
        public Theme LoadFromFile(string themeName)
        {
            string stFilePath = $"{stFolderPath}/{themeName}";

            if (!File.Exists(stFilePath))
                return null;

            var serializer = new XmlSerializer(typeof(Theme));
            using (var fs = new FileStream(stFilePath, FileMode.Open))
            {
                return (Theme)serializer.Deserialize(fs);
            }
        }

        public List<Theme> GetThemes()
        {
            string[] files = Directory.GetFiles(stFolderPath, "*.xml");
            List<Theme> result = new List<Theme>();

            foreach (string stFilePath in files)
            {
                string stFileName = Path.GetFileName(stFilePath);

                Theme loadedTheme = LoadFromFile(stFileName);
                result.Add(loadedTheme);
            }

            return result;
        }
    }
}
