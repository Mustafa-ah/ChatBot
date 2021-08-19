using Prism.Navigation;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class MapPageViewModel : BaseViewModel
    {
        public Location UserLocation { get; set; }
        public MapPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            UserLocation = new Location(24.68773, 46.72185);
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(Constants.ParameterKey.Location))
            {
                UserLocation = parameters[Constants.ParameterKey.Location] as Location;

                MessagingCenter.Send(this, "OnNavigatedToMap");
            }
        }
    }
}
