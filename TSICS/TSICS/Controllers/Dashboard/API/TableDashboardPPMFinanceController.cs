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
        // GET: TableDashboardPPMFinance
        [HttpPost]
        public ActionResult TableDashboardPPMFinance(FormCollection fc, string hid, string rental, string inventory, string others, string dateRangeFrom, string dateRangeEnd, string PartNumber, string PartDescription, string ModelDashboard, string PrefixSN, string sn, string model, string download="0")
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var searchSerialNo = Convert.ToBoolean(fc["columns[0][searchable]"]);
            var searchModel = Convert.ToBoolean(fc["columns[1][searchable]"]);
            var searchProdProbDesc = Convert.ToBoolean(fc["columns[2][searchable]"]);
            var searchComment = Convert.ToBoolean(fc["columns[3][searchable]"]); 
            var searchServiceOrder = Convert.ToBoolean(fc["columns[4][searchable]"]);
            var searchServiceMtrMeasr = Convert.ToBoolean(fc["columns[5][searchable]"]);
            var searchUnitMes = Convert.ToBoolean(fc["columns[6][prod_prob_desc]"]);
            var searchSalesOffice = Convert.ToBoolean(fc["columns[7][searchable]"]);
            var searchPartNo = Convert.ToBoolean(fc["columns[8][searchable]"]);
            var searchPartDesc = Convert.ToBoolean(fc["columns[9][searchable]"]);
            var searchRepairDate = Convert.ToBoolean(fc["columns[10][prod_prob_desc]"]);
            var searchCurrency = Convert.ToBoolean(fc["columns[11][searchable]"]);
            var searchTotalCostSO = Convert.ToBoolean(fc["columns[12][searchable]"]);
            var selectColumn = Convert.ToInt32(fc["order[0][column]"]);
            var orderByColumn = fc["order[0][dir]"];
            var valueSearch = fc["search[value]"];
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

            if (!string.IsNullOrWhiteSpace(ModelDashboard))
            {
                splitModel = ModelDashboard.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PrefixSN))
            {
                splitPrefixSN = PrefixSN.Split(',');
            }

            var getData = PartResponBS.GetDataTableDPPMFinance(dateRangeFrom, dateRangeEnd,valueSearch, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, sn, model, hid, rental, inventory, others);
            var listData = new List<TableDashboardPPMFinanceModel.Data>();
            if(download == "0")
            {
                Session["searchValuePPMFinance"] = valueSearch;
                Session["SNChart"] = sn;
                Session["ModelChart"] = model;
                foreach (var item in getData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableDashboardPPMFinanceModel.Id();
                    id.Row = item.Row;
                    var serialNo = new TableDashboardPPMFinanceModel.ValueData();
                    serialNo.Value = item.SerialNumber;
                    serialNo.Style = "";
                    var modelDashboard = new TableDashboardPPMFinanceModel.ValueData();
                    modelDashboard.Value = item.Model;
                    modelDashboard.Style = "";
                    var productProblem = new TableDashboardPPMFinanceModel.ValueData();
                    productProblem.Value = item.ProductProblemDescription;
                    productProblem.Style = "";
                    var comment = new TableDashboardPPMFinanceModel.ValueData();
                    comment.Value = item.Comment;
                    comment.Style = "";
                    var serviceOrder = new TableDashboardPPMFinanceModel.ValueData();
                    serviceOrder.Value = item.ServiceOrder;
                    serviceOrder.Style = "";
                    var smu = new TableDashboardPPMFinanceModel.ValueData();
                    smu.Value = item.ServiceMeterMeasurement.ToString();
                    smu.Style = "";
                    var unitMes = new TableDashboardPPMFinanceModel.ValueData();
                    unitMes.Value = item.UnitMes;
                    unitMes.Style = "";
                    var salesOffice = new TableDashboardPPMFinanceModel.ValueData();
                    salesOffice.Value = item.SalesOffice;
                    salesOffice.Style = "";
                    var partNo = new TableDashboardPPMFinanceModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TableDashboardPPMFinanceModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var repairDate = new TableDashboardPPMFinanceModel.ValueData();
                    repairDate.Value = item.RepairDate;
                    repairDate.Style = "";
                    var currency = new TableDashboardPPMFinanceModel.ValueData();
                    currency.Value = item.Currency;
                    currency.Style = "";
                    var totalSoCost = new TableDashboardPPMFinanceModel.ValueData();
                    totalSoCost.Value = item.TotalSoCost.ToString("C");
                    totalSoCost.Style = "";
                    var itemData = new TableDashboardPPMFinanceModel.Data();
                    itemData.Id = id;
                    itemData.SerialNo = serialNo;
                    itemData.Model = modelDashboard;
                    itemData.ProdProbDesc = productProblem;
                    itemData.Comment = comment;
                    itemData.ServiceOrder = serviceOrder;
                    itemData.ServiceMtrMeasr = smu;
                    itemData.UnitMes = unitMes;
                    itemData.SalesOffice = salesOffice;
                    itemData.PartNo = partNo;
                    itemData.PartDescription = partDesc;
                    itemData.RepairDate = repairDate;
                    itemData.Currency = currency;
                    itemData.TotalCostSO = totalSoCost;
                    listData.Add(itemData);
                }
            }
            if(download == "1")
            {
                var getSearchValue = Session["searchValuePPMFinance"].ToString();
                var getModelChart = Session["ModelChart"].ToString();
                var getSNChart = Session["SNChart"].ToString();
                getData = PartResponBS.GetDataTableDPPMFinance(dateRangeFrom, dateRangeEnd, getSearchValue, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, getSNChart, getModelChart, hid, rental, inventory, others);
                foreach (var item in getData)
                {
                    var id = new TableDashboardPPMFinanceModel.Id();
                    id.Row = item.Row;
                    var serialNo = new TableDashboardPPMFinanceModel.ValueData();
                    serialNo.Value = item.SerialNumber;
                    serialNo.Style = "";
                    var modelDashboard = new TableDashboardPPMFinanceModel.ValueData();
                    modelDashboard.Value = item.Model;
                    modelDashboard.Style = "";
                    var productProblem = new TableDashboardPPMFinanceModel.ValueData();
                    productProblem.Value = item.ProductProblemDescription;
                    productProblem.Style = "";
                    var comment = new TableDashboardPPMFinanceModel.ValueData();
                    comment.Value = item.Comment;
                    comment.Style = "";
                    var serviceOrder = new TableDashboardPPMFinanceModel.ValueData();
                    serviceOrder.Value = item.ServiceOrder;
                    serviceOrder.Style = "";
                    var smu = new TableDashboardPPMFinanceModel.ValueData();
                    smu.Value = item.ServiceMeterMeasurement.ToString();
                    smu.Style = "";
                    var unitMes = new TableDashboardPPMFinanceModel.ValueData();
                    unitMes.Value = item.UnitMes;
                    unitMes.Style = "";
                    var salesOffice = new TableDashboardPPMFinanceModel.ValueData();
                    salesOffice.Value = item.SalesOffice;
                    salesOffice.Style = "";
                    var partNo = new TableDashboardPPMFinanceModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TableDashboardPPMFinanceModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var repairDate = new TableDashboardPPMFinanceModel.ValueData();
                    repairDate.Value = item.RepairDate;
                    repairDate.Style = "";
                    var currency = new TableDashboardPPMFinanceModel.ValueData();
                    currency.Value = item.Currency;
                    currency.Style = "";
                    var totalSoCost = new TableDashboardPPMFinanceModel.ValueData();
                    totalSoCost.Value = item.TotalSoCost.ToString("C");
                    totalSoCost.Style = "";
                    var itemData = new TableDashboardPPMFinanceModel.Data();
                    itemData.Id = id;
                    itemData.SerialNo = serialNo;
                    itemData.Model = modelDashboard;
                    itemData.ProdProbDesc = productProblem;
                    itemData.Comment = comment;
                    itemData.ServiceOrder = serviceOrder;
                    itemData.ServiceMtrMeasr = smu;
                    itemData.UnitMes = unitMes;
                    itemData.SalesOffice = salesOffice;
                    itemData.PartNo = partNo;
                    itemData.PartDescription = partDesc;
                    itemData.RepairDate = repairDate;
                    itemData.Currency = currency;
                    itemData.TotalCostSO = totalSoCost;
                    listData.Add(itemData);
                }
            }
            

            var status = new TableDashboardPPMFinanceModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableDashboardPPMFinanceModel.ResponseJson();
            responseJson.Status = status;
            responseJson.RecordsFiltered = getData.Count();
            responseJson.RecordsTotal = getData.Count();
            responseJson.Data = listData;
            responseJson.Draw = draw;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}