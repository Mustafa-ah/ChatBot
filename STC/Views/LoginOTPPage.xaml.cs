using STC.Common.Enums;
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
    public partial class LoginOTPPage : ContentPage
    {
        private LoginOTPPageViewModel LoginOTPPageViewModel;
        public LoginOTPPage()
        {
            InitializeComponent();
            LoginOTPPageViewModel = BindingContext as LoginOTPPageViewModel;
          
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            OTP1.RequestFocus();
        }
        protected override bool OnBackButtonPressed()
        {
            //if(LoginOTPPageViewModel==null)
            //{
            //    LoginOTPPageViewModel = BindingContext as LoginOTPPageViewModel;
            //}
            //if(LoginOTPPageViewModel.VerifyType== (int)VerifyType.Email || LoginOTPPageViewModel.VerifyType == (int)VerifyType.Phone)
            //{
             
            //    LoginOTPPageViewModel.NavigationService.GoBackAsync();
            //}
    
            return base.OnBackButtonPressed();
        }
        protected override void OnDisappearing()
        {
          
           
            base.OnDisappearing();

        }
      
        
    }
}