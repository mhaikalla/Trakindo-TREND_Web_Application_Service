using System.Net;

namespace Com.Trakindo.Utility.HttpUtility
{

    public class WebClient
    {
        protected static string ConvertParameter(ListUtility.Parameters parameters)
        {
            HttpParameterListProcessor httpParameterListProcessor = new HttpParameterListProcessor();
            string sParameters = parameters.Convert(httpParameterListProcessor);
            return sParameters;
        }

        public static string Post(string url, ListUtility.Parameters parameters)
        {
            string sParameters = ConvertParameter(parameters);
            string htmlResult;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                htmlResult = wc.UploadString(url, sParameters);
            }

            return htmlResult;
        }

        public static string Post(string url, string json)
        {
            string sParameters = json;
            string htmlResult;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                htmlResult = wc.UploadString(url, sParameters);
            }

            return htmlResult;
        }

        public static string Get(string url, ListUtility.Parameters parameters)
        {
            string sParameters = ConvertParameter(parameters);
            string htmlResult;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                htmlResult = wc.DownloadString(url + "?" + sParameters);
            }

            return htmlResult;
        }
    }
}
