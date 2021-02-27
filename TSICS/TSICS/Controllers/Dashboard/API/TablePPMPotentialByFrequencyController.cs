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
        // GET: TablePPMPotentialByFrequency
        [HttpPost]
        public ActionResult TablePPMPotentialByFrequency(FormCollection fc, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN, string download = "0")
        {
            var searchSerialNo = Convert.ToBoolean(fc["columns[0][searchable]"]);
            var searchModel = Convert.ToBoolean(fc["columns[1][searchable]"]);
            var searchProductProblemDescription = Convert.ToBoolean(fc["columns[2][searchable]"]);
            var searchComment = Convert.ToBoolean(fc["columns[3][searchable]"]);
            var searchServiceOrder = Convert.ToBoolean(fc["columns[4][searchable]"]);
            var searchServiceMeter = Convert.ToBoolean(fc["columns[5][searchable]"]);
            var searchUnitMes = Convert.ToBoolean(fc["columns[6][searchable]"]);
            var searchSalesOffice = Convert.ToBoolean(fc["columns[7][searchable]"]);
            var searchPartNo = Convert.ToBoolean(fc["columns[8][searchable]"]);
            var searchPartDesc = Convert.ToBoolean(fc["columns[9][searchable]"]);
            var searchRepairDate = Convert.ToBoolean(fc["columns[10][searchable]"]);
            var searchCurrency = Convert.ToBoolean(fc["columns[11][searchable]"]);
            var searchTotalCostSO = Convert.ToBoolean(fc["columns[12][searchable]"]);
            var Draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
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
            var getListData = PartResponBS.GetDataForTablePPMByFrequency(dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, fc["search[value]"], Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);
            var listData = new List<TablePPMPotentialByFrequencyModel.Data>();

            if(download == "0")
            {
                Session["searchValuePPMPotentialByFrecuency"] = fc["search[value]"];
                foreach (var item in getListData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TablePPMPotentialByFrequencyModel.Id();
                    id.Row = item.Row;
                    var partNo = new TablePPMPotentialByFrequencyModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var groupNo = new TablePPMPotentialByFrequencyModel.ValueDataGroupNo();
                    groupNo.Value = item.GroupNo;
                    groupNo.Style = "";
                    var groupDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    groupDesc.Value = item.GroupDesc;
                    groupDesc.Style = "";
                    var prodProbDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    prodProbDesc.Value = item.ProductProblemDescription;
                    prodProbDesc.Style = "";
                    var countOFRepair = new TablePPMPotentialByFrequencyModel.ValueData();
                    countOFRepair.Value = item.CountOfRepair.ToString();
                    countOFRepair.Style = "";
                    var itemData = new TablePPMPotentialByFrequencyModel.Data();
                    itemData.Id = id;
                    itemData.PartNo = partNo;
                    itemData.PartDesc = partDesc;
                    itemData.ProdProbDesc = prodProbDesc;
                    itemData.GroupNo = groupNo;
                    itemData.GroupDesc = groupDesc;
                    itemData.CountOfRepair = countOFRepair;
                    listData.Add(itemData);
                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searchValuePPMPotentialByFrecuency"].ToString();
                getListData = PartResponBS.GetDataForTablePPMByFrequency(dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, getSearchValue, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);
                foreach (var item in getListData)
                {
                    var id = new TablePPMPotentialByFrequencyModel.Id();
                    id.Row = item.Row;
                    var partNo = new TablePPMPotentialByFrequencyModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var groupNo = new TablePPMPotentialByFrequencyModel.ValueDataGroupNo();
                    groupNo.Value = item.GroupNo;
                    groupNo.Style = "";
                    var groupDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    groupDesc.Value = item.GroupDesc;
                    groupDesc.Style = "";
                    var prodProbDesc = new TablePPMPotentialByFrequencyModel.ValueData();
                    prodProbDesc.Value = item.ProductProblemDescription;
                    prodProbDesc.Style = "";
                    var countOFRepair = new TablePPMPotentialByFrequencyModel.ValueData();
                    countOFRepair.Value = item.CountOfRepair.ToString();
                    countOFRepair.Style = "";
                    var itemData = new TablePPMPotentialByFrequencyModel.Data();
                    itemData.Id = id;
                    itemData.PartNo = partNo;
                    itemData.PartDesc = partDesc;
                    itemData.ProdProbDesc = prodProbDesc;
                    itemData.GroupNo = groupNo;
                    itemData.GroupDesc = groupDesc;
                    itemData.CountOfRepair = countOFRepair;
                    listData.Add(itemData);
                }
            }
            
            var status = new TablePPMPotentialByFrequencyModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TablePPMPotentialByFrequencyModel.ResponseJson();
            responseJson.Draw = Draw;
            responseJson.Data = listData;
            responseJson.RecordsFilter = getListData.Count();
            responseJson.RecordTotal = getListData.Count();
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}