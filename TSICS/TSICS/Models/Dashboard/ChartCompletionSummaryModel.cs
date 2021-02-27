using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class ChartCompletionSummaryModel
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
            [JsonProperty("vertical_bar")]
            public VerticalBar VerticalBar { get; set; }
        }

        public class VerticalBar
        {
            [JsonProperty("labels")]
            public List<string> Label { get; set; }
            
            [JsonProperty("bar_size")]
            public decimal BarSize { get; set; }

            [JsonProperty("y_axes")]
            public YAxes YAxes { get; set; }
            
            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }
        }

        public class YAxes
        {
            [JsonProperty("min")]
            public int Min { get; set; }

            [JsonProperty("max")]
            public int Max { get; set; }

            [JsonProperty("step")]
            public int Step { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("data")]
            public List<decimal> DataPercent { get; set; }

            [JsonProperty("raw_data")]
            public List<List<decimal>> RawData { get; set; }

            [JsonProperty("backgroundColor")]
            public string BackgroundColor { get; set; }

            [JsonProperty("borderColor")]
            public string BorderColor { get; set; }
        }
    }
}