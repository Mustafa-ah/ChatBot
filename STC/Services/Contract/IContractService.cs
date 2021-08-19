using STC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.Contract
{
    public interface IContractService
    {
        Task<Response<Attachment>> GetRequestContract(string requestId, string token);
        Task<Response<Attachment>> GetContractSignature(string requestId, string token);
        Task<Response<bool>> UpdateRequestContractViewState(string attachmentId, bool isViewd, string token);
        Task<Response<bool>> IsFinalRequestContract(string attachmentId, string requestId, int versionNumber, string token);
        Task<Response<string>> SignRequestContract(string requestId, Stream streamData, string fileName, string token);
    }
}
