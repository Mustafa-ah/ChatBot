using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

            BarBackgroundColor = Color.FromHex("#f5f5f5");
            SelectedTabColor = Color.Red;
            UnselectedTabColor = Color.FromHex("#9A9A9A");

           // On<Android>().SetBarItemColor(Color.FromHex("#9A9A9A"));  // Unselected image+text color
           // On<Android>().SetBarSelectedItemColor(Color.Red); // Selected image+text color

        }

        
    }
}
