using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Common.Converters
{
    public class FirstValidationErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<string> errors)
            {
                if (errors.Count > 0)
                {
                    return errors[0];
                }
                
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
