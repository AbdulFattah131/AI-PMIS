
using System.Windows;

namespace MusicPlayer.Data.Objects
{
    public class Settings
    {
        public string CurrentThemeName
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


    }
}
