using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: TablePPMPotentialByCost
        private readonly PartResponsibleCostImpactAnalysisBusinessService PartResponBS = Factory.Create<PartResponsibleCostImpactAnalysisBusinessService>("PartResponsibleCostImpactAnalysis", ClassType.clsTypeBusinessService);

        [HttpPost]
        public ActionResult TablePPMPotentialByCost(FormCollection fc, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN, string download = "0")
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
            var listData = new List<TablePPMPotentialByCostModel.Data>();
            var getListData = PartResponBS.GetDataForTablePPMByCost(dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, fc["search[value]"], Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);

            if(download == "0")
            {
                Session["searchValuePPMPotentialByCost"] = fc["search[value]"];
                Session["ColumnTable"] = Convert.ToInt32(fc["order[0][column]"]);
                Session["Order"] = fc["order[0][dir]"];
                foreach (var item in getListData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TablePPMPotentialByCostModel.Id();
                    id.Row = item.Row;
                    var prodProbDesc = new TablePPMPotentialByCostModel.ValueData();
                    prodProbDesc.Value = item.ProductProblemDescription;
                    prodProbDesc.Style = "";
                    var partNo = new TablePPMPotentialByCostModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TablePPMPotentialByCostModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var groupNo = new TablePPMPotentialByCostModel.ValueData();
                    groupNo.Value = item.GroupNo.ToString();
                    groupNo.Style = "";
                    var groupDesc = new TablePPMPotentialByCostModel.ValueData();
                    groupDesc.Value = item.GroupDesc;
                    groupDesc.Style = "";
                    //decimal getTotalSo = GetTotalSoCost(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, fc["search[value]"], Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);
                    decimal getTotalClaim = GetTotalSoClaim(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, fc["search[value]"], Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);
                    decimal getTotalSettled = GetTotalSoSettled(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, fc["search[value]"], Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], hid, rental, inventory, others);
                    var totalSoCost = new TablePPMPotentialByCostModel.ValueData();
                    totalSoCost.Value = item.TotalSoCost.ToString("C");
                    totalSoCost.Style = "";
                    var soClaim = new TablePPMPotentialByCostModel.ValueData();
                    soClaim.Value = getTotalClaim.ToString("C");
                    soClaim.Style = "";
                    var soSettled = new TablePPMPotentialByCostModel.ValueData();
                    soSettled.Value = getTotalSettled.ToString("C");
                    soSettled.Style = "";
                    var currency = new TablePPMPotentialByCostModel.ValueData();
                    currency.Value = item.Currency;
                    currency.Style = "";
                    var itemData = new TablePPMPotentialByCostModel.Data();
                    itemData.Id = id;
                    itemData.PartNo = partNo;
                    itemData.PartDesc = partDesc;
                    itemData.GroupNo = groupNo;
                    itemData.GroupDesc = groupDesc;
                    itemData.ProbDesc = prodProbDesc;
                    itemData.SoCost = totalSoCost;
                    itemData.SoClaim = soClaim;
                    itemData.SoSettled = soSettled;
                    itemData.Currency = currency;
                    listData.Add(itemData);
                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searchValuePPMPotentialByCost"].ToString();
                var getColumn = Convert.ToInt32(Session["ColumnTable"]);
                var order = Session["Order"].ToString();
                getListData = PartResponBS.GetDataForTablePPMByCost(dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, getSearchValue, getColumn, order, hid, rental, inventory, others);
                foreach (var item in getListData)
                {
                    var id = new TablePPMPotentialByCostModel.Id();
                    id.Row = item.Row;
                    var prodProbDesc = new TablePPMPotentialByCostModel.ValueData();
                    prodProbDesc.Value = item.ProductProblemDescription;
                    prodProbDesc.Style = "";
                    var partNo = new TablePPMPotentialByCostModel.ValueData();
                    partNo.Value = item.PartNo;
                    partNo.Style = "";
                    var partDesc = new TablePPMPotentialByCostModel.ValueData();
                    partDesc.Value = item.PartDescription;
                    partDesc.Style = "";
                    var groupNo = new TablePPMPotentialByCostModel.ValueData();
                    groupNo.Value = item.GroupNo.ToString();
                    groupNo.Style = "";
                    var groupDesc = new TablePPMPotentialByCostModel.ValueData();
                    groupDesc.Value = item.GroupDesc;
                    groupDesc.Style = "";
                    //decimal getTotalSo = GetTotalSoCost(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, getSearchValue, getColumn, order, hid, rental, inventory, others);
                    decimal getTotalClaim = GetTotalSoClaim(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, getSearchValue, getColumn, order, hid, rental, inventory, others);
                    decimal getTotalSettled = GetTotalSoSettled(item.PartNo, dateRangeFrom, dateRangeEnd, splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, searchSerialNo, searchModel, searchProductProblemDescription, searchComment, searchServiceOrder, searchServiceMeter, searchUnitMes, searchSalesOffice, searchPartNo, searchPartDesc, searchRepairDate, searchCurrency, searchTotalCostSO, getSearchValue, getColumn, order, hid, rental, inventory, others);
                    var totalSoCost = new TablePPMPotentialByCostModel.ValueData();
                    totalSoCost.Value = item.TotalSoCost.ToString("C");
                    totalSoCost.Style = "";
                    var soClaim = new TablePPMPotentialByCostModel.ValueData();
                    soClaim.Value = getTotalClaim.ToString("C");
                    soClaim.Style = "";
                    var soSettled = new TablePPMPotentialByCostModel.ValueData();
                    soSettled.Value = getTotalSettled.ToString("C");
                    soSettled.Style = "";
                    var currency = new TablePPMPotentialByCostModel.ValueData();
                    currency.Value = item.Currency;
                    currency.Style = "";
                    var itemData = new TablePPMPotentialByCostModel.Data();
                    itemData.Id = id;
                    itemData.PartNo = partNo;
                    itemData.PartDesc = partDesc;
                    itemData.GroupNo = groupNo;
                    itemData.GroupDesc = groupDesc;
                    itemData.ProbDesc = prodProbDesc;
                    itemData.SoCost = totalSoCost;
                    itemData.SoClaim = soClaim;
                    itemData.SoSettled = soSettled;
                    itemData.Currency = currency;
                    listData.Add(itemData);
                }
            }
            
            var Draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;

            var status = new TablePPMPotentialByCostModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TablePPMPotentialByCostModel.ResponseJson();
            responseJson.Draw = Draw;
            responseJson.RecordsFilter = getListData.Count();
            responseJson.RecordTotal = getListData.Count();
            responseJson.Status = status;
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }

        public decimal GetTotalSoCost(string partNo, string dateRangeFrom, string dateRangeEnd, string[] splitPartNumber, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, bool searchSerialNo, bool searchModel, bool searchProductProblemDescription, bool searchComment, bool searchServiceOrder, bool searchServiceMeter, bool searchUnitMes, bool searchSalesOffice, bool searchPartNo, bool searchPartDesc, bool searchRepairDate, bool searchCurrency, bool searchTotalCostSO, string valueSearch, int column, string order, string hid, string rental, string inventory, string others)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }
            if (hid == "on")
            {
                data = data.Where(w => w.HIDTaskId != null);
            }
            if (rental == "on")
            {
                data = data.Where(w => w.RentStatus != null);
            }
            if (inventory == "on")
            {
                data = data.Where(w => w.Plant != null);
            }
            if (others == "on")
            {
                data = data.Where(w => w.HIDTaskId != null && w.RentStatus != null && w.Plant != null);
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => partNo.Contains(w.PartCausingFailure));
            }
            if(splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if(splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Any(a => w.SerialNumber.Contains(a)));
            }
            result = data.Select(s => s.TotalSO).Sum();
            return result;
        }

        public decimal GetTotalSoClaim(string partNo, string dateRangeFrom, string dateRangeEnd, string[] splitPartNumber, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, bool searchSerialNo, bool searchModel, bool searchProductProblemDescription, bool searchComment, bool searchServiceOrder, bool searchServiceMeter, bool searchUnitMes, bool searchSalesOffice, bool searchPartNo, bool searchPartDesc, bool searchRepairDate, bool searchCurrency, bool searchTotalCostSO, string valueSearch, int column, string order, string hid, string rental, string inventory, string others)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }
            if (hid == "on")
            {
                data = data.Where(w => w.HIDTaskId != null);
            }
            if (rental == "on")
            {
                data = data.Where(w => w.RentStatus != null);
            }
            if (inventory == "on")
            {
                data = data.Where(w => w.Plant != null);
            }
            if (others == "on")
            {
                data = data.Where(w => w.HIDTaskId != null && w.RentStatus != null && w.Plant != null);
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => partNo.Contains(w.PartCausingFailure));
            }
            if (splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Any(a => w.SerialNumber.Contains(a)));
            }
            result = data.Select(s => s.TotalClaim).Sum();
            return result;
        }

        public decimal GetTotalSoSettled(string partNo, string dateRangeFrom, string dateRangeEnd, string[] splitPartNumber, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, bool searchSerialNo, bool searchModel, bool searchProductProblemDescription, bool searchComment, bool searchServiceOrder, bool searchServiceMeter, bool searchUnitMes, bool searchSalesOffice, bool searchPartNo, bool searchPartDesc, bool searchRepairDate, bool searchCurrency, bool searchTotalCostSO, string valueSearch, int column, string order, string hid, string rental, string inventory, string others)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }
            if (hid == "on")
            {
                data = data.Where(w => w.HIDTaskId != null);
            }
            if (rental == "on")
            {
                data = data.Where(w => w.RentStatus != null);
            }
            if (inventory == "on")
            {
                data = data.Where(w => w.Plant != null);
            }
            if (others == "on")
            {
                data = data.Where(w => w.HIDTaskId != null && w.RentStatus != null && w.Plant != null);
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => partNo.Contains(w.PartCausingFailure));
            }
            if (splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Any(a => w.SerialNumber.Contains(a)));
            }
            result = data.Select(s => s.TotalSettled).Sum();
            return result;
        }
    }
}