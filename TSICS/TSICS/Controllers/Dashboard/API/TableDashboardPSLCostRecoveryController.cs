using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System;
using Com.Trakindo.TSICS.Data.Context;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        // GET: TableDashboardPSLCostRecovery
        [HttpPost]
        public ActionResult TableDashboardPSLCostRecovery(FormCollection fc, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string Area, string pslType, string pslStatus, string model, string salesOffice, string others, string download = "0")
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var searchArea = Convert.ToBoolean(fc["columns[0][searchable]"]);
            var searchPSLNo = Convert.ToBoolean(fc["columns[1][searchable]"]);
            var searchModel = Convert.ToBoolean(fc["columns[2][searchable]"]);
            var searchUnitQty = Convert.ToBoolean(fc["columns[3][searchable]"]);
            var searchCompleted = Convert.ToBoolean(fc["columns[4][searchable]"]);
            var searchTotalSO = Convert.ToBoolean(fc["columns[5][searchable]"]);
            var searchTotalClaim = Convert.ToBoolean(fc["columns[6][searchable]"]);
            var searchSettled = Convert.ToBoolean(fc["columns[7][searchable]"]);
            var searchSettlement = Convert.ToBoolean(fc["columns[8][searchable]"]);
            var searchRecovery = Convert.ToBoolean(fc["columns[9][searchable]"]);
            int orderColumn = Convert.ToInt32(fc["order[0][column]"]);
            var orderBy = fc["order[0][dir]"];
            var splitArea = new string[] { };
            var splitModeel = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLType = new string[] { };
            var splitPSLStatus = new string[] { };


            if (!string.IsNullOrWhiteSpace(Area))
            {
                splitArea = Area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                splitModeel = model.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(salesOffice))
            {
                splitSalesOffice = salesOffice.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(pslStatus))
            {
                splitPSLStatus = pslStatus.Split(',');
            }
            var searchValue = (fc["search[value]"] != null) ? fc["search[value]"] : "";
            var skip = (Convert.ToInt32(fc["start"]) == 0) ? 0 : Convert.ToInt32(fc["start"]);
            var take = (Convert.ToInt32(fc["length"]) == 0) ? 0 : Convert.ToInt32(fc["length"]);
            //var sessionValueSerchCurrent = (Session["searchValuePSLCostRecovery"] != null) ? Session["searchValuePSLCostRecovery"].ToString() : "";
            //var sessionValueSearchLast = (Session["searchValuePSLCostRecoveryLast"] != null) ? Session["searchValuePSLCostRecoveryLast"].ToString() : "";
            //var searchValues = (sessionValueSerchCurrent != "" && ((sessionValueSearchLast == "") || (sessionValueSearchLast != "" && sessionValueSearchLast == sessionValueSerchCurrent))) ? sessionValueSerchCurrent : searchValue;
            //Session["searchValuePSLCostRecoveryLast"] = sessionValueSerchCurrent;
            //Session["searchValuePSLCostRecovery"] = searchValue;
            var searchData = "";
            if(download == "0")
            {
                Session["searchValuePSLCostRecovery"] = searchValue;
                searchData = searchValue;
            }
            else
            {
                searchData = Session["searchValuePSLCostRecovery"].ToString();
            }
            var getData = pslBS.GetDataTablePSLCostRecovery(dateRangeFrom, dateRangeEnd, splitArea, splitModeel, splitSalesOffice, splitPSLType, splitPSLStatus, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchModel, searchUnitQty, searchCompleted, searchTotalSO, searchTotalClaim, searchSettled, searchSettlement, searchRecovery, searchData, hid, rental, inventory, others, download, skip, take);
            var countGetDataPage = 0;
            var listData = new List<TableDashboardPSLCostRecoveryModel.Data>();
            foreach (var item in getData)
            {
                countGetDataPage = item.CountData;
                //var getUnitQty = CountUnitQty(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //var getCountCompleted = CountCompleted(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //var countTotalSettled = CountAllTotalSettled(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //var countTotalAmount = CountAllTotalAmount(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //var countTotalSO = CountAllTotalSo(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //var countTotalClaim = CountAllTotalClaim(item.PSLId, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitArea, splitModeel, splitSalesOffice);
                //decimal convertUnitQty = Convert.ToDecimal(getUnitQty);
                //decimal convertTotalCompleted = Convert.ToDecimal(getCountCompleted);
                //decimal resultCompleted = Math.Round((convertTotalCompleted / convertUnitQty) * 100, 2);
                //decimal resultRecovery = (countTotalSettled != 0 && countTotalAmount != 0) ? Math.Round(countTotalSettled / countTotalAmount, 2) : 0;
                var listPSLId = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var id = new TableDashboardPSLCostRecoveryModel.Id();
                id.Row = item.Row;
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
                //var getDataPSLId = getPSLId(item.Area, dateRangeFrom, dateRangeEnd, splitModeel, splitSalesOffice, splitPSLType, splitPSLStatus, hid, rental, inventory, others);
                var subsPSLId = item.PSLId.Substring(0, 2);
                var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                var subsDate = item.TerminationDate - item.ReleaseDate;
                var pslNo = new TableDashboardPSLCostRecoveryModel.ValVar();
                if (item.Validation.Contains("Outstanding"))
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
                var getDataModel = getModel(item.PSLId, item.Area, dateRangeFrom, dateRangeEnd, splitModeel, splitSalesOffice, splitPSLType, splitPSLStatus, hid, rental, inventory, others);
                //getDataModel =  getDataModel.Where(w => w.ToLower().Contains(searchValue.ToLower())).ToList();
                var listModel = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataUnitQty = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataTotalClaim = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataCompleted = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                //var listDataTotalSO = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataTotalAmount = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataTotalSettled = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var listDataRecovery = new List<TableDashboardPSLCostRecoveryModel.ValVar>();
                var unitQty = new TableDashboardPSLCostRecoveryModel.ValueData();
                var completed = new TableDashboardPSLCostRecoveryModel.ValueData();
                var totalSO = new TableDashboardPSLCostRecoveryModel.ValueData();
                var totalClaim = new TableDashboardPSLCostRecoveryModel.ValueData();
                var settled = new TableDashboardPSLCostRecoveryModel.ValueData();
                //decimal totalRecovery = resultRecovery * 100;
                var recovery = new TableDashboardPSLCostRecoveryModel.ValueData();
                foreach (var dataModel in getDataModel)
                {
                    var Model = new TableDashboardPSLCostRecoveryModel.ValVar();
                    Model.Value = dataModel;
                    Model.Divider = true;
                    Model.Title = "";
                    listModel.Add(Model);
                    var getDataUnitQty = CountUnitQty(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
                    var getDataComplete = CountCompletedData(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
                    var CountCompleted = (Convert.ToDecimal(getDataComplete) / Convert.ToDecimal(getDataUnitQty)) * 100;
                    var resultCompleted = Math.Round(CountCompleted, 2);
                    var getDataTotalClaim = CountTotalClaim(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
                    //var getDataTotalSO = SumTotalSO(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
                    var getDataTotalAmount = SumTotalAmount(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
                    var getDataTotalSettled = SumTotalSettled(item.Area, item.PSLId, dataModel, inventory, rental, hid, others, splitPSLType, splitPSLStatus, splitSalesOffice);
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
            responseJson.Draw = draw;
            responseJson.TotalNeedToRespond = countGetDataPage;
            responseJson.RecordsFiltered = countGetDataPage;
            responseJson.RecordsTotal = countGetDataPage;
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }

         public List<string> getModel(string pslid, string area, string dateFrom, string dateEnd, string[] splitModel, string[] splitSalesOffice, string[] splitPslType, string[] splitPslStatus, string hid, string rental, string inventory, string others)
        {
            var getData = (from item in _ctx.PSLMaster
                           where item.Area == area && item.PSLId == pslid && item.TerminationDate.Value.Year == DateTime.Now.Year
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.TerminationDate.Value >= convertToDateTimeFrom && item.TerminationDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.TerminationDate.Value == convertToDateTimeFrom
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.RentStatus != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.RentStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus == "" || item.RentStatus == "" || item.Plant == ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (splitModel.Count() > 0 && splitModel != null)
            {
                getData = (from item in getData
                           where splitModel.Contains(item.Model)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }
            if (splitSalesOffice.Count() > 0 && splitSalesOffice != null)
            {
                getData = (from item in getData
                           where splitSalesOffice.Contains(item.SalesOffice)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }

            if (splitPslType.Count() > 0 && splitPslType != null)
            {
                getData = (from item in getData
                           where splitPslType.Contains(item.PSLType)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }
            if (splitPslStatus.Count() > 0 && splitPslStatus != null)
            {
                getData = (from item in getData
                           where splitPslStatus.Contains(item.SapPSLStatus)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus
                           });
            }
            var groupData = (from item in getData
                             group item by item.Model into model
                             select new
                             {
                                 Model = model.FirstOrDefault().Model
                             });
            var listItem = new List<string>();
            foreach (var item in groupData)
            {
                listItem.Add(item.Model);
            }
            return listItem;

        }

        public List<GetPSLIdFromAreaCostRecovery> getPSLId(string area, string dateFrom, string dateEnd, string[] splitModel, string[] splitSalesOffice, string[] splitPslType, string[] splitPslStatus, string hid, string rental, string inventory, string others)
        {
            var getData = (from item in _ctx.PSLMaster
                           where item.Area == area && item.TerminationDate.Value.Year == DateTime.Now.Year
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.TerminationDate.Value >= convertToDateTimeFrom && item.TerminationDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.TerminationDate.Value == convertToDateTimeFrom
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" 
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.RentStatus != "" 
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDStatus != "" || item.RentStatus != "" || item.Plant != ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDStatus == "" || item.RentStatus == "" || item.Plant == ""
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (splitModel.Count() > 0 && splitModel != null)
            {
                getData = (from item in getData
                           where splitModel.Contains(item.Model)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }
            if (splitSalesOffice.Count() > 0 && splitSalesOffice != null)
            {
                getData = (from item in getData
                           where splitSalesOffice.Contains(item.SalesOffice)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            if (splitPslType.Count() > 0 && splitPslType != null)
            {
                getData = (from item in getData
                           where splitPslType.Contains(item.PSLType)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }
            if (splitPslStatus.Count() > 0 && splitPslStatus != null)
            {
                getData = (from item in getData
                           where splitPslStatus.Contains(item.SapPSLStatus)
                           select new
                           {
                               PSLId = item.PSLId,
                               TerminationDate = item.TerminationDate,
                               HIDStatus = item.HIDStatus,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               Model = item.Model,
                               SalesOffice = item.SalesOffice,
                               PSLType = item.PSLType,
                               SapPSLStatus = item.SapPSLStatus,
                               LetterDate = item.LetterDate,
                               DaysToExpired = item.DaysToExpired,
                               Validation = item.Validation
                           });
            }

            var groupData = (from item in getData
                             group item by item.PSLId into pslid
                             select new
                             {
                                 PSLId = pslid.FirstOrDefault().PSLId,
                                 TerminationDate = pslid.FirstOrDefault().TerminationDate,
                                 ReleaseDate = pslid.FirstOrDefault().LetterDate,
                                 DayToExpired = pslid.FirstOrDefault().DaysToExpired,
                                 Validation = pslid.FirstOrDefault().Validation
                             });
            var listItem = new List<GetPSLIdFromAreaCostRecovery>();
            foreach (var item in groupData)
            {
                var data = new GetPSLIdFromAreaCostRecovery();
                //var getDataModel = getModel(area, item.PSLId, dateFrom, dateEnd, splitArea, splitModel, splitSalesOffice, splitPslType, splitPslStatus, hid, rental, inventory, others);
                data.PSLId = item.PSLId;
                //data.Model = getDataModel;
                data.TerminationDate = item.TerminationDate;
                data.LetterDate = item.ReleaseDate;
                data.DaysToExpired = item.DayToExpired;
                data.Validation = item.Validation;
                listItem.Add(data);
            }
            return listItem;
        }


        public int CountUnitQty(string area,string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.SerialNumber).Distinct().Count();
            return result;
        }

        public decimal CountTotalClaim(string area, string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.WarrantyClaimTotal).Sum();
            return result;
        }

        public decimal SumTotalSO(string area, string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.SoTotalCost).Sum();
            return result;
        }

        public decimal SumTotalAmount(string area, string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.TotalAmount).Sum();
            return result;
        }

        public decimal SumTotalSettled(string area, string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model);
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.TotalSettled).Sum();
            return result;
        }

        public int CountCompletedData(string area, string pslId, string model, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] salesoffice)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId == pslId && w.Area == area && w.Model == model && w.Validation == "Completed");
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "");
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != "" || w.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDStatus != "" || w.RentStatus != "" || w.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDStatus == "" || w.RentStatus == "" || w.Plant == "");
            }
            if (salesoffice.Count() > 0)
            {
                getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
            }
            if (psltype.Count() > 0)
            {
                getData = getData.Where(w => psltype.Contains(w.PSLType));
            }
            if (pslstatus.Count() > 0)
            {
                getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
            }
            var result = getData.Select(s => s.SerialNumber).Distinct().Count();
            return result;
        }


        //public int CountCompleted(string pslId, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] area, string[] model, string[] salesoffice)
        //{
        //    var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslId) && w.SapPSLStatus.Contains("Completed"));
        //    if (inventory == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "");
        //    }
        //    if (rental == "on")
        //    {
        //        getData = getData.Where(w => w.RentStatus != null && w.RentStatus != "");
        //    }
        //    if (hid == "on")
        //    {
        //        getData = getData.Where(w => w.HIDStatus != null && w.HIDStatus != "");
        //    }
        //    if (others == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "" && w.RentStatus != null && w.RentStatus != "" && w.HIDStatus != null && w.HIDStatus != "");
        //    }

        //    if (area.Count() > 0)
        //    {
        //        getData = getData.Where(w => area.Contains(w.Area));
        //    }
        //    if (model.Count() > 0)
        //    {
        //        getData = getData.Where(w => model.Contains(w.Model));
        //    }
        //    if (salesoffice.Count() > 0)
        //    {
        //        getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
        //    }
        //    if (psltype.Count() > 0)
        //    {
        //        getData = getData.Where(w => psltype.Contains(w.PSLType));
        //    }
        //    if (pslstatus.Count() > 0)
        //    {
        //        getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
        //    }
        //    var result = getData.Count();
        //    return result;
        //}

        //public decimal CountAllTotalSo(string pslId, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] area, string[] model, string[] salesoffice)
        //{
        //    var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslId));
        //    if (inventory == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "");
        //    }
        //    if (rental == "on")
        //    {
        //        getData = getData.Where(w => w.RentStatus != null && w.RentStatus != "");
        //    }
        //    if (hid == "on")
        //    {
        //        getData = getData.Where(w => w.HIDStatus != null && w.HIDStatus != "");
        //    }
        //    if (others == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "" && w.RentStatus != null && w.RentStatus != "" && w.HIDStatus != null && w.HIDStatus != "");
        //    }

        //    if (area.Count() > 0)
        //    {
        //        getData = getData.Where(w => area.Contains(w.Area));
        //    }
        //    if (model.Count() > 0)
        //    {
        //        getData = getData.Where(w => model.Contains(w.Model));
        //    }
        //    if (salesoffice.Count() > 0)
        //    {
        //        getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
        //    }
        //    if (psltype.Count() > 0)
        //    {
        //        getData = getData.Where(w => psltype.Contains(w.PSLType));
        //    }
        //    if (pslstatus.Count() > 0)
        //    {
        //        getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
        //    }
        //    var result = getData.Select(s => s.SoTotalCost).Sum();
        //    return result;
        //}

        //public decimal CountAllTotalClaim(string pslId, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] area, string[] model, string[] salesoffice)
        //{
        //    var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslId));
        //    if (inventory == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "");
        //    }
        //    if (rental == "on")
        //    {
        //        getData = getData.Where(w => w.RentStatus != null && w.RentStatus != "");
        //    }
        //    if (hid == "on")
        //    {
        //        getData = getData.Where(w => w.HIDStatus != null && w.HIDStatus != "");
        //    }
        //    if (others == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "" && w.RentStatus != null && w.RentStatus != "" && w.HIDStatus != null && w.HIDStatus != "");
        //    }

        //    if (area.Count() > 0)
        //    {
        //        getData = getData.Where(w => area.Contains(w.Area));
        //    }
        //    if (model.Count() > 0)
        //    {
        //        getData = getData.Where(w => model.Contains(w.Model));
        //    }
        //    if (salesoffice.Count() > 0)
        //    {
        //        getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
        //    }
        //    if (psltype.Count() > 0)
        //    {
        //        getData = getData.Where(w => psltype.Contains(w.PSLType));
        //    }
        //    if (pslstatus.Count() > 0)
        //    {
        //        getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
        //    }
        //    var result = getData.Select(s => s.WarrantyClaimTotal).Sum();
        //    return result;
        //}
        //public decimal CountAllTotalSettled(string pslId, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] area, string[] model, string[] salesoffice)
        //{
        //    var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslId));
        //    if (inventory == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "");
        //    }
        //    if (rental == "on")
        //    {
        //        getData = getData.Where(w => w.RentStatus != null && w.RentStatus != "");
        //    }
        //    if (hid == "on")
        //    {
        //        getData = getData.Where(w => w.HIDStatus != null && w.HIDStatus != "");
        //    }
        //    if (others == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "" && w.RentStatus != null && w.RentStatus != "" && w.HIDStatus != null && w.HIDStatus != "");
        //    }

        //    if (area.Count() > 0)
        //    {
        //        getData = getData.Where(w => area.Contains(w.Area));
        //    }
        //    if (model.Count() > 0)
        //    {
        //        getData = getData.Where(w => model.Contains(w.Model));
        //    }
        //    if (salesoffice.Count() > 0)
        //    {
        //        getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
        //    }
        //    if (psltype.Count() > 0)
        //    {
        //        getData = getData.Where(w => psltype.Contains(w.PSLType));
        //    }
        //    if (pslstatus.Count() > 0)
        //    {
        //        getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
        //    }
        //    var result = getData.Select(s => s.TotalSettled).Sum();
        //    return result;
        //}
        //public decimal CountAllTotalAmount(string pslId, string inventory, string rental, string hid, string others, string[] psltype, string[] pslstatus, string[] area, string[] model, string[] salesoffice)
        //{
        //    var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslId));
        //    if (inventory == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "");
        //    }
        //    if (rental == "on")
        //    {
        //        getData = getData.Where(w => w.RentStatus != null && w.RentStatus != "");
        //    }
        //    if (hid == "on")
        //    {
        //        getData = getData.Where(w => w.HIDStatus != null && w.HIDStatus != "");
        //    }
        //    if (others == "on")
        //    {
        //        getData = getData.Where(w => w.Plant != null && w.Plant != "" && w.RentStatus != null && w.RentStatus != "" && w.HIDStatus != null && w.HIDStatus != "");
        //    }

        //    if (area.Count() > 0)
        //    {
        //        getData = getData.Where(w => area.Contains(w.Area));
        //    }
        //    if (model.Count() > 0)
        //    {
        //        getData = getData.Where(w => model.Contains(w.Model));
        //    }
        //    if (salesoffice.Count() > 0)
        //    {
        //        getData = getData.Where(w => salesoffice.Contains(w.SalesOffice));
        //    }
        //    if (psltype.Count() > 0)
        //    {
        //        getData = getData.Where(w => psltype.Contains(w.PSLType));
        //    }
        //    if (pslstatus.Count() > 0)
        //    {
        //        getData = getData.Where(w => pslstatus.Contains(w.SapPSLStatus));
        //    }
        //    var result = getData.Select(s => s.TotalAmount).Sum();
        //    return result;
        //}
    }
}