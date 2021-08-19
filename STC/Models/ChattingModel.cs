using System;
using System.Collections.Generic;
using System.Text;
using STC.Enums;

namespace STC.Models
{
   public class ChattingModel
    {
        public string Text { get; set; }
        public string Date { get; set; }
        public ChatListItemType ChatType { get; set; }
        public bool IsAttachment { get; set; }
        public string AttachmentId { get; set; }
        public byte[] AttachmentData { get; set; }
        public string FileName { get; set; }
        public string Id { get; set; }
        public bool? IsImage { get; set; }
    }
}
