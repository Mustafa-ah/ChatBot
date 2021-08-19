using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using STC.Common.Validations;
using STC.Common.Validations.Rules;
using STC.Models;
using STC.Services.Requests;
using STC.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.ViewModels
{
    public class NewRequestPageViewModel : BaseViewModel
    {
        public Location UserLocation { get; set; }
        private readonly IRequestService _requestService;
        private string _requestId;
        private bool IsProcessingNewRequest = false;
        private bool AddingAttachementUnderProcessing = false;
        public NewRequestPageViewModel(INavigationService navigationService,
            IRequestService requestService,
            ISettingsService settingsService) : base(navigationService, settingsService)
        {
            AppLang = settingsService.AppLanguage;

            UserLocation = new Location(24.68773, 46.72185);
            Attachments = new ObservableCollection<Attachment>();

            _requestService = requestService;
            IsEnabled = true;
            IsContinueEnabled = false;
            AddValidations();
        }
        private int AppLang;
        //public object Titl { get; set; }


        private double _TotalHieght;
        public double TotalHieght
        {
            get { return _TotalHieght; }
            set { SetProperty(ref _TotalHieght, value); }

        }
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }
        private bool _isContinueEnabled;
        public bool IsContinueEnabled
        {
            get { return _isContinueEnabled; }
            set {  _isContinueEnabled=value;  RaisePropertyChanged(); }
        }
        ValidatableObject<string> _attachmentTitle;
        public ValidatableObject<string> AttachmentTitle
        {
            get
            {
                return _attachmentTitle;
            }
            set
            {
                _attachmentTitle = value; RaisePropertyChanged();
            }
        }
        ValidatableObject<string> _requestDescription;
        public ValidatableObject<string> RequestDescription
        {
            get
            {
                return _requestDescription;
            }
            set
            {
                _requestDescription = value; RaisePropertyChanged();
            }
        }


        private ObservableCollection<Attachment> _Attachments;
        public ObservableCollection<Attachment> Attachments
        {
            get { return _Attachments; }
            set { SetProperty(ref _Attachments, value); }

        }

        public ICommand OpenMapCommand => new Command(OpenMap);

        public void OpenMap(object obj)
        {
            var parameters = new NavigationParameters {
                    { Constants.ParameterKey.Location, UserLocation}
                };

            NavigationService.NavigateAsync(Routes.ViewsRoutes.MapPageRoute, parameters);
        }

        public ICommand AddAttachmentCommand => new Command(AddAttachment);
        public ICommand NewRequestBackCommand => new Command(ExecuteNewRequestBackCommand);
        //TODO: Refactor this
        private async void AddAttachment(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                if (AddingAttachementUnderProcessing)
                    return;
                AddingAttachementUnderProcessing = true;
                if (!AttachmentTitleValid)
                {
                    AddingAttachementUnderProcessing = false;
                    return;
                }
               

                if (Attachments.Count == 8)
                {

                    ShowErrorToast(Resources.AppResources.NumberAttachmentsValidation);
                    AddingAttachementUnderProcessing = false;
                    return;
                }
                var result = await FilePicker.PickAsync();
                if (result != null)
                {

                    if (ValidateFileType(result))
                    {
                        var stream = await result.OpenReadAsync();

                        if (ValidateFileSize(stream))
                        {
                            var title = AttachmentTitle.Value;
                            ShowLoading();
                            if (string.IsNullOrEmpty(_requestId))
                            {
                                _requestId = await AddRequest();
                            }
                            var request = await _requestService.AddRequestAttachment(_requestId, title, stream, result.FileName, Setting.AuthAccessToken);
                            if(request.StatusCode==200)
                            {
                                if (TotalHieght<120)
                                {
                                    TotalHieght += 60;
                                }
                               
                                Attachment attachment = new Attachment() { Title = AttachmentTitle.Value,IsImage=Path.GetExtension(result.FileName).ToLower()==".pdf"?false:true };
                                Attachments.Add(attachment);
                                IsContinueEnabled = true;
                                AttachmentTitle.Value = string.Empty;//for UI
                                attachment.Id = request.Data;
                                if(Attachments.Count>=8)
                                {
                                    IsEnabled = false;
                                }
                            }
                            else
                            {
                                ShowErrorToast( request.Message);
                            }
                            HideLoading();
                            
                        }
                        else
                        {
                            ShowErrorToast( Resources.AppResources.SizeValidation);
                        }
                       
                    }
                    else
                    {
                        ShowErrorToast( Resources.AppResources.ExtensionValidation);
                    }

      
                }
                AddingAttachementUnderProcessing = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AddingAttachementUnderProcessing = false;
                HideLoading();
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
                       file.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase);
        }

        public ICommand DeleteAttachmentCommand => new Command(DeleteAttachment);
        public ICommand ContinueRequestCommand => new Command(ContinueRequest);


        private async void DeleteAttachment(object obj)
        {
            var attachment = obj as Attachment;
            Attachments.Remove(attachment);
            if (Attachments.Count < 8)
                IsEnabled = true;
            if(Attachments.Count==0)
                IsContinueEnabled = false;
            TotalHieght -= 60;
            await _requestService.DeleteAttachment(attachment.Id, Setting.AuthAccessToken);
        }

        public async void ExecuteNewRequestBackCommand()
        {
            if (Attachments.Count!=0 || !string.IsNullOrEmpty(AttachmentTitle.Value) || !string.IsNullOrEmpty(RequestDescription.Value))
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.DidntSaveDialog());
            }
            else
                await NavigationService.GoBackAsync();

        }
        private async void ContinueRequest(object obj)
        {
            try
            {
                if (!IsConncted())
                {
                    return;
                }
                if (Attachments.Count == 0)
                {
                    ShowErrorToast( Resources.AppResources.ChooseAttachment);
                    return;
                }
                else if (UserLocation == null)
                {
                    ShowErrorToast( Resources.AppResources.ChooseLocation);
                    return;
                }

                RequestDTO requestDTO = new RequestDTO()
                {
                    id = _requestId,
                    description = RequestDescription.Value,
                    latitudeRequest = UserLocation.Latitude.ToString(),
                    longitudeRequest = UserLocation.Longitude.ToString()
                };

                IsBusy = true;
                var request = await _requestService.UpdateRequests(requestDTO, Setting.AuthAccessToken);


                var parameters = new NavigationParameters {
                    { Constants.ParameterKey.RequestId, requestDTO.id }
                };

                await NavigationService.NavigateAsync(Routes.ViewsRoutes.RequestCreatedPageRoute, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                IsBusy = false;
            }
        }

        string _userName;


        private async Task<string> AddRequest()
        {
            if (!IsConncted())
            {
                return string.Empty;
            }

            if (IsProcessingNewRequest)
                return string.Empty;
            IsProcessingNewRequest = true;
            try
            {
                var requet = await _requestService.AddRequests(Setting.AuthAccessToken);

                return requet.Data.requestId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
            finally
            {
                IsProcessingNewRequest = false;
            }
         
        }

        private void AddValidations()
        {
            _requestDescription = new ValidatableObject<string>();

            _requestDescription.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = Resources.AppResources.Required
            });

            _attachmentTitle = new ValidatableObject<string>();

            _attachmentTitle.Validations.Add( new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = Resources.AppResources.Required
            });
            _attachmentTitle.Validations.Add(new NameSpecialCharsRule<string>
            {
                ValidationMessage = Resources.AppResources.invalidName
            });
        }

        private bool AttachmentTitleValid => _attachmentTitle.Validate();
        private bool RequestDescriptionValid => _requestDescription.Validate();

    }
}
