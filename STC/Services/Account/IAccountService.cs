using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using STC.Models;

namespace STC.Services.Account
{
    public interface IAccountService
    {
        Task<Response<UserDTO>> Login(string emailOrPhone, string password ,  bool ismobile);
        Task<Response<UserDTO>> Register(UserDTO user);
        Task<Response<UserDTO>> EditRegister(UserDTO user);
        Task<Response<UserToken>> VerifyOTP(string userId, string otp);
        Task<Response<UserToken>> VerifyEmail(string userId, string otp,bool isNotNull=false);
        Task<Response<UserToken>> VerifyPhone(string userId, string otp,bool isNotNull=false);
        Task<Response<ResendOtpDto>> ResendOtp(string UserId);
        Task<Response<bool>> ChangePassword(string userId, string currPassword, string newPass, string confirmNewPass , string token);
        Task<Response<Profile>> GetAccountDetails(string token);
            
        Task<Response<int>> UpdateProfile(string fullName, string email, string mobile, string  token);

        Task<Response<ForgetPassword>> ForgetPassword(string emailOrPhone, bool ismobile = true);
        Task<Response<SetNewPassword>> SetNewPassword(string UserId, string Password, string ConfirmNewPassword, bool IsVerifyMobile, string digits);
        Task<Response<VerifyOTP>> ResendVerifyNewEmail(string token, string userID, string Email);
        Task<Response<VerifyOTP>> ResendVerifyNewPhone(string token, string userID, string Phone);
        Task<Response<VerifyOTP>> ResendVerifyPhone(string token, string userID);
        Task<Response<VerifyOTP>> ResendVerifyEmail(string token, string userID);
        Task<Response<object>> AddFirebaseDeviceToken(string token, string firebaseToken, string userID, string lang);
        Task<Response<object>> UpdateFirebaseLanguage(string token, string userID, string lang);
        Task<Response<object>> DeleteFirebaseDeviceToken(string token, string firebaseToken, string userID);
        Task<Response<object>> UpdateProfilePicture(string userId, Stream content, string fileName, string token);

        Task<Response<List<Notification>>> GetUserNotifications(string token);
        Task<Response<int>> EditEmail(string email, string userId, string token);

        Task<Response<int>> EditMobileNumber(string mobile, string userId, string token);

        Task<Response<int>> EditFullName(string fullName, string userId, string token);
        
        Task Logout();
    }
}