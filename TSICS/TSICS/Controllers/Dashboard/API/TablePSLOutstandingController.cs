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
        // GET: TablePSLOutstanding
        [HttpPost]
        public ActionResult TablePSLOutstanding(FormCollection fc, string PipSafety, string PipPriority, string PspProactive, string PspAfterFailure, string ReleaseDateFrom, string ReleaseDateEnd, string TerminationDateFrom, string TerminationDateEnd, string Inventory, string Rental, string HID, string Areass, string SalesOffice, string PSLStatus, string PSLNo, string AgeIndicator, string SerialNumber, string Model, string Priority, string area, string type, string others, string download="0")
        {
            //Session["type"] = type;
            //Session["area"] = area;
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

            #region Split String
            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLStatus = new string[] { };
            var splitPSLNo = new string[] { };
            var splitAgeIndicator = new string[] { };
            var splitSerialNumber = new string[] { };
            var splitModel = new string[] { };
            var splitPriority = new string[] { };

            if (!string.IsNullOrWhiteSpace(Areass))
            {
                splitArea = Areass.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                splitSalesOffice = SalesOffice.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PSLStatus))
            {
                splitPSLStatus = PSLStatus.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PSLNo))
            {
                splitPSLNo = PSLNo.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(AgeIndicator))
            {
                splitAgeIndicator = AgeIndicator.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                splitSerialNumber = SerialNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(Model))
            {
                splitModel = Model.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(Priority))
            {
                splitPriority = Priority.Split(',');
            }
            #endregion
            var listData = new List<TablePSLOutstandingModel.Data>();
            var getTablePSL = pslBS.GetListPSL(ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, fc["search[value]"], PipSafety, PipPriority, PspProactive, PspAfterFailure, area, type, HID, Rental, Inventory, others);
            //var countRecord = pslBS.CountRecord(ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, fc["search[value]"]);
            var countData = getTablePSL.Count();
            if(download == "0")
            {
                Session["searchTableOutstanding"] = fc["search[value]"];
                Session["type"] = type;
                Session["area"] = area;
                foreach (var item in getTablePSL.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TablePSLOutstandingModel.Id();
                    id.Row = item.Row;
                    var Area = new TablePSLOutstandingModel.ValueData();
                    Area.Value = item.Area;
                    Area.Style = "";
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDateFormat - item.ReleaseDateFormat;

                    var pslNo = new TablePSLOutstandingModel.ValueData();
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
                                pslNo.Style = "";
                            }
                        }
                        else if (subsPSLId != "PI" && subsPSLId != "PS")
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
                                pslNo.Style = "";
                            }
                        }
                    }
                    else
                    {
                        pslNo.Value = item.PSLNo;
                        pslNo.Style = "";
                    }
                    var storeName = new TablePSLOutstandingModel.ValueData();
                    storeName.Value = item.StoreName;
                    storeName.Style = "";
                    var model = new TablePSLOutstandingModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TablePSLOutstandingModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }
                    srNo.Style = "";
                    var quotNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TablePSLOutstandingModel.ValueData();
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

                    var catpslstatus = new TablePSLOutstandingModel.ValueData();
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

                    var issuedate = new TablePSLOutstandingModel.ValueData();
                    issuedate.Value = item.ReleaseDate;
                    issuedate.Style = "";
                    var terminationdate = new TablePSLOutstandingModel.ValueData();
                    terminationdate.Value = item.TerminationDate;
                    terminationdate.Style = "";
                    var itemData = new TablePSLOutstandingModel.Data();
                    itemData.Id = id;
                    itemData.Area = Area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;
                    itemData.CatPslStatus = catpslstatus;
                    itemData.IssueDate = issuedate;
                    itemData.TerminationDate = terminationdate;

                    listData.Add(itemData);
                }
            }
            if(download == "1")
            {
                var getSearchValue = Session["searchTableOutstanding"].ToString();
                var getArea = Session["area"].ToString();
                var getType = Session["type"].ToString();
                getTablePSL = pslBS.GetListPSL(ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, getSearchValue, PipSafety, PipPriority, PspProactive, PspAfterFailure, getArea, getType, HID, Rental, Inventory, others);
                foreach (var item in getTablePSL)
                {
                    var id = new TablePSLOutstandingModel.Id();
                    id.Row = item.Row;
                    var Area = new TablePSLOutstandingModel.ValueData();
                    Area.Value = item.Area;
                    Area.Style = "";
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDateFormat - item.ReleaseDateFormat;

                    var pslNo = new TablePSLOutstandingModel.ValueData();
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
                                pslNo.Style = "";
                            }
                        }
                        else if (subsPSLId != "PI" && subsPSLId != "PS")
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
                            else
                            {
                                pslNo.Value = item.PSLNo;
                                pslNo.Style = "";
                            }
                        }
                    }
                    else
                    {
                        pslNo.Value = item.PSLNo;
                        pslNo.Style = "";
                    }
                    var storeName = new TablePSLOutstandingModel.ValueData();
                    storeName.Value = item.StoreName;
                    storeName.Style = "";
                    var model = new TablePSLOutstandingModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TablePSLOutstandingModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }
                    srNo.Style = "";
                    var quotNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TablePSLOutstandingModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TablePSLOutstandingModel.ValueData();
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

                    var catpslstatus = new TablePSLOutstandingModel.ValueData();
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

                    var issuedate = new TablePSLOutstandingModel.ValueData();
                    issuedate.Value = item.ReleaseDate;
                    issuedate.Style = "";
                    var terminationdate = new TablePSLOutstandingModel.ValueData();
                    terminationdate.Value = item.TerminationDate;
                    terminationdate.Style = "";
                    var itemData = new TablePSLOutstandingModel.Data();
                    itemData.Id = id;
                    itemData.Area = Area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = storeName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;
                    itemData.CatPslStatus = catpslstatus;
                    itemData.IssueDate = issuedate;
                    itemData.TerminationDate = terminationdate;

                    listData.Add(itemData);
                }
            }

            var status = new TablePSLOutstandingModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TablePSLOutstandingModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Draw = draw;
            responseJson.TotalNeedToRespond = getTablePSL.Count();
            responseJson.RecordsFiltered = getTablePSL.Count();
            responseJson.RecordsTotal = getTablePSL.Count();
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}