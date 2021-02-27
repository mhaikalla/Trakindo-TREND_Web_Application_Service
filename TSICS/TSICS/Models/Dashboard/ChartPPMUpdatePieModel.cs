using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace TSICS.Models.Dashboard
{
    public class ChartPPMUpdatePieModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public class Status
        {
            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }
        }

        public class Data
        {
            [JsonProperty("pie")]
            public PIE Pie { get; set; }
        }

        public class PIE
        {
            [JsonProperty("labels")]
            public List<string> Label { get; set; }

            [JsonProperty("percent_label")]
            public bool PercentLabel { get; set; }

            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("backgroundColor")]
            public List<string> BackgroundColor { get; set; }

            [JsonProperty("data")]
            public List<decimal> data { get; set; }
        }
    }
}