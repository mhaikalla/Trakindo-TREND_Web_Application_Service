using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Models.Dashboard
{
    public class AutoSuggestOverviewModel : Controller
    {
        // GET: AutoSuggestOverviewModel
        public class ResponseJson
        {
            [JsonProperty("status")]
            public status status { get; set; }

            [JsonProperty("data")]
            public List<data> data { get; set; }
        }
        public class status 
        {
            [JsonProperty("code")]
            public int code { get; set; }

            [JsonProperty("message")]
            public String message { get; set; }
        }
        public class data
        {
            [JsonProperty("value")]
            public string value { get; set; }
            [JsonProperty("label")]
            public string label { get; set; }
            [JsonProperty("style")]
            public string style { get; set; }
        }



    }
}