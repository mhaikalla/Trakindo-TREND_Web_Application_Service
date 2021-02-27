using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: TableAffectedUnitDPPMFinancial
        public ActionResult TableAffectedUnitDPPMFinancial(string cost, string freq, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN)
        {
            var splitPartNumber = new string[] { };
            var splitPartDescription = new string[] { };
            var splitModel = new string[] { };
            var splitPrefixSN = new string[] { };


            if (!string.IsNullOrWhiteSpace(PartNumber))
            {
                splitPartNumber = PartNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PartDescription))
            {
                splitPartDescription = PartDescription.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(Model))
            {
                splitModel = Model.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PrefixSN))
            {
                splitPrefixSN = PrefixSN.Split(',');
            }
            var getData = PartResponBS.TableAffectedUnit(cost, freq, dateRangeFrom, dateRangeEnd, hid, inventory, rental,others, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN);
            var listModel = new List<string>();
            var listData = new List<TableAffectedUnitDPPMFinancialModel.Data>();
            foreach(var item in getData)
            {
                var data = new TableAffectedUnitDPPMFinancialModel.Data();
                data.Title = item.Model;
                data.List = item.SerialNumber;
                listData.Add(data);
            }
            

            var status = new TableAffectedUnitDPPMFinancialModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableAffectedUnitDPPMFinancialModel.ResponseJson();
            responseJson.Data = listData;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}