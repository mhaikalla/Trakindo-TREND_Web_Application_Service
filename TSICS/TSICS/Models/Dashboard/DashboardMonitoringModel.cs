using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class DashboardMonitoringModel
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
            [JsonProperty("pie_half")]
            public PieHalf PieHalf { get; set; }
        }

        public class PieHalf
        {
            [JsonProperty("labels")]
            public List<string> Label { get; set; }

            [JsonProperty("datasets")]
            public DataSets DataSets { get; set; }

            [JsonProperty("legend")]
            public Legend Legend { get; set; }

            [JsonProperty("event")]
            public Event Event { get; set; }
        }

        public class DataSets
        {
            [JsonProperty("data")]
            public List<int> Data { get; set; }

            [JsonProperty("post_data")]
            public List<List<PostData>> PostData { get; set; }

            [JsonProperty("backgroundColor")]
            public List<string> BackgroundColor { get; set; }

        }

        public class Legend
        {
            [JsonProperty("unit")]
            public string Unit { get; set; }
        }

        public class Event
        {
            [JsonProperty("change_dt")]
            public ChangeDT ChangeDT { get; set; }
        }

        public class ChangeDT
        {
            [JsonProperty("target")]
            public string Target { get; set; }

            [JsonProperty("api")]
            public List<string> API { get; set; }
        }

        public class PostData
        {
            [JsonProperty("key")]
            public string Key { get; set; }

            [JsonProperty("val")]
            public int Val { get; set; }
        }

    }
}