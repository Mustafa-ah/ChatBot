using System;
using System.Threading.Tasks;
using STC.Models;

namespace STC.Services.Inquiry
{
    public interface IInquiryService
    {
        Task<Response<InquiryDTO>> GetIquiry( string token, string inquiryId);
        Task<Response<InquiryDTO>> SendIquiry( string token, string inquiry, string inquiryId);
        Task<Response<string>> CreateGeneralInquiry(string token);
        Task<InquiryPageDTO> GetAllInquiries(string token, string inquiryId, int page, int pageSize);
    }
}
