using STC.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace STC.Services.FAQs
{
   
    public interface IFAQService
    {
     
        Task<Response<List<FAQDTO>>> GetAllFAQS(string token);
    }
}
