using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class AutoSuggestPrefixSNModel
    {
            public class ResponseJson
            {
                [JsonProperty("status")]
                public Status Status { get; set; }

                [JsonProperty("data")]
                public List<string> Data { get; set; }
            }

            public class Status
            {
                [JsonProperty("code")]
                public int Code { get; set; }

                [JsonProperty("message")]
                public string Message { get; set; }
            }
        }
}