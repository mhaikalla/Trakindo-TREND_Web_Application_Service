using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableDPPMUpdateModel
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

            [JsonProperty("columnDefs")]
            public List<ColumnDefs> ColumnDefs { get; set; }

            [JsonProperty("data")]
            public List<Data> Data { get; set; }
        }

        public class ColumnDefs
        {
            [JsonProperty("visible")]
            public bool Visible { get; set; }
            [JsonProperty("targets")]
            public List<int> Targets { get; set; }
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

            [JsonProperty("dppm_no")]
            public ValueData DPPMNo { get; set; }

            [JsonProperty("title")]
            public ValueData Title { get; set; }

            [JsonProperty("desc")]
            public ValueData Desc { get; set; }

            [JsonProperty("status")]
            public ValueData Status { get; set; }

            [JsonProperty("dealer_contact")]
            public ValueData DealerContact { get; set; }

            [JsonProperty("cat_rep")]
            public ValueData CatReps { get; set; }

            [JsonProperty("ica")]
            public ValueData ICA { get; set; }

            [JsonProperty("pca")]
            public ValueData PCA { get; set; }

            [JsonProperty("date_created")]
            public ValueData DateCreated { get; set; }

            [JsonProperty("date_last_update")]
            public ValueData DateLastUpdate { get; set; }
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