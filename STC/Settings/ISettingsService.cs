using STC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STC.Settings
{
    public interface ISettingsService
    {
        string AuthAccessToken { get; set; }
        string PushNotificationDeviceToken { get; set; }
        string UserId { get; set; }
        string AuthIdToken { get; set; }
        string GeneralInquiryId { get; set; }
        bool IsLoggedin { get; set; }

        int AppLanguage { get; set; }

        string IdentityEndpointBase { get; set; }
        string EndpointBase { get; set; }

        string Latitude { get; set; }
        string Longitude { get; set; }
        bool AllowGpsLocation { get; set; }
        bool HasNewNotifications { get; set; }
        Guid TempNotificationId { get; set; }
        int VerifyType { get; set; }//temp solution for dialog
        T GetValueOrDefault<T>(string key, T defaultValue);

        Task AddOrUpdateValue<T>(string key, T value);

        string GeneralInquiryMessage { get; set; }
        List<RequestInquiryMessage> RequestInquiryMessageList { get; set; }
    }
}
