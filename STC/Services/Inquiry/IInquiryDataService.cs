using STC.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.Inquiry
{
    public interface IInquiryDataService
    {
        Task<List<ChattingModel>> GetInquiries(string token, string inquiryId,ObservableCollection<object> Inquiries);
        Task<Response<InquiryDTO>> SendIquiry(string token, string inquiry, string inquiryId);
        int getCurrentPageNum();
        void ResetPagination();
        public int getPageSize();
        public int getCurrentNumberofDateChats();
    }
}
