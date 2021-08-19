using Prism.Navigation;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
using STC.Services.Account;
using STC.Settings;
using STC.ViewModels.Routes;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class SetNewPasswordPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public SetNewPasswordPageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            AddValidations();
            _accountService = accountService;

        }
        public ICommand OpenCongratulationsPageCommand => new Command(OpenCongratulationsRoute);
        public ICommand EntryFocusedCommand => new Command(ExecuteEntryFocusedCommand);
        public ICommand EntryUnFocusedCommand => new Command(ExecuteEntryUnFocusedCommand);
        public ICommand OpenLoginPageCommand => new Command(OpenLoginRoute);
        private bool IsVerifyMobile;
        private string digits;
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

        public void ExecuteEntryFocusedCommand()
        {

            KeyboardFoucsed = true;
        }
        public void ExecuteEntryUnFocusedCommand()
        {

            KeyboardFoucsed = false;
        }

        private async void OpenCongratulationsRoute()
        {
            if (!IsConncted())
            {
                return;
            }
            var valid = ValidateData();
            if (valid)
            {
                ShowLoading();
                await Task.Delay(500);
                try
                {
             

                    var respons = await _accountService.SetNewPassword(Setting.UserId, NewPassword.Value,ConfirmNewPassword.Value, IsVerifyMobile,digits);

                    if (respons.StatusCode==200)
                    {

                        var parameters = new NavigationParameters
                           {
                                {Constants.ParameterKey.ViewRoute ,  Routes.ViewsRoutes.LoginRoute },
                                     {"Message",Resources.AppResources.CongratulationsPasswordChanged }
                            };
                        var naved = await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.CongratulationsRoute}",parameters);
                        // for IOS
                        if (!naved.Success)
                        {
                            await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.CongratulationsRoute}", parameters);
                        }

                    }
                    else
                    {
                        ShowErrorToast(respons.Message);


                    }
                }
                catch (Exception ex)
                {
                    ShowErrorToast( ex.Message);
                }

                HideLoading();
            }
        }
        #region Progs 

        private bool _keyboardFoucsed;
        public bool KeyboardFoucsed
        {
            get { return _keyboardFoucsed; }
            set { SetProperty(ref _keyboardFoucsed, value); }
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
        #endregion
        private void AddValidations()
        {
            _newPassword = new ValidatableObject<string>();
            _newPassword.Validations.Add(new WhiteSpacesRule<string>
            {

                ValidationMessage = Resources.AppResources.PassowrdContains
            });

            
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

            _confirmNewPassword.Validations.Add(new WhiteSpacesRule<string>
            {

                ValidationMessage = Resources.AppResources.PassowrdContains
            });
            _confirmNewPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {



                 ValidationMessage = Resources.AppResources.PasswordRequiredMsg
            });

        }
        private bool ValidateData()
        {
            if(_newPassword.Value != _confirmNewPassword.Value )
            {
                _confirmNewPassword.Value = null;
            }
            var newPasswordValid = _newPassword.Validate();
            var ConfirmNewPasswordValid = _confirmNewPassword.Validate();
            return newPasswordValid && ConfirmNewPasswordValid;
        }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("IsVerifyMobile"))
            {
                IsVerifyMobile = (bool)parameters["IsVerifyMobile"];
      
            }
            if (parameters.ContainsKey("Code"))
            {
                digits = (string)parameters["Code"];

            }



        }
    }
}
