using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace wpfBroadcast.Converter
{
   public  class TimeSegToDateTimeConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime dt = System.Convert.ToDateTime(value);
            return string.Format("{0:00}:{1:00}", dt.Hour, dt.Minute);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           Regex  reg=new Regex("([0-9]{1,2}):([0-9]{1,2})");
           Match match = reg.Match(value.ToString());
           if (match.Success)
           {
               int hour = System.Convert.ToInt32(match.Groups[1].Value);
               int min = System.Convert.ToInt32(match.Groups[2].Value);
               DateTime? dt = new DateTime(2000, 1, 1, hour, min, 00);
               return dt;
           }
           else

               throw new Exception("Pattern Error!");

            //throw new NotImplementedException();
        }
    }
}
