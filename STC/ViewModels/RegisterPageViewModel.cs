using Prism.Navigation;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
using STC.Models;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class RegisterPageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;

        public RegisterPageViewModel(INavigationService navigationService,
             IAccountService accountService,
            ISettingsService settingsService)
            : base(navigationService, settingsService)
        {
            AddValidations();
            _accountService = accountService;
            Setting.UserId = string.Empty;
        }


        //ValidatableObject<string> _userName;
        //public ValidatableObject<string> UserName
        //{
        //    get
        //    {
        //        return _userName;
        //    }
        //    set
        //    {
        //        _userName = value; RaisePropertyChanged();
        //    }
        //}

        #region methods
        private void AddValidations()
        {

            _fullName = new ValidatableObject<string>();

            _fullName.Validations.Add(new IsNotNullOrEmptyRule<string>
            {

                ValidationMessage = Resources.AppResources.FullNameReuquired
            });
            _fullName.Validations.Add(new NameMaxLengthRule<string>
            {

                ValidationMessage = Resources.AppResources.NameMaxLength
            });
            _fullName.Validations.Add(new NameSpecialCharsRule<string>
            {

                ValidationMessage = Resources.AppResources.invalidName
            });
            _email = new ValidatableObject<string>();

            _email.Validations.Add(new IsNotNullOrEmptyRule<string>
            {

                ValidationMessage = Resources.AppResources.EmailRequired
            });
            _email.Validations.Add(new EmailMaxLengthRule<string>
            {

                ValidationMessage = Resources.AppResources.EmailMaxLength
            });
            _email.Validations.Add(new EmailRule<string>
            {

                ValidationMessage = Resources.AppResources.InvalidEmail
            });
          


            _phone = new ValidatableObject<string>();

            _phone.Validations.Add(new IsNotNullOrEmptyRule<string> 
            {
                ValidationMessage = Resources.AppResources.MobileRequired 
            });
            _phone.Validations.Add(new MobileMaxLengthRule<string>
            { 
                ValidationMessage = Resources.AppResources.MobileMaxLength 
            });
            _phone.Validations.Add(new MobileFormatRule<string>
            {
                ValidationMessage = Resources.AppResources.InvalidMobile 
            });

            _password = new ValidatableObject<string>();

            _password.Validations.Add(new PasswordRule<string>
            {

                ValidationMessage = Resources.AppResources.PassowrdContains
            });
            _password.Validations.Add(new PasswordMaxLengthRule<string>
            {

                ValidationMessage = Resources.AppResources.PasswordMaxLength
            });

            _confirmPassword = new ValidatableObject<string>();
  

            _confirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                
                ValidationMessage = Resources.AppResources.PassowrdNotMatched
            });
        }

        private bool ValidateData()
        {
            var newPasswordValid = _password.Validate();
            if(_password.Value != _confirmPassword.Value)
            {
                _confirmPassword.Value = null; 
            }
            var ConfirmNewPasswordValid = _confirmPassword.Validate();
            var FullnameValid = _fullName.Validate();
            var EmailValid = _email.Validate();
            var PhoneValid = _phone.Validate();

            return newPasswordValid && ConfirmNewPasswordValid&& FullnameValid&&EmailValid && PhoneValid;
        }
        #endregion

        #region Progs 
        ValidatableObject<string> _fullName;
        public ValidatableObject<string> FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                _fullName = value; RaisePropertyChanged();
            }
        }
        ValidatableObject<string> _email;
        public ValidatableObject<string> Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value; RaisePropertyChanged();
            }
        }

        ValidatableObject<string> _phone;
        public ValidatableObject<string> Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = value; RaisePropertyChanged();
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
        ValidatableObject<string> _confirmPassword;
        public ValidatableObject<string> ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value; RaisePropertyChanged();
            }
        }


        private bool _keyboardFoucsed;
        public bool KeyboardFoucsed
        {
            get { return _keyboardFoucsed; }
            set { SetProperty(ref _keyboardFoucsed, value); }
        }


        #endregion
        #region Commands

        public ICommand RegisterCommand => new Command(Register);
        public ICommand EntryFocusedCommand => new Command(ExecuteEntryFocusedCommand);
        public ICommand EntryUnFocusedCommand => new Command(ExecuteEntryUnFocusedCommand);


        #endregion



        #region methods
        public void ExecuteEntryFocusedCommand()
        {

            KeyboardFoucsed = true;
        }
        public void ExecuteEntryUnFocusedCommand()
        {

            KeyboardFoucsed = false;
        }
        private async void Register()
        {
            if (!IsConncted())
            {
                return;
            }
            ShowLoading();
            var valid = ValidateData();
            UserDTO user = new UserDTO()
            {
                email = _email.Value,
                password = _password.Value,
                userConfirmPassword = _confirmPassword.Value,
                fullName = _fullName.Value,
                phone = _phone.Value,
               
            };
            if (valid)
            {
                if (string.IsNullOrEmpty(Setting.UserId) )
                {
                    try
                    {
                        var response = await _accountService.Register(user);


                        if (!string.IsNullOrEmpty(response.Data.Id))
                        {
                            Setting.UserId = response.Data.Id;
                            var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, Routes.ViewsRoutes.VerifiedEmailandSms }, { "Email",response.Data.email}, { "Phone", response.Data.phone } };
                            await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailRoute, parameters);
                        }
                        else
                        {
                            ShowErrorToast( response.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowErrorToast( ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        user.Id = Setting.UserId;
                        var response = await _accountService.EditRegister(user);
                        if (response.StatusCode == 205 || response.StatusCode == 208 || response.StatusCode == 207)
                        {
                            //await Application.Current.MainPage.DisplayAlert(Resources.AppResources.info, response.Message, Resources.AppResources.OK);

                            if (response.StatusCode == 205)
                            {

                                var parameters = new NavigationParameters { { "Email",response.Data.email  }, { "PMobile", response.Data.phone}, { "Name", FullName.Value }, { "NotUpdateProfile", true } };

                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                            }
                            else if (response.StatusCode == 208)
                            {
                                var parameters = new NavigationParameters { { "PEmail", response.Data.email }, { "Mobile", response.Data.phone }, { "Name", FullName.Value }, { "NotUpdateProfile", true } };

                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                            }
                            else if (response.StatusCode == 207)
                            {
                                var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, Routes.ViewsRoutes.VerifiedEmailandSms }, { "Email", response.Data.email }, { "Phone", response.Data.phone } };
                                await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailRoute, parameters);
                            }
                            //if (response.StatusCode == 200)
                            //{
                            //    Setting.UserId = response.Data.Id;
                            //    var parameters = new NavigationParameters { { Constants.ParameterKey.ViewRoute, Routes.ViewsRoutes.VerifiedEmailandSms }, { "Email", Email.Value }, { "Phone", Phone.Value } };
                            //    await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailRoute, parameters);
                            //}
                        }
                        else
                        {
                            if (response.StatusCode == 200)
                                ShowSucessToast(response.Message);
                            else
                                ShowErrorToast(response.Message);
                        }

                    }
                    catch (Exception ex)
                    {
                        ShowErrorToast( ex.Message);
                    }
                }
            }
           
            HideLoading();
        }
        #endregion
    }
}
