using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Trakindo.Utility.ThirdParty.Google
{
    public class GoogleCaptchaVerfication
    {
        public static string GoogleCaptchaVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
        public static Com.Trakindo.Utility.ThirdParty.Google.CaptchaResponse Verify(string googleCaptchaSecret, string responseToken)
        {
            Com.Trakindo.Utility.ListUtility.Parameters parameters = new ListUtility.Parameters();
            parameters = parameters.Add("secret", googleCaptchaSecret).Add("response", responseToken);
            string json = Com.Trakindo.Utility.HttpUtility.WebClient.Post(GoogleCaptchaVerificationUrl, parameters);
            Com.Trakindo.Utility.ThirdParty.Google.CaptchaResponse response = Com.Trakindo.Utility.Serializer.Json.Deserialize<Com.Trakindo.Utility.ThirdParty.Google.CaptchaResponse>(json);
            return response;
        }
    }
}
