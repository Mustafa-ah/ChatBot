using System;
using System.Collections.Generic;
using STC.Resources;
using Xamarin.Forms;

namespace STC.ContentViews
{
    public partial class HomeContentView : ContentView
    {
        public event EventHandler MenuToggled;
        public HomeContentView()
        {
            InitializeComponent();

            if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour < 12)
                Greetings.Text = AppResources.GoodMorning;
            else
                Greetings.Text = AppResources.GoodEvening;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            MenuToggled.Invoke(sender, e);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.iOS)
            {
                if (width > height)
                {
                    RootGrid.Margin = new Thickness(100, 5, 100, 30);
                }
                else
                {
                    RootGrid.Margin = new Thickness(20, 80, 20, 20);
                }
            }
           
        }
    }
}
