using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class ChartComboFinancialModel
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
            [JsonProperty("combo_bar")]
            public ComboBar ComboBar { get; set; }
        }

        public class ComboBar
        {
            [JsonProperty("labels")]
            public List<List<string>> Labels { get; set; }

            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("borderColor")]
            public string BorderColor { get; set; }

            [JsonProperty("backgroundColor")]
            public string BackgroundColor { get; set; }

            [JsonProperty("data")]
            public List<decimal> Data { get; set; }
        }
    }
}