using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using STC.Services;
using STC.Views;
using Prism.Unity;
using Prism.Ioc;
using Prism;
using STC.ViewModels;
using STC.ViewModels.Routes;
using STC.ViewModels.DialogsViewModels;
using STC.Dialogs;
using STC.ContentViews;
using STC.Settings;
using STC.Interfaces;
using STC.Common.Enums;
using STC.Services.RequestProvider;
using STC.Services.Inquiry;
using STC.Services.Account;
using STC.Services.Requests;
using STC.Services.FAQs;
using STC.Services.Contract;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Plugin.FirebasePushNotification;
using System.Globalization;
using STC.Services.APPStaticData;

[assembly: ExportFont("STCForward-Bold.otf", Alias = "STCForwardBold")]
[assembly: ExportFont("STCForward-BoldItalic.otf", Alias = "STCForwardBoldItalic")]
[assembly: ExportFont("STCForward-ExtraBold.otf", Alias = "STCForwardExtraBold")]
[assembly: ExportFont("STCForward-ExtraBoldItalic.otf", Alias = "STCForwardExtraBoldItalic")]
[assembly: ExportFont("STCForward-Italic.otf", Alias = "STCForwardItalic")]
[assembly: ExportFont("STCForward-Light.otf", Alias = "STCForwardLight")]
[assembly: ExportFont("STCForward-LightItalic.otf", Alias = "STCForwardLightItalic")]
[assembly: ExportFont("STCForward-Medium.otf", Alias = "STCForwardMedium")]
[assembly: ExportFont("STCForward-MediumItalic.otf", Alias = "STCForwardMediumItalic")]
[assembly: ExportFont("STCForward-Regular.otf", Alias = "STCForwardRegular")]
[assembly: ExportFont("STCForward-Thin.otf", Alias = "STCForwardThin")]
[assembly: ExportFont("STCForward-ThinItalic.otf", Alias = "STCForwardThinItalic")]
namespace STC
{
    public partial class App : PrismApplication
    {
        public App()
        {
            InitializeComponent();

          //  MainPage = new MainPage();
        }

        protected override void OnStart()
        {
           // RegisterPushNtificationEvents();
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send(this, "OnSleep");
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, "OnResume");
        }

        public App(IPlatformInitializer platformInitializer = null) : base(platformInitializer) { }

        protected override void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDI2ODkxQDMxMzkyZTMxMmUzMEx0TVNpT3hZYnBzWnZHNGJlbVE4L0RZaXhRNXdjMjZXQzFWTXBZVTBTM0E9");

            InitializeComponent();

           // this.Container.Resolve<IAppCulture>().SetAppCulture((Common.Enums.Languages)this.Container.Resolve<ISettingsService>().AppLanguage);

            var setting = this.Container.Resolve<ISettingsService>();
            //set app language
            SetAppLang(setting);

            if (string.IsNullOrEmpty(setting.AuthAccessToken))
            {
                NavigationService.NavigateAsync($"{ViewsRoutes.HomeRoute}");
            }
            else
            {
                NavigationService.NavigateAsync($"{ViewsRoutes.HomeRoute}");
            }
            System.Diagnostics.Debug.WriteLine($" CrossFirebasePushNotification TOKEN : {setting.PushNotificationDeviceToken}");
            // NavigationService.NavigateAsync(ViewsRoutes.HomeRoute);
            // MainPage = new SignaturePage();
            //test commit
        }

        private void SetAppLang(ISettingsService setting)
        {
            Languages appLang = Languages.English;
            if (setting.AppLanguage == 0)
            {
                var cl = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLower();

                if (cl == "ar")
                {
                     appLang = Languages.Arabic;
                }
                setting.AppLanguage = (int)appLang;
            }
            else
            {
                appLang = (Languages)setting.AppLanguage;
            }
            this.Container.Resolve<IAppCulture>().SetAppCulture(appLang);
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>(ViewsRoutes.NavigationPage);
            containerRegistry.RegisterForNavigation<ContactusPage, ContactusPageViewModel>(ViewsRoutes.ContactUsRoute);
            containerRegistry.RegisterForNavigation<RequestsListPage, RequestsListPageViewModel>(ViewsRoutes.RequestListRoute);
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>(ViewsRoutes.HomeRoute);
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>(ViewsRoutes.ProfileRoute);
            containerRegistry.RegisterForNavigation<RequestDetailsPage, RequestDetailsPageViewModel>(ViewsRoutes.RequestDetailsRoute);
            containerRegistry.RegisterForNavigation<NewInquiryPage, NewInquiryPageViewModel>(ViewsRoutes.InquiriesRoute);
            containerRegistry.RegisterForNavigation<NewRequestPage, NewRequestPageViewModel>(ViewsRoutes.NewRequetRoute);
            containerRegistry.RegisterForNavigation<AboutUsPage, AboutUsPageViewModel>(ViewsRoutes.AboutUsRoute);
            containerRegistry.RegisterForNavigation<ForgetPassowrdPage, ForgetPassowrdPageViewModel>(ViewsRoutes.ForgetPasswordRoute);
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>(ViewsRoutes.LoginRoute) ;
            containerRegistry.RegisterForNavigation<NotificationsPage, NotificationsPageViewModel>(ViewsRoutes.NotificationRoute);
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>(ViewsRoutes.RegisterRoute);
            containerRegistry.RegisterForNavigation<SignaturePage, SignaturePageViewModel>(ViewsRoutes.SignatureRoute);
            containerRegistry.RegisterForNavigation<VerifySmsPage, VerifySmsPageViewModel>(ViewsRoutes.VerifySMSRoute);
            containerRegistry.RegisterForNavigation<VerifyEmailPage, VerifyEmailPageViewModel>(ViewsRoutes.VerifyEmailRoute);
            containerRegistry.RegisterForNavigation<SettingsPage, SettingsPageViewModel>(ViewsRoutes.SettingsRoute);
            containerRegistry.RegisterForNavigation<LoginOTPPage, LoginOTPPageViewModel>(ViewsRoutes.LoginOTPtRoute);
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>(ViewsRoutes.ChangePasswordtRoute);
            containerRegistry.RegisterForNavigation<VerifedEmailandSMSPage, VerifedEmailandSMSPageViewModel>(ViewsRoutes.VerifiedEmailandSms);
            containerRegistry.RegisterForNavigation<ContractPage, ContractPageViewModel>(ViewsRoutes.ContractPageRout);
            containerRegistry.RegisterForNavigation<CongratulationsPage, CongratulationsPageViewModel>(ViewsRoutes.CongratulationsRoute);
            containerRegistry.RegisterForNavigation<SetNewPasswordPage, SetNewPasswordPageViewModel>(ViewsRoutes.SetNewPasswordRoute);
            containerRegistry.RegisterForNavigation<FilePage, FilePageViewModel>(ViewsRoutes.FileRoute);
            containerRegistry.RegisterForNavigation<VerifyEmailMobileProfilePage, VerifyEmailMobileProfilePageViewModel>(ViewsRoutes.VerifyEmailMobileProfilePage);
            containerRegistry.RegisterForNavigation<CongratulationsRequestCreatedPage, CongratulationsRequestCreatedPageViewModel>(ViewsRoutes.RequestCreatedPageRoute);
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>(ViewsRoutes.MapPageRoute);


            //containerRegistry.RegisterForNavigation<MyPage, MyPageViewModel>(); 
            containerRegistry.RegisterDialog<TermsDialog, TermsDialogViewModel>();

            containerRegistry.Register<DidntSaveDialogViewModel>();
            containerRegistry.Register<LanguageDialogViewModel>();

            containerRegistry.RegisterSingleton<ISettingsService, SettingsService>();
            containerRegistry.RegisterSingleton<IAppCulture, AppCulture>();


            containerRegistry.RegisterSingleton<IRequestProvider, RequestProvider>();
            
            containerRegistry.RegisterSingleton<IFAQService, FAQService>();
            containerRegistry.RegisterSingleton<IAccountService, AccountService>();
            containerRegistry.RegisterSingleton<IRequestService, RequestService>();

            containerRegistry.Register<IInquiryService, InquiryService>();
            containerRegistry.Register<IInquiryDataService, InquiryDataService>();
            containerRegistry.Register<IContractService, ContractService>();
            containerRegistry.Register<IAPPStaticDataService, APPStaticDataService>();
        }


        #region Push notification Events

        private void RegisterPushNtificationEvents()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                try
                {
                    var setting = this.Container.Resolve<ISettingsService>();
                    setting.PushNotificationDeviceToken = p.Token;
                    if (!string.IsNullOrEmpty(setting.AuthAccessToken))
                    {
                        string lang = setting.AppLanguage == (int)Common.Enums.Languages.Arabic ? "ar" : "en";

                        this.Container.Resolve<IAccountService>().AddFirebaseDeviceToken(setting.AuthAccessToken, setting.PushNotificationDeviceToken, setting.UserId, lang);
                    }
                    System.Diagnostics.Debug.WriteLine($" CrossFirebasePushNotification TOKEN : {p.Token}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            };

            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                this.Container.Resolve<ISettingsService>().HasNewNotifications = true;
                System.Diagnostics.Debug.WriteLine("Received");

            };

            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }


            };
        }

        #endregion

    }
}
