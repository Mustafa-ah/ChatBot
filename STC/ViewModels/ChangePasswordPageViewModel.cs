using Prism.Navigation;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
using STC.Services.Account;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class ChangePasswordPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public ChangePasswordPageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            AddValidations();
            _accountService = accountService;
            KeyboardFoucsed = false;
        }
        
        #region Commands
        public ICommand OpenCongratulationsPageCommand => new Command(async () => 
        {
            var vaild = ValidateData();
            if (vaild)
            {
                try
                {
                    if (!IsConncted())
                    {
                        return;
                    }
                    ShowLoading();
                    var response = await _accountService.ChangePassword(Setting.UserId, _oldPassword.Value, _newPassword.Value, _confirmNewPassword.Value, Setting.AuthAccessToken);
                    if (response.StatusCode == 200)
                    {
                        var parameters = new NavigationParameters
                        {
                            {Constants.ParameterKey.ViewRoute ,  Routes.ViewsRoutes.HomeRoute },
                            {"Message",Resources.AppResources.CongratulationsPasswordChanged }
                        };
                        var naved = await NavigationService.NavigateAsync($"/{ViewsRoutes.CongratulationsRoute}", parameters);
                        // for IOS
                        if (!naved.Success)
                        {
                            await NavigationService.NavigateAsync($"/{ViewsRoutes.CongratulationsRoute}", parameters);
                        }
                        
                    }
                    else
                    {
                       ShowErrorToast(response.Message);
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorToast(ex.Message);
                }
                HideLoading();
              
            }
        });
        public ICommand EntryFocusedCommand => new Command(ExecuteEntryFocusedCommand);
        public ICommand EntryUnFocusedCommand => new Command(ExecuteEntryUnFocusedCommand);


        #endregion

        #region Progs 
        ValidatableObject<string> _oldPassword;
        public ValidatableObject<string> OldPassword
        {
            get
            {
                return _oldPassword;
            }
            set
            {
                _oldPassword = value; RaisePropertyChanged();
            }
        }
        ValidatableObject<string> _newPassword;
        public ValidatableObject<string> NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                _newPassword = value; RaisePropertyChanged();
            }
        }
        ValidatableObject<string> _confirmNewPassword;
        public ValidatableObject<string> ConfirmNewPassword
        {
            get
            {
                return _confirmNewPassword;
            }
            set
            {
                _confirmNewPassword = value; RaisePropertyChanged();
            }
        }

        private bool _keyboardFoucsed;
        public bool KeyboardFoucsed
        {
            get { return _keyboardFoucsed; }
            set { SetProperty(ref _keyboardFoucsed, value); }
        }

        #endregion
        public void ExecuteEntryFocusedCommand()
        {
            
            KeyboardFoucsed = true;
        }
        public void ExecuteEntryUnFocusedCommand()
        {

            KeyboardFoucsed = false;
        }
        private void AddValidations()
        {
            _oldPassword = new ValidatableObject<string>();

            _oldPassword.Validations.Add(new PasswordRule<string>
            {

                ValidationMessage = Resources.AppResources.InvalidPassword
            });

           

            _newPassword = new ValidatableObject<string>();
            _newPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {

                ValidationMessage = Resources.AppResources.PasswordRequiredMsg
            });
            _newPassword.Validations.Add(new PasswordMaxLengthRule<string>
            {

                ValidationMessage = Resources.AppResources.PasswordMaxLength
            });
            _newPassword.Validations.Add(new PasswordRule<string>
            {

                ValidationMessage = Resources.AppResources.PassowrdContains
            });

            _confirmNewPassword = new ValidatableObject<string>();

            _confirmNewPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {

                ValidationMessage = Resources.AppResources.PassowrdNotMatched
            });

        }
        private bool ValidateData()
        {
            var OldPasswordValid = _oldPassword.Validate();
            var newPasswordValid = _newPassword.Validate();
            if(ConfirmNewPassword.Value != NewPassword.Value)
            {
                _confirmNewPassword.Value = null; 
            }
            var ConfirmNewPasswordValid = _confirmNewPassword.Validate();

            return newPasswordValid && ConfirmNewPasswordValid && OldPasswordValid;
        }


    }
}
