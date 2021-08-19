using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using STC.Enums;
using STC.Interfaces;
using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace STC.Services.Requests
{
    public class RequestService : BaseService, IRequestService
    {
        public RequestService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }

        public async Task<Response<RequestDTO>> AddInquiryAttachment(RequestDTO request, AttachmentType attachmentTypeId, Stream streamData, string fileName, string token)
        {
            string url = $"{BaseUrl}Attachments/AttachmentInquiryReplies";
            List<KeyValuePair<string, string>> formData = null;
            if (!string.IsNullOrEmpty(request.id))
            {
                 formData = new List<KeyValuePair<string, string>>()
            {
//<<<<<<< HEAD
               
                new KeyValuePair<string, string>("RequestId", request.id),
                new KeyValuePair<string, string>("RequestInquiryId", request.inquiryId),
                 new KeyValuePair<string, string>("Title",Path.GetFileName( fileName)),
//=======
                //new KeyValuePair<string, string>("RequestId", request.id),
                //new KeyValuePair<string, string>("RequestInquiryId", request.inquiryId),
                //new KeyValuePair<string, string>("Title", "Title"),
                //new KeyValuePair<string, string>("FileName", "gg.png"),
                //new KeyValuePair<string, string>("DirectoryPath", ""),
                //new KeyValuePair<string, string>("AttachmentTypeId", ((int)attachmentTypeId).ToString())
//>>>>>>> 35ccaf9f5094cee2d74ee7baa45cbeef36ff0d60
            };
            }
            else
            {
               formData = new List<KeyValuePair<string, string>>()
            {

               
                new KeyValuePair<string, string>("RequestInquiryId", request.inquiryId),
                 new KeyValuePair<string, string>("Title",Path.GetFileName( fileName)),
            };
            }


            return await _requestProvider.PostFormDataAsync<RequestDTO>(url, formData, streamData, fileName, token);
        }

        public async Task<Response<string>> AddRequestAttachment(string requestId, string title, Stream streamData, string fileName,string token)
        {
            string url = $"{BaseUrl}Attachments/CreateRequestAttachment";

            List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("RequestId", requestId),
                new KeyValuePair<string, string>("Title", title),
                //new KeyValuePair<string, string>("FileName", "gg.png"),
                //new KeyValuePair<string, string>("DirectoryPath", "")
            };

            return await _requestProvider.PostFormDataAsync<string>(url, formData, streamData, fileName, token);
        }

        public async Task<Response<RequestDTO>> AddRequests(string token)
        {
            string url = $"{BaseUrl}Requests/CreateRequest";

            return await _requestProvider.PostDataAsync<RequestDTO>(url, new object(), token);
        }

        public Task DeleteAttachment(string id, string token)
        {
            string url = $"{BaseUrl}Attachments/Delete?id={id}";

            return  _requestProvider.DeleteAsync(url, token);
        }

        public async Task<Response<Attachment>> GetAttachmentById(string attachmentId, string token)
        {
            string url = $"{BaseUrl}Attachments/Get?id={attachmentId}";

            return await _requestProvider.GetAsync<Response<Attachment>>(url, token);
        }

        public async Task<Response<List<RequestDTO>>> GetAllRequests(string token)
        {
            string url = $"{BaseUrl}Requests/GetAll?ByUserId=true";

            return await _requestProvider.GetAsync<Response<List<RequestDTO>>>(url, token);
        }

        public async Task<Response<List<Attachment>>> GetRequestAttachments(string requestId, string inquryId, string token)
        {
            string url = $"{BaseUrl}Attachments/GetRequestAttachments?RequestId={requestId}";
            List<KeyValuePair<string, string>> header = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("RequestId", requestId),
                new KeyValuePair<string, string>("RequestInquiryId", inquryId)
            };
            return await _requestProvider.GetAsync<Response<List<Attachment>>>(url, token, header);
        }

        public async Task<Response<RequestDTO>> GetRequestDetails(string requestId, string token)
        {
            string url = $"{BaseUrl}Requests/GetRequest?id={requestId}";

            return await _requestProvider.GetAsync<Response<RequestDTO>>(url, token);
        }

        public async void OpenAttachment(string fileName, byte[] fileArray)
        {
            try
            {
                IFileHelper _fileService = DependencyService.Get<IFileHelper>();

                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.AskPermissionDialog());
                    }

                    var status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (status2 != PermissionStatus.Granted)
                    {
                        
                        return;
                    }
                }

                var status3 = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (status3 != PermissionStatus.Granted)
                {
                    if (status3 == PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.AskPermissionDialog());
                    }
                    var status4 = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status4 != PermissionStatus.Granted)
                    {
                        
                        return;
                    }
                }


                string path = _fileService.GetFilePath("Files");
                //string path = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string filepath = Path.Combine(path, fileName);

                if (File.Exists(filepath))
                {
                    //_fileService.OpenFile(filepath, false);
                    await Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(filepath) });
                }
                else
                {
                    //using (var fileStream = _fileService.OpenStream(filepath, fileArray.Length))
                    //{
                    //   await fileStream.WriteAsync(fileArray, 0, 1);
                    //}
                        File.WriteAllBytes(filepath, fileArray);
                   // _fileService.OpenFile(filepath, false);

                   await Launcher.OpenAsync(new OpenFileRequest() { File = new ReadOnlyFile(filepath) });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
           
        }

        public  byte[] StreamToByteArray(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
        public async Task<bool> DownloadAttachment(string fileName, byte[] fileArray)
        {
            try
            {
                IFileHelper _fileService = DependencyService.Get<IFileHelper>();

                var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();

                if (status != PermissionStatus.Granted)
                {
                    if (status == PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.AskPermissionDialog());
                    }
                    var status2 = await Permissions.RequestAsync<Permissions.StorageWrite>();
                    if (status2 != PermissionStatus.Granted)
                    {
                        
                        return false;
                    }
                }

                var status3 = await Permissions.CheckStatusAsync<Permissions.Photos>();

                if (status3 != PermissionStatus.Granted)
                {
                    if (status3 == PermissionStatus.Denied && Device.RuntimePlatform == Device.iOS)
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Dialogs.AskPermissionDialog());
                    }

                    var status4 = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status4 != PermissionStatus.Granted)
                    {
                        return false;
                    }
                }

                string path = _fileService.SaveFile(fileArray,fileName);

                //await App.Current.MainPage.DisplayAlert("For Debuging", path, "ok");
                //if (!Directory.Exists(path))
                //{
                //    Directory.CreateDirectory(path);
                //}

                //string filepath = Path.Combine(path, fileName);

                //if (!File.Exists(filepath))
                //{
                //    File.WriteAllBytes(filepath, fileArray);
                //}

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }
        public async Task<Response<object>> UpdateRequests(RequestDTO request, string token)
        {
            string url = $"{BaseUrl}Requests/UpdateRequest";

            return await _requestProvider.PutDataAsync<object>(url, request, token);
        }

    }
}
