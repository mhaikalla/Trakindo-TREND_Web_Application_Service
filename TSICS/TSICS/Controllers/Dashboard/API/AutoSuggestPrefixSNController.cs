using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: AutoSuggestPrefixSN
        public ActionResult AutoSuggestPrefixSN()
        {
            var data = new List<string>();
            var listData = PartResponBS.GetListPrefixSN();
            var distinctData = listData.Distinct();
            foreach (var item in distinctData)
            {
                data.Add(item);
            };

            var Status = new AutoSuggestPrefixSNModel.Status();
            Status.Code = 200;
            Status.Message = "Sukses";

            var responseJson = new AutoSuggestPrefixSNModel.ResponseJson();
            responseJson.Status = Status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}