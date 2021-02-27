using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System;
using System.Data;
using System.Text;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        [HttpPost]
        public ActionResult getDataPotentialByCostOverview(FormCollection fc,  int type, string Area, string SalesOffice, string Customer, string Family, string Industry, string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Others, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST = false, int download = 0, string[] columns = null)
        {
            var orderByColumn = "so_cost";
            var orderByDir = "desc";
            string[] SplitArea = new string[] { };
            string[] SplitCustomer = new string[] { };
            string[] SplitFamily = new string[] { };
            string[] SplitSalesOffice = new string[] { };
            string[] SplitModel = new string[] { };
            string[] SplitSerialNumber = new string[] { };
            string[] SplitProduct = new string[] { };
            string[] SplitPlant = new string[] { };
            string[] SplitIndustry = new string[] { };
            if (!string.IsNullOrWhiteSpace(Area))
            {
                SplitArea = Area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Customer))
            {
                SplitCustomer = Customer.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Model))
            {
                SplitModel = Model.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                SplitSerialNumber = SerialNumber.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Family))
            {
                SplitFamily = Family.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Industry))
            {
                SplitIndustry = Industry.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Plant))
            {
                SplitPlant = Plant.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Product))
            {
                SplitProduct = Product.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                SplitSalesOffice = SalesOffice.Split(',');
            }
            _partResponsibleCostAnalysisBs.CheckIfDecimalTypeisNull();
            var RentalVal = Rental.Replace("?model=1", "");
            string[] dataFiltered = getListSerialNumberFiltered(type, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitModel, SplitSerialNumber, RepairDateFrom,RepairDateEnd,PurchaseDateFrom,PurchaseDateEnd,DeliveryDateFrom,DeliveryDateEnd, SmuRangeFrom, SmuRangeTo, SplitProduct, SplitPlant, Hid, RentalVal, Others,paramsModel, paramsSerialNumber, btnPOST);
            var getData = _partResponsibleCostAnalysisBs.GetDataForTablePPMByCostOverview(dataFiltered,orderByDir, orderByColumn, download, columns);
            var listData = new List<TablePPMPotentialByCostModel.Data>();
            foreach (var item in getData.Item1)
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
                var totalSoCost = new TablePPMPotentialByCostModel.ValueData();
                totalSoCost.Value = item.TotalSoCost.ToString("C");
                totalSoCost.Style = "";
                var soClaim = new TablePPMPotentialByCostModel.ValueData();
                soClaim.Value = item.SoClaim.ToString("C");
                soClaim.Style = "";
                var soSettled = new TablePPMPotentialByCostModel.ValueData();
                soSettled.Value = item.SOSettled.ToString("C");
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
            var status = new TablePPMPotentialByCostModel.Status();
            status.Code = 200;
            status.Message = "Sukses";
            var Draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var responseJson = new TablePPMPotentialByCostModel.ResponseJson();
            responseJson.Draw = Draw;
            responseJson.RecordsFilter = getData.Item2;
            responseJson.RecordTotal = getData.Item2;
            responseJson.Status = status;
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
        [HttpPost]
        public ActionResult getDataPotentialByFrequencyOverview(FormCollection fc, int type, string Area, string SalesOffice, string Customer, string Family, string Industry,string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Others, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST = false, int download = 0, string[] columns = null)
        {
            var orderByColumn = "count_of_repair";
            var orderByDir = "desc";
            var start = Convert.ToInt32(fc["start"]);
            var length = Convert.ToInt32(fc["length"]);
            string[] SplitArea = new string[] { };
            string[] SplitCustomer = new string[] { };
            string[] SplitFamily = new string[] { };
            string[] SplitSalesOffice = new string[] { };
            string[] SplitModel = new string[] { };
            string[] SplitSerialNumber = new string[] { };
            string[] SplitProduct = new string[] { };
            string[] SplitPlant = new string[] { };
            string[] SplitIndustry = new string[] { };
            if (!string.IsNullOrWhiteSpace(Area))
            {
                SplitArea = Area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Customer))
            {
                SplitCustomer = Customer.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Model))
            {
                SplitModel = Model.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                SplitSerialNumber = SerialNumber.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Family))
            {
                SplitFamily = Family.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Industry))
            {
                SplitIndustry = Industry.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Plant))
            {
                SplitPlant = Plant.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Product))
            {
                SplitProduct = Product.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                SplitSalesOffice = SalesOffice.Split(',');
            }
            var RentalVal = Rental.Replace("?model=1", "");
            string[] dataFiltered = getListSerialNumberFiltered(type, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily, SplitIndustry, SplitModel, SplitSerialNumber, RepairDateFrom, RepairDateEnd, PurchaseDateFrom, PurchaseDateEnd, DeliveryDateFrom, DeliveryDateEnd, SmuRangeFrom, SmuRangeTo, SplitProduct, SplitPlant, Hid, Rental, Others, paramsModel, paramsSerialNumber, btnPOST);

            var getData = _partResponsibleCostAnalysisBs.GetDataForTablePPMByFrequencyOverview(dataFiltered, orderByDir, orderByColumn, download, columns);
            var listData = new List<TablePPMPotentialByFrequencyModel.Data>();
            foreach (var item in getData)
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
            var status = new TablePPMPotentialByFrequencyModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TablePPMPotentialByFrequencyModel.ResponseJson();
            var Draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            responseJson.Draw = Draw;
            responseJson.Data = listData;
            responseJson.RecordsFilter = getData.Count();
            responseJson.RecordTotal = getData.Count();
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }

        public decimal GetTotalSoCost(string partNo)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
          
            result = data.Select(s => s.TotalSO).Sum();
            return result;
        }

        public decimal GetTotalSoClaim(string partNo)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
         
            result = data.Select(s => s.TotalClaim).Sum();
            return result;
        }

        public decimal GetTotalSoSettled(string partNo)
        {
            decimal result;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure.Contains(partNo) && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
         
            result = data.Select(s => s.TotalSettled).Sum();
            return result;
        }
    }
}