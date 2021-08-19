using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STC.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerifyEmailPage : ContentPage
    {
        public VerifyEmailPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            OTP1.RequestFocus();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
       
        }
        protected override bool OnBackButtonPressed()
        {
            (BindingContext as VerifyEmailPageViewModel)?.OpenLoginPageCommand.Execute(null);
            return true;
        }
    }
}