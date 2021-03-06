﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TablePPMPotentialByCostModel
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

            [JsonProperty("part_no")]
            public ValueData PartNo { get; set; }

            [JsonProperty("part_desc")]
            public ValueData PartDesc { get; set; }

            [JsonProperty("group_no")]
            public ValueData GroupNo { get; set; }

            [JsonProperty("group_desc")]
            public ValueData GroupDesc { get; set; }

            [JsonProperty("prob_desc")]
            public ValueData ProbDesc { get; set; }

            [JsonProperty("so_cost")]
            public ValueData SoCost { get; set; }

            [JsonProperty("so_claim")]
            public ValueData SoClaim { get; set; }

            [JsonProperty("so_settled")]
            public ValueData SoSettled { get; set; }

            [JsonProperty("currency")]
            public ValueData Currency { get; set; }
        }

        public class Id
        {
            [JsonProperty("row")]
            public string Row { get; set; }
        }

        public class ValueData
        {
            [JsonProperty("style")]
            public string Style { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}