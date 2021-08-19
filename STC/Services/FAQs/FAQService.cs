using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.FAQs
{
    class FAQService : BaseService, IFAQService
    {
        public FAQService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }
        public async Task<Response<List<FAQDTO>>> GetAllFAQS(string token)
        {
            string url = $"{BaseUrl}FAQs/GetAll";

            return await _requestProvider.GetAsync<Response<List<FAQDTO>>>(url, token);
        }
    }
}
