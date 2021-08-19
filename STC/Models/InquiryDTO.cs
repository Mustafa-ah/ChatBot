using System;
using System.Collections.Generic;

namespace STC.Models
{
    public class InquiryDTO
    {
        public string Id { get; set; }
        public string InquiryId { get; set; }
        public string UserType { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Number { get; set; }
        public string AttachmentId { get; set; }
        // public Attachment RequestAttachment { get; set; }
    }

    public class InquiryPageDTO
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
        public List<InquiryDTO> Items { get; set; }
    }
 
}
