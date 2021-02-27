using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class ChartCompletionDoughnutFilterSafetyLegacyModel
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
            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("code")]
            public int Code { get; set; }
        }

        public class Data
        {
            [JsonProperty("doughnut")]
            public Doughnut Doughnut { get; set; }
        }

        public class Doughnut
        {
            [JsonProperty("labels")]
            public List<string> Labels { get; set; }

            [JsonProperty("use_chart")]
            public bool UseChart { get; set; }

            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("data")]
            public List<decimal> Data { get; set; }

            [JsonProperty("borderWidth")]
            public int BorderWidth { get; set; }

            [JsonProperty("use_percent")]
            public bool UsePercent { get; set; }

            [JsonProperty("backgroundColor")]
            public List<string> BackgroundColor { get; set; }
        }
    }
}