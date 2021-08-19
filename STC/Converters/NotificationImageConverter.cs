
using System;
using System.Globalization;
using Xamarin.Forms;

namespace STC.Converters
{
    public class NotificationImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string img = "ic_comment.png";
            if (value is int id)
            {
                if (id == (int)Enums.NotificationType.SignContract || id == (int)Enums.NotificationType.UploadContract)
                {
                    img = "ic_cotract.png";
                }
                else if (id == (int)Enums.NotificationType.NewReply)
                    img = "Group6.png";
            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
