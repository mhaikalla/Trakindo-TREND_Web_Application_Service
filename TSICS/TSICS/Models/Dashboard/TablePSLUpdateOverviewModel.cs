using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TablePSLUpdateOverviewModel
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
            public Id Id { get; set; }

            [JsonProperty("psl_id")]
            public ValueData PSLId { get; set; }

            [JsonProperty("area")]
            public ValueData Area { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("count_of_model")]
            public ValueData CountOfModel { get; set; }

            [JsonProperty("unit_qty")]
            public ValueData UnitQty { get; set; }

            [JsonProperty("completed")]
            public ValueData Completed { get; set; }

            [JsonProperty("total_so")]
            public ValueData TotalSO { get; set; }

            [JsonProperty("total_claim")]
            public ValueData TotalClaim { get; set; }

            [JsonProperty("settled")]
            public ValueData Settled { get; set; }

            [JsonProperty("settlement")]
            public ValueData Settlement { get; set; }
            
            [JsonProperty("recovery")]
            public ValueData Recovery { get; set; }
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