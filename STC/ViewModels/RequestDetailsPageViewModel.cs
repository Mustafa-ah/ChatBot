using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using STC.Enums;
using STC.Models;
using STC.Services.Contract;
using STC.Services.Inquiry;
using STC.Services.Requests;
using STC.Settings;
using Syncfusion.ListView.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class RequestDetailsPageViewModel : BaseViewModel
    {
        private int AppLang;
        private string _requestContractMsg;
        private readonly IRequestService _requestService;
        private readonly IInquiryDataService _inquiryDataService;
        private readonly IContractService _contractService;
        private bool firstTime = true;
        private bool canMore = true;
        private bool IsSending = false;

        private ObservableCollection<Attachment> _files;
        public ObservableCollection<Attachment> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value); }

        }
        private string CurrentRequestId { get; set; }
        private RequestDTO _userRequest;
        public RequestDTO UserRequest
        {
            get { return _userRequest; }
            set { SetProperty(ref _userRequest, value); }
        }

        private string _formatedRequestDate;
        public string FormatedRequestDate 
        {
            get { return _formatedRequestDate; }
            set { SetProperty(ref _formatedRequestDate, value); }
        }
        private bool _isKeyboardOpen;
        public bool IsKeyboardOpen
        {
            get { return _isKeyboardOpen; }
            set { SetProperty(ref _isKeyboardOpen, value); }
        }
        private bool _noInquiries;
        public bool NoInquiries
        {
            get { return _noInquiries; }
            set { SetProperty(ref _noInquiries, value); }
        }
        private bool _noContract;
        public bool NoContract
        {
            get { return _noContract; }
            set { SetProperty(ref _noContract, value); }
        }
        private bool _isChatVisible;
        public bool IsChatVisible
        {
            get { return _isChatVisible; }
            set { SetProperty(ref _isChatVisible, value); }
        }
        private ObservableCollection<Attachment> _contracts;
        public ObservableCollection<Attachment> Contracts
        {
            get { return _contracts; }
            set { SetProperty(ref _contracts, value); }

        }
        private LoadMoreOption loadMoreBehavior = LoadMoreOption.Auto;
        public LoadMoreOption LoadMoreBehavior
        {
            get { return loadMoreBehavior; }
            set { SetProperty(ref loadMoreBehavior, value); }

        }
        public string Tab { get; set;}
  

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

        public ICommand DownloadContractCommand => new Command(DownloadContractCommandExcute);
        public ICommand DownloadFileCommand => new Command(DownloadFileCommandExcute);

       
        public ICommand OpenLocationCommand => new Command(OpenLocation);

        public ICommand ViewContractCommand => new Command(ViewContractCommandExcute);
        public ICommand ViewFileCommand => new Command(ViewFileCommandExcute);

        public ICommand NextRequestCommant => new Command(OpenNextRequest);

        public ICommand PreviousRequestCommant => new Command(OpenPreviousRequest);
        public ICommand DownloadChatAttachmentCommand => new Command(DownloadChatAttachmentCommandExecute);
        public ICommand LoadMoreCommand => new Command(LoadMore, canLoadore);
        public ICommand GoBackCommand => new Command(GoBack);

        private async void GoBack(object obj)
        {
            string navPath = NavigationService.GetNavigationUriPath().ToLower();
            if (navPath.Contains(Routes.ViewsRoutes.HomeRoute.ToLower()))
            {
              await  NavigationService.GoBackAsync();
            }
            else
            {
               var naved = await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.HomeRoute}");
                // for IOS
                if (!naved.Success)
                {
                     await NavigationService.NavigateAsync($"/{Routes.ViewsRoutes.HomeRoute}");
                }
            }
            UnsubscribeMessaging();
        }

        private void LoadMore(object obj)
        {

            GetInquiries();

        }
        private bool canLoadore(object obj)
        {

            if (canMore)
                return true;
            else
            {
                LoadMoreBehavior = LoadMoreOption.None;
                return false;
            }


        }

        private async void DownloadChatAttachmentCommandExecute(object obj)
        {
            if (!IsConncted())
            {
                return;
            }
            var model = obj as ChattingModel;

            if (model.AttachmentData!=null)
            {

                bool dowon = await _requestService.DownloadAttachment(model.FileName, model.AttachmentData);

                if (dowon)
                {
                    ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                }
                else
                {
                    ShowErrorToast(Resources.AppResources.FileNotDonwloaded);
                }
            }
            else
            {
                ShowLoading();
                var res = await _requestService.GetAttachmentById(model.AttachmentId, Setting.AuthAccessToken);
                if (res.StatusCode == 200)
                {
                   bool dowon= await _requestService.DownloadAttachment(res.Data.FileName, res.Data.DataArray);

                    if (dowon)
                    {
                        ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                    }
                    else
                    {
                        ShowErrorToast(Resources.AppResources.FileNotDonwloaded);
                    }
                }
                else
                    ShowErrorToast(res.Message);
                HideLoading();
            }


        }

        //ViewContractCommand

        public RequestDetailsPageViewModel(INavigationService navigationService,
             IRequestService requestService,
              IInquiryDataService inquiryService,
              IContractService contractService,
            ISettingsService settingsService)
            : base(navigationService, settingsService)
        {
            _requestService = requestService;
            _inquiryDataService = inquiryService;
            _contractService = contractService;
            AppLang = settingsService.AppLanguage;

            Contracts = new ObservableCollection<Attachment>();
            Files = new ObservableCollection<Attachment>();
            Messages = new ObservableCollection<object>();
            NoInquiries = true;
            IsChatVisible = true;
            MessagingCenter.Subscribe<ContractPageViewModel>(this, "UpdateContract", OnUpdateContract);

            MessagingCenter.Subscribe<BaseViewModel, Notification>(this, "broadcastnotify", async (sender, arg) =>
            {
                if(arg.NotificationType==(int)NotificationType.UploadContract && arg.RequestId.ToString()==CurrentRequestId)
                {
                    ReloadRequest();
                }
                if(arg.NotificationType==(int)NotificationType.InProgressRequest && arg.RequestId.ToString() == CurrentRequestId)
                {
                    ReloadRequest();
                }
            });
        }
        public async void ReloadRequest()
        {
            ShowLoading();
            _inquiryDataService.ResetPagination();
            firstTime = true;
            if (Messages != null)
                Messages.Clear(); 
            UserRequest = await GetRequestDetails(CurrentRequestId);
            NoInquiries = true;
            await GetRequestFiles();
            await GetInquiries();
            await GetRequestContract(CurrentRequestId);
            await GetContractSignature(CurrentRequestId);
            HideLoading();
        }
        private async void OnUpdateContract(ContractPageViewModel obj)
        {
           await GetRequestContract(UserRequest.id);
        }

      

        private async void DownloadContractCommandExcute(object obj)
        {
           
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                ShowLoading();
                Attachment att = obj as Attachment;

                if (att != null)
                {
                    att = await GetAttachment(att.Id);
                    bool downloaded = await _requestService.DownloadAttachment(att.FileName, att.DataArray);
                    if (downloaded)
                    {
                        ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                    }
                    else
                    {
                        ShowErrorToast(Resources.AppResources.FileNotDonwloaded);
                    }
                }
               // ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private async Task<Attachment> GetAttachment(string id)
        {
            
            Attachment attachment = new Attachment();
            if (!IsConncted())
            {
                return attachment;
            }
            try
            {
                var response = await _requestService.GetAttachmentById(id, Setting.AuthAccessToken);

                attachment = response.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return attachment;
        }
        private async void DownloadFileCommandExcute(object obj)
        {
            try
            {
                ShowLoading();

                Attachment att = obj as Attachment;

                if (att != null)
                {
                    att = await GetAttachment(att.Id);
                    bool downloaded = await _requestService.DownloadAttachment(att.FileName, att.DataArray);
                    if (downloaded)
                    {
                        ShowSucessToast(Resources.AppResources.FileDonwloadedSuccessfully);
                    }
                    else
                    {
                        ShowErrorToast(Resources.AppResources.FileNotDonwloaded);
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);    
            }
            finally
            {
                HideLoading();
            }
        }



        private async void OpenPreviousRequest(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                if (UserRequest.PreviousRequestId != null)
                {
                    var previousRequestId = UserRequest.PreviousRequestId;
                    ShowLoading();
                    _inquiryDataService.ResetPagination();
                    firstTime = true;
                    if (Messages != null)
                        Messages.Clear();

                    CurrentRequestId = previousRequestId;
                    UserRequest = await GetRequestDetails(previousRequestId);
                    NoInquiries = true;
                    await GetRequestFiles();
                    await GetInquiries();
                    await GetRequestContract(previousRequestId);
                    await GetContractSignature(previousRequestId);

                }
                else
                {
                    ShowSucessToast(Resources.AppResources.NoPreviouseRequest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }
        private async void OpenNextRequest(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                if (UserRequest.NextRequestId != null)
                {
                    string nextRequestId = UserRequest.NextRequestId;

                    ShowLoading();
                    _inquiryDataService.ResetPagination();
                    firstTime = true;
                    if (Messages != null)
                        Messages.Clear();

                    CurrentRequestId = nextRequestId;
                    UserRequest = await GetRequestDetails(nextRequestId);
                    NoInquiries = true;
                    await GetRequestFiles();
                    await GetInquiries();
                    await GetRequestContract(nextRequestId);
                    await GetContractSignature(nextRequestId);

                }
                else
                {
                    ShowSucessToast(Resources.AppResources.NoNextRequest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }
        private async void ViewContractCommandExcute(object obj)
        {
            try
            {
                Attachment attachment = obj as Attachment;

                if (!string.IsNullOrEmpty(_requestContractMsg))
                {
                    ShowWarningToast(_requestContractMsg);
                    _requestContractMsg = string.Empty;
                }

                ShowLoading();

               

                var vN = attachment.VersionNumber;
                attachment = await GetAttachment(attachment.Id);

                attachment.VersionNumber = vN;

                if (attachment.AttachmentTypeId == (int)Enums.AttachmentType.Contract)
                {
                    var parameters = new NavigationParameters {
                    { Constants.ParameterKey.Contract, attachment },
                     { Constants.ParameterKey.RequestStatusId, UserRequest.requestStatusId },
                     { Constants.ParameterKey.RequestId, UserRequest.id }
                };

                    NavigationService.NavigateAsync(Routes.ViewsRoutes.ContractPageRout, parameters);
                }
                else
                {
                    _requestService.OpenAttachment(attachment.FileName, attachment.DataArray);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            HideLoading();
        }

        private async void ViewFileCommandExcute(object obj)
        {
           
            try
            {
                ShowLoading();
                Attachment att = obj as Attachment;

                if (att != null)
                {
                    att = await GetAttachment(att.Id);
                    _requestService.OpenAttachment(att.FileName, att.DataArray);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                HideLoading();
            }
        }

        private async void OpenLocation(object obj)
        {
            if (double.TryParse(UserRequest.latitudeRequest, out double lat) && double.TryParse(UserRequest.longitudeRequest, out double lon))
            {
                var location = new Location(lat, lon);
                var options = new MapLaunchOptions { Name = UserRequest.number };
                await Map.OpenAsync(location, options);
            }

        }
        private bool ValidateFileSize(Stream file)
        {
            if (file == null)
            {
                return false;
            }
            double m = file.Length / (1024d * 1024d);
            return Math.Round(m, MidpointRounding.AwayFromZero) <= 2;
        }

        private bool ValidateFileType(FileResult file)
        {
            if (file == null)
            {
                return false;
            }

            return file.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase) ||
                       file.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                       file.FileName.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) ||
                       file.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase);
        }
        private async Task<RequestDTO> GetRequestDetails( string requestId)
        {
            try
            {
                if (!IsConncted())
                {
                    return null;
                }
                ShowLoading();
                var request = await _requestService.GetRequestDetails(requestId, Setting.AuthAccessToken);
                if (Lang==1)
                    FormatedRequestDate = string.Format("{0} {1} {2}", request.Data.createdAt.Day,
                request.Data.createdAt.ToString("MMMM", new CultureInfo("ar-AE")),
                 request.Data.createdAt.Year);
                else
                    FormatedRequestDate = string.Format("{0} {1} {2}", request.Data.createdAt.Day,
                            request.Data.createdAt.ToString("MMMM", new CultureInfo("en-US")),
                             request.Data.createdAt.Year);
                if (Lang == 1)
                    request.Data.requestStatus = request.Data.requestStatusAr;
                if (request.Data.requestStatusId == (int)Enums.RequestStatus.Signed)
                {
                    IsChatVisible = false;
                    NoInquiries = false;

                }
                else
                {
                    IsChatVisible = true;
                    NoInquiries = true;
                }
        
                return request.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new RequestDTO();
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
        public ICommand SendMessageCommand => new Command(SendMessage);

        public async Task GetInquiries()
        {
            try
            {

                if (!IsConncted())
                {
                    return;
                }
               

                var chatList = await _inquiryDataService.GetInquiries(Setting.AuthAccessToken, UserRequest.inquiryId, Messages);
               
                if (chatList != null)
                {
                    NoInquiries = Messages.Count == 0;
                    //foreach (var item in chatList)
                    //{
                    //    Chatting.Add(item);
                    //    Messages.Add(item);
                    //}
                }
                NoInquiries = Messages.Count() == 0;
                if (chatList.Count < _inquiryDataService.getPageSize()+_inquiryDataService.getCurrentNumberofDateChats())
                    canMore = false;
                else
                    canMore = true;
                if (chatList.Count < _inquiryDataService.getPageSize() + _inquiryDataService.getCurrentNumberofDateChats())
                {
                    LoadMoreBehavior = LoadMoreOption.None;
                }
                else
                {
                    LoadMoreBehavior = LoadMoreOption.Auto;
                }
                if (firstTime)
                {
                    if(Messages.Count!=0)
                    MessagingCenter.Send(this, "toBottom");
                    firstTime = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        private async void SendMessage(object obj)
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
            // IsBusy = true;

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
                AttachmentId=null,
                IsAttachment=false,
                AttachmentData = null
            };
            Messages.Add(chat);
            NoInquiries = Messages.Count() == 0;
            string _message = Message;
            Message = string.Empty;
            var request = await _inquiryDataService.SendIquiry(Setting.AuthAccessToken, _message, UserRequest.inquiryId);

        

            NoInquiries = false;
            IsSending = false;
            MessagingCenter.Send(this, "toBottom");
            // IsBusy = false;

        }
        private async Task GetRequestFiles()
        {
            try
            {
                Files.Clear();
                var request = await _requestService.GetRequestAttachments(UserRequest.id, UserRequest.inquiryId, Setting.AuthAccessToken);

                var listFiles = request.Data;

                if (listFiles != null)
                {
                    foreach(var item in listFiles)
                    {
                        item.IsImage = Path.GetExtension(item.FileName).ToLower() == ".pdf" ? false : true;
                        Files.Add(item);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private async Task GetRequestContract(string requestId)
        {
            try
            {
                Contracts.Clear();
                var request = await _contractService.GetRequestContract(requestId, Setting.AuthAccessToken);

                if (request.StatusCode == (int)StatusCode.Ok)
                {
                    Contracts.Add(request.Data);
                }

                if (request.Data.IsNotifyUser)
                {
                    _requestContractMsg = request.Message;
                }
                if (Contracts.Count != 0)
                    NoContract = false;
                else
                    NoContract = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //HideLoading();
            }
        }

        private async Task GetContractSignature(string requestId)
        {
            try
            {

                var request = await _contractService.GetContractSignature(requestId, Setting.AuthAccessToken);

                if (request.StatusCode == (int)StatusCode.Ok)
                {
                    //Signature
                    request.Data.AttachmentTypeId = (int)AttachmentType.Signature;
                    Contracts.Add(request.Data);
                }
                if (Contracts.Count != 0)
                    NoContract = false;
                else
                    NoContract = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                //HideLoading();
            }
        }


        public async Task PickFileAsync(bool isImage)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }

                FileResult result;

                if (isImage)
                {
                    result = await MediaPicker.PickPhotoAsync();
                }
                else
                {
                    result = await FilePicker.PickAsync();
                }
               
                if (result != null)
                {
                    if (ValidateFileType(result))
                    {
                        ShowLoading();

                        var stream = await result.OpenReadAsync();
                        if (ValidateFileSize(stream))
                        {

                            //AttachmentType
                            byte[] buffer = null;
                            buffer = _requestService.StreamToByteArray(stream);
                            var request = await _requestService.AddInquiryAttachment(UserRequest, Enums.AttachmentType.Inquiry, stream, result.FileName, Setting.AuthAccessToken);

                            if (request.StatusCode == 200)
                            {
                                ShowSucessToast(request.Message);
                                string date = "";
                                if (Lang == 1)
                                    date = DateTime.Now.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("ar-AE"));
                                else
                                    date = DateTime.Now.ToString("hh:mm tt", CultureInfo.CreateSpecificCulture("en-US"));
                                ChattingModel chat = new ChattingModel()
                                {
                                    Date = date,
                                    Text = result.FileName,
                                    ChatType = ChatListItemType.Sender,
                                    AttachmentId = null,
                                    IsAttachment = true,
                                    AttachmentData = buffer,
                                    FileName = result.FileName,
                                    IsImage = (Path.GetExtension(result.FileName)).ToLower() == ".pdf" ? false : true
                                };
                                NoInquiries = false;
                                Messages.Add(chat);
                                MessagingCenter.Send(this, "toBottom");
                                //  await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();

                                await GetRequestFiles();
                            }
                            else
                                ShowErrorToast(request.Message);
                        }
                        else
                        {
                            ShowErrorToast(Resources.AppResources.SizeValidation);
                        }
                    }
                    else
                    {
                        ShowErrorToast(Resources.AppResources.ExtensionValidation);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowErrorToast(Resources.AppResources.AccessDenied);
            }
            catch (Exception ex)
            {
                ShowErrorToast(ex.Message);
            }
            HideLoading();
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey(Constants.ParameterKey.RequestId))
            {
                string _requestId = parameters[Constants.ParameterKey.RequestId].ToString();
                CurrentRequestId = _requestId;
                ShowLoading();
                UserRequest = await GetRequestDetails(_requestId);
        

                await GetRequestFiles();
                await GetInquiries();
                await GetRequestContract(_requestId);
                await GetContractSignature(_requestId);
                HideLoading();

                MessagingCenter.Subscribe<BaseViewModel, InquiryDTO>(this, "broadcastreply", (sender, arg) =>
                {
                    bool found = false;
                    foreach (ChattingModel item in Messages)
                    {
                        if (item.Id == arg.Id)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found && UserRequest.inquiryId == arg.InquiryId)
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

            }
            if (parameters.ContainsKey("Tab"))
            {
                Tab = parameters["Tab"].ToString();
                if (Tab == "Contract")
                    MessagingCenter.Send(this, "OpenContractTab");
                else if(Tab== "Inquiries")
                    MessagingCenter.Send(this, "OpenInquiriesTab");
            }

            InitMessage(UserRequest.id);
        }

        private void InitMessage(string requestId)
        {
            try
            {
                var list = Setting.RequestInquiryMessageList;

                RequestInquiryMessage request = list.FirstOrDefault(r => r.Id == requestId);

                if (request != null)
                {
                    Message = request.Message;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        private void UnsubscribeMessaging()
        {
            MessagingCenter.Unsubscribe<BaseViewModel, Notification>(this, "broadcastnotify");
            MessagingCenter.Unsubscribe<BaseViewModel, InquiryDTO>(this, "broadcastreply");
            MessagingCenter.Unsubscribe<ContractPageViewModel>(this, "UpdateContract");
        }
    }
}
