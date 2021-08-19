using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Navigation;
using STC.Common.Validations;
using STC.Resources;
using STC.Services.Account;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace STC.ViewModels
{
    class ProfilePageViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        public ProfilePageViewModel(INavigationService navigationService, IAccountService accountService, ISettingsService settingsService) : base(navigationService, settingsService)
        {
            _accountService = accountService;
            ImgName= ImageSource.FromFile("pp_ic");
            //GetUserDetails();
            IsArabicName = false;
            IsEnglishName = true;



        }
        public ICommand ChangePasswordCommand => new Command(async() => {
            if (IsBusy)
                return;
            ShowLoading();
            var parameters = new NavigationParameters
            {
                { "FromProfile", 1 }
            };
            await NavigationService.NavigateAsync(Routes.ViewsRoutes.ChangePasswordtRoute,parameters);
            HideLoading();
        });
        private string PrevEmail;
        private string PrevMobile;
        private string PrevName;
        #region Props
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        private MediaFile _mediaFile;
        public MediaFile MediaFile
        {
            get { return _mediaFile; }
            set { SetProperty(ref _mediaFile, value); }
        }
        private string _mobile;
        public string Mobile
        {
            get { return _mobile; }
            set { SetProperty(ref _mobile, value); }
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }
        private ImageSource _imgname;
        public ImageSource ImgName
        {
            get { return _imgname; }
            set { SetProperty(ref _imgname, value); }
        }
        private bool _nameEnable;
        public bool NameEnable
        {
            get { return _nameEnable; }
            set { SetProperty(ref _nameEnable, value); }
        }
        private bool _emailEnable;
        public bool EmailEnable
        {
            get { return _emailEnable; }
            set { SetProperty(ref _emailEnable, value); }
        }
        private bool _mobileEnable;
        public bool MobileEnable
        {
            get { return _mobileEnable; }
            set { SetProperty(ref _mobileEnable, value); }
        }
        private bool _nameEditStackVisability;
        public bool NameEditStackVisability
        {
            get { return _nameEditStackVisability; }
            set { SetProperty(ref _nameEditStackVisability, value); }
        }
        private bool _emailEditStackVisability;
        public bool EmailEditStackVisability
        {
            get { return _emailEditStackVisability; }
            set { SetProperty(ref _emailEditStackVisability, value); }
        }
        private bool _mobileEditStackVisability;
        public bool MobileEditStackVisability
        {
            get { return _mobileEditStackVisability; }
            set { SetProperty(ref _mobileEditStackVisability, value); }
        }
        private bool _isArabicName;
        public bool IsArabicName
        {
            get { return _isArabicName; }
            set { SetProperty(ref _isArabicName, value); }
        }
        private bool _isEnglishName;
        public bool IsEnglishName
        {
            get { return _isEnglishName; }
            set { SetProperty(ref _isEnglishName, value); }
        }



        public void ExecuteEditName()
        {
            NameEnable = true;
            NameEditStackVisability = true;
        }
        public void ExecuteVerifyEditName()
        {
            NameEnable = false;
            NameEditStackVisability = false;

            ExecuteUpdateName();
        }
        public void ExecuteCancelEditNameCommand()
        {
            NameEnable = false;
            NameEditStackVisability = false;
            Name = PrevName;

        }

        public void ExecuteEditEmail()
        {
            EmailEnable = true;
            EmailEditStackVisability = true;
        }
        public void ExecuteVerifyEditEmail()
        {
            EmailEnable = false;
            EmailEditStackVisability = false;
            ExecuteVerifyEmailCommand();

        }
        public void ExecuteCancelEditEmailCommand()
        {
            EmailEnable = false;
            EmailEditStackVisability = false;
            Email = PrevEmail;

        }

        public void ExecuteEditMobile()
        {
            MobileEnable = true;
            MobileEditStackVisability = true;
        }
        public void ExecuteVerifyEditMobile()
        {
            MobileEnable = false;
            MobileEditStackVisability = false;
            ExecuteVerifyMobileCommand();

        }
        public void ExecuteCancelEditMobileCommand()
        {
            MobileEnable = false;
            MobileEditStackVisability = false;
            Mobile = PrevMobile;

        }
        private bool _keyboardFoucsed;
        public bool KeyboardFoucsed

        {
            get { return _keyboardFoucsed; }
            set { SetProperty(ref _keyboardFoucsed, value); }
        }
        #endregion
        public ICommand EditNameCommand => new Command(ExecuteEditName);
        public ICommand VerifyEditNameCommand => new Command(ExecuteVerifyEditName);
        public ICommand CancelEditNameCommand => new Command(ExecuteCancelEditNameCommand);

        public ICommand EditEmailCommand => new Command(ExecuteEditEmail);
        public ICommand VerifyEditEmailCommand => new Command(ExecuteVerifyEditEmail);
        public ICommand CancelEditEmailCommand => new Command(ExecuteCancelEditEmailCommand);

        public ICommand EditMobileCommand => new Command(ExecuteEditMobile);
        public ICommand VerifyEditMobileCommand => new Command(ExecuteVerifyEditMobile);
        public ICommand CancelEditMobileCommand => new Command(ExecuteCancelEditMobileCommand);

        public ICommand OpenCameraCommand => new Command(ExecuteOpenCamera);
        public ICommand OpenPickerCommand => new Command(ExecuteOpenPicker);

        public ICommand SaveCommand => new Command(ExecuteSaveCommand);
        public ICommand ProfileBackCommand => new Command(ExecuteProfileBackCommand);
        public ICommand EntryFocusedCommand => new Command(ExecuteEntryFocusedCommand);
        public ICommand EntryUnFocusedCommand => new Command(ExecuteEntryUnFocusedCommand);

      


        public void ExecuteEntryFocusedCommand(object obj)
        {

            KeyboardFoucsed = true;
        }
        public void ExecuteEntryUnFocusedCommand(object obj)
        {

            KeyboardFoucsed = false;
        }

        public async void ExecuteProfileBackCommand()
        {
            if(PrevEmail !=Email || PrevMobile != Mobile || PrevName != Name)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.DidntSaveDialog());
            }
            else 
            await  NavigationService.GoBackAsync();

        }

        public async void ExecuteSaveCommand ()
        {
            if (!IsConncted())
            {
                return;
            }
            PrevEmail = Email;
            PrevMobile = Mobile;
            PrevName = Name;
            try
            {
    
                ShowLoading();
                await System.Threading.Tasks.Task.Delay(1000);
                var respons = await _accountService.UpdateProfilePicture(Setting.UserId, this.MediaFile.GetStream(), Path.GetFileName( this.MediaFile.Path), Setting.AuthAccessToken);
                if (respons.StatusCode != 200)
                    ShowErrorToast( respons.Message);
                else
                    ShowSucessToast(Resources.AppResources.ProfilePictureUpdated);

            }
            catch (Exception ex)
            {
                ShowErrorToast( ex.Message);
            }
            HideLoading();
        }
        public async void ExecuteUpdateName()
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }

                string check = CheckName(Name);
                if (!string.IsNullOrEmpty(check))

                {
                     Name= PrevName;
                    ShowErrorToast( check); return;
                }
                if(Name==PrevName)
                {
                    ShowErrorToast( Resources.AppResources.SameName); return;
                }
                ShowLoading();
                await System.Threading.Tasks.Task.Delay(1000);

                var respons = await _accountService.EditFullName(Name, Setting.UserId,Setting.AuthAccessToken);
                if (respons.StatusCode != 200)
                    ShowErrorToast( respons.Message);
                else
                    ShowSucessToast(respons.Message);
                if (respons.StatusCode != 200)
                    Name = PrevName;
                else
                {
                    PrevName = Name;
                    Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
                    if (regex.IsMatch(Name))
                    {
                        IsArabicName = true;
                        IsEnglishName = false;
                    }
                    else
                    {
                        IsArabicName = false;
                        IsEnglishName = true;
                    }
                }
                  


            }
            catch (Exception ex)
            {
                ShowErrorToast(ex.Message);
            }

            HideLoading();
        }
        public async void ExecuteVerifyEmailCommand()
        {
            if (!IsConncted())
            {
                return;
            }
            try
            {

                if(PrevEmail==Email)
                {
                    ShowErrorToast( Resources.AppResources.SameData); return;
                }
                string check = CheckEmail(Email);
                if (!string.IsNullOrEmpty(check))

                {
                    Email = PrevEmail;
                    ShowErrorToast( check);return;
                }
                ShowLoading();
                await System.Threading.Tasks.Task.Delay(1000);

                var respons = await _accountService.ResendVerifyNewEmail( Setting.AuthAccessToken,Setting.UserId,Email);
                if (respons.StatusCode == 200)
                {
                    var parameters = new NavigationParameters { { "Email", Email }, { "PMobile", PrevMobile }, { "Name", PrevName } };

                    await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                }
                else
                {
                    Email = PrevEmail;
                    ShowErrorToast( respons.Message);
                }


            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(Resources.AppResources.Error, ex.Message, Resources.AppResources.OK);
            }

            HideLoading();


        }
        public async void ExecuteVerifyMobileCommand()
        {
            if (!IsConncted())
            {
                return;
            }
            try
            {
                if (PrevMobile == Mobile)
                {
                    ShowErrorToast( Resources.AppResources.SameData); return;
                }
                var check =  CheckMobile(Mobile);
                if(!string.IsNullOrEmpty(check))
                {
                    Mobile = PrevMobile;
                    ShowErrorToast(check); return; 
                }
                ShowLoading();
           

                var respons = await _accountService.ResendVerifyNewPhone(Setting.AuthAccessToken, Setting.UserId, Mobile);
                
                if (respons.StatusCode==200)
                {
                    var parameters = new NavigationParameters { { "PEmail", PrevEmail }, { "Mobile", Mobile }, { "Name", PrevName } };

                    await NavigationService.NavigateAsync(Routes.ViewsRoutes.VerifyEmailMobileProfilePage, parameters);
                }
                else 
                {
                    Mobile = PrevMobile;
                    ShowErrorToast(respons.Message);
                }
            

            }
            catch (Exception ex)
            {
                ShowErrorToast( ex.Message);
            }

            HideLoading();


        }
        public async void ExecuteOpenCamera()
        {
            if (!IsConncted())
            {
                return;
            }
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.AppResources.info, "No Camera Detected.", Resources.AppResources.OK);
                    return;
                }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    // Variable para guardar la foto en el album público
                    SaveToAlbum = true
                });
                this.MediaFile = file;

                if (file == null)
                    return;

                this.ImgName = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });



            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert(Resources.AppResources.Error, Resources.AppResources.CameraAccessDenied, Resources.AppResources.OK);
            }
        }
        public async void ExecuteOpenPicker()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert(Resources.AppResources.info, "No Camera Detected.", Resources.AppResources.OK);
                    return;
                }
                try
                {
                    var file = await CrossMedia.Current.PickPhotoAsync();
                    this.MediaFile = file;

                    if (file == null)
                        return;

                    this.ImgName = ImageSource.FromStream(() =>
                    {
                        var stream = file.GetStream();
                        return stream;
                    });

                }
                catch (Exception ex)
                {

                }
       


            }
            catch (Exception ex)
            {

                await Application.Current.MainPage.DisplayAlert(Resources.AppResources.Error, Resources.AppResources.CameraAccessDenied, Resources.AppResources.OK);
            }
        }
        public async void GetUserDetails()
        {
            if (!IsConncted())
            {
                return;
            }
            try
            {
                ShowLoading();
                await System.Threading.Tasks.Task.Delay(1000);
                var respons = await _accountService.GetAccountDetails(Setting.AuthAccessToken);
                Email = respons.Data.email;
                Name = respons.Data.fullName;
                Mobile = respons.Data.phone;
                PrevEmail = Email;
                PrevMobile = Mobile;
                PrevName = Name;
                Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
                if (regex.IsMatch(Name))
                {
                    IsArabicName = true;
                    IsEnglishName = false;
                }
                else
                {
                    IsArabicName = false;
                    IsEnglishName = true;
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
           

        }

        #region Validations 
        public string CheckMobile(string value)
        {
            if (value == null)
            {

                return Resources.AppResources.MobileRequired;
            }

            var str = value as string;
                  string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,*";
            foreach (var item in specialChar)
            {
                if (str.Contains(item)) return Resources.AppResources.InvalidMobile; 
            }
            if (str.Length != 10)
            {

                return Resources.AppResources.MobileMaxLength ;
            }
            else if(str.Contains(" "))
            {
                return Resources.AppResources.InvalidMobile;
            }
            else
            {

                if (str[0] == '0' && str[1] == '5') return null;
                return Resources.AppResources.InvalidMobile; ;
            }

        }
        public string CheckEmail(string value)
        {
            if (value == null)
            {
                return Resources.AppResources.EmailRequired;
            }
            var str = value as string;
            if (str.Length > 65) 
            {
              return  Resources.AppResources.EmailMaxLength;
            }
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(str);
            if(match.Success) return null;
            return Resources.AppResources.InvalidEmail;
        }
        public string CheckName(string value)
        {
            
            if (value == null)
            {
                return Resources.AppResources.FullNameReuquired; 
            }
            while (value.Length > 0)
            {
                if (value[0] == ' ')
                {
                    value = value.Remove(0, 1);
                    if (value == null || value.Length == 0 || string.IsNullOrEmpty(value)) return Resources.AppResources.invalidName;
                }
             
                else break;
            }
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            value = regex.Replace(value, " ");
            if (string.IsNullOrEmpty(value))
            {
                return Resources.AppResources.invalidName;
            }
            var str = value as string;
            if (str.Length > 32)
            {
                return Resources.AppResources.NameMaxLength;
            }
           
            string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
            foreach (var item in specialChar)
            {
                if (str.Contains(item)) return Resources.AppResources.invalidName; 
            }
            Name = value;
            return null;
        }

        #endregion
    }

}

