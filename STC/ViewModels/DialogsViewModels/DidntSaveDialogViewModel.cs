using Prism.Navigation;
using STC.Interfaces;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class DidntSaveDialogViewModel : BaseViewModel
    {
        public DidntSaveDialogViewModel(INavigationService navigationService,
        IAppCulture _appCulture,
       ISettingsService settingsService) : base(navigationService, settingsService)
        {
           

        }
        public ICommand OKCommand => new Command(OKCommandExcute);

        private async void OKCommandExcute(object obj)
        {

            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
            await NavigationService.GoBackAsync();
           
        }
        public ICommand CancelCommand => new Command(CancelCommandExcute);

        private async void CancelCommandExcute(object obj)
        {


            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(true);
        

        }
    }
}
