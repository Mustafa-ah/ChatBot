using System;
using System.ComponentModel;
using Foundation;
using STC.Common.CommonControlls;
using STC.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(CustomEntryRenderer))]
namespace STC.iOS.Renderers
{
    public class UIBackwardsTextField : UITextField
    {
        // A delegate type for hooking up change notifications.
        public delegate void DeleteBackwardEventHandler(object sender, EventArgs e);

        // An event that clients can use to be notified whenever the
        // elements of the list change.
        public event DeleteBackwardEventHandler OnDeleteBackward;


        public void OnDeleteBackwardPressed()
        {
            if (OnDeleteBackward != null)
            {
                OnDeleteBackward(null, null);
            }
        }

        public UIBackwardsTextField()
        {
            BorderStyle = UITextBorderStyle.RoundedRect;
            ClipsToBounds = true;
        }

        public override void DeleteBackward()
        {
            base.DeleteBackward();
            OnDeleteBackwardPressed();
        }
    }
    public class CustomEntryRenderer : EntryRenderer, IUITextFieldDelegate
    {

        BorderlessEntry borderlessEntry;
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = UITextBorderStyle.None;
            Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;

            if (e.PropertyName == "IsOTP")
            {
                Control.TextContentType = UITextContentType.OneTimeCode;
               
            }
        }

        IElementController ElementController => Element as IElementController;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            if (Element == null)
            {
                return;
            }

            if ((e.NewElement as BorderlessEntry).IsOTP)
            {
                if (Control != null)
                {
                    Control.TextContentType = UITextContentType.OneTimeCode;
                }


                borderlessEntry = (BorderlessEntry)Element;
                var textField = new UIBackwardsTextField();
                textField.EditingChanged += OnEditingChanged;
                textField.OnDeleteBackward += (sender, a) =>
                {
                    borderlessEntry.OnDelete();
                };

                SetNativeControl(textField);

            }
           

            base.OnElementChanged(e);
        }

        void OnEditingChanged(object sender, EventArgs eventArgs)
        {
            if( Control.Text.Length>1)
            {
                string tmpText = Control.Text;
                Control.Text = tmpText[0].ToString();
            }
            ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
            borderlessEntry.OnEditingChanged(Control.Text);
        }

    }
    //public class BorderlessEntryRenderer : EntryRenderer, IUITextFieldDelegate
    //{
    //    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        base.OnElementPropertyChanged(sender, e);

    //        Control.Layer.BorderWidth = 0;
    //        Control.BorderStyle = UITextBorderStyle.None;
    //        Control.VerticalAlignment = UIControlContentVerticalAlignment.Top;
    //    }

    //    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    //    {
    //        base.OnElementChanged(e);
    //        if (e.NewElement != null)
    //        {
    //            if ((e.NewElement as BorderlessEntry).IsOTP)
    //            {
    //                Control.TextContentType = UITextContentType.OneTimeCode;

    //               // Control.EditingChanged += Control_EditingChanged;

    //                var entry = (BorderlessEntry)Element;

    //                var textField = new CustomTextField();

    //                textField.EditingChanged += OnEditingChanged;
    //                textField.OnDeleteBackwardKey += (sender, a) =>
    //                {
    //                    entry.OnDelete();
    //                };

    //            }
    //        }
    //    }

    //    IElementController ElementController => Element as IElementController;

    //    void OnEditingChanged(object sender, EventArgs eventArgs)
    //    {
    //        ElementController.SetValueFromRenderer(Entry.TextProperty, Control.Text);
    //    }

    //    //[Export("textField:shouldChangeCharactersInRange:replacementString:")]
    //    //public bool ShouldChangeCharacters(UITextField textField, Foundation.NSRange range, string replacementString)
    //    //{
    //    //    var ddd = replacementString;

    //    //    return true;
    //    //}   
    //}

    
}
