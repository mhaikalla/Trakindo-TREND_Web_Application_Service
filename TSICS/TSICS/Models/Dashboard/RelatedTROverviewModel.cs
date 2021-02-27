using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TSICS.Models.Dashboard
{
    public class RelatedTROverviewModel
    {
        public class ResponseJson
        {
            public header Header { get; set; }
            public body Body { get; set; }
        }
        public class Status
        {
            //[JsonProperty("code")]
            public int Code { get; set; }

            //[JsonProperty("Message")]
            public string Message { get; set; }
        }
        public class header
        {
            //[JsonProperty("id")]
            public string id { get; set; }
            //[JsonProperty("title")]
            public string Title { get; set; }
            //[JsonProperty("desc")]
            public string Description { get; set; }
            //[JsonProperty("status")]
            public string Status { get; set; }
            //[JsonProperty("age")]
            public string Age { get; set; }
        }
        public class body
        {
            //[JsonProperty("main")]
            public ValueData Main { get; set; }

            //[JsonProperty("aside")]
            public ValueData Aside { get; set; }

        }
        public class ValueData
        {
            //[JsonProperty("title")]
            public int Title { get; set; }
            //[JsonProperty("text")]
            public int Text { get; set; }
        }
    }
}