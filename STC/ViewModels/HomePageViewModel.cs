using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using STC.Enums;
using STC.Interfaces;
using STC.Models;
using STC.Services.Account;
using STC.Services.APPStaticData;
using STC.Services.FAQs;
using STC.Services.Inquiry;
using STC.Settings;
using STC.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {

        private readonly IFAQService _faqservice;
        private readonly IAccountService _accountService;
        private readonly IAPPStaticDataService _APPStaticDataService;
        private string facebookLink { get; set; }
        private string twitterLink { get; set; }
        private string youtubeLink { get; set; }
        private string instgramLink { get; set; }
        private string mobile { get; set; }
        private string hotline { get; set; }
        private string mail { get; set; }
        private bool _isFBAccount;

        private ObservableCollection<Chanel> _chanles;
        public ObservableCollection<Chanel> Chanels
        {
            get { return _chanles; }
            set { SetProperty(ref _chanles, value); }

        }

        public bool IsFBAccount
        {
            get { return _isFBAccount; }
            set { SetProperty(ref _isFBAccount, value); }
        }
        private bool _openToaster;
        public bool OpenToaster
        {
            get { return _openToaster; }
            set { SetProperty(ref _openToaster, value); }
        }
        private bool _isTWAccount;
        public bool IsTWAccount
        {
            get { return _isTWAccount; }
            set { SetProperty(ref _isTWAccount, value); }
        }
        private bool _isYoAccount;
        public bool IsYoAccount
        {
            get { return _isYoAccount; }
            set { SetProperty(ref _isYoAccount, value); }
        }
        private bool _isInstccount;
        public bool IsInstAccount
        {
            get { return _isInstccount; }
            set { SetProperty(ref _isInstccount, value); }
        }
        private bool IsGetRequestsProcessing = false;
        public HomePageViewModel(INavigationService navigationService, IAPPStaticDataService APPStaticDataService, IFAQService faqservice, IAccountService accountService,
            ISettingsService settingsService)
            : base(navigationService, settingsService)
        {
            //SelectedTab = HomeTabs.Home;
            _faqservice = faqservice;
            _accountService = accountService;
            _APPStaticDataService = APPStaticDataService;
            //FAQContentViewViewModel = new FAQPageViewModel(navigationService, _faqservice, settingsService);


            Chanels = new ObservableCollection<Chanel>();
            //for (int i = 0; i < 5; i++)
            //{
            //    Chanels.Add(new Chanel());
            //}

        }

        private void UnsubscribeMessages()
        {
            MessagingCenter.Unsubscribe<BaseViewModel, Notification>(this, "broadcastnotify");
            MessagingCenter.Unsubscribe<App>(this, "OnSleep");
            MessagingCenter.Unsubscribe<App>(this, "OnResume");
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private bool _hasNewNotifications;
        public bool HasNewNotifications
        {
            get { return _hasNewNotifications; }
            set { SetProperty(ref _hasNewNotifications, value); }
        }

        //
        private HomeTabs _selectedTab;
        public HomeTabs SelectedTab
        {
            get => _selectedTab;
            set
            {
                this.SetProperty(ref this._selectedTab, value);
                OnSelectedTabChanged();
            }
        }

        private FAQPageViewModel _fAQContentViewViewModel;
        public FAQPageViewModel FAQContentViewViewModel
        {
            get { return _fAQContentViewViewModel; }
            set { SetProperty(ref _fAQContentViewViewModel, value); }
        }

        public void OnSelectedTabChanged()
        {
            switch (SelectedTab)
            {
                case HomeTabs.FAQ:
                    FAQContentViewViewModel.ViewAppeared();
                    break;
                default:
                    break;
            }
        }
        public async void GetUserChanels()
        {
            try
            {
                //if (!IsConncted()|| string.IsNullOrEmpty(Setting.AuthAccessToken))
                //{
                //    return;
                //}
                ShowLoading();

                var respons = await _accountService.GetUserChanels(Setting.AuthAccessToken);

                Name = "";
                if(respons.Success)
                {
                    foreach (var item in respons.Channels)
                    {
                        Chanels.Add(item);
                    }
                }


                HideLoading();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //ShowErrorToast( ex.Message);
            }

        }


        #region Commands
        public ICommand OpenRequestsListCommand => new Command(async () => {

            if (IsGetRequestsProcessing)
                return;
            IsGetRequestsProcessing = true;
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.RequestListRoute);
            IsGetRequestsProcessing = false;
        }
        );
        public ICommand OpenNewInquiryCommand => new Command(() => NavigationService.NavigateAsync(Routes.ViewsRoutes.InquiriesRoute));
        public ICommand OpenProfileCommand => new Command( async() => {
            if (IsBusy)
                return;
            ShowLoading();
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.ProfileRoute);
            HideLoading();
        });
        public ICommand OpenAboutUsCommand => new Command(async() => {
            if (IsBusy)
                return;
            ShowLoading();
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.AboutUsRoute);
            HideLoading();
        });
        public ICommand OpenContactUsCommand => new Command(async () => {
            if (IsBusy)
                return;
            ShowLoading();
            var parameters = new NavigationParameters
                        {
                           { "Mobile",mobile},
                           {"Email",mail},
                           {"Hotline",hotline}
                        };
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.ContactUsRoute, parameters);
            HideLoading();
        });
        public ICommand OpenInquiriesCommand => new Command(ExecuteOpenInquiriesCommand);

        private async void ExecuteOpenInquiriesCommand(object obj)
        {
            if (IsBusy)
                return;
            ShowLoading();
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.InquiriesRoute);
            HideLoading();
        }

        private async void OpenNewRequestCommandExcute(object obj)
        {
            if (IsBusy)
                return;

            ShowLoading();

            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            await NavigationService.NavigateAsync(Routes.ViewsRoutes.NewRequetRoute);

            HideLoading();
        }
        public ICommand OpenNewRequestCommand => new Command(OpenNewRequestCommandExcute);


        public ICommand OpenSettingsCommand => new Command(() => NavigationService.NavigateAsync(Routes.ViewsRoutes.SettingsRoute));
        public ICommand OpenFaceBook => new Command(() =>OpenLink(facebookLink));
        public ICommand OpenTwitter => new Command(() => OpenLink(twitterLink));
        public ICommand OpenYoutube => new Command(() => OpenLink(youtubeLink));
        public ICommand OpenInstagram => new Command(() => OpenLink(instgramLink));
       // public ICommand OpenNotificationsCommand => new Command(() => NavigationService.NavigateAsync(Routes.ViewsRoutes.NotificationRoute));
        public ICommand LogoutCommand => new Command(ExecuteLogoutCommand);

        private async void OpenLink(string Link)
        {

            try
            {
                await Launcher.OpenAsync(Link);
            }
            catch
            {
                ShowErrorToast(Resources.AppResources.CanNotOpenLink);
            }
            
        }
        private async void ExecuteLogoutCommand()
        {
            ShowLoading();
            UnsubscribeMessages();
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.LoginRoute);
            HideLoading();
        }

        #endregion

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            HasNewNotifications = Setting.HasNewNotifications;
        }

       
        public void GetContactUsData()
        {
            Task.Run(async () =>
            {
                try
                {
                    if (!IsConncted())
                    {
                        return;
                    }
                    var res = await this._APPStaticDataService.ContactUs(Setting.AuthAccessToken);
                    if(res.StatusCode==200)
                    {
                        this.mail = res.Data[0].email;
                        this.mobile = res.Data[0].mobileNumber;
                        this.hotline = res.Data[0].hotline;
                        this.facebookLink = res.Data[0].facebookAccount;
                        this.twitterLink = res.Data[0].twitterAccount;
                        this.instgramLink = res.Data[0].instagramAccount;
                        this.youtubeLink = res.Data[0].youtubeAccount;
                        this.IsInstAccount = res.Data[0].isInstgramVisible;
                        this.IsTWAccount = res.Data[0].isTwitterVisible;
                        this.IsYoAccount = res.Data[0].isYouTubeVisibe;
                        this.IsFBAccount = res.Data[0].isFaceBookVisible;
                    }
                
                }
                catch (Exception ex)
                {

                }
           

            }
            );
          
        }
    }
}
