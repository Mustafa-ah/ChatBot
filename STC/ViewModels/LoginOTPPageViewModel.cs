using Prism.Navigation;
using STC.Common.Enums;
using STC.Models;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class LoginOTPPageViewModel : BaseViewModel
    {
        private bool FromForgetPassword;
        private int _countSeconds = 60;


        private readonly IAccountService _accountService;
        public LoginOTPPageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _accountService = accountService;
            Device.StartTimer(TimeSpan.FromSeconds(1), CuontDown);
            DidntRecive = false;
            OTPEnabled = true;
        }

        private bool CuontDown()
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                TimeLeft = string.Format("00:{0:00}", _countSeconds);
            });

            _countSeconds--;

            if (_countSeconds < 1)
            {
                DidntRecive = true;
                if(OTPEnabled)
                {
                    OTPEnabled = false;
                    Digit1 = "";
                    Digit2 = "";
                    Digit3 = "";
                    Digit4 = "";
                    try
                    {
                        MessagingCenter.Send(this, "UnfocusOTPNative");
                    }
                    catch(Exception ex)
                    {

                    }
     
                }

                return false;//, False = Stop the timer
            }
            return true; // True = Repeat again
        }


        private string _timeLeft;
        public string TimeLeft
        {
            get { return _timeLeft; }
            set { SetProperty(ref _timeLeft, value); }
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
        private int _verifyType;
        public int VerifyType
        {
            get { return _verifyType; }
            set { SetProperty(ref _verifyType, value); }
        }
        private bool _didntRecive;
        public bool DidntRecive
        {
            get { return _didntRecive; }
            set { SetProperty(ref _didntRecive, value); }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }
        private string _notMaskedEmail;
        public string NotMaskedEmail
        {
            get { return _notMaskedEmail; }
            set { SetProperty(ref _notMaskedEmail, value); }
        }
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }
        private string _notMaskedPhone;
        public string NotMaskedPhone
        {
            get { return _notMaskedPhone; }
            set { SetProperty(ref _notMaskedPhone, value); }
        }
        private bool _OTPEnabled;
        public bool OTPEnabled
        {
            get { return _OTPEnabled; }
            set { SetProperty(ref _OTPEnabled, value); }
        }
        #region Commands
        public ICommand VerifyCommand => new Command(VerifyCode);

        public ICommand OpenForgetPasswordPageCommand => new Command(() => NavigationService.GoBackAsync());

        public ICommand ReSendCodeCommand => new Command(ReSendCode);
        //public ICommand CustomGoBackCommand => new Command( () =>
        //  {
        //      if (VerifyType == (int)STC.Common.Enums.VerifyType.Email || VerifyType == (int)STC.Common.Enums.VerifyType.Phone)
        //      {

        //           NavigationService.GoBackAsync();
        //          NavigationService.GoBackAsync();
        //      }
        //      else
        //          NavigationService.GoBackAsync();
        //  });
        private void ReSendCode(object obj)
        {
            _countSeconds = 60;
            Device.StartTimer(TimeSpan.FromSeconds(1), CuontDown);
            DidntRecive = false;
            ExecuteResendCode();
           
        }


        #endregion
        private async void ExecuteResendCode()
        {

            if (!IsConncted())
            {
                return;
            }
            ShowLoading();
            try
            {
                Response<ForgetPassword> respons;
                if(VerifyType==0)
                {
                    Response<ResendOtpDto> response;
                    response=await _accountService.ResendOtp(Setting.UserId);
                    if (response.StatusCode != 200)
                        ShowErrorToast(response.Message);
                    HideLoading();
                    OTPEnabled = true;
                    return;
                }
                else if (VerifyType == 1)
                {

                     respons = await _accountService.ForgetPassword(NotMaskedEmail);
                }
                else
                {

                     respons = await _accountService.ForgetPassword(NotMaskedPhone);
                }
                if (respons.StatusCode != 200)
                    ShowErrorToast(respons.Message);
                HideLoading();
                OTPEnabled = true;

            }
            catch (Exception ex)
            {
                if (VerifyType == 1)
                    ShowErrorToast( ex.Message);
                else
                    ShowErrorToast(ex.Message);
                HideLoading();

            }




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

            string rootNav = Device.RuntimePlatform == Device.iOS ? "./" : "/";

            ShowLoading();
            // await System.Threading.Tasks.Task.Delay(1000);
            string digits = Digit1 + Digit2 + Digit3 + Digit4;
           /* if (Setting.AppLanguage == (int)Languages.Arabic)
            {
                char[] charArray = digits.ToCharArray();
                Array.Reverse(charArray);
                digits= new string(charArray);
            }*/
            try
            {
                if (_verifyType == (int)Common.Enums.VerifyType.Email)
                {
                    Response<UserToken> respons = null;
                    if (FromForgetPassword)
                        respons = await _accountService.VerifyEmail(Setting.UserId, digits, true);
                    else
                        respons = await _accountService.VerifyEmail(Setting.UserId, digits);


                    if (respons.StatusCode==200)
                    {

                      
                        if (FromForgetPassword == true)
                        {
                            await NavigationService.NavigateAsync(Routes.ViewsRoutes.SetNewPasswordRoute, new NavigationParameters
                            {
                                { "IsVerifyMobile", false },
                                {"Code",digits }
                            });
                        }
                        else
                        {
                            await NavigationService.NavigateAsync($"{rootNav}{Routes.ViewsRoutes.HomeRoute}");
                        }
                    }
                    else
                    {


                        ShowErrorToast(respons.Message);

                     

                    }
                }
                else if (_verifyType == (int)Common.Enums.VerifyType.Phone)
                {

                    Response<UserToken> respons = null;
                    if (FromForgetPassword)
                     respons = await _accountService.VerifyPhone(Setting.UserId, digits,true);
                    else
                        respons = await _accountService.VerifyPhone(Setting.UserId, digits);

                    if (respons.StatusCode == 200)
                    {


                        if (FromForgetPassword == true)
                        {
                            await NavigationService.NavigateAsync(Routes.ViewsRoutes.SetNewPasswordRoute, new NavigationParameters
                            {
                                { "IsVerifyMobile", true },
                                {"Code",digits }
                            });
                        }
                        else
                        {
                            await NavigationService.NavigateAsync($"{rootNav}{Routes.ViewsRoutes.HomeRoute}");
                        }
                    }
                    else
                    {

                        ShowErrorToast( Resources.AppResources.EnterValidOTP);
                    }
                }
                else
                {
                    var respons = await _accountService.VerifyOTP(Setting.UserId, digits);

                    if (!string.IsNullOrEmpty(respons.Data.AccessToken))
                    {
                        Setting.AuthAccessToken = respons.Data.AccessToken;

                        if (FromForgetPassword == true)
                        {
                            await NavigationService.NavigateAsync(Routes.ViewsRoutes.SetNewPasswordRoute);
                        }
                        else
                        {
                            await NavigationService.NavigateAsync($"{rootNav}{Routes.ViewsRoutes.HomeRoute}");
                        }
                    }
                    else
                    {

                        ShowErrorToast( respons.Message);

                

                    }
                }

                AddFirebaseDeviceToken();
                HideLoading();
            }
            catch (Exception ex)
            {
                ShowErrorToast( ex.Message);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("ForgetPassowrd"))
            {
                VerifyType = (int)parameters["ForgetPassowrd"];
                FromForgetPassword = true;
            }
            else
            {
                FromForgetPassword = false;
            }
            if (parameters.ContainsKey("Email"))
            {
                Email = parameters["Email"].ToString();
            }
            if (parameters.ContainsKey("Phone"))
            {
                Phone = parameters["Phone"].ToString();
            }
            if (parameters.ContainsKey("NotMaskedEmail"))
            {
                NotMaskedEmail = parameters["NotMaskedEmail"].ToString();
            }
            if (parameters.ContainsKey("NotMaskedPhone"))
            {
                NotMaskedPhone = parameters["NotMaskedPhone"].ToString();
            }


        }

        private bool ValidOTP()
        {
            return !string.IsNullOrEmpty(Digit1) && !string.IsNullOrEmpty(Digit2) && !string.IsNullOrEmpty(Digit3) && !string.IsNullOrEmpty(Digit4);
        }

        private Task AddFirebaseDeviceToken()
        {
            try
            {
                if (!IsConncted())
                {
                    return Task.FromResult(0);
                }
                string lang = Setting.AppLanguage == (int)Common.Enums.Languages.Arabic ? "ar" : "en";
                return _accountService.AddFirebaseDeviceToken(Setting.AuthAccessToken, Setting.PushNotificationDeviceToken, Setting.UserId, lang);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Task.FromResult(0);
            }

        }
    }
}

