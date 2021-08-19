using Prism.Navigation;
using STC.Common.Validations;
using STC.Enums;
using STC.Models;
using STC.Services.Inquiry;
using STC.Services.Requests;
using STC.Settings;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Chat;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace STC.ViewModels
{
    class NewInquiryPageViewModel : BaseViewModel
    {
        private int AppLang;
        private readonly IInquiryService _inquiryService;
        private readonly IRequestService _requestService;
        private readonly IInquiryDataService _inquiryDataService;
        private string generalInquiryId;
        private bool IsSending = false;
        private bool firstTime = true;
        private bool canMore = true;

        /// <summary>
        /// Current user of chat.
        /// </summary>


        public NewInquiryPageViewModel(INavigationService navigationService,
             IInquiryService inquiryService,
            ISettingsService settingsService,IRequestService requestService,  IInquiryDataService _inquiryDataService)
            : base(navigationService, settingsService)
        {
            AppLang = settingsService.AppLanguage;
            _inquiryService = inquiryService;
            _requestService = requestService;
            generalInquiryId = Setting.GeneralInquiryId;
          this._inquiryDataService=_inquiryDataService;
            
            
            MessagingCenter.Subscribe < BaseViewModel, InquiryDTO > (this, "broadcastreply", (sender, arg) =>
            {
                bool found = false;
                foreach(ChattingModel item  in Messages)
                {
                    if(item.Id==arg.Id)
                    {
                        found = true;
                        break;
                    }
                }
                if(!found && arg.InquiryId== generalInquiryId)
                {
                    string date = "";
                    if (Lang == 1)
                        date = arg.CreatedAt.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-AE"));
                    else
                        date = arg.CreatedAt.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                    bool? isImage = null;
                    if (!string.IsNullOrEmpty(arg.Text))
                    {
                        if (!string.IsNullOrEmpty(arg.AttachmentId) && (Path.GetExtension(arg.Text)).ToLower() == ".pdf")
                            isImage = false;
                        else
                            isImage = true;
                    }
                    Messages.Add(new ChattingModel()
                    {
                        ChatType = ChatListItemType.Receiver,
                        Text = arg.Text,
                        Date = date,
                        AttachmentId = arg.AttachmentId,
                        IsAttachment = string.IsNullOrEmpty(arg.AttachmentId) ? false : true,
                        AttachmentData = null,
                        Id = arg.Id,
                        IsImage = isImage
                    });
                    NoInquiries = Messages.Count() == 0;
                    MessagingCenter.Send(this, "toBottom");
                }

            });
            

            this.Messages = new ObservableCollection<object>();

 
         

           // this.GenerateMessages();
        }
        public ICommand DownloadChatAttachmentCommand => new Command(DownloadChatAttachmentCommandExecute);
     
        private LoadMoreOption loadMoreBehavior=LoadMoreOption.Auto;
        public LoadMoreOption LoadMoreBehavior
        {
            get { return loadMoreBehavior; }
            set { SetProperty(ref loadMoreBehavior, value); }

        }
        private bool _noInquiries;
        public bool NoInquiries
        {
            get { return _noInquiries; }
            set { SetProperty(ref _noInquiries, value); }
        }
       
        private string _inquireyNumber;
        public string InquireyNumber
        {
            get { return _inquireyNumber; }
            set { SetProperty(ref _inquireyNumber, value); }
        }


        /// <summary>
        /// Gets or sets the collection of Messages of a conversation.
        /// </summary>
        /// 
        private ObservableCollection<object> messages;
        public ObservableCollection<object> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
            }
        }

        string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value; RaisePropertyChanged();
            }
        }
        /// <summary>
        /// Gets or sets the current user of the message.
        /// </summary>
  
   
        private bool _IsLoading = false;
        public bool IsLoading
        {
            get { return _IsLoading; }
            set { SetProperty(ref _IsLoading, value); }
        }
        public ICommand SendMessageCommand => new Command(SendMessage);
        public ICommand LoadMoreCommand => new Command<object>(LoadMore, canLoadMore);

        private  void LoadMore(object  obj)
        {
            if (!firstTime)
            {
                GetInquiries();
            }
            

        }
        private bool canLoadMore(object obj)
        {

            if (canMore)
                return true;
            else
            {
                LoadMoreBehavior = LoadMoreOption.None;
                return false;
            }
        

        }

        private async void SendMessage(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }

                if (IsSending)
                    return;
                if (Message == null || string.IsNullOrEmpty(Message))
                {
                    return;
                }

                IsSending = true;

                string date = "";
                if (Lang == 1)
                    date = DateTime.Now.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-AE"));
                else
                    date = DateTime.Now.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                ChattingModel chat = new ChattingModel()
                {
                    Date = date,
                    Text = Message,
                    ChatType = ChatListItemType.Sender,
                    IsAttachment = false,
                    AttachmentId = null,
                    AttachmentData = null
                };
                Messages.Add(chat);
                NoInquiries = Messages.Count() == 0;
                string _message = Message;
                Message = string.Empty;
                var request = await _inquiryService.SendIquiry(Setting.AuthAccessToken, _message, generalInquiryId);


                IsSending = false;
                MessagingCenter.Send(this, "toBottom");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsSending = false;
            }

        

        }
        private async void DownloadChatAttachmentCommandExecute(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                var model = obj as ChattingModel;

                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {

                    await Permissions.RequestAsync<Permissions.StorageWrite>();
                    await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                if (model.AttachmentData != null)
                {

                    _requestService.DownloadAttachment(model.FileName, model.AttachmentData);

                    ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                }
                else
                {
                    ShowLoading();
                    var res = await _requestService.GetAttachmentById(model.AttachmentId, Setting.AuthAccessToken);
                    if (res.StatusCode == 200)
                    {
                        _requestService.DownloadAttachment(res.Data.FileName, res.Data.DataArray);

                        ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                    }
                    else
                        ShowErrorToast( res.Message);
                    HideLoading();
                }
            }
            catch (Exception ex)
            {
                ShowErrorToast(ex.Message);
            }
          
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
           // Message = Setting.GeneralInquiryMessage;
           
            await CreateGeneralInquiry();
            await GetInquiries();
  
            GetInquiryNumber();
        }

        private async Task CreateGeneralInquiry ()
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                if (string.IsNullOrEmpty(generalInquiryId))
                {
                    ShowLoading();
                    var request = await _inquiryService.CreateGeneralInquiry(Setting.AuthAccessToken);

                    generalInquiryId = request.Data;

                    Setting.GeneralInquiryId = generalInquiryId;
                    HideLoading();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }
  
        public async Task GetInquiries()
        {
            try
            {

                if (!IsConncted())
                {
                    return;
                }
               // IsBusy = true;
                var chatList = await _inquiryDataService.GetInquiries(Setting.AuthAccessToken, generalInquiryId, Messages );
                if (chatList.Count < _inquiryDataService.getPageSize() + _inquiryDataService.getCurrentNumberofDateChats())
                    canMore = false;
                else
                    canMore = true;

                if (chatList.Count < _inquiryDataService.getPageSize() + _inquiryDataService.getCurrentNumberofDateChats())
                    LoadMoreBehavior = LoadMoreOption.None;
                else
                {
                    LoadMoreBehavior = LoadMoreOption.Auto;
                }

                if (chatList != null)
                {
                    //foreach (var item in chatList)
                    //{
                    //    this.Messages.Add(item);

                    //    //Chatting.Add(item);
                    //}
                    NoInquiries = Messages.Count == 0;
                   
                }
                if(firstTime)
                {
                    if (Messages.Count != 0)
                        MessagingCenter.Send(this, "toBottom");
                    firstTime = false;
                }
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                 //IsBusy = false;
                //HideLoading();
            }

        }


               

        private async Task GetInquiryNumber()
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                var request = await _inquiryService.GetIquiry(Setting.AuthAccessToken, generalInquiryId);

                InquireyNumber = request.Data.Number;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }

    }
}
