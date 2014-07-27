using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace wpfBroadcast.Converter
{
   public  class BoolToColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool res = System.Convert.ToBoolean(value);
            if (res)
                return new System.Windows.Media.SolidColorBrush(Colors.Red);
            else
                return new System.Windows.Media.SolidColorBrush(Color.FromArgb(255,0,255,0));

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
