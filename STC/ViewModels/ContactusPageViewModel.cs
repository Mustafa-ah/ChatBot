using Prism.Navigation;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace STC.ViewModels
{
    class ContactusPageViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        private string _hotline;
        public string Hotline
        {
            get { return _hotline; }
            set { SetProperty(ref _hotline, value); }
        }
        public ContactusPageViewModel(INavigationService navigationService, ISettingsService settingsService) : base(navigationService, settingsService)
        {

        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetRoute(parameters);
            if (parameters.ContainsKey("Email"))
            {
                Email = parameters["Email"].ToString();
            }
            if (parameters.ContainsKey("Mobile"))
            {
                Phone = parameters["Mobile"].ToString();
            }
            if (parameters.ContainsKey("Hotline"))
            {
                Hotline = parameters["Hotline"].ToString();
            }
        }
    }
}
