using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableDashboardPSLCostRecoveryModel
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

            [JsonProperty("psl_id")]
            public PSLID PSLNo { get; set; }

            [JsonProperty("PSLDescription")]
            public ValueData Description { get; set; }

            [JsonProperty("model")]
            public Model Model { get; set; }

            [JsonProperty("unit_qty")]
            public UnitQty UnitQty { get; set; }

            [JsonProperty("completed")]
            public Completed Completed { get; set; }

            [JsonProperty("total_amount")]
            public TotalAmount TotalAmount { get; set; }

            [JsonProperty("total_claim")]
            public TotalClaim TotalClaim { get; set; }

            [JsonProperty("settled")]
            public Settled Settled { get; set; }
            
            [JsonProperty("recovery")]
            public Recovery Recovery { get; set; }
        }

        public class Id
        {
            [JsonProperty("row")]
            public int Row { get; set; }
        }

        public class ValueData
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("style")]
            public string Style { get; set; }
        }

        public class PSLID
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }
        public class Model
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class Completed
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class TotalAmount
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class TotalClaim
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class Settled
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class Settlement
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class Recovery
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("val_arr")]
            public List<ValVar> ValVar { get; set; }
        }

        public class UnitQty
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