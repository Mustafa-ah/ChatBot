using System;
using STC.Common.CommonControlls;
using STC.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(UnscrollableScrollView), typeof(CustomScrollRenderer))]
namespace STC.iOS.Renderers
{
    public class CustomScrollRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            // IsScrollEnabled just a custom property
            // handled it in OnPropertyChanged too
            ScrollEnabled = false;
        }
    }
}
