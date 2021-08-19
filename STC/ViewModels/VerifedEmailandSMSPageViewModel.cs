using Prism.Navigation;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class VerifedEmailandSMSPageViewModel : BaseViewModel
    {
        public VerifedEmailandSMSPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {

        }

      
        public ICommand OpenCongratulationsRegisterCommand => new Command(OpenLoginRoute);
        public ICommand OpenLoginPageCommand => new Command(OpenLoginRoute);

        private async void OpenLoginRoute()
        {
            ShowLoading();
            var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            // for IOS
            if (!naved.Success)
            {
                await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            }
            HideLoading();
        }


    }
}
