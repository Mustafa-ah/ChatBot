using System;
using STC.Common.CommonControlls;
using STC.iOS.Renderers;
using STC.ViewModels;
using STC.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ChatEditor), typeof(ChatEditorRenderer))]
namespace STC.iOS.Renderers
{
    public class ChatEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            var element = this.Element as ChatEditor;

            Control.InputAccessoryView = null;
            Control.ShouldEndEditing += DisableHidingKeyboard;

            MessagingCenter.Subscribe<BaseViewModel>(this, "FocusKeyboardStatus", (sender) =>
            {

                if (Control != null)
                {
                    
                    Control.ShouldEndEditing += EnableHidingKeyboard;

                    UIApplication.SharedApplication.KeyWindow.EndEditing(true);
                }

                MessagingCenter.Unsubscribe<BaseViewModel>(this, "FocusKeyboardStatus");
            });
            MessagingCenter.Subscribe<RequestDetailsPage>(this, "FocusKeyboardStatus", (sender) =>
            {

                if (Control != null)
                {
                    //Control.InputView = new UIView();
                    Control.ShouldEndEditing += EnableHidingKeyboard;

                    UIApplication.SharedApplication.KeyWindow.EndEditing(true);

                    Control.ShouldEndEditing += DisableHidingKeyboard;
                }

            });
        }

        private bool DisableHidingKeyboard(UITextView textView)
        {
            return false;
        }

        private bool EnableHidingKeyboard(UITextView textView)
        {
            return true;
        }
    }
}
