using System;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Common.Converters
{
    public class FlowDirectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int lang)
            {
                return lang == (int)Enums.Languages.Arabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            }
            return FlowDirection.MatchParent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
