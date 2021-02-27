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
        // GET: TableDashboardCompletionSummary
        [HttpPost]
        public ActionResult TableDashboardCompletionSummary(FormCollection fc, string dateRangeFrom, string dateRangeEnd, string dateRangeFromIssue, string dateRangeEndIssue, string inventory, string rental, string hid, string area, string PSLType, string SalesOffice, string Model, string others, string type, string month, string download = "0")  
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var fcType = (fc["tabs"] != null) ? fc["tabs"] : "";
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
            var splitSalesOffice = new string[] { };
            var splitMode = new string[] { };
            var splitPSLType = new string[] { };
            var splitDateRangeFrom = new string[] { };
            var splitDateRangeEnd = new string[] { };
            var splitDateRangeFromIssue = new string[] { };
            var splitDateRangeEndIssue = new string[] { };
            var splitMonthFrom = 0;
            var splitMonthEnd = 0;
            var splitMonthFromIssue = 0;
            var splitMonthEndIssue = 0;
            var splitYearFrom = 0;
            var splitYearEnd = 0;
            var splitYearFromIssue = 0;
            var splitYearEndIssue = 0;

            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                splitSalesOffice = SalesOffice.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Model))
            {
                splitMode = Model.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PSLType))
            {
                splitPSLType = PSLType.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(dateRangeFrom))
            {
                splitDateRangeFrom = dateRangeFrom.Split('-');
                splitMonthFrom = Convert.ToInt32(splitDateRangeFrom[1]);
                splitYearFrom = Convert.ToInt32(splitDateRangeFrom[2]);

            }
            if (!string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                splitDateRangeEnd = dateRangeEnd.Split('-');
                splitMonthEnd = Convert.ToInt32(splitDateRangeEnd[1]);
                splitYearEnd = Convert.ToInt32(splitDateRangeEnd[2]);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFromIssue))
            {
                splitDateRangeFromIssue = dateRangeFromIssue.Split('-');
                splitMonthFromIssue = Convert.ToInt32(splitDateRangeFromIssue[1]);
                splitYearFromIssue = Convert.ToInt32(splitDateRangeFromIssue[2]);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeEndIssue))
            {
                splitDateRangeEndIssue = dateRangeEndIssue.Split('-');
                splitMonthEndIssue = Convert.ToInt32(splitDateRangeEndIssue[1]);
                splitYearEndIssue = Convert.ToInt32(splitDateRangeEndIssue[2]);
            }

            var listData = new List<TableDashboardPSLCompletionSummary.Data>();
            var getDataTableCompletionPSP = pslBS.GetDataTableCompletionSummary(dateRangeFrom, dateRangeEnd, dateRangeFromIssue, dateRangeEndIssue, splitArea, splitPSLType, splitSalesOffice, splitMode, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, fc["search[value]"], hid, rental, inventory, others, splitMonthFrom, splitMonthEnd, splitMonthFromIssue, splitMonthEndIssue, splitYearFrom, splitYearEnd, splitYearFromIssue, splitYearEndIssue, type, month, fcType);

            if(download == "0")
            {
                Session["searchValueCompletionSummary"] = fc["search[value]"];
                Session["type"] = type;
                Session["month"] = month;
                foreach (var item in getDataTableCompletionPSP.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableDashboardPSLCompletionSummary.Id();
                    id.Row = item.Row;
                    var areavalue = new TableDashboardPSLCompletionSummary.ValueData();
                    areavalue.Value = item.Area;
                    areavalue.Style = "";
                    var pslNo = new TableDashboardPSLCompletionSummary.ValueData();
                    pslNo.Value = item.PSLNo;
                    pslNo.Style = "";
                    var storeName = new TableDashboardPSLCompletionSummary.ValueData();
                    storeName.Value = item.StoreName;
                    storeName.Style = "";
                    var model = new TableDashboardPSLCompletionSummary.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TableDashboardPSLCompletionSummary.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }

                    srNo.Style = "";
                    var quotNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TableDashboardPSLCompletionSummary.ValueData();
                    if (item.PSLStatus == "Outstanding")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-info";
                    }
                    else if (item.PSLStatus == "Open")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-danger";
                    }
                    else if (item.PSLStatus == "Released")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-purple";
                    }
                    else if (item.PSLStatus == "In Progress")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-warning";
                    }
                    else if (item.PSLStatus == "Completed")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-completed";
                    }
                    else if (item.PSLStatus == "Over Limit")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else if (item.PSLStatus == "Ignored")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else if (item.PSLStatus == "Customer Rejection")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else
                    {
                        pslStatus.Value = "-";
                        pslStatus.Style = "";
                    }

                    var catpslstatus = new TableDashboardPSLCompletionSummary.ValueData();
                    if (item.CatPSLStatus == "Open")
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "badge badge-danger";
                    }
                    else if (item.CatPSLStatus == "Complete")
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "badge badge-completed";
                    }
                    else
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "";
                    }
                    var issueDate = new TableDashboardPSLCompletionSummary.ValueData();
                    issueDate.Value = item.ReleaseDateText;
                    issueDate.Style = "";
                    var termDate = new TableDashboardPSLCompletionSummary.ValueData();
                    termDate.Value = item.TerminationDateText;
                    termDate.Style = "";
                    var itemData = new TableDashboardPSLCompletionSummary.Data();
                    itemData.Id = id;
                    itemData.Area = areavalue;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;
                    itemData.CatPslStatus = catpslstatus;
                    itemData.IssueDate = issueDate;
                    itemData.TerminationDate = termDate;

                    listData.Add(itemData);
                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searchValueCompletionSummary"].ToString();
                var getType = Session["type"].ToString();
                var getMonth = Session["month"].ToString();
                getDataTableCompletionPSP = pslBS.GetDataTableCompletionSummary(dateRangeFrom, dateRangeEnd, dateRangeFromIssue, dateRangeEndIssue, splitArea, splitPSLType, splitSalesOffice, splitMode, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, getSearchValue, hid, rental, inventory, others, splitMonthFrom, splitMonthEnd, splitMonthFromIssue, splitMonthEndIssue, splitYearFrom, splitYearEnd, splitYearFromIssue, splitYearEndIssue, getType, getMonth, fcType);
                foreach (var item in getDataTableCompletionPSP)
                {
                    var id = new TableDashboardPSLCompletionSummary.Id();
                    id.Row = item.Row;
                    var areavalue = new TableDashboardPSLCompletionSummary.ValueData();
                    areavalue.Value = item.Area;
                    areavalue.Style = "";
                    var pslNo = new TableDashboardPSLCompletionSummary.ValueData();
                    pslNo.Value = item.PSLNo;
                    pslNo.Style = "";
                    var storeName = new TableDashboardPSLCompletionSummary.ValueData();
                    storeName.Value = item.StoreName;
                    storeName.Style = "";
                    var model = new TableDashboardPSLCompletionSummary.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TableDashboardPSLCompletionSummary.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }

                    srNo.Style = "";
                    var quotNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TableDashboardPSLCompletionSummary.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TableDashboardPSLCompletionSummary.ValueData();
                    if (item.PSLStatus == "Outstanding")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-info";
                    }
                    else if (item.PSLStatus == "Open")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-danger";
                    }
                    else if (item.PSLStatus == "Released")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-purple";
                    }
                    else if (item.PSLStatus == "In Progress")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-warning";
                    }
                    else if (item.PSLStatus == "Completed")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-completed";
                    }
                    else if (item.PSLStatus == "Over Limit")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else if (item.PSLStatus == "Ignored")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else if (item.PSLStatus == "Customer Rejection")
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "badge badge-success";
                    }
                    else
                    {
                        pslStatus.Value = "-";
                        pslStatus.Style = "";
                    }

                    var catpslstatus = new TableDashboardPSLCompletionSummary.ValueData();
                    if (item.CatPSLStatus == "Open")
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "badge badge-danger";
                    }
                    else if (item.CatPSLStatus == "Complete")
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "badge badge-completed";
                    }
                    else
                    {
                        catpslstatus.Value = item.CatPSLStatus;
                        catpslstatus.Style = "";
                    }
                    var issueDate = new TableDashboardPSLCompletionSummary.ValueData();
                    issueDate.Value = item.ReleaseDateText;
                    issueDate.Style = "";
                    var termDate = new TableDashboardPSLCompletionSummary.ValueData();
                    termDate.Value = item.TerminationDateText;
                    termDate.Style = "";
                    var itemData = new TableDashboardPSLCompletionSummary.Data();
                    itemData.Id = id;
                    itemData.Area = areavalue;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;
                    itemData.CatPslStatus = catpslstatus;
                    itemData.IssueDate = issueDate;
                    itemData.TerminationDate = termDate;

                    listData.Add(itemData);
                }

            }
            
            var status = new TableDashboardPSLCompletionSummary.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableDashboardPSLCompletionSummary.ResponseJson();
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