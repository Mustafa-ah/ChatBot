using Newtonsoft.Json;
using STC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace STC.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Setting Constants

        private const string AccessToken = "access_token";
        private const string NotificationToken = "Notification_Token";
        private const string userId = "User_id";
        private const string tempNotificationId = "Temp_Notification_Id";
        private const string IdToken = "id_token";
        private const string AppLang = "AppLang";//
        private const string GeneralInquiryIdKey = "GeneralInquiryIdKey";
        private const string HasNewNotificationsKey = "HasNewNotificationsKey";

        private const string IdIdentityBase = "url_base";
        private const string IdEndpointBase = "IdEndpointBase";
        private const string GeneralInquiryMeassageKey = "GeneralInquiryMeassageKey";
        private const string RequestInquiryMeassageKey = "RequestInquiryMeassageKey";
        private const string IsLoggedinKey = "IsLoggedinKey";



        private const string IdLatitude = "latitude";
        private const string IdLongitude = "longitude";
        private const string IdAllowGpsLocation = "allow_gps_location";
        private readonly string AccessTokenDefault = string.Empty;
        private readonly string UserIdDefault = string.Empty;
        private readonly string IdTokenDefault = string.Empty;

        private readonly bool AllowGpsLocationDefault = false;
        private readonly double FakeLatitudeDefault = 47.604610d;
        private readonly double FakeLongitudeDefault = -122.315752d;

        private readonly string UrlIdentityDefault = "";
        private readonly string IdEndpointBaseDefault = "";
        private readonly string GeneralInquiryIdDefault = string.Empty;
        //

        private readonly int AppLangDefault = 0;// (int)Common.Enums.Languages.Arabic;
        #endregion

        #region Settings Properties

        public string UserId
        {
            get => GetValueOrDefault(userId, UserIdDefault);
            set => AddOrUpdateValue(userId, value);
        }
        public Guid TempNotificationId
        {
            get => GetValueOrDefault(tempNotificationId, Guid.Empty);
            set => AddOrUpdateValue(tempNotificationId, value);
        }
        public string AuthAccessToken
        {
            get => GetValueOrDefault(AccessToken, AccessTokenDefault);
            set => AddOrUpdateValue(AccessToken, value);
        }

        public string PushNotificationDeviceToken
        {
            get => GetValueOrDefault(NotificationToken, AccessTokenDefault);
            set => AddOrUpdateValue(NotificationToken, value);
        }

        public string AuthIdToken
        {
            get => GetValueOrDefault(IdToken, IdTokenDefault);
            set => AddOrUpdateValue(IdToken, value);
        }

        public string GeneralInquiryId
        {
            get => GetValueOrDefault(GeneralInquiryIdKey, GeneralInquiryIdDefault);
            set => AddOrUpdateValue(GeneralInquiryIdKey, value);
        }

        public string IdentityEndpointBase
        {
            get => GetValueOrDefault(IdIdentityBase, UrlIdentityDefault);
            set => AddOrUpdateValue(IdIdentityBase, value);
        }

        

        public string Latitude
        {
            get => GetValueOrDefault(IdLatitude, FakeLatitudeDefault.ToString());
            set => AddOrUpdateValue(IdLatitude, value);
        }

        public string Longitude
        {
            get => GetValueOrDefault(IdLongitude, FakeLongitudeDefault.ToString());
            set => AddOrUpdateValue(IdLongitude, value);
        }

        public bool AllowGpsLocation
        {
            get => GetValueOrDefault(IdAllowGpsLocation, AllowGpsLocationDefault);
            set => AddOrUpdateValue(IdAllowGpsLocation, value);
        }

        public bool HasNewNotifications
        {
            get => GetValueOrDefault(HasNewNotificationsKey, false);
            set => AddOrUpdateValue(HasNewNotificationsKey, value);
        }
        public string EndpointBase
        {
            get => GetValueOrDefault(IdEndpointBase, IdEndpointBaseDefault);
            set => AddOrUpdateValue(IdEndpointBase, value);
        }

        public int AppLanguage
        {
            get => GetValueOrDefault<int>(AppLang, AppLangDefault);
            set => AddOrUpdateValue(AppLang, value);
        }

        public int VerifyType
        {
            get => GetValueOrDefault<int>("VerifyType", 1);
            set => AddOrUpdateValue("VerifyType", value);
        }

        public string GeneralInquiryMessage
        {
            get => GetValueOrDefault(GeneralInquiryMeassageKey, "");
            set => AddOrUpdateValue(GeneralInquiryMeassageKey, value);
        }

        public List<RequestInquiryMessage> RequestInquiryMessageList
        {
            get => DeSerializeObject<List<RequestInquiryMessage>>(GetValueOrDefault(RequestInquiryMeassageKey, "[]"));
            set => AddOrUpdateValue(RequestInquiryMeassageKey, SerializeObject(value));
        }

        public bool IsLoggedin
        {
            get => GetValueOrDefault<bool>(IsLoggedinKey, false);
            set => AddOrUpdateValue(IsLoggedinKey, value);
        }
        #endregion

        #region Public Methods

        private string SerializeObject(object obj) => JsonConvert.SerializeObject(obj);
        private T DeSerializeObject<T>(string obj) => JsonConvert.DeserializeObject<T>(obj);

        public Task AddOrUpdateValue<T>(string key, T value) => AddOrUpdateValueInternal(key, value);
        //public Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        //public bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        public T GetValueOrDefault<T>(string key, T defaultValue) => GetValueOrDefaultInternal<T>(key, defaultValue);

        #endregion

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default(T))
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        #endregion
    }
}
