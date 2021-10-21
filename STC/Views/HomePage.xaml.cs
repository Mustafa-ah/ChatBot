using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using STC.Enums;
using STC.ViewModels;
using Xamarin.Forms;

namespace STC.Views
{
    public partial class HomePage : ContentPage
    {

        const float FlyoutCornerRadius = 25f;
        readonly int x_dir;
        bool _isFlyoutOpen = false;
        double _scale;
        double _tabsScale;
        uint _flyoutSpeed = 200;
        double _pagePositionX;
        double _flyoutTranslationX;

        public HomePageViewModel ViewModel { get; set; }

        public HomePage()
        {
            InitializeComponent();
            ViewModel = this.BindingContext as HomePageViewModel;

            _scale = MainContent.Scale;
            _tabsScale = 1.3;
            _pagePositionX = MainContent.TranslationX;

            x_dir = ViewModel.Lang == (int)Common.Enums.Languages.Arabic ? 1 : -1;

            // Add event listeners for SizeChanged - Allows us to capture page values after it is rendered
            MainContent.SizeChanged += OnMainContentSizeChanged;


            BtnInquiries_Tapped(this, null);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            (this.BindingContext as HomePageViewModel).GetUserChanels();
        }

        private double width = 0;
        private double height = 0;

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height); //must be called
            if (this.width != width || this.height != height)
            {
                this.width = width;
                this.height = height;
                //reconfigure layout
                _flyoutTranslationX = Width * 0.70;

                ReToggleFlyout();
            }
        }

        void OnMainContentSizeChanged(object sender, EventArgs e)
        {
            MainContent.SizeChanged -= OnMainContentSizeChanged;
            _flyoutTranslationX = MainContent.Width * 0.70;

            //if (Flyout.Children.Count == 1 && Flyout.Children[0] is Layout menuPage)
            //{
            //    var flyoutPadding = Flyout.Width - (Flyout.Width * .8);
            //    (Flyout.Children[0] as Layout).Padding = new Thickness(0, 0, flyoutPadding, 0);
            //}

            //homeTab.WidthRequest = MainContent.Width;
            //RequestTab.WidthRequest = MainContent.Width;
            //FaqTab.WidthRequest = MainContent.Width;
            //InquiresTab.WidthRequest = MainContent.Width;
        }


        void ToggleFlyout()
        {
            if (_isFlyoutOpen)
            {
                //MainContent.ScaleTo(_scale, _flyoutSpeed);
                MainContent.TranslateTo(_pagePositionX * x_dir, 0, _flyoutSpeed);
                //MainContent.CornerRadius = 0;
            }
            else
            {
                var sumX = (Flyout.TranslationX + _flyoutTranslationX) * x_dir;
                // MainContent.ScaleTo(_scale * .9, _flyoutSpeed);
                MainContent.TranslateTo(sumX, 0, _flyoutSpeed);
                //MainContent.CornerRadius = FlyoutCornerRadius;
            }

            _isFlyoutOpen = !_isFlyoutOpen;
        }



        //void SwipeGestureRecognizer_Swiped(System.Object sender, Xamarin.Forms.SwipedEventArgs e)
        //{
        //    Console.WriteLine(e.ToString());
        //    MainScroll.ScrollToAsync(MainContent.Width, 0, true);
        //}



        void BtnHome_Tapped(Object sender, EventArgs e)
        {
            SetDefaultScal();
            ViewModel.SelectedTab = HomeTabs.Home;
            HideAllTabs();
          //  homeTab.IsVisible = true;
            //ImageHome.ScaleTo(_tabsScale, speed);
        }
        void BtnInquiries_Tapped(Object sender, EventArgs e)
        {
            SetDefaultScal();
            ViewModel.SelectedTab = HomeTabs.Inquiries;
            HideAllTabs();
            InquiresTab.IsVisible = true;
            ImageInquiries.ScaleTo(_tabsScale, speed);
        }
        void BtnRequests_Tapped(Object sender, EventArgs e)
        {
            SetDefaultScal();
            ViewModel.SelectedTab = HomeTabs.Requests;
            HideAllTabs();
            RequestTab.IsVisible = true;
            ImageRequests.ScaleTo(_tabsScale, speed);
        }
        void BtnFAQ_Tapped(Object sender, EventArgs e)
        {
            SetDefaultScal();
            ViewModel.SelectedTab = HomeTabs.FAQ;
            HideAllTabs();
            //FaqTab.IsVisible = true;
           // ImageFaq.ScaleTo(_tabsScale, speed);
        }
        private uint speed = 100;
        private void SetDefaultScal()
        {
          //  ImageHome.ScaleTo(1, speed);
            ImageInquiries.ScaleTo(1, speed);
            ImageRequests.ScaleTo(1, speed);
            //ImageFaq.ScaleTo(1, speed);
        }

        private void HideAllTabs()
        {
            //homeTab.IsVisible = false;
            RequestTab.IsVisible = false;
           // FaqTab.IsVisible = false;
            InquiresTab.IsVisible = false;
        }

        void HomeContentView_MenuToggled(System.Object sender, System.EventArgs e)
        {
            ToggleFlyout();
        }

        void InquiriesContentView_FaqsClicked(System.Object sender, System.EventArgs e)
        {
            BtnFAQ_Tapped(sender,e);
        }

        private void ReToggleFlyout()
        {
            if (_isFlyoutOpen)
            {
                _isFlyoutOpen = !_isFlyoutOpen;

                ToggleFlyout();
            }
        }
    }
}
