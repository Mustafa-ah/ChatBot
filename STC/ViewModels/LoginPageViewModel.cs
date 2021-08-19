using Prism.Navigation;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class LoginPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public LoginPageViewModel(
            INavigationService navigationService,
            IAccountService accountService,
            ISettingsService settingsService)
            : base(navigationService, settingsService)
        {
            AddValidations();

            _accountService = accountService;
            isEnabled = false;

            SelectedType = 1;
            Setting.VerifyType = 1;
        }

        #region Props


        private bool _canProceed;
        public bool CanProceed
        {
            get
            {
                return _canProceed;
            }
            set
            {
                _canProceed = value; RaisePropertyChanged();
            }
        }

        private bool _isEnabled;
        public bool isEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        ValidatableObject<string> _emailOrMobile;
        public ValidatableObject<string> EmailOrMobile
        {
            get
            {
                return _emailOrMobile;
            }
            set
            {
                _emailOrMobile = value; RaisePropertyChanged();
            }
        }
        ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value; RaisePropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand OpenVerifyEmailPageCommand => new Command(() => NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailRoute));
       
        public ICommand OpenRegisterPageCommand => new Command(ExecuteRegisterCommand);
        public ICommand LoginCommand => new Command(Login);
        public ICommand UserNameChangedCommand => new Command(UserNameChanged);

      

        public ICommand PasswordChangedCommand => new Command(PasswordChanged);
        private async void ExecuteRegisterCommand()
        {
            if (IsBusy)
                return;
            ShowLoading();
            var dd = NavigationService.GetNavigationUriPath();
          await  NavigationService.NavigateAsync(Routes.ViewsRoutes.RegisterRoute);
            HideLoading();
        }

        private void PasswordChanged(object obj)
        {
            CanProceed = !string.IsNullOrEmpty(Password.Value) && !string.IsNullOrEmpty(EmailOrMobile.Value);
        }
        private void UserNameChanged(object obj)
        {
            CanProceed = !string.IsNullOrEmpty(Password.Value) && !string.IsNullOrEmpty(EmailOrMobile.Value);
        }
        private async void Login(object obj)
        {
            if (!IsConncted())
            {
                return;
            }
            var valid = ValidateData();
            if (valid)
            {
                try
                {
                    ShowLoading();
                    var response = await _accountService.Login(_emailOrMobile.Value, _password.Value,  true);
                    if (!string.IsNullOrEmpty(response.StatusCode.ToString()) && response.StatusCode==200)
                    {
                         Setting.UserId = response.Data.Id;
                        Setting.GeneralInquiryId = response.Data.GeneralInquiryId;
                        var mobileNumber = response.Data.mobileNumber;
                        var parameters = new NavigationParameters
                        {
                            {"Phone" ,  mobileNumber }

                        };
                       
                        await NavigationService.NavigateAsync(Routes.ViewsRoutes.LoginOTPtRoute,parameters);

                    }
                    else
                    {




                        if (response.StatusCode == 205 || response.StatusCode == 208 || response.StatusCode == 207)
                        {

                            ShowErrorToast( response.Message);
                            string Email = "";
                            string Name = "";
                            string Mobile = "";
                            if (response != null)
                            {
                                Email = response.Data.email;
                                Mobile = response.Data.mobileNumber;
                                Setting.UserId = response.Data.Id;
                            }

                            if (response.StatusCode == 205)
                            {
                                var parameters = new NavigationParameters { { "Email", Email }, { "PMobile", Mobile }, { "Name", Name }, { "NotUpdateProfile", true } };

                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                            }
                            else if (response.StatusCode == 208)
                            {
                                var parameters = new NavigationParameters { { "PEmail", Email }, { "Mobile", Mobile }, { "Name", Name }, { "NotUpdateProfile", true } };

                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                            }
                            else if (response.StatusCode == 207)
                            {
                                var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, Routes.ViewsRoutes.VerifiedEmailandSms }, { "Email", Email }, { "Phone", Mobile } };
                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailRoute, parameters);
                            }
                        }
                        else
                        {
                       
                            ShowErrorToast(response.Message);
                        }
                           
                    }
                }
                catch (Exception ex)
                {

                    ShowErrorToast(Resources.AppResources.EmailorPasswordInCorrect);

                }
                HideLoading(); 

            }
            else
            {

                ShowErrorToast( Resources.AppResources.EmailorPasswordInCorrect);
            }

        }
        #endregion


        #region methods
        private void AddValidations()
        {
            _emailOrMobile = new ValidatableObject<string>();

            _emailOrMobile.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = Resources.AppResources.EamilRequiredMsg
            });

            _password = new ValidatableObject<string>();

            _password.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = Resources.AppResources.PasswordRequiredMsg
            });
        }

        private bool ValidateData()
        {
            var emailValid = _emailOrMobile.Validate();
            var passwordValid = _password.Validate();
            return emailValid && passwordValid;
        }
        #endregion

        #region Recover Account
        int _selectedType;

        public int SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        //  public ICommand ChangeLanguageCommand => new Command(ChangeLanguageCommandExcute);
        public ICommand ChooseEmailCommand => new Command(() => { SelectedType = Setting.VerifyType = 1; });
        public ICommand ChoosePhoneCommand => new Command(() => { SelectedType = Setting.VerifyType = 2; });

        #endregion
    }
}
