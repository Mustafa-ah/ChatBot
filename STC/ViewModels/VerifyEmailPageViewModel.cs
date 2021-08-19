using Prism.Navigation;
using STC.Common.Enums;
using STC.Services.Account;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class VerifyEmailPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

        private int _countSeconds = 60;
            private int _countMinutes = 0;
        private readonly int AppLang; 
    
        public VerifyEmailPageViewModel(
            INavigationService navigationService,
            IAccountService accountService,
            ISettingsService settingsService)
            : base(navigationService, settingsService)
        {
           
            _accountService = accountService;
            ViewRoute = Routes.ViewsRoutes.VerifiedEmailandSms;
            AppLang = Setting.AppLanguage;
            ReSendCode(null);
            OTPEnabled = true;
        }

        void SendCode()
        {
            _countSeconds = 60;
            _countMinutes = 0;
            DidntRecive = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;
                if (_countSeconds == 0 && _countMinutes > 0) { _countSeconds = 60; _countMinutes--; }
                if (_countSeconds < 10)
                {
                    TimerSec = "0" + _countSeconds.ToString();
                }
                else
                {
                    TimerSec = _countSeconds.ToString();

                }
                TimerMin = "0" + _countMinutes.ToString();
                if ((_countSeconds | _countMinutes) == 0)
                {
                    DidntRecive = true;
                    OTPEnabled = false;
                    Digit1 = "";
                    Digit2 = "";
                    Digit3 = "";
                    Digit4 = "";
                    try
                    {
                        MessagingCenter.Send(this, "UnfocusOTPNative");
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return Convert.ToBoolean(_countMinutes | _countSeconds);
            });
        }

        private Boolean _DidntRecive;
        public Boolean DidntRecive
        {
            get { return _DidntRecive; }
            set { SetProperty(ref _DidntRecive, value); }
        }
        private string _timerMin;
        public string TimerMin
        {
            get { return _timerMin; }
            set { SetProperty(ref _timerMin, value); }
        }

        private string _timerSec;
        public string TimerSec
        {
            get { return _timerSec; }
            set { SetProperty(ref _timerSec, value); }
        }

        private string _digit1;
        public string Digit1
        {
            get { return _digit1; }
            set { SetProperty(ref _digit1, value); }
        }
        private bool _OTPEnabled;
        public bool OTPEnabled
        {
            get { return _OTPEnabled; }
            set { SetProperty(ref _OTPEnabled, value); }
        }
        private string _digit2;
        public string Digit2
        {
            get { return _digit2; }
            set { SetProperty(ref _digit2, value); }
        }

        private string _digit3;
        public string Digit3
        {
            get { return _digit3; }
            set { SetProperty(ref _digit3, value); }
        }

        private string _digit4;
        public string Digit4
        {
            get { return _digit4; }
            set { SetProperty(ref _digit4, value); }
        }
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


        #region Commands

        public ICommand OpenRegisterPageCommand => new Command(() => NavigationService.GoBackAsync());
        public ICommand SendCodeAgainCommand => new Command(SendCode);

        public ICommand OpenLoginPageCommand => new Command(OpenLoginRoute);

        public ICommand VerifyCommand => new Command(VerifyCode);
        #endregion

        public ICommand ReSendCodeCommand => new Command(ReSendCode);

        private async void ReSendCode(object obj)
        {
            if (!IsConncted())
            {
                return;
            }
            _countSeconds = 60;
            SendCode();
            DidntRecive = false;
            try
            {

                ShowLoading();

                var respons = await _accountService.ResendVerifyEmail(Setting.AuthAccessToken, Setting.UserId);

                if (respons.StatusCode != 200)
                    ShowErrorToast(respons.Message);
                HideLoading();
                OTPEnabled = true;
            }
            catch (Exception ex)
            {
                ShowErrorToast( ex.Message);
            }
        }

        private async void VerifyCode(object obj)
        {
            if (!ValidOTP())
            {
                return;
            }
            ShowLoading();
            string digits = Digit1 + Digit2 + Digit3 + Digit4;
          /* if (Setting.AppLanguage == (int)Languages.Arabic)
            {
                char[] charArray = digits.ToCharArray();
                Array.Reverse(charArray);
                digits = new string(charArray);
            }*/
            //if(digits != "1234")
            //{

            //        await Application.Current.MainPage.DisplayAlert(Resources.AppResources.info, Resources.AppResources.EnterValidOTP, Resources.AppResources.OK);

            //    HideLoading();
            //    return;
            //}

            //if(digits != "1234")
            //{
            //    if (AppLang == 2)
            //        await Application.Current.MainPage.DisplayAlert("invalid code", "Please enter valid code", "ok");
            //    else
            //        await Application.Current.MainPage.DisplayAlert("معلومة", "برجاء ادخال كود صحيح", "موافقة");
            //    Digit1 = "";
            //    Digit2 = "";
            //    Digit3 = "";
            //    Digit4 = "";
            //    HideLoading();
            //    return;
            //}

            try
            {
                var respons = await _accountService.VerifyEmail(Setting.UserId, digits);

                if (!string.IsNullOrEmpty(respons.Message))
                {
                    if (respons.StatusCode == 200)
                    {
                        var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, ViewRoute } , { "Phone", Phone } };

                        await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifySMSRoute, parameters);
                    }
                    else
                    {
                        ShowErrorToast(Resources.AppResources.EnterValidOTP);


                    }

                }
                else
                {


                    ShowErrorToast( Resources.AppResources.EnterValidOTP);
                

                }
            }
            catch (Exception ex)
            {

            }
           

            HideLoading();
        }

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

        private bool ValidOTP()
        {
            return !string.IsNullOrEmpty(Digit1) && !string.IsNullOrEmpty(Digit2) && !string.IsNullOrEmpty(Digit3) && !string.IsNullOrEmpty(Digit4);
        }
  
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetRoute(parameters);
            if (parameters.ContainsKey("Email"))
            {
                Email = parameters["Email"].ToString();
            }
            if (parameters.ContainsKey("Phone"))
            {
                Phone = parameters["Phone"].ToString();
            }
        }

    }
}
