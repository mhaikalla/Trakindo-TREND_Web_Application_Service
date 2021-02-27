using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableMonthlySubmitterDetailModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

            [JsonProperty("feedback")]
            public FeedBack FeedBack { get; set; }

            [JsonProperty("draw")]
            public int Draw { get; set; }

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

        public class FeedBack
        {
            [JsonProperty("tables")]
            public Tables Tables { get; set; }
        }

        public class Tables
        {
            [JsonProperty("order")]
            public string Order { get; set; }
        }

        public class Data
        {
            [JsonProperty("id")]
            public Id Id { get; set; }

            [JsonProperty("ticket_no")]
            public ValueData TicketNo { get; set; }

            [JsonProperty("category")]
            public ValueData Category { get; set; }

            [JsonProperty("title")]
            public ValueData Title { get; set; }

            [JsonProperty("submitter")]
            public ValueData Submitter { get; set; }

            [JsonProperty("responder")]
            public ValueData Responder { get; set; }

            [JsonProperty("date_created")]
            public ValueData DateCreated { get; set; }

            [JsonProperty("date_closed")]
            public ValueData DateClosed { get; set; }

            [JsonProperty("date_updated")]
            public ValueData DateUpdated { get; set; }

            [JsonProperty("status_tr")]
            public ValueData StatusTR { get; set; }

            [JsonProperty("status_user")]
            public ValueData StatusUser { get; set; }


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