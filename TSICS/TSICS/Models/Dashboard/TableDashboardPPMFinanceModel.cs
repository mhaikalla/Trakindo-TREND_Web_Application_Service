using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace TSICS.Models.Dashboard
{
    public class TableDashboardPPMFinanceModel
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

            [JsonProperty("serial_no")]
            public ValueData SerialNo { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("prod_prob_desc")]
            public ValueData ProdProbDesc { get; set; }

            [JsonProperty("comment")]
            public ValueData Comment { get; set; }

            [JsonProperty("service_order")]
            public ValueData ServiceOrder { get; set; }

            [JsonProperty("service_mtr_measr")]
            public ValueData ServiceMtrMeasr { get; set; }

            [JsonProperty("unit_mes")]
            public ValueData UnitMes { get; set; }

            [JsonProperty("sales_office")]
            public ValueData SalesOffice { get; set; }

            [JsonProperty("part_no")]
            public ValueData PartNo { get; set; }

            [JsonProperty("part_desc")]
            public ValueData PartDescription { get; set; }

            [JsonProperty("repair_date")]
            public ValueData RepairDate { get; set; }

            [JsonProperty("currency")]
            public ValueData Currency { get; set; }

            [JsonProperty("total_cost_so")]
            public ValueData TotalCostSO { get; set; }
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