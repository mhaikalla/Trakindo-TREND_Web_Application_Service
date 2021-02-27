using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Models.Dashboard;
using Newtonsoft.Json;
using PagedList;
using TSICS.Helper;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        private readonly DPPMBusinessService dppmBS = Factory.Create<DPPMBusinessService>("DPPM", ClassType.clsTypeBusinessService);
        // GET: AutoSuggestPrefixSNDPPMFinancial
        public ActionResult AutoSuggestPrefixSNDPPMFinancial()
        {
            var data = new List<string>();
            var listData = PartResponBS.GetListPrefixSN();
            var result = listData.Distinct();
            foreach (var item in result)
            {
                data.Add(item);
            }

            var Status = new AutoSuggestPartNumberModel.Status();
            Status.Code = 200;
            Status.Message = "Sukses";

            var responseJson = new AutoSuggestPartNumberModel.ResponseJson();
            responseJson.Status = Status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}