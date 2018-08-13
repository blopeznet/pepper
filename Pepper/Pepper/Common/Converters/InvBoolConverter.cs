using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pepper.Common.Converters
{
    /// <summary>
    /// Converter inverse bool
    /// </summary>
    public class InvBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                bool booleanArgs = System.Convert.ToBoolean(value);
                return !booleanArgs;                                                
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }    
    }
}
