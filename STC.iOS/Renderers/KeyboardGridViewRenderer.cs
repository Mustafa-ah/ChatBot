using CoreGraphics;
using Foundation;
using STC.Common.CommonControlls;
using STC.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(KeyboardGridView), typeof(KeyboardGridViewRenderer))]
namespace STC.iOS.Renderers
{
    public class KeyboardGridViewRenderer : ViewRenderer
    {
        //https://www.flokri.com/development/xamarin-development/howto-xamarin-forms-automatically-move-controls-when-keyboard-is-shown/
        #region instances
        NSObject _showObserver;
        NSObject _hideObserver;
        #endregion

        #region overrides
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            // disable the animation to prevent that the controls are flickering when the margin changes
            AnimationsEnabled = false;

            if (e.NewElement != null)
            {
                // initialte the observer
                _showObserver = _showObserver ?? UIKeyboard.Notifications.ObserveWillShow(OnKeyboardShow);
                _hideObserver = _hideObserver ?? UIKeyboard.Notifications.ObserveWillHide(OnKeyboardHide);
            }
            if (e.OldElement != null)
            {
                // dispose the observer
                _showObserver?.Dispose();
                _hideObserver?.Dispose();
            }
        }
        #endregion

        #region EventHandler
        /// <summary>
        /// Will be called when the keyboard will be shown and set the correct margin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnKeyboardShow(object sender, UIKeyboardEventArgs args)
        {
            NSValue result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            CGSize keyboardSize = result.RectangleFValue.Size;

            if (Element != null)
            {
                int _extraMargin = 0;
                var parent_ = Element as KeyboardGridView;
                var inGeneral = parent_.InGeneral;
                if (inGeneral)
                {
                    _extraMargin = 50;
                }
                // push the view up to keyboard heigth when keyboard is activated
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height - _extraMargin);
            }
        }

        /// <summary>
        /// Will be called when the keyboard will be dismissed and removes the margin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnKeyboardHide(object sender, UIKeyboardEventArgs args)
        {
            // if the element is not null, set the margin to zero when the keyboard is dismisssed
            if (Element != null)
                Element.Margin = new Thickness(0);
        }
        #endregion
    }

}