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
        // GET: TableTotalInventoryDPPMFinancial
        [HttpPost]
        public ActionResult TableTotalInventoryDPPMFinancial(FormCollection fc)
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;

            var listData = new List<TableTotalInventoryDPPMFinancialModel.Data>();
            var data = new TableTotalInventoryDPPMFinancialModel.Data();
            var area = new TableTotalInventoryDPPMFinancialModel.ValueData();
            area.Value = "Tes";
            area.Style = "";
            var salesOffice = new TableTotalInventoryDPPMFinancialModel.ValueData();
            salesOffice.Value = "SalesOffice1";
            salesOffice.Style = "";
            var quantity = new TableTotalInventoryDPPMFinancialModel.ValueData();
            quantity.Value = "Quantity";
            quantity.Style = "";
            data.Area = area;
            data.SalesOffice = salesOffice;
            data.Quantity = quantity;
            listData.Add(data);

            var status = new TableTotalInventoryDPPMFinancialModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableTotalInventoryDPPMFinancialModel.ResponseJson();
            responseJson.Data = listData;
            responseJson.Draw = draw;
            responseJson.RecordsFiltered = 1;
            responseJson.RecordsTotal = 1;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}