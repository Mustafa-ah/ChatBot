using Plugin.Media;
using STC.Common.CommonControlls;
using STC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
          
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            (BindingContext as ProfilePageViewModel).GetUserDetails();
        }
        protected override bool OnBackButtonPressed()
        {
            (BindingContext as ProfilePageViewModel).ProfileBackCommand.Execute(null);
            return true;
        }


    }
}