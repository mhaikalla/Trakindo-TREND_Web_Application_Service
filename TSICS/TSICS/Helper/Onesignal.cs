using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;

namespace TSICS.Helper
{
    public class Onesignal
    {
        private static readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        public static void PushNotif(string content, List<string> playerid, string title, int trid, string ticktnum, int ticketCategory, string desc)
        {
            try
            {
                var request = WebRequest.Create("https://onesignal.com/api/v1/notifications") as HttpWebRequest;
                if (request != null)
                {
                    request.KeepAlive = true;
                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Headers.Add("authorization", "Basic MGY1NTFmZWUtMjBiMi00YzgxLTlkZWUtNmU5ZmVhNzcxMWU4");
                    var serializer = new JavaScriptSerializer();


                    var obj = new
                    {
                        app_id = "47a3b935-0907-4e4d-8df6-415397f614b7",
                        contents = new { en = Regex.Replace(content, "<.*?>", String.Empty) },
                        include_player_ids = playerid,
                        headings = new { en = Regex.Replace(title, "<.*?>", String.Empty) },
                        data = new
                        {
                            to = "detail",
                            technicalRequestId = trid,
                            technicalRequestNo = ticktnum,
                            technicalRequestCategory = ticketCategory,
                            technicalRequestDescription = desc,
                            title
                        }
                    };
                    var param = serializer.Serialize(obj);
                    byte[] byteArray = Encoding.UTF8.GetBytes(param);

                    string responseContent = null;
                    try
                    {
                        using (var writer = request.GetRequestStream())
                        {
                            writer.Write(byteArray, 0, byteArray.Length);
                        }

                        using (var response = request.GetResponse() as HttpWebResponse)
                        {
                            if (response != null)
                                // ReSharper disable once AssignNullToNotNullAttribute
                                using (var reader = new StreamReader(response.GetResponseStream()))
                                {
                                    responseContent = reader.ReadToEnd();
                                }
                        }
                    }
                    catch (WebException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        _logErrorBService.WriteLog("OneSignalPushResponse", MethodBase.GetCurrentMethod().Name, ex.Message);
                        //System.Diagnostics.Debug.WriteLine(new StreamReader(ex.Response.GetResponseStream()).ReadToEnd());
                    }

                    System.Diagnostics.Debug.WriteLine(responseContent);
                }
            }
            catch (Exception e)
            {
                _logErrorBService.WriteLog("SetUpOnesignal", MethodBase.GetCurrentMethod().Name, e.ToString());
            }
        }
    }
}