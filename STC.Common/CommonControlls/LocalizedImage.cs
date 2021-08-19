using System;
using Xamarin.Forms;

namespace STC.Common.CommonControlls
{
    public class LocalizedImage : Image
    {
        public LocalizedImage()
        {
        }

        public static readonly BindableProperty LangProperty =
            BindableProperty.Create(nameof(Lang), typeof(int), typeof(LocalizedImage), (int)Enums.Languages.English,
                BindingMode.TwoWay, null, OnLangPropertyChanged);


        public static void OnLangPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as LocalizedImage;
            if (control != null)
            {
                if(newValue is int lang)
                {
                    if (lang == (int)Enums.Languages.Arabic)
                    {
                        control.RotationY = 180;
                    }
                    else 
                    {
                        control.RotationY = 0;
                    }
                }
            }
        }

        public int Lang
        {
            get { return (int)GetValue(LangProperty); }
            set { SetValue(LangProperty, value); }
        }
    }
}
