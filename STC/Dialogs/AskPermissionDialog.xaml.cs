using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace STC.Dialogs
{
    public partial class AskPermissionDialog : PopupPage
    {
        public AskPermissionDialog()
        {
            InitializeComponent();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Xamarin.Essentials.Launcher.OpenAsync(new Uri("app-settings:"));
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }
    }
}
