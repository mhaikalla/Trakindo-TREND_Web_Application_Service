using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility.ThirdParty.Google
{
    public class CaptchaResponse
    {
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("challenge_ts")]
        public DateTime Challenge_TS { get; set; }

        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }

    }
}
