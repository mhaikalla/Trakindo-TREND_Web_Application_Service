using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class DashboardDetailModel
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
            [JsonProperty("doughnut_gauge")]
            public DoughnutGauge DoughnutGauge { get; set; }
        }

        #region DoughnutGauge   
        public class DoughnutGauge
        {
            [JsonProperty("background_color")]
            public List<string> BackgroundColor { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("gauge_limits")]
            public List<int> GaugeLimit { get; set; }
            
        }
        #endregion
    }
}