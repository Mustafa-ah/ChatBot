using System;
using STC.Services.RequestProvider;
using STC.Settings;

namespace STC.Services
{
    public class BaseService
    {

        public const string BaseHubUrl = "http://15.185.62.3:50120/";
        private readonly string baseAddress = "http://15.185.62.3:50120/api";

        //test commit
        public static string BaseUrl { get; set; }

        public readonly IRequestProvider _requestProvider;
        public readonly ISettingsService _setting;

        public BaseService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            this._requestProvider = requestProvider;
            _setting = settingsService;

            string iso = "en-US";
            if (_setting.AppLanguage == (int)Common.Enums.Languages.Arabic)
            {
                iso = "ar-SA";
            }

            BaseUrl = string.Format("{0}/{1}/", baseAddress, iso);
        }
    }
}
