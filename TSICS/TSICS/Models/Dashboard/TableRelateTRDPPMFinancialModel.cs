using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace TSICS.Models.Dashboard
{
    public class TableRelateTRDPPMFinancialModel
    {
        public class ResponseJson
        {
            [JsonProperty("status")]
            public Status Status { get; set; }

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

            [JsonProperty("ticketno")]
            public ValueData TicketNo { get; set; }

            [JsonProperty("industry")]
            public ValueData Industry { get; set; }

            [JsonProperty("family")]
            public ValueData Family { get; set; }

            [JsonProperty("model")]
            public ValueData Model { get; set; }

            [JsonProperty("category")]
            public ValueData Category { get; set; }

            [JsonProperty("title")]
            public ValueData Title { get; set; }

            [JsonProperty("desc")]
            public ValueData Desc { get; set; }

            [JsonProperty("resolution")]
            public ValueData Resolution { get; set; }

            [JsonProperty("date_created")]
            public ValueData DateCreated { get; set; }

            [JsonProperty("date_closed")]
            public ValueData DateClosed { get; set; }

            [JsonProperty("status_ticket")]
            public ValueData StatusTicket { get; set; }
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