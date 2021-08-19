using System;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Common.Converters
{
    public class RotationConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int lang)
            {
                return lang == (int)Enums.Languages.Arabic ? 180 : 0;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
