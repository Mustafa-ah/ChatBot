using Prism.Navigation;
using STC.Common.Enums;
using STC.Services.Account;
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
    public class VerifySmsPageViewModel : BaseViewModel
    {
        private int _countSeconds = 60;
        private int _countMinutes = 0;
        private System.Timers.Timer _timer;
        private readonly IAccountService _accountService;
        public VerifySmsPageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {

           // InitTimer();
            _accountService = accountService;
            ViewRoute = Routes.ViewsRoutes.VerifiedEmailandSms;
            ReInitTimer();
            OTPEnabled = true;
        }
        async void InitTimer()
        {
            DidntRecive = false;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1200;
            _timer.Elapsed += OnTimedEvent;
            _countSeconds = 60;
            _countMinutes = 0;
            _timer.Enabled = true;
        }
        async void ReInitTimer()
        {
            DidntRecive = false;
            _timer = new System.Timers.Timer();
            _timer.Interval = 1200;
            _timer.Elapsed += OnTimedEvent;
            _countSeconds = 60;
            _countMinutes = 0;
            _timer.Enabled = true;
            try
            {

                ShowLoading();

                var respons = await _accountService.ResendVerifyPhone(Setting.AuthAccessToken, Setting.UserId);
                if (respons.StatusCode != 200)
                    ShowErrorToast(respons.Message);
                HideLoading();
                OTPEnabled = true;
            }
            catch (Exception ex)
            {
                HideLoading();
                ShowErrorToast( ex.Message);
            }
        }
        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            _countSeconds--;


            if (_countSeconds == 0 && _countMinutes > 0) { _countSeconds = 60; _countMinutes--; }
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
                _timer.Stop();
            }
            Task.Delay(200);
            if (_countSeconds < 10)
            {
                TimerSec = "0" + _countSeconds.ToString();
            }
            else
            {
                TimerSec = _countSeconds.ToString();

            }
            TimerMin = "0" + _countMinutes.ToString();
           
        }

        void SendCode()
        {
            _countSeconds = 60;
            _countMinutes = 1;
            DidntRecive = false;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _countSeconds--;
               
               
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
        private bool _OTPEnabled;
        public bool OTPEnabled
        {
            get { return _OTPEnabled; }
            set { SetProperty(ref _OTPEnabled, value); }
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
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        #region Commands
        public ICommand OpenLoginPageCommand => new Command(OpenLoginRoute);

        public ICommand VerifyCommand => new Command(VerifyCode);
        public ICommand SendCodeAgainCommand => new Command(ReInitTimer);


        public ICommand OpenRegisterPageCommand => new Command(() => {
            NavigationService.GoBackAsync();
            NavigationService.GoBackAsync();

        });

        #endregion
        private bool FromForgetPassword;

       

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

        private async void VerifyCode(object obj)
        {
            if (!IsConncted())
            {
                return;
            }
            if (!ValidOTP())
            {
                return;
            }
            string digits = Digit1 + Digit2 + Digit3 + Digit4;
            if (Setting.AppLanguage == (int)Languages.Arabic)
            /*{
                char[] charArray = digits.ToCharArray();
                Array.Reverse(charArray);
                digits = new string(charArray);
            }*/

            //if (digits != "1234")
            //{
            //    await Application.Current.MainPage.DisplayAlert("invalid code", "Please enter valid code", "ok");
            //    HideLoading();
            //    return;
            //}

            //if (digits != "1234")
            //{
            //    await Application.Current.MainPage.DisplayAlert("invalid code", "Please enter valid code", "ok");
            //    Digit1 = "";
            //    Digit2 = "";
            //    Digit3 = "";
            //    Digit4 = "";
            //    HideLoading();
            //    return;
            //}

            ShowLoading();
            await Task.Delay(1000);
            try
            {
                var respons = await _accountService.VerifyPhone(Setting.UserId, digits);

                if (!string.IsNullOrEmpty(respons.Message))
                {
                    if (respons.StatusCode == 200)
                    {
                        var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, ViewRoute } };
                        await NavigationService.NavigateAsync(ViewRoute);
                    }
                    else
                    {
                        ShowErrorToast( Resources.AppResources.EnterValidOTP);


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

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SetRoute(parameters);
            if (parameters.ContainsKey("Phone"))
            {
                Phone = parameters["Phone"].ToString();
            }
        }
        private bool ValidOTP()
        {
            return !string.IsNullOrEmpty(Digit1) && !string.IsNullOrEmpty(Digit2) && !string.IsNullOrEmpty(Digit3) && !string.IsNullOrEmpty(Digit4);
        }

    }
}
