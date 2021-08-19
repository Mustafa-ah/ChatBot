using Prism.Navigation;
using STC.Common.Enums;
using STC.Models;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class VerifyEmailMobileProfilePageViewModel : BaseViewModel
    {
        private bool FromForgetPassword;
        private int _countSeconds = 60;

        private readonly IAccountService _accountService;
        public VerifyEmailMobileProfilePageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _accountService = accountService;
            //Device.StartTimer(TimeSpan.FromSeconds(1), CuontDown);
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
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private bool _isNotUpdateProfile;
        public bool IsNotUpdateProfile
        {
            get { return _isNotUpdateProfile; }
            set { SetProperty(ref _isNotUpdateProfile, value); }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private bool _didntRecive;
        public bool DidntRecive
        {
            get { return _didntRecive; }
            set { SetProperty(ref _didntRecive, value); }
        }
        private int _verifyType;
        public int VerifyType
        {
            get { return _verifyType; }
            set { SetProperty(ref _verifyType, value); }
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

        private async void ReSendCode(object obj)
        {
            _countSeconds = 60;
            Device.StartTimer(TimeSpan.FromSeconds(1), CuontDown);
            DidntRecive = false;
            try
            {
               
                ShowLoading();
                Response<VerifyOTP> respons;
                if (!IsNotUpdateProfile)
                {
            
                    if (VerifyType == 1)
                    {
                        respons = await _accountService.ResendVerifyNewEmail(Setting.AuthAccessToken, Setting.UserId, Email);
                    }
                    else
                    {
                        respons = await _accountService.ResendVerifyNewPhone(Setting.AuthAccessToken, Setting.UserId, PhoneNumber);
                    }
                }
                else
                {
                
                    if (VerifyType == 1)
                    {
                        respons = await _accountService.ResendVerifyEmail(Setting.AuthAccessToken, Setting.UserId);
                    }
                    else
                    {
                        respons = await _accountService.ResendVerifyPhone(Setting.AuthAccessToken, Setting.UserId);
                    }
         
                }

                if (respons.StatusCode != 200)
                    ShowErrorToast(respons.Message);
                HideLoading();
                OTPEnabled = true;

            }
            catch (Exception ex)
            {
                ShowErrorToast(ex.Message);
            }

        }


        #endregion


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
            ShowLoading();
            // await System.Threading.Tasks.Task.Delay(1000);
            string digits = Digit1 + Digit2 + Digit3 + Digit4;
            /*if (Setting.AppLanguage == (int)Languages.Arabic)
            {
                char[] charArray = digits.ToCharArray();
                Array.Reverse(charArray);
                digits = new string(charArray);
            }*/
            try
            {
                
                        Response<UserToken> response = null;
                        if (VerifyType == 1)
                        {
                             response = await _accountService.VerifyEmail(Setting.UserId, digits);
                        }
                        else
                        {
                             response = await _accountService.VerifyPhone(Setting.UserId, digits);
                        }
                        if (response.StatusCode == 200)
                        {
                            if(IsNotUpdateProfile)
                            {
                        ShowSucessToast(Resources.AppResources.VerficationSuccessed);
                        var parameters = new NavigationParameters
                           {
                                {Constants.ParameterKey.ViewRoute ,  Routes.ViewsRoutes.LoginRoute },
                                     {"Message",Resources.AppResources.CongratulationsVerified }
                            };

                        var naved = await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.CongratulationsRoute}", parameters);
                        // for IOS
                        if (!naved.Success)
                        {
                            await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.CongratulationsRoute}", parameters);
                        }
                    }
                            else
                            {
                        Response<int> respons;
                        if (VerifyType == 1)
                        {
                            respons = await _accountService.EditEmail(Email,Setting.UserId,Setting.AuthAccessToken);
                        }
                        else
                        {
                            respons = await _accountService.EditMobileNumber(PhoneNumber,Setting.UserId ,Setting.AuthAccessToken);
                        }
                        //var respons = await _accountService.UpdateProfile(Name, Email, PhoneNumber, Setting.AuthAccessToken);

                                if (respons.StatusCode == 200)
                                  {
                            ShowSucessToast(Resources.AppResources.ProfileUpdated);


                           

                            }
                            else
                            {
                            ShowErrorToast( respons.Message);
                            }
                            await NavigationService.GoBackAsync();
                        }
                            
                        }
                        else
                        {

                    ShowErrorToast( Resources.AppResources.EnterValidOTP);
                        


                        }
                    
                






             
            }
            catch (Exception ex)
            {
                ShowErrorToast( ex.Message);
            }
            HideLoading();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if(parameters.ContainsKey("NotUpdateProfile"))
            {
                IsNotUpdateProfile = bool.Parse(parameters["NotUpdateProfile"].ToString());
            }
            if (parameters.ContainsKey("Email"))
            {
                VerifyType = 1;
                Email = parameters["Email"].ToString();
                PhoneNumber = parameters["PMobile"].ToString();
                Name = parameters["Name"].ToString();
              
            }
            else
            {

                VerifyType = 2;
                Email = parameters["PEmail"].ToString();
                PhoneNumber = parameters["Mobile"].ToString();
                Name = parameters["Name"].ToString();

            }

            ReSendCode(null);
        }

        private bool ValidOTP()
        {
          
            return !string.IsNullOrEmpty(Digit1) && !string.IsNullOrEmpty(Digit2) && !string.IsNullOrEmpty(Digit3) && !string.IsNullOrEmpty(Digit4);
        }

    }
}

