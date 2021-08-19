using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;

namespace STC.Services.APPStaticData
{
    public class APPStaticDataService:  BaseService,IAPPStaticDataService
    {
        public APPStaticDataService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }

        public async Task<Response<List<AboutUsDTO>>> AboutAs(string token)
        {
            string url = $"{BaseUrl}AboutUs/GetAll";

            return await _requestProvider.GetAsync<Response<List<AboutUsDTO>>>(url, token);
        }
        public async Task<Response<List<ContactUsDTO>>> ContactUs(string token)
        {
            string url = $"{BaseUrl}ContactUs/GetAll";

            return await _requestProvider.GetAsync<Response<List<ContactUsDTO>>>(url, token);
        }
    }
}
