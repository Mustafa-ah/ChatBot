using Prism.Navigation;
using STC.Common.Enums;
using STC.Services.Account;
using STC.Settings;
using STC.ViewModels.Routes;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class SettingsPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public SettingsPageViewModel(INavigationService navigationService, IAccountService accountService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _accountService = accountService;
            SetCurrentLanguage();
        }

        public string CurrentLanguage { get; set; }

        public ICommand ChangeLanguageCommand => new Command(ChangeLanguageCommandExcute);
        public ICommand LogoutCommand => new Command(LogoutCommandExcute);


        private async  void LogoutCommandExcute(object obj)

        {
            if (!IsConncted())
            {
                return;
            }
            ShowLoading();
           await _accountService.DeleteFirebaseDeviceToken(Setting.AuthAccessToken, Setting.PushNotificationDeviceToken, Setting.UserId);

            Setting.AuthAccessToken = string.Empty;
            Setting.UserId = string.Empty;
            Setting.GeneralInquiryId = string.Empty;

            var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            // for IOS
            if (!naved.Success)
            {
                await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            }

        }

        private async void ChangeLanguageCommandExcute(object obj)
        {
           await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.LanguageDialog());
        }

        private void SetCurrentLanguage()
        {
            CurrentLanguage = Setting.AppLanguage switch
            {
                (int)Common.Enums.Languages.Arabic => "العربية",
                (int)Common.Enums.Languages.English => "English",
                _ => string.Empty
            };
        }
    }
}
