using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using STC.Common.Enums;
using STC.Interfaces;
using STC.Services.Account;
using STC.Settings;
using STC.ViewModels.Routes;
using Xamarin.Forms;

namespace STC.ViewModels.DialogsViewModels
{
    public class LanguageDialogViewModel : BaseViewModel
    {
        int _selectedLanguage;

        public int SelectedLanguage
        {
            get { return _selectedLanguage; }
            set { SetProperty(ref _selectedLanguage, value); }
        }

        private readonly IAppCulture _appCulture;
        private readonly IAccountService _accountService;

        public LanguageDialogViewModel(INavigationService navigationService,
             IAppCulture appCulture,
             IAccountService accountService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _appCulture = appCulture;
            _accountService = accountService;
            SelectedLanguage = Setting.AppLanguage;
        }

        public ICommand ChangeLanguageCommand => new Command(ChangeLanguageCommandExcute);
        public ICommand ChooseEnglishLanguageCommand => new Command(ChooseEnglishanguageCommandExcute);
        public ICommand ChooseArabicLanguageCommand => new Command(ChooseArabicLanguageCommandExcute);

        
        private async void ChangeLanguageCommandExcute(object obj)
        {
            try
            {
                if (SelectedLanguage == Setting.AppLanguage)
                {
                    return;
                }

                string lang = "";
                if (SelectedLanguage == (int)Languages.English)
                {
                    Setting.AppLanguage = (int)Languages.English;
                    _appCulture.SetAppCulture(Languages.English);
                    lang = "en";
                    await NavigateToHome();
                }
                else if (SelectedLanguage == (int)Languages.Arabic)
                {
                    Setting.AppLanguage = (int)Languages.Arabic;
                    _appCulture.SetAppCulture(Languages.Arabic);

                    lang = "ar";
                    await NavigateToHome();
                }

              await  _accountService.UpdateFirebaseLanguage(Setting.AuthAccessToken, Setting.UserId, lang);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           

           
        }

        private void ChooseArabicLanguageCommandExcute(object obj)
        {
            SelectedLanguage = (int)Common.Enums.Languages.Arabic;
        }

        private void ChooseEnglishanguageCommandExcute(object obj)
        {
            SelectedLanguage = (int)Common.Enums.Languages.English;
        }

        private async Task NavigateToHome()
        {
            var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.HomeRoute}");
            // for IOS
            if (!naved.Success)
            {
                await NavigationService.NavigateAsync($"/{ViewsRoutes.HomeRoute}");
            }

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        }
    }
}
