using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Data;

namespace MusicPlayer.UIComponents.Convertor

{
    public class DurationToDisplayTimeConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "--:--";
            }

            double nValue = 0;
            if (value is not double)
            {
                double.TryParse(value.ToString(), out nValue);
            }

            double nDuration = nValue;

            if (nDuration is < 0)
            {
                return "--:--";
            }

            double nMinutes = Math.Floor(nDuration / 60);
            double nSeconds = nDuration % 60;

            return $"{nMinutes:00}:{nSeconds:00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static class SoundInfo
        {
            [DllImport("winmm.dll")]
            private static extern uint mciSendString(string command, StringBuilder returnValue, int returnLength, IntPtr winHandle);

            public static int GetSoundLength(string fileName) // gets the sound length
            {
                StringBuilder lengthBuf = new StringBuilder(32);

                if (fileName.EndsWith(".mp3"))
                {
                    mciSendString(string.Format("open \"{fileName}\" type mpegvideo alias mp3", fileName), null, 0, IntPtr.Zero);
                    mciSendString("status mp3 length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
                    mciSendString("close mp3", null, 0, IntPtr.Zero);
                }
                else if(fileName.EndsWith(".m4a"))
                {
                    mciSendString(string.Format("open \"{fileName}\" type mpegvideo alias m4a", fileName), null, 0, IntPtr.Zero);
                    mciSendString("status m4a length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
                    mciSendString("close m4a", null, 0, IntPtr.Zero);
                }
                int length = 0;
                int.TryParse(lengthBuf.ToString(), out length);

                return length;
            }
        }
    }
}
