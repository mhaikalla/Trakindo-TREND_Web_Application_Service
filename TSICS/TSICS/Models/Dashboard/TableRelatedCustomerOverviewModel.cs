using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableRelatedCustomerOverviewModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("total_need_to_respond")]
            public int TotalNeedToRespond { get; set; }

            [JsonProperty("draw")]
            public int Draw { get; set; }

            [JsonProperty("recordsTotal")]
            public int RecordsTotal { get; set; }

            [JsonProperty("recordsFiltered")]
            public int RecordsFiltered { get; set; }

            [JsonProperty("data")]
            public List<Data> Data { get; set; }
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
            [JsonProperty("id")]
            public ValueData CustomerId { get; set; }

            [JsonProperty("customer_name")]
            public ValueData CustomerName { get; set; }

            [JsonProperty("location")]
            public ValueData Location { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("count")]
            public ValueData CountOfSN { get; set; }
        }

        public class Id
        {
            [JsonProperty("id")]
            public int Row { get; set; }
        }

        public class ValueData
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("style")]
            public string Style { get; set; }
        }
    }
}