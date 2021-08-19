using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace STC.Views
{
    public partial class FilePage : ContentPage
    {
        public FilePage()
        {
            InitializeComponent();
        }
        void WebView_Navigated(System.Object sender, Xamarin.Forms.WebNavigatedEventArgs e)
        {
            var viewModel = this.BindingContext as ViewModels.FilePageViewModel;
            viewModel.IsBusy = false;
        }
    }
}
