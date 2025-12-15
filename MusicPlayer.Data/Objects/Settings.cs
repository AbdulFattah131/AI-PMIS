
using System.Windows;

namespace MusicPlayer.Data.Objects
{
    public class Settings
    {
        public string CurrentThemeName
        {
            get;
            set;
        } = "Lavender";

        public string MusicLibraryPath
        {
            get;
            set;
        }

        public Point LastWindowCoordinates
        {
            get;
            set;
        } = new Point(100, 100);

        public Point LastWindowDimensions
        {
            get;
            set;
        } = new Point(1280, 720);

        public float? LastKnownVolume
        {
            get;
            set;
        } = 0.5f;
    }
}
