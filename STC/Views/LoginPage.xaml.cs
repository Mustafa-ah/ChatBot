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
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel ViewModel;
       
        public LoginPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as LoginPageViewModel;

           
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            ForgetView.TranslateTo(0, ForgetView.Height, 50, Easing.SinOut);
        }
        private void EntryContentView_Focused(object sender, EventArgs e)
        {
            ShrinkView();
        }

        private void EntryPasswordContentView_Focused(object sender, EventArgs e)
        {
            ShrinkView();
        }

        private void ContentEntry_Unfocused(object sender, EventArgs e)
        {
            ExpandView();
        }

        private void PasswordEntry_Unfocused(object sender, EventArgs e)
        {
            ExpandView();
        }

        private void ShrinkView()
        {
            //LogoImg.Margin = new Thickness(0, 20, 0, 20);
            WelcomeStack.Margin = new Thickness(0, 0, 0, 10);
            MainStack.Spacing = 15;
            //RootScroll.ScrollToAsync(0, 1000, false);
        }

        private void ExpandView()
        {
           // LogoImg.Margin = new Thickness(0,20,0,35);
            WelcomeStack.Margin = new Thickness(0,0,0,10);
            MainStack.Spacing = 30;
            //RootScroll.ScrollToAsync(0, 0, false);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _isRecoverDailogOpen = true;
            DimBox.IsVisible = true;
            ForgetView.TranslateTo(0, 0, 250, Easing.SinIn);
        }

        private void TapGestureRecognizer_Tapped_Space(object sender, EventArgs e)
        {
            CloseRecoverDailog();
        }

        private void Button_Recover_Clicked(object sender, EventArgs e)
        {
            CloseRecoverDailog();
            ViewModel.NavigationService.NavigateAsync(ViewModels.Routes.ViewsRoutes.ForgetPasswordRoute);
        }

        private bool _isRecoverDailogOpen;
        private void CloseRecoverDailog()
        {
            _isRecoverDailogOpen = false;
            DimBox.IsVisible = false;
            ForgetView.TranslateTo(0, ForgetView.Height, 250, Easing.SinOut);
        }

        protected override bool OnBackButtonPressed()
        {
            if (_isRecoverDailogOpen)
            {
                CloseRecoverDailog();
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}