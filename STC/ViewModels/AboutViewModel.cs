using System;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Prism.Navigation;
using STC.Resources;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "About";
            OpenWebCommand = new Command(Open);
        }

        private void Open(object obj)
        {
            CultureInfo cultureInfo = new CultureInfo("ar");
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
           // Thread.CurrentThread.CurrentUICulture = cultureInfo;
            AppResources.Culture = cultureInfo;
            IsBusy = true;
            //NavigationService.NavigateAsync(ViewModels.Routes.ViewsRoutes.LogineRoute);
        }

        public ICommand OpenWebCommand { get; }
    }
}