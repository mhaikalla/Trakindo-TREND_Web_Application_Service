using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class CharCompletionPipModel
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
            [JsonProperty("vertical_bar_stacked")]
            public VerticalBarStacked VerticalBarStacked { get; set; }
        }

        public class VerticalBarStacked
        {
            [JsonProperty("labels")]
            public List<string> Label { get; set; }

            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("data")]
            public List<decimal> Data { get; set; }

            [JsonProperty("raw_data")]
            public List<int> RawData { get; set; }

            [JsonProperty("backgroundColor")]
            public string BackgroundColor { get; set; }

            [JsonProperty("borderColor")]
            public string BorderColor { get; set; }
        }
    }
}