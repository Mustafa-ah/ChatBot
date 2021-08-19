using System;
namespace STC.Models
{
    public class BaseResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }

    public class Response<T> : BaseResponse
    {
        public T Data { get; set; }
    }
}
