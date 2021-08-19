using System;
using Prism.Navigation;
using STC.Settings;

namespace STC.ViewModels
{
    public class FilePageViewModel : BaseViewModel
    {
        public string FileURL { get; set; }
        public FilePageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            FileURL = "https://drive.google.com/file/d/0BziB7kxHIi8yc3RhcnRlcl9maWxl/view?usp=sharing";
            IsBusy = true;
        }
    }
}
