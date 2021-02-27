using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableAffectedUnitDPPMFinancialModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("data")]
            public List<Data> Data { get; set; }
        }

        public class Status
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }
        }

        public class Data
        {
            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("lists")]
            public List<string> List { get; set; }
        }
    }
}