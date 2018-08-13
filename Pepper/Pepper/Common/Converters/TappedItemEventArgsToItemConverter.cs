using System;
using System.Globalization;
using Xamarin.Forms;

namespace Pepper.Common.Converters
{
    /// <summary>
    /// Converter need to tap on item listview exectute command
    /// </summary>
    public class TappedItemEventArgsToItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ItemTappedEventArgs;
            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }    
    }
}
