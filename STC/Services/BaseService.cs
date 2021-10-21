using System;
using STC.Services.RequestProvider;
using STC.Settings;

namespace STC.Services
{
    public class BaseService
    {

        public const string BaseHubUrl = "http://15.185.62.3:50120/";
        public const string Apikey = "gsBkxMbNt6UX2wgRh50niEJF3g4dReV7";
        //private readonly string baseAddress = "https://www.khoolasa.chat/api";
        // private readonly string baseAddress = "https://www.khoolasa.chat/api";


        public static string BaseUrl { get; set; } = "https://www.khoolasa.chat/api";

        public readonly IRequestProvider _requestProvider;
        public readonly ISettingsService _setting;

        public BaseService(IRequestProvider requestProvider, ISettingsService settingsService)
        {
            this._requestProvider = requestProvider;
            _setting = settingsService;

            //string iso = "en-US";
            //if (_setting.AppLanguage == (int)Common.Enums.Languages.Arabic)
            //{
            //    iso = "ar-SA";
            //}

            //BaseUrl = string.Format("{0}/{1}/", baseAddress, iso);
        }
    }
}
