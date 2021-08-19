using System;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Converters
{
    public class AttachmentIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string img = "file";
            if (value is bool image)
            {
                if (image)
                {
                    img = "image";
                }
                else 
                    img = "pdf";
            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

