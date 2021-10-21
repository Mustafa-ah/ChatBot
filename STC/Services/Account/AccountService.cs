using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;

namespace STC.Services.Account
{
    public class AccountService : BaseService,IAccountService
    {

        public AccountService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }
        public async Task<LoginDTO> Login(string emailOrPhone, string passwordMobile, bool ismobile)
        {
            string url = $"{BaseUrl}/users/login";

            return await _requestProvider.PostAsync<LoginDTO>(url, new { info = emailOrPhone, password = passwordMobile }, null);
        }

        public async Task<Response<UserDTO>> Register(UserDTO user)
        {
            string url = $"{BaseUrl}Account/Register";

            return await _requestProvider.PostDataAsync<UserDTO>(url, user,null);

        }

        public async Task<Response<UserToken>> VerifyOTP(string userId, string otp)
        {
            string url = $"{BaseUrl}Account/VerifyOTP";

            return await _requestProvider.PostDataAsync<UserToken>(url, new { Id = userId, Code = otp }, null);

        }
        

        public async Task<Response<bool>> ChangePassword(string UserId, string currPassword, string newPass, string confirmNewPass,  string token)
        {
            string url = $"{BaseUrl}Account/ChangePassword";
            return await _requestProvider.PostAsync<Response<bool>>(url, new { userId = UserId, newPassword = newPass, confirmNewPassword = confirmNewPass, cuurentPassword = currPassword }, token);
        }
        public async Task<Response<int>> UpdateProfile(string fullName, string email, string mobile , string token)
        {
            string url = $"{BaseUrl}Account/UpdateProfile";
            return await _requestProvider.PostDataAsync<int>(url, new { FullName = fullName, Email = email, Mobile = mobile}, token);
        }
        public async Task<Response<int>> EditEmail( string email, string userId, string token)
        {
            string url = $"{BaseUrl}Account/EditEmail";
            return await _requestProvider.PostDataAsync<int>(url, new { email = email, userId = userId }, token);
        }
        public async Task<Response<int>> EditMobileNumber( string mobile, string userId, string token)
        {
            string url = $"{BaseUrl}Account/EditMobileNumber";
            return await _requestProvider.PostDataAsync<int>(url, new { userId = userId, mobileNumber = mobile }, token);
        }
        public async Task<Response<int>> EditFullName(string fullName, string userId, string token)
        {
            string url = $"{BaseUrl}Account/EditFullName";
            return await _requestProvider.PostDataAsync<int>(url, new { fullName = fullName, userId = userId }, token);
        }
        public async Task<Response<object>> UpdateProfilePicture(string userId, Stream content, string fileName,string token)
        {
            string url = $"{BaseUrl}Account/UpdateProfilePicture";
             List< KeyValuePair<string, string> > headerList = new List<KeyValuePair<string, string>>() ;
            headerList.Add(new KeyValuePair<string, string>("userId", userId));
            return await _requestProvider.PostFormDataAsync<object>(url, headerList, content, fileName, token);
        }
        public async Task<Response<Profile>> GetAccountDetails(string token)
        {
            string url = $"{BaseUrl}Account/Get";
            return await _requestProvider.PostDataAsync<Profile>(url,new { }, token);
        }

        public async Task<Response<ForgetPassword>> ForgetPassword(string emailOrPhone , bool ismobile = true)
        {
            string url = $"{BaseUrl}Account/ForgetPassword";
            return await _requestProvider.PostDataAsync<ForgetPassword>(url,new { emailAddressOrPhoneNumber = emailOrPhone , isMobile = ismobile },null);
        }

        public async Task<Response<UserToken>> VerifyEmail(string userId, string otp,bool isNotNull=false)
        {
            string url = $"{BaseUrl}Account/VerifyEmail";

            return await _requestProvider.PostDataAsync<UserToken>(url, new { Id = userId, Code = otp, IsNotNull = isNotNull }, null);
        }

        public async Task<Response<UserToken>> VerifyPhone(string userId, string otp,bool isNotNull=false)
        {
            string url = $"{BaseUrl}Account/VerifyPhone";

            return await _requestProvider.PostDataAsync<UserToken>(url, new { Id = userId, Code = otp, IsNotNull= isNotNull }, null);
        }

        public async Task<Response<SetNewPassword>> SetNewPassword(string UserId, string Password, string ConfirmNewPassword,bool IsVerifyMobile,string digits)
        {
            string url = $"{BaseUrl}Account/SetNewPassword";

            return await _requestProvider.PostDataAsync<SetNewPassword>(url, new { Id = UserId, userPassword = Password , userConfirmPassword = ConfirmNewPassword,  IsVerifyMobile,Code= digits }, null);
        }
        public async Task<Response<ResendOtpDto>> ResendOtp(string UserId)
        {
            string url = $"{BaseUrl}Account/ResendOtp";

            return await _requestProvider.PostDataAsync<ResendOtpDto>(url, new { Id = UserId }, null);
        }

        public async Task<Response<VerifyOTP>> ResendVerifyNewEmail(string token, string userID, string Email)
        {
            string url = $"{BaseUrl}Account/ResendVerifyNewEmailCode";

            return await _requestProvider.PostDataAsync<VerifyOTP>(url, new { userId = userID, email = Email }, token);
        }

        public async Task<Response<VerifyOTP>> ResendVerifyNewPhone(string token, string userID, string Phone)
        {
            string url = $"{BaseUrl}Account/ResendVerifyNewMobileCode";

            return await _requestProvider.PostDataAsync<VerifyOTP>(url, new { userId = userID, mobileNumber = Phone }, token);
        }

        public async Task<Response<object>> AddFirebaseDeviceToken(string token, string firebaseToken, string userID, string lang)
        {
            string url = $"{BaseUrl}Account/AddUserFireBaseToken";

            var data = new { UserId = userID, Token = firebaseToken, Lang = lang};

            return await _requestProvider.PostDataAsync<object>(url, data, token);
        }

        public async Task<Response<object>> UpdateFirebaseLanguage(string token, string userID, string lang)
        {
            string url = $"{BaseUrl}Account/UpdateUserFireBaseLang";

            var data = new { UserId = userID, Lang = lang };

            return await _requestProvider.PostDataAsync<object>(url, data, token);
        }

        public async Task<Response<object>> DeleteFirebaseDeviceToken(string token, string firebaseToken, string userID)
        {
            string url = $"{BaseUrl}Account/DeleteUserFireBaseToken";

            var data = new { UserId = userID};

            return await _requestProvider.PostDataAsync<object>(url, data, token);
        }

        public async Task<Response<UserDTO>> EditRegister(UserDTO user)
        {
            string url = $"{BaseUrl}Account/EditRegister";

            return await _requestProvider.PostDataAsync<UserDTO>(url, user, null);
        }

        public async Task<Response<List<Notification>>> GetUserNotifications(string token)
        {
            string url = $"{BaseUrl}Notifications/GetAll";

            return await _requestProvider.GetAsync<Response<List<Notification>>>(url, token);
        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<Response<VerifyOTP>> ResendVerifyPhone(string token, string userID)
        {
            string url = $"{BaseUrl}Account/ResendVerifyMobileCode";

            return await _requestProvider.PostDataAsync<VerifyOTP>(url, new { id = userID }, token);
        }

        public async Task<Response<VerifyOTP>> ResendVerifyEmail(string token, string userID)
        {
            string url = $"{BaseUrl}Account/ResendVerifyEmailCode";

            return await _requestProvider.PostDataAsync<VerifyOTP>(url, new { id = userID }, token);
        }

        public async Task<ChanelResponse> GetUserChanels(string token)
        {
            string url = $"{BaseUrl}/users/channels";
            return await _requestProvider.GetAsync<ChanelResponse>(url,token);
        }
    }
}
