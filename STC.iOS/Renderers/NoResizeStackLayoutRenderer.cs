using System;
using System.Linq;
using STC.Common.CommonControlls;
using STC.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoResizeStackLayout), typeof(NoResizeStackLayoutRenderer))]
namespace STC.iOS.Renderers
{
    public class NoResizeStackLayoutRenderer : ViewRenderer<StackLayout, UIView>
    {
        //https://www.flokri.com/development/xamarin-development/xamarin-forms-non-resize-stack-view-ios/
        #region overrides
        protected override void OnElementChanged(ElementChangedEventArgs<StackLayout> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                e.NewElement.SizeChanged += StopResizing;
            if (e.OldElement != null)
                e.OldElement.SizeChanged -= StopResizing;
        }
        #endregion

        #region EventHandler
        /// <summary>
        /// Remove the margin from all child elements to stop resizing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StopResizing(object sender, EventArgs args) =>
            ((NoResizeStackLayout)sender).Children.ToList().ForEach(x => x.Margin = new Thickness(0));
        #endregion 
    }
}
