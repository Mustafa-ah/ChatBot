using Prism.Navigation;
using STC.Interfaces;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels.DialogsViewModels
{
    class ForgetPassowrdDialogViewModel : BaseViewModel
    {
        int _selectedType;

        public int SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        public ForgetPassowrdDialogViewModel(INavigationService navigationService,
          IAppCulture _appCulture,
         ISettingsService settingsService) : base(navigationService, settingsService)
        {
            SelectedType = 1;
            Setting.VerifyType = 1;
        }

      //  public ICommand ChangeLanguageCommand => new Command(ChangeLanguageCommandExcute);
        public ICommand ChooseEmailCommand => new Command(()=> { SelectedType = Setting.VerifyType  = 1; });
        public ICommand ChoosePhoneCommand => new Command(()=> { SelectedType = Setting.VerifyType  = 2; });

        public ICommand SaveCommand => new Command(SaveCommandExcute);

        private async void SaveCommandExcute(object obj)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
           // await NavigationService.NavigateAsync(ViewsRoutes.ForgetPasswordRoute);
        }

    }

}
