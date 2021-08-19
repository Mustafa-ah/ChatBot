using Prism.Navigation;
using STC.Common.Enums;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
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
    class ForgetPassowrdPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public ForgetPassowrdPageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            appLang = settingsService.AppLanguage;
            _accountService = accountService;

            AddValidations();

        }
       
        public int _verifyType;
        public int VerifyType
        {
            get { return _verifyType; }
            set { SetProperty(ref _verifyType, value); }
        }
        int appLang;

        string _selectedVerify;
        public string SelectedVerify
        {
            get { return _selectedVerify; }
            set { SetProperty(ref _selectedVerify, value); }
        }

        #region Commands
        public ICommand OpenVerifyEmailPageCommand => new Command(OpenVerifyEmailRoute);
        private async void OpenVerifyEmailRoute()
        {
            bool valid = _forgetPassword.Validate();
            if (valid)
            {
                if (!IsConncted())
                {
                    return;
                }

                if (IsBusy)
                    return;
                ShowLoading();
                try
                {
                    var respons = await _accountService.ForgetPassword(ForgetPassword.Value);
                    if(!string.IsNullOrEmpty(respons.Message)) {
                        if(!string.IsNullOrEmpty(respons.Data.id ))
                        {
                            Setting.UserId = respons.Data.id;
                          
                            var parameters = new NavigationParameters
                            {
                                { "ForgetPassowrd", VerifyType }
                            };
                            if(VerifyType == 1)
                            {
                                parameters.Add("NotMaskedEmail", ForgetPassword.Value);
                                parameters.Add("Email", respons.Data.email);
                            }
                            else if (VerifyType == 2)
                            {
                                parameters.Add("NotMaskedPhone", ForgetPassword.Value);
                                parameters.Add("Phone", respons.Data.mobileNumber);
                            }
                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.LoginOTPtRoute, parameters);
                            
                        }
                        else
                        {
                            if (VerifyType == 1)
                                ShowErrorToast( respons.Message);
                            else
                                ShowErrorToast(respons.Message);
                          
                        }
                    }
                }
                catch (Exception ex)
                {
                    //if (VerifyType == 1)
                    //    await Application.Current.MainPage.DisplayAlert("invalid data", "Please Enter a valid email", "ok");
                    //else
                    //    await Application.Current.MainPage.DisplayAlert("invalid data", "Please Enter a valid mobile", "ok");
                    ShowErrorToast(ex.Message);
          

                }

                HideLoading();

            }
        }

        public ICommand BackLoginCommand => new Command(async () => {

            if (IsBusy)
                return;
            ShowLoading();

            var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            // for IOS
            if (!naved.Success)
            {
                await NavigationService.NavigateAsync($"/{ViewsRoutes.LoginRoute}");
            }

            HideLoading();

        });

        #endregion
        #region Progs 
        ValidatableObject<string> _forgetPassword;
        public ValidatableObject<string> ForgetPassword
        {
            get
            {
                return _forgetPassword;
            }
            set
            {
                _forgetPassword = value; RaisePropertyChanged();
            }
        }
        #endregion
        private void AddValidations()
        {


            _forgetPassword = new ValidatableObject<string>();

           
                _forgetPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
                {

                    ValidationMessage = Resources.AppResources.EamilRequiredMsg
                });

            



        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            VerifyType = Setting.VerifyType;
            if (VerifyType == 1)
            {
                if (appLang == (int)Common.Enums.Languages.Arabic)
                {
                    SelectedVerify = "البريد الالكتروني";
                }
                else
                {
                    SelectedVerify = "E-mail address";
                }
                _forgetPassword.Validations[0].ValidationMessage = Resources.AppResources.EmailRequired;
                _forgetPassword.Validations.Add(new EmailRule<string>
                {

                    ValidationMessage = Resources.AppResources.InvalidEmail
                });
                _forgetPassword.Validations.Add(new EmailMaxLengthRule<string>
                {

                    ValidationMessage = Resources.AppResources.EmailMaxLength
                });

            }
            else
            {
                if (appLang == (int)Common.Enums.Languages.Arabic)
                {
                    SelectedVerify = "رقم الجوال";
                }
                else
                {
                    SelectedVerify = "Mobile Number";
                }
                _forgetPassword.Validations[0].ValidationMessage = Resources.AppResources.MobileRequired;
                _forgetPassword.Validations.Add(new MobileFormatRule<string>
                {

                    ValidationMessage = Resources.AppResources.InvalidMobile
                });
                _forgetPassword.Validations.Add(new MobileMaxLengthRule<string>
                {

                    ValidationMessage = Resources.AppResources.MobileMaxLength
                });

            }
        }

    }
}
