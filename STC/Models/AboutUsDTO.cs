using System;
namespace STC.Models
{
    public class AboutUsDTO
    {
        public string id { get; set; }
        public string title { get; set; }
        public string titleAr { get; set; }
        public string text { get; set; }
        public string textAr { get; set; }
        public string createdBy { get; set; }
     public string updatedBy{ get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public bool isDeleted { get; set; }
    }
}
