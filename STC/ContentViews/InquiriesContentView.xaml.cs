using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace STC.ContentViews
{
    public partial class InquiriesContentView : ContentView
    {
        public InquiriesContentView()
        {
            InitializeComponent();
        }

        public event EventHandler FaqsClicked;

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            FaqsClicked?.Invoke(sender, e);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            RootGrid.HeightRequest = height;
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (width > height)
                {
                    RootGrid.Margin = new Thickness(100, 5, 100, 30);
                }
                else
                {
                    RootGrid.Margin = new Thickness(20, 40, 20, 20);
                }
            }
            else
            {
                RootGrid.Margin = new Thickness(20, 20, 20, 20);
            }
        }
    }
}
