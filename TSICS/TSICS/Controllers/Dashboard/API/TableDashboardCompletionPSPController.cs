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
        // GET: TableDashboardCompletionPSP
        [HttpPost]
        public ActionResult TableDashboardCompletionPSP(FormCollection fc, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string Area, string storeName,string others, string month, string download = "0")
        {  
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var searchArea = Convert.ToBoolean(fc["columns[0][searchable]"]);
            var searchPSLNo = Convert.ToBoolean(fc["columns[1][searchable]"]);
            var searchStoreName = Convert.ToBoolean(fc["columns[2][searchable]"]);
            var searchModel = Convert.ToBoolean(fc["columns[3][searchable]"]);
            var searchSerialNumber = Convert.ToBoolean(fc["columns[4][searchable]"]);
            var searchSRNo = Convert.ToBoolean(fc["columns[5][searchable]"]);
            var searchQuotNo = Convert.ToBoolean(fc["columns[6][searchable]"]);
            var searchSoNo = Convert.ToBoolean(fc["columns[7][searchable]"]);
            var searchSAPPSLStatus = Convert.ToBoolean(fc["columns[8][searchable]"]);
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(Area))
            {
                splitArea = Area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            var listData = new List<TableDashboardPSLCompletionPSPModel.Data>();
            var getDataTableCompletionPSP = pslBS.getDataTableCompletionPSP(dateRangeFrom, dateRangeEnd, splitArea, splitStoreName, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, fc["search[value]"], hid, rental, inventory, others, month);
            if(download == "0")
            {
                Session["searchValueCompletionPSP"] = fc["search[value]"];
                foreach (var item in getDataTableCompletionPSP.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableDashboardPSLCompletionPSPModel.Id();
                    id.Row = item.Row;
                    var area = new TableDashboardPSLCompletionPSPModel.ValueData();
                    area.Value = item.Area;
                    area.Style = "";
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDate - item.ReleaseDate;
                    var pslNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (item.Validation.Contains("Outstanding"))
                    {

                        if (subsPSLId == "PS")
                        {
                            if (convDayToExpired > 0 && convDayToExpired <= 182)
                            {

                                pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                            else if (convDayToExpired > 182 && convDayToExpired <= 365)
                            {
                                pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                        }
                        else if (subsPSLId == "PI")
                        {
                            if (item.PslAge > 0 && item.PslAge <= 182)
                            {
                                pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                            else if (item.PslAge > 182 && item.PslAge <= 365)
                            {
                                pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                            else if (item.PslAge > 365)
                            {
                                pslNo.Value = "<span class=\"badge badge-danger\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                        }
                        else if (subsPSLId != "PI" && subsPSLId == "PS")
                        {
                            if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                            {
                                pslNo.Value = "<span class=\"badge badge-success\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                            else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                            {
                                pslNo.Value = "<span class=\"badge badge-warning\">" + item.PSLNo + "</span>";
                                pslNo.Style = "";
                            }
                        }
                    }
                    else
                    {
                        pslNo.Value = item.PSLNo;
                        pslNo.Style = "";
                    }
                    var storeNameData = new TableDashboardPSLCompletionPSPModel.ValueData();
                    storeNameData.Value = item.StoreName;
                    storeNameData.Style = "";
                    var model = new TableDashboardPSLCompletionPSPModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TableDashboardPSLCompletionPSPModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.QuotNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }

                    srNo.Style = "";
                    var quotNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }
                    quotNo.Style = "";
                    var soNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TableDashboardPSLCompletionPSPModel.ValueData();
                    pslStatus.Value = item.PSLStatus;
                    pslStatus.Style = "badge badge-warning";
                    var itemData = new TableDashboardPSLCompletionPSPModel.Data();
                    itemData.Id = id;
                    itemData.Area = area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeNameData;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;

                    listData.Add(itemData);
                }
            }
            if(download == "1")
            {
                var getSearchValue = Session["searchValueCompletionPSP"].ToString();
                getDataTableCompletionPSP = pslBS.getDataTableCompletionPSP(dateRangeFrom, dateRangeEnd, splitArea, splitStoreName, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, getSearchValue, hid, rental, inventory, others, month);
                foreach (var item in getDataTableCompletionPSP)
                {
                    var id = new TableDashboardPSLCompletionPSPModel.Id();
                    id.Row = item.Row;
                    var area = new TableDashboardPSLCompletionPSPModel.ValueData();
                    area.Value = item.Area;
                    area.Style = "";
                    var pslNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    pslNo.Value = item.PSLNo;
                    pslNo.Style = "";
                    var storeNameData = new TableDashboardPSLCompletionPSPModel.ValueData();
                    storeNameData.Value = item.StoreName;
                    storeNameData.Style = "";
                    var model = new TableDashboardPSLCompletionPSPModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TableDashboardPSLCompletionPSPModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.QuotNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }

                    srNo.Style = "";
                    var quotNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }
                    quotNo.Style = "";
                    var soNo = new TableDashboardPSLCompletionPSPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TableDashboardPSLCompletionPSPModel.ValueData();
                    pslStatus.Value = item.PSLStatus;
                    pslStatus.Style = "badge badge-warning";
                    var itemData = new TableDashboardPSLCompletionPSPModel.Data();
                    itemData.Id = id;
                    itemData.Area = area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeNameData;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;

                    listData.Add(itemData);
                }
            }
            
            var status = new TableDashboardPSLCompletionPSPModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableDashboardPSLCompletionPSPModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Draw = draw;
            responseJson.TotalNeedToRespond = getDataTableCompletionPSP.Count();
            responseJson.RecordsFiltered = getDataTableCompletionPSP.Count();
            responseJson.RecordsTotal = getDataTableCompletionPSP.Count();
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}