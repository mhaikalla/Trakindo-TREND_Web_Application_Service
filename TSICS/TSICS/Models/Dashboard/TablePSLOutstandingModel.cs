using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TablePSLOutstandingModel
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

            [JsonProperty("area")]
            public ValueData Area { get; set; }

            [JsonProperty("store_name")]
            public ValueData StoreName { get; set; }

            [JsonProperty("psl_no")]
            public ValueData PSLNo { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("serial_no")]
            public ValueData SerialNumber { get; set; }

            [JsonProperty("sr_no")]
            public ValueData SRNo { get; set; }

            [JsonProperty("quot_no")]
            public ValueData QuotNo { get; set; }

            [JsonProperty("so_no")]
            public ValueData SoNo { get; set; }

            [JsonProperty("cat_psl_status")]
            public ValueData CatPslStatus { get; set; }

            [JsonProperty("psl_stat")]
            public ValueData PSLStatus { get; set; }

            [JsonProperty("issue_date")]
            public ValueData IssueDate { get; set; }

            [JsonProperty("termination_date")]
            public ValueData TerminationDate { get; set; }
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