using STC.Models;
using STC.Services.RequestProvider;
using STC.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.Contract
{
    public class ContractService : BaseService, IContractService
    {
        public ContractService(IRequestProvider requestProvider, ISettingsService settingsService) : base(requestProvider, settingsService)
        {

        }

        public async Task<Response<Attachment>> GetContractSignature(string requestId, string token)
        {
            string url = $"{BaseUrl}Attachments/GetRequestSignature?RequestId={requestId}";

            return await _requestProvider.GetAsync<Response<Attachment>>(url, token);
        }

        public async Task<Response<Attachment>> GetRequestContract( string requestId, string token)
        {
            string url = $"{BaseUrl}Attachments/GetRequestContract?RequestId={requestId}";

            return await _requestProvider.GetAsync<Response<Attachment>>(url, token);
        }

        public async Task<Response<bool>> IsFinalRequestContract(string attachmentId, string requestId, int versionNumber, string token)
        {
            string url = $"{BaseUrl}Attachments/IsFinalVersion?AttachmentId={attachmentId}&RequestId={requestId}&VersionNumber={versionNumber}";

            return await _requestProvider.GetAsync<Response<bool>>(url, token);
        }

        public async Task<Response<string>> SignRequestContract(string requestId, Stream streamData, string fileName, string token)
        {
            string url = $"{BaseUrl}Attachments/CreateSignatureAttachment";

            List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("RequestId", requestId),
                new KeyValuePair<string, string>("Title", "Signature")
            };

            return await _requestProvider.PostFormDataAsync<string>(url, formData, streamData, fileName, token);
        }

        public async Task<Response<bool>> UpdateRequestContractViewState(string attachmentId, bool isViewd, string token)
        {
            string url = $"{BaseUrl}Attachments/IsView";

            return await _requestProvider.PutDataAsync<bool>(url, new { AttachmentId = attachmentId, IsView = isViewd }, token);
        }
    }
}
