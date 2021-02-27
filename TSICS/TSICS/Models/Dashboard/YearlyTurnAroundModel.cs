using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class YearlyTurnAroundModel
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
            [JsonProperty("horizontal_bar")]
            public HorizontalBar HorizontalBar { get; set; }
        }

        #region HorizontalBar
        public class HorizontalBar
        {
            [JsonProperty("data")]
            public List<int> Data { get; set; }

            [JsonProperty("legend")]
            public string Legend { get; set; }

            [JsonProperty("bg_color")]
            public string BgColor { get; set; }

            [JsonProperty("y_axis")]
            public YAxisHorizontalBar YAxis { get; set; }
        }

        public class YAxisHorizontalBar
        {
            [JsonProperty("line")]
            public Line Line { get; set; }
        }

        public class Line
        {
            [JsonProperty("labels")]
            public List<string> Label { get; set; }
        }
        #endregion
    }
}