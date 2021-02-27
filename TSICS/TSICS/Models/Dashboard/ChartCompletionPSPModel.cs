using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;


namespace TSICS.Models.Dashboard
{
    public class ChartCompletionPSPModel
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
            [JsonProperty("horiz_bar_stacked")]
            public HorizBarStacked HorizBarStacked { get; set; }
        }

        public class HorizBarStacked
        {
            [JsonProperty("labels")]
            public List<string> Labels { get; set; }

            [JsonProperty("id")]
            public List<int> Id { get; set; }

            [JsonProperty("datasets")]
            public List<DataSets> DataSets { get; set; }

            [JsonProperty("annotation")]
            public Anotation Anotation { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("yAxisID")]
            public string yAxisID { get; set; }

            [JsonProperty("data")]
            public List<decimal> Data { get; set; }

            [JsonProperty("raw_data")]
            public List<int> RawData { get; set; }

            [JsonProperty("backgroundColor")]
            public string BackgroundColor { get; set; }

            [JsonProperty("borderColor")]
            public string BorderColor { get; set; }
        }

        public class Anotation
        {
            [JsonProperty("min")]
            public int Min { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }
        }
    }
}