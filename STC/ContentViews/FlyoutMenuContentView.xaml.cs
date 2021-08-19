using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace STC.ContentViews
{
    public partial class FlyoutMenuContentView : ContentView
    {
        public event EventHandler MenuToggled;
        public FlyoutMenuContentView()
        {
            InitializeComponent();
        }

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            MenuToggled.Invoke(sender, e);
        }

        void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            //MenuToggled.Invoke(sender, e);
        }
    }
}
