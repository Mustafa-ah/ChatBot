using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using STC.Enums;
using STC.Models;

namespace STC.Services.Requests
{
    public interface IRequestService
    {
        Task<Response<List<RequestDTO>>> GetAllRequests(string token);
        Task<Response<RequestDTO>> AddRequests(string token);
        Task<Response<RequestDTO>> GetRequestDetails(string requestId, string token);
        Task DeleteAttachment(string id, string token);
        Task<Response<object>> UpdateRequests(RequestDTO request, string token);
        Task<Response<string>> AddRequestAttachment(string requestId, string title, Stream streamData, string fileName,string token);
        Task<Response<RequestDTO>> AddInquiryAttachment(RequestDTO request, AttachmentType attachmentTypeId, Stream streamData, string fileName,string token);

        Task<Response<List<Attachment>>> GetRequestAttachments(string requestId, string inquryId, string token);
        Task<Response<Attachment>> GetAttachmentById(string attachmentId, string token);

        void OpenAttachment(string fileName, byte[] fileArray);
        Task<bool> DownloadAttachment(string fileName, byte[] fileArray);
        byte[] StreamToByteArray(System.IO.Stream stream);
        //PostFormDataAsync<T>(string uri, List<KeyValuePair<string, string>> formData, Stream streamData, string token)
    }
}
