using System;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Converters
{
    public class ContractImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string img = "contractagreement";
            if (value is int id)
            {
                if (id == (int)Enums.AttachmentType.Signature)
                {
                    img = "ic_signature";
                }
            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
