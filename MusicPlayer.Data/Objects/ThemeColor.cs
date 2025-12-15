using System.ComponentModel;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MusicPlayer.Data.Objects
{
    public class ThemeColor : IThemeColor
    {
        [XmlIgnore] // "Ignore" below
        public Brush Brush { get; set; } // Brush Property
        public string Hex // Brush to/from String Convertor
        {
            get
            {
                if (Brush == null)
                    return "#FF000000";

                return Brush.ToString(); //#FF00000
            }

            set
            {
                if (value == null || (value.Length != 9 && value.Length != 7))
                    return;

                Brush = (SolidColorBrush)(new BrushConverter().ConvertFromString(value));
                OnPropertyChanged(new PropertyChangedEventArgs("Brush"));
            }
        }

        public ThemeColor() { // Constructor Solid Color
            Brush = Brushes.Black;
        }
        public ThemeColor(string hex = "#FF00000") // Constructor Hex Checker
        {
            Hex = hex;
        }

        public ThemeColor (Brush brush) // Constructor Brush Checker
        {
            this.Brush = brush;
            if (brush is SolidColorBrush solid)
            {
                Hex = solid.Color.ToString();
            }
            else
            {
                Hex = null;
            }
        }
        
        public ThemeColor(byte r, byte g, byte b) // Constructor RGB
        {
            this.Brush = new SolidColorBrush(Color.FromRgb(r, g, b));
        }

        public event PropertyChangedEventHandler? PropertyChanged; // Event Declaration

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
    }
}
