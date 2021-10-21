using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace STC.Models
{
    public class Chanel
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("__v")]
        public long V { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        public string FullImage
        {
            get
            {
                return string.Format("https://www.khoolasa.chat/api/uploads/{0}",Image);
            }
        }

        public string FormatedDate
        {
            get
            {
                return string.Format("last update: {0}", UpdatedAt.ToString("MM/dd/yyyy"));
            }
        }

    }

    public class ChanelResponse
    {
        public bool Success { get; set; }
        public List<Chanel> Channels { get; set; }
    }
}
