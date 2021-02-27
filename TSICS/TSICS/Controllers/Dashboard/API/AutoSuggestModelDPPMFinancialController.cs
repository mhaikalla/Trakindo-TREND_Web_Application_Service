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
        // GET: AutoSuggestModelDPPMFinancial
        public ActionResult AutoSuggestModelDPPMFinancial()
        {
            var data = new List<string>();
            var listData = PartResponBS.GetListModel();
            foreach (var item in listData)
            {
                data.Add(item);
            }

            var Status = new AutoSuggestModelModel.Status();
            Status.Code = 200;
            Status.Message = "Sukses";

            var responseJson = new AutoSuggestModelModel.ResponseJson();
            responseJson.Status = Status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}