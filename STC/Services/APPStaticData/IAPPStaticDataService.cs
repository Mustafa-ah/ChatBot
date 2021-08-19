using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using STC.Models;

namespace STC.Services.APPStaticData
{
    public interface IAPPStaticDataService
    {
        Task<Response<List<AboutUsDTO>>> AboutAs(string token);
        Task<Response<List<ContactUsDTO>>> ContactUs(string token);
    }
}
