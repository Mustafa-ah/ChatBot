using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace STC.ContentViews
{
    public partial class RequestsContentView : ContentView
    {
        public RequestsContentView()
        {
            InitializeComponent();
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
