using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableDashboardPSLExecutionModel
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
            public int RecordTotal { get; set; }

            [JsonProperty("recordsFiltered")]
            public int RecordsFilter { get; set; }

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

            [JsonProperty("serial_no")]
            public ValueData SerialNo { get; set; }

            [JsonProperty("area")]
            public ValueData Area { get; set; }

            [JsonProperty("sales_office")]
            public ValueData SalesOffice { get; set; }

            [JsonProperty("customer")]
            public ValueData Customer { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("ls44043")]
            public LS44043 Status { get; set; }
        }

        public class Id
        {
            [JsonProperty("row")]
            public int Row { get; set; }
        }

        public class ValueData
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class LS44043
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class ValVar
        {
            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("divider")]
            public bool Divider { get; set; }
        }
    }
}