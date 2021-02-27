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
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        // GET: TablePSLUpdateOverview

        [HttpPost]
        public ActionResult TablePSLUpdateOverview(FormCollection fc, int type, string Area, string SalesOffice, string Customer, string Family, string Industry, string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, String Others, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST = false, int download = 0, string[] columns = null)
        {
                var orderByColumn = String.Empty;/*fc["columns[" + fc["order[0][column]"] + "][data]"];*/
                var orderByDir = String.Empty;/*fc["order[0][dir]"]*/;
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
                var RentalVal = Rental;
                if ("?model=1".Contains(RentalVal))
                {
                    RentalVal = "";
                }
                string[] dataFiltered = getListSerialNumberFiltered(type, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily, SplitIndustry, SplitModel, SplitSerialNumber, RepairDateFrom, RepairDateEnd, PurchaseDateFrom, PurchaseDateEnd, DeliveryDateFrom, DeliveryDateEnd, SmuRangeFrom, SmuRangeTo, SplitProduct, SplitPlant, Hid, RentalVal, Others, paramsModel, paramsSerialNumber, btnPOST);

                var listData = new List<TableDashboardPSLCostRecoveryModel.Data>();
                var getData = pslBS.GetDataForTablePSLUpdateOverview(dataFiltered, start, length, download);
               
                foreach (var item in getData.Item1)
                {
                        var listPSLId = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var id = new TableDashboardPSLCostRecoveryModel.Id();
                        id.Row = 0;
                        var area = new TableDashboardPSLCostRecoveryModel.ValueData();
                        area.Value = item.Area;
                        area.Style = "";
                        var varlistPSLId = new TableDashboardPSLCostRecoveryModel.PSLID();
                        var varlistModel = new TableDashboardPSLCostRecoveryModel.Model();
                        var varlistUnitQty = new TableDashboardPSLCostRecoveryModel.UnitQty();
                        var varListTotalClaim = new TableDashboardPSLCostRecoveryModel.TotalClaim();
                        var varlistTotalAmount = new TableDashboardPSLCostRecoveryModel.TotalAmount();
                        var varlistTotaSettled = new TableDashboardPSLCostRecoveryModel.Settled();
                        var varlistCompleted = new TableDashboardPSLCostRecoveryModel.Completed();
                        var varlistRecovery = new TableDashboardPSLCostRecoveryModel.Recovery();
                        var itemData = new TableDashboardPSLCostRecoveryModel.Data();
                        var subsPSLId = item.PSLId.Substring(0, 2);
                        var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                        var subsDate = item.TerminationDate - item.ReleaseDate;
                        var pslNo = new TableDashboardPSLCostRecoveryModel.ValVar();
                        if (item.Validation != null && item.Validation.Contains("Outstanding"))
                        {

                            if (subsPSLId == "PS")
                            {
                                if (convDayToExpired > 0 && convDayToExpired <= 182)
                                {

                                    pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else if (convDayToExpired > 182 && convDayToExpired <= 365)
                                {
                                    pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else
                                {
                                    pslNo.Value = item.PSLId;
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                            }
                            else if (subsPSLId == "PI")
                            {
                                if (item.PslAge > 0 && item.PslAge <= 182)
                                {
                                    pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else if (item.PslAge > 182 && item.PslAge <= 365)
                                {
                                    pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else if (item.PslAge > 365)
                                {
                                    pslNo.Value = "<span class=\"badge badge-danger\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else
                                {
                                    pslNo.Value = item.PSLId;
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                            }
                            else if (subsPSLId != "PI" && subsPSLId == "PS")
                            {
                                if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                                {
                                    pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                                {
                                    pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLId + "</span>";
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                                else
                                {
                                    pslNo.Value = item.PSLId;
                                    pslNo.Divider = true;
                                    listPSLId.Add(pslNo);
                                }
                            }
                        }
                        else
                        {
                            pslNo.Value = item.PSLId;
                            pslNo.Divider = true;
                            listPSLId.Add(pslNo);
                        }
                        var getDataModel = getModel(item.PSLId, item.Area, dataFiltered);
                        var listModel = new List<TableDashboardPSLCostRecoveryModel.ValVar>();

                        var listDataUnitQty = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var listDataTotalClaim = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var listDataCompleted = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var listDataTotalAmount = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var listDataTotalSettled = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var listDataRecovery = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                        var unitQty = new TableDashboardPSLCostRecoveryModel.ValueData();
                        var completed = new TableDashboardPSLCostRecoveryModel.ValueData();
                        var totalSO = new TableDashboardPSLCostRecoveryModel.ValueData();
                        var totalClaim = new TableDashboardPSLCostRecoveryModel.ValueData();
                        var settled = new TableDashboardPSLCostRecoveryModel.ValueData();
                        var Description = new TableDashboardPSLCostRecoveryModel.ValueData();
                        Description.Value =getDesc(item.PSLId,item.Area);
                        Description.Style = "";
                        var recovery = new TableDashboardPSLCostRecoveryModel.ValueData();
                        foreach (var dataModel in getDataModel)
                        {
                            var ModelData = new TableDashboardPSLCostRecoveryModel.ValVar();
                            ModelData.Value = dataModel;
                            ModelData.Divider = true;
                            ModelData.Title = "";
                            listModel.Add(ModelData);
                            var getDataUnitQty = CountUnitQtyPSLOverview(item.Area, item.PSLId, dataModel, dataFiltered);
                            var getDataComplete = CountCompletedDataPSLOverview(item.Area, item.PSLId, dataModel, dataFiltered);
                            var CountCompleted = (Convert.ToDecimal(getDataComplete) / Convert.ToDecimal(getDataUnitQty)) * 100;
                            var resultCompleted = Math.Round(CountCompleted, 2);
                            var getDataTotalClaim = CountTotalClaimPSLOverview(item.Area, item.PSLId, dataModel, dataFiltered);
                            //var getDataTotalSO = SumTotalSO(item.Area, item.PSLId, dataModel);
                            var getDataTotalAmount = SumTotalAmountPSLOverview(item.Area, item.PSLId, dataModel, dataFiltered);
                            var getDataTotalSettled = SumTotalSettledPSLOverview(item.Area, item.PSLId, dataModel, dataFiltered);
                            var resultRecovery = (getDataTotalAmount != 0 && getDataTotalSettled != 0) ? Math.Round(getDataTotalSettled / getDataTotalAmount, 2) : 0;
                            var UnitQty = new TableDashboardPSLCostRecoveryModel.ValVar();
                            UnitQty.Value = getDataUnitQty.ToString();
                            UnitQty.Title = "";
                            UnitQty.Divider = true;
                            listDataUnitQty.Add(UnitQty);
                            var TotalClaim = new TableDashboardPSLCostRecoveryModel.ValVar();
                            TotalClaim.Value = "$ " + getDataTotalClaim.ToString();
                            TotalClaim.Title = "";
                            TotalClaim.Divider = true;
                            listDataTotalClaim.Add(TotalClaim);
                            var Completed = new TableDashboardPSLCostRecoveryModel.ValVar();
                            Completed.Value = resultCompleted.ToString();
                            Completed.Title = "";
                            Completed.Divider = true;
                            listDataCompleted.Add(Completed);
                            var TotalAmount = new TableDashboardPSLCostRecoveryModel.ValVar();
                            TotalAmount.Value = "$ " + getDataTotalAmount.ToString();
                            TotalAmount.Title = "";
                            TotalAmount.Divider = true;
                            listDataTotalAmount.Add(TotalAmount);
                            var TotalSettled = new TableDashboardPSLCostRecoveryModel.ValVar();
                            TotalSettled.Value = "$ " + getDataTotalSettled.ToString();
                            TotalSettled.Title = "";
                            TotalSettled.Divider = true;
                            listDataTotalSettled.Add(TotalSettled);
                            var resultRecoveryFinal = resultRecovery * 100;
                            var Recovery = new TableDashboardPSLCostRecoveryModel.ValVar();
                            Recovery.Value = resultRecoveryFinal.ToString();
                            Recovery.Title = "";
                            Recovery.Divider = true;
                            listDataRecovery.Add(Recovery);

                        }

                        varlistPSLId.Style = "";
                        varlistPSLId.ValVar = listPSLId;

                        varlistModel.Style = "";
                        varlistModel.ValVar = listModel;

                        varlistUnitQty.Style = "";
                        varlistUnitQty.ValVar = listDataUnitQty;

                        varlistCompleted.Style = "";
                        varlistCompleted.ValVar = listDataCompleted;

                        varlistRecovery.Style = "";
                        varlistRecovery.ValVar = listDataRecovery;

                        varlistTotalAmount.Style = "";
                        varlistTotalAmount.ValVar = listDataTotalAmount;

                        varlistTotaSettled.Style = "";
                        varlistTotaSettled.ValVar = listDataTotalSettled;

                        varListTotalClaim.Style = "";
                        varListTotalClaim.ValVar = listDataTotalClaim;

                        itemData.PSLNo = varlistPSLId;
                        itemData.Model = varlistModel;
                        itemData.Description = Description;
                        itemData.Id = id;
                        itemData.Area = area;
                        itemData.UnitQty = varlistUnitQty;
                        itemData.Completed = varlistCompleted;
                        itemData.TotalAmount = varlistTotalAmount;
                        itemData.TotalClaim = varListTotalClaim;
                        itemData.Settled = varlistTotaSettled;
                        itemData.Recovery = varlistRecovery;
                        listData.Add(itemData);

                    }
                
                var status = new TableDashboardPSLCostRecoveryModel.Status();
                status.Code = 200;
                status.Message = "Sukses";

                var responseJson = new TableDashboardPSLCostRecoveryModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = listData;
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item2;
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
          

        }
        public int CountUnitQtyPSLOverview(string area, string pslId, string model, string[] SerialNumber)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model).Select(s => s.SerialNumber).Distinct();
            int count = 0;
            foreach (var item in getData)
            {
                if (SerialNumber.Contains(item))
                {
                    count = count + 1;
                }
            }
            return count;
        }

        public decimal CountTotalClaimPSLOverview(string area, string pslId, string model, string[] SerialNumber)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            decimal totalClaim = 0;
            foreach (var item in getData)
            {
                if (SerialNumber.Contains(item.SerialNumber))
                {
                    totalClaim = totalClaim + item.WarrantyClaimTotal;
                }
            }
            return totalClaim;
        }

        public decimal SumTotalSOPSLOverview(string area, string pslId, string model, string[] serialNumber)
        {
            var getData= _ctx.PSLMaster.Where(w =>  w.PSLId == pslId && w.Area == area && w.Model == model);
            decimal totalCost = 0;
            foreach (var item in getData)
            {
                if (serialNumber.Contains(item.SerialNumber))
                {
                    totalCost = totalCost + item.SoTotalCost;
                }
            }
            return totalCost;

        }

        public decimal SumTotalAmountPSLOverview(string area, string pslId, string model, string[] serialnumber)
        {

            var getData =_ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            decimal totalAmount = 0;
            foreach (var item in getData)
            {
                if (serialnumber.Contains(item.SerialNumber))
                {
                    totalAmount = totalAmount + item.TotalAmount;
                }
            }
            return totalAmount;
        }

        public decimal SumTotalSettledPSLOverview(string area, string pslId, string model, string[] serialnumber)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            decimal totalSettled = 0;
            foreach(var item in getData)
            {
                if (serialnumber.Contains(item.SerialNumber))
                {
                    totalSettled = totalSettled + item.TotalSettled;
                }
            }
            return totalSettled;

        }

        public int CountCompletedDataPSLOverview(string area, string pslId, string model, string[] SerialNumber)
        {
            var getData =_ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model && w.Validation == "Completed").Select(s => s.SerialNumber).Distinct();
            int count = 0;
            foreach(var item in getData)
            {
                if (SerialNumber.Contains(item))
                {
                    count = count + 1;
                }
            }
            return count;
        }

        public List<string> getModel(string pslid, string area, string[] serialNumber)
        {
            List<String> ResultModel = new List<String>();
         
            var getData = _ctx.PSLMaster.Where(item => item.PSLId == pslid && item.Area == area).Select(s => new { Serial_Number = s.SerialNumber , Model = s.Model});
           
            foreach(var item in getData)
            {
                if (serialNumber.Contains(item.Serial_Number))
                {
                    ResultModel.Add(item.Model); 
                }
            }
            return ResultModel.Distinct().ToList();
        }
        public String getDesc(string pslid, string area)
        {
           var desc = _ctx.PSLMaster.Where(item => item.PSLId == pslid && item.Area == area).Select(s => s.Description).FirstOrDefault();
            return String.IsNullOrWhiteSpace(desc) ? "-" : desc;
        }
    }
}