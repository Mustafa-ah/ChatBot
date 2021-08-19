using System;
using System.Threading.Tasks;
using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;

namespace STC.Services.Inquiry
{
    public class InquiryService : BaseService, IInquiryService
    {

        public InquiryService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }

        public async Task<Response<string>> CreateGeneralInquiry(string token)
        {
            string url = $"{BaseUrl}Inquiries/Create";
            return await this._requestProvider.PostDataAsync<string>(url, new object(), token);
        }

        public async Task<InquiryPageDTO> GetAllInquiries(string token, string inquiryId, int page, int pageSize)
        {
            string url = $"{BaseUrl}InquiryReplies/GetAllPaginated?InquiryId={inquiryId}&PageNumber={page}&PageSize={pageSize}";
            return await this._requestProvider.GetAsync<InquiryPageDTO>(url, token);
        }

        public async Task<Response<InquiryDTO>> GetIquiry(string token, string inquiryId)
        {
            string url = $"{BaseUrl}Inquiries/Get?id={inquiryId}";
            return await this._requestProvider.GetAsync<Response<InquiryDTO>>(url, token);
        }

        public async Task<Response<InquiryDTO>> SendIquiry(string token, string inquiry, string inquiryId)
        {
            string url = $"{BaseUrl}InquiryReplies/Create";
            return await this._requestProvider.PostDataAsync<InquiryDTO>(url, new { Text = inquiry, InquiryId = inquiryId}, token);
        }
    }
}
