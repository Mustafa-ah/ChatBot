using Prism.Navigation;
using STC.Models;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
namespace STC.ViewModels
{
    class CongratulationsRequestCreatedPageViewModel : BaseViewModel
    {

        public string RequestId { get; set; }
        public CongratulationsRequestCreatedPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {

        }
        public ICommand CloseCommand => new Command(Close);

        private async void Close(object obj)
        {
            try
            {
                ShowLoading();
                var parameters = new NavigationParameters {
                    { Constants.ParameterKey.RequestId, RequestId }
                };


                var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.RequestDetailsRoute}", parameters);
                // for IOS
                if (!naved.Success)
                {
                    await NavigationService.NavigateAsync($"/{ViewsRoutes.RequestDetailsRoute}", parameters);
                }
                HideLoading();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(Constants.ParameterKey.RequestId))
            {
                RequestId = parameters[Constants.ParameterKey.RequestId].ToString();
            }
        }
    }
}
