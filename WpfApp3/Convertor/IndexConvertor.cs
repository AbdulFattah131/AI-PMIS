using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicPlayer.UIComponents.Convertor
{
    public class IndexConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = (ListBoxItem)value;
            ListBox listbox = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
            int index = listbox.ItemContainerGenerator.IndexFromContainer(item) + 1; // +1 for 1-based indexing
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
