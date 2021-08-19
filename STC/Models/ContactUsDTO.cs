using System;
namespace STC.Models
{
    public class ContactUsDTO
    {
      public string  id { get; set; }
        public string mobileNumber { get; set; }
        public string hotline { get; set; }
        public string email { get; set; }
        public string facebookAccount { get; set; }
        public string twitterAccount { get; set; }
        public string instagramAccount { get; set; }
        public string youtubeAccount { get; set; }
        public string createdBy { get; set; }
    public string updatedBy { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public bool isDeleted { get; set; }
    public bool isFaceBookVisible { get; set; }
    public bool isTwitterVisible { get; set; }
    public bool isInstgramVisible { get; set; }
    public bool isYouTubeVisibe { get; set; }
}
}
