#region Usings
using STC.iOS.Renderers;
using STC.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Page = Xamarin.Forms.Page;
using UIModalPresentationStyle = UIKit.UIModalPresentationStyle;
#endregion
[assembly: ExportRenderer(typeof(ContentPage), typeof(ContentPageRenderer))]
namespace STC.iOS.Renderers
{
    public sealed class ContentPageRenderer : PageRenderer
    {
        #region Overrides
        public override void WillMoveToParentViewController(UIViewController page)
        {
            if (page != null)
            {
                page.ModalPresentationStyle = UIModalPresentationStyle.FullScreen;
            }
            if (this.Element is Page formsPage)
            {
                if (formsPage is HomePage)
                {
                    //ignore SetUseSafeArea
                }
                else
                {
                    formsPage.On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
                }
                formsPage.BackgroundColor = Color.White;// (Color)STC.App.Current.Resources["PrimaryColor"];
            }
            base.WillMoveToParentViewController(page);
        }
        #endregion
    }
}
