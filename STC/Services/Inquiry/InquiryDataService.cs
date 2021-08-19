using STC.Enums;
using STC.Models;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.Inquiry
{
    public class InquiryDataService : IInquiryDataService
    {
        private int _page = 1;
        private int _pageSize = 10;
        private int _numberofDateChats = 0;
        private bool canLoadMore = true;

        private readonly IInquiryService _inquiryService;
        private readonly ISettingsService _settingsService;
        public InquiryDataService(IInquiryService inquiryService, ISettingsService settingsService)
        {
            _inquiryService = inquiryService;
            _settingsService = settingsService;
        }

        public async Task<Response<InquiryDTO>> SendIquiry(string token, string inquiry, string inquiryId)
        {
            return await _inquiryService.SendIquiry(token, inquiry, inquiryId);
        }
        public int getCurrentPageNum()
        {
            return _page;
        }
        public int getPageSize()
        {
            return _pageSize;
        }
        public int getCurrentNumberofDateChats()
        {
            return _numberofDateChats;
        }
        public void ResetPagination()
        {
            _page = 1;
            _pageSize = 10;
            canLoadMore = true;
        }
        public async Task<List<ChattingModel>> GetInquiries(string token, string inquiryId, ObservableCollection<object> Inquiries)
        {
            _numberofDateChats = 0;
            var Chatting = new List<ChattingModel>();

            if (!canLoadMore)
            {
                return Chatting;
            }
            canLoadMore = false;


            InquiryPageDTO inquiryPage = await _inquiryService.GetAllInquiries(token, inquiryId, _page, _pageSize);

            canLoadMore = inquiryPage.HasNextPage;

            if (inquiryPage.Items != null && inquiryPage.Items.Count > 0)
            {
                var groupedByDateItems = inquiryPage.Items.GroupBy((d) => d.CreatedAt.Date).OrderBy(x=>x.Key).ToList();

                foreach (var item in groupedByDateItems)
                {
                    string it="";
                    if(_settingsService.AppLanguage==1)
                     it = item.Key.ToString("MMMM dd", CultureInfo.CreateSpecificCulture("ar-AE"));
                    else
                      it = item.Key.ToString("MMMM dd", CultureInfo.CreateSpecificCulture("en-US"));
                    if (Inquiries.Count!=0 && (Inquiries[0] as ChattingModel).Date == it)
                    {
                        Inquiries.RemoveAt(0);
                    }
                    ChattingModel chatDate = new ChattingModel()
                    {

                            Date = it,
                            ChatType = ChatListItemType.Date
                    };
                    Chatting.Add(chatDate);
                    _numberofDateChats++;

                    var itemList=item.ToList();
                    var orderedItemList=itemList.OrderBy(x => x.CreatedAt);
                    foreach (var ch in orderedItemList)
                    {
                        ChatListItemType listItemType = ch.UserType == "EndUser" ? ChatListItemType.Sender : ChatListItemType.Receiver;
                        string createAt = "";
                        if (_settingsService.AppLanguage == 1)
                            createAt = ch.CreatedAt.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-AE"));
                        else
                            createAt = ch.CreatedAt.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                        bool? isImage = null;
                        if(!string.IsNullOrEmpty(ch.Text))
                        {
                            if (!string.IsNullOrEmpty(ch.AttachmentId) && (Path.GetExtension(ch.Text)).ToLower() == ".pdf")
                                isImage = false;
                            else
                                isImage = true;
                        }
                            ChattingModel chat = new ChattingModel()
                        {
                            Date = createAt,
                            Text = ch.Text,
                            ChatType = listItemType,
                            AttachmentId=ch.AttachmentId,
                            IsAttachment=string.IsNullOrEmpty( ch.AttachmentId)?false:true,
                            AttachmentData=null,
                            Id = ch.Id,
                            IsImage = isImage

                            };
                        Chatting.Add(chat);
                    }

                }
                _page++;
                for (int i = Chatting.Count - 1; i >= 0; i--)
                    Inquiries.Insert(0, Chatting[i]);

            }
            return Chatting;
        }
    }
}
