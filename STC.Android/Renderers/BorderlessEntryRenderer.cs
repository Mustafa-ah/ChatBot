using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Views.InputMethods;
using Plugin.CurrentActivity;
using STC.Common.CommonControlls;
using STC.Droid.Renderers;
using STC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace STC.Droid.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer(Context context) : base(context)
        {
            MessagingCenter.Subscribe<LoginOTPPageViewModel>(this, "UnfocusOTPNative", UnfocusOTPNative);
            MessagingCenter.Subscribe<VerifyEmailMobileProfilePageViewModel>(this, "UnfocusOTPNative", UnfocusOTPNative);
            MessagingCenter.Subscribe<VerifyEmailPageViewModel>(this, "UnfocusOTPNative", UnfocusOTPNative);
            MessagingCenter.Subscribe<VerifySmsPageViewModel>(this, "UnfocusOTPNative", UnfocusOTPNative);
        }
        private void UnfocusOTPNative(object obj)
        {
            if (isOTP)
            {
                hideSoftKeyboard();
                Control.ClearFocus();
            }
        }
        bool isOTP = false;
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Element == null)
                return;

            if (e!=null && (e.NewElement as BorderlessEntry).IsOTP)
            {
                isOTP = true;

            }
            else
                isOTP = false;
            if (e.OldElement == null)
            {
                this.Control.SetPadding(0, 0, 0, 3);

                Control.Background = null;

                // Control.KeyPress += Control_KeyPress;

                Control.TextChanged += Control_TextChanged;
            }
        }

        public override bool DispatchKeyEvent(KeyEvent e)
        {
            if (e.Action == KeyEventActions.Down)
            {
                if (e.KeyCode == Keycode.Del)
                {
                    if (string.IsNullOrWhiteSpace(Control.Text))
                    {
                        (this.Element as BorderlessEntry).OnDelete();
                    }
                }
                //else
                //{
                //    (this.Element as BorderlessEntry).OnEditingChanged(Control.Text);
                //}
            }
           
            return base.DispatchKeyEvent(e);
        }

        private void Control_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            (this.Element as BorderlessEntry).OnEditingChanged(Control.Text);
        }

        //private void Control_KeyPress(object sender, KeyEventArgs e)
        //{
        //    bool deleteKey =  e.KeyCode == Android.Views.Keycode.Del;
        //    if (deleteKey)
        //    {
        //        (this.Element as BorderlessEntry).OnDelete();
        //    }
        //}

        public void hideSoftKeyboard()
        {
            var Activity = CrossCurrentActivity.Current.Activity;
            var currentFocus = CrossCurrentActivity.Current.Activity.CurrentFocus;
            if (currentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)Activity.GetSystemService(Context.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        }
        ~BorderlessEntryRenderer()
        {
            MessagingCenter.Unsubscribe<LoginOTPPageViewModel>(this, "UnfocusOTPNative");
            MessagingCenter.Unsubscribe<VerifyEmailMobileProfilePageViewModel>(this, "UnfocusOTPNative");
            MessagingCenter.Unsubscribe<VerifyEmailPageViewModel>(this, "UnfocusOTPNative");
            MessagingCenter.Unsubscribe<VerifySmsPageViewModel>(this, "UnfocusOTPNative" );
        }

    }
}
