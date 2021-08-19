using System;
using System.Collections.Generic;

namespace STC.Models
{
    public class RequestDTO
    {
        public string id { get; set; }
        public int numberKey { get; set; }
        public string number { get; set; }
        public string requestId { get; set; }
        public string inquiryId { get; set; }
        public int requestStatusId { get; set; }
        public string description { get; set; }
        public DateTime createdAt { get; set; }
        public string requestStatus { get; set; }
        public string longitudeRequest { get; set; }
        public string latitudeRequest { get; set; }
        public string requestStatusAr { get; set; }
        public string requestStatusColor { get; set; }
        public string NextRequestId { get; set; }
        public string PreviousRequestId { get; set; }
    }
}
