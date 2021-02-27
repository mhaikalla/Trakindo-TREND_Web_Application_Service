using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableMEPOverviewModel
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

            [JsonProperty("serial_number")]
            public ValueData SerialNumber { get; set; }

            [JsonProperty("smu")]
            public ValueData SMU { get; set; }

            [JsonProperty("smu_update")]
            public ValueData SMU_Update { get; set; }

            [JsonProperty("last_serviced")]
            public ValueData LastServiced { get; set; }

            [JsonProperty("delivery_date")]
            public ValueData DeliveryDate { get; set; }

            [JsonProperty("purchase_date")]
            public ValueData Purchase_Date { get; set; }

            [JsonProperty("industry")]
            public ValueData Industry { get; set; }
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