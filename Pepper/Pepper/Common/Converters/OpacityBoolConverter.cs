using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pepper.Common.Converters
{
    /// <summary>
    /// Converter bool opacity
    /// </summary>
    public class OpacityBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                bool booleanArgs = System.Convert.ToBoolean(value);
                if (booleanArgs)
                    return 1.0f;
                else
                    return 0.1f;
            }
            catch
            {
                return 1.0f;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }    
    }
}
