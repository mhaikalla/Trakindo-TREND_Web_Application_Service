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
        // GET: TableDashboardPSLExecution
        [HttpPost]
        public ActionResult TableDashboardPSLExecution(FormCollection fc, string Inventory, string Rental, string HID, string Others, string Areas, string SalesOffice, string PSLStatus, string PSLNo, string AgeIndicator, string SerialNumber, string Model, string Priority, string SRNo, string QuotNo, string SONo, string SOStatus, string checklistPSLStatus, string checklistSOStatus, string download = "0") 
        {
            var Draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var orderByColumn = (fc["order[0][column]"] == null || fc["columns[" + fc["order[0][column]"] + "][data]"] == null) ? "": fc["columns[" + fc["order[0][column]"] + "][data]"];
            var orderByDir = (fc["order[0][dir]"] == null) ? "":fc["order[0][dir]"];
            var start = (fc["start"] == null) ? 0:Convert.ToInt32(fc["start"]);
            var length = (fc["length"] == null) ? 0:Convert.ToInt32(fc["length"]);
            var searchSerialNo = Convert.ToBoolean(fc["columns[0][searchable]"]);
            var searchArea = Convert.ToBoolean(fc["columns[1][searchable]"]);
            var searchSalesOffice = Convert.ToBoolean(fc["columns[2][searchable]"]);
            var searchCustomer = Convert.ToBoolean(fc["columns[3][searchable]"]);
            var searchLS44043 = Convert.ToBoolean(fc["columns[4][searchable]"]);
            var searchPS90886 = Convert.ToBoolean(fc["columns[5][searchable]"]);
            var searchPS54047 = Convert.ToBoolean(fc["columns[6][searchable]"]);
            var searchPS46027 = Convert.ToBoolean(fc["columns[7][searchable]"]);
            var countDataTotal = 0;

            #region Split String
            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLStatus = new string[] { };
            var splitPSLNo = new string[] { };
            var splitAgeIndicator = new string[] { };
            var splitSerialNumber = new string[] { };
            var splitModel = new string[] { };
            var splitPriority = new string[] { };
            var splitSOStatus = new string[] { };

            if (!string.IsNullOrWhiteSpace(Areas))
            {
                splitArea = Areas.Split(',');
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

            if (!string.IsNullOrWhiteSpace(SOStatus))
            {
                splitSOStatus = SOStatus.Split(',');
            }
            #endregion

            var listData = new List<TableDashboardPSLExecutionModel.Data>();
            
            if(download == "0")
            {
                var getListDataTable = pslBS.GetDataForTablePSLExecuton(splitArea, splitModel, splitPSLNo, splitPSLStatus, splitSOStatus, searchSerialNo, searchArea, searchSalesOffice, searchCustomer, searchLS44043, fc["search[value]"], HID, Rental, Inventory, orderByDir, orderByColumn, start, length, Others);
                countDataTotal = getListDataTable.Count();
                Session["searchValuePSLExecution"] = fc["search[value]"];
                foreach (var item in getListDataTable.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var listLs44043 = new TableDashboardPSLExecutionModel.LS44043();
                    var itemData = new TableDashboardPSLExecutionModel.Data();
                    var serialNo = new TableDashboardPSLExecutionModel.ValueData();
                    var area = new TableDashboardPSLExecutionModel.ValueData();
                    var salesOffice = new TableDashboardPSLExecutionModel.ValueData();
                    var customer = new TableDashboardPSLExecutionModel.ValueData();
                    var model = new TableDashboardPSLExecutionModel.ValueData();
                    var id = new TableDashboardPSLExecutionModel.Id();
                    id.Row = item.Row;

                    #region LS44043
                    serialNo.Style = "";
                    serialNo.Value = item.SerialNo;

                    area.Style = "";
                    area.Value = item.Area;

                    salesOffice.Style = "";
                    salesOffice.Value = item.SalesOffice;

                    customer.Style = "";
                    customer.Value = item.Customer;

                    model.Style = "";
                    model.Value = item.Model;

                    var listLS44043 = new List<TableDashboardPSLExecutionModel.ValVar>();
                    bool divider = false;
                    foreach (var data in item.Status)
                    {
                        var ls44043ValVarPSLId = new TableDashboardPSLExecutionModel.ValVar();
                        if (splitPSLStatus.Count() <= 0 && splitSOStatus.Count() <= 0 && SRNo != "on" && QuotNo != "on" && SONo != "on" && checklistPSLStatus != "on" && checklistSOStatus != "on")
                        {
                            if (divider == true)
                            {
                                divider = false;
                            }
                            else
                            {
                                divider = true;
                            }
                            var subsPSLId = data.PSLId.Substring(0, 2);
                            var convDayToExpired = (data.DayToExpired != "Expired") ? Convert.ToInt32(data.DayToExpired) : 0;
                            var subsDate = (data.TerminationDate != null && data.ReleaseDate != null) ? data.TerminationDate - data.ReleaseDate : DateTime.Now-DateTime.Now;
                            if (data.Validation.Contains("Outstanding"))
                            {

                                if (subsPSLId == "PS")
                                {
                                    if (convDayToExpired > 0 && convDayToExpired <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (convDayToExpired > 182 && convDayToExpired <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId == "PI")
                                {
                                    if (data.PSLAge > 0 && data.PSLAge <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 182 && data.PSLAge <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-danger\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId != "PI" && subsPSLId != "PS")
                                {
                                    if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                            }
                            else
                            {
                                ls44043ValVarPSLId.Title = "PSL Id";
                                ls44043ValVarPSLId.Value = data.PSLId;
                                ls44043ValVarPSLId.Divider = divider;
                                listLS44043.Add(ls44043ValVarPSLId);
                            }

                        }
                        else
                        {
                            if (divider == true)
                            {
                                divider = false;
                            }
                            else
                            {
                                divider = true;
                            }
                            var subsPSLId = data.PSLId.Substring(0, 2);
                            var convDayToExpired = (data.DayToExpired != "Expired") ? Convert.ToInt32(data.DayToExpired) : 0;
                            var subsDate = data.TerminationDate - data.ReleaseDate;
                            if (data.Validation.Contains("Outstanding"))
                            {

                                if (subsPSLId == "PS")
                                {
                                    if (convDayToExpired > 0 && convDayToExpired <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (convDayToExpired > 182 && convDayToExpired <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId == "PI")
                                {
                                    if (data.PSLAge > 0 && data.PSLAge <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 182 && data.PSLAge <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-danger\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId != "PI" && subsPSLId == "PS")
                                {
                                    if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                            }
                            else
                            {
                                ls44043ValVarPSLId.Title = "PSL Id";
                                ls44043ValVarPSLId.Value = data.PSLId;
                                ls44043ValVarPSLId.Divider = divider;
                                listLS44043.Add(ls44043ValVarPSLId);
                            }
                            var ls44043ValVarPSLStatus = new TableDashboardPSLExecutionModel.ValVar();
                            if (checklistPSLStatus == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                ls44043ValVarPSLStatus.Title = "PSL Status";
                                if (string.IsNullOrWhiteSpace(data.PSLStatus))
                                {
                                    ls44043ValVarPSLStatus.Value = "-";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Outstanding")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-info\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Completed")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-success\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Open")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-danger\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else
                                {
                                    ls44043ValVarPSLStatus.Value = data.PSLStatus;
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                listLS44043.Add(ls44043ValVarPSLStatus);
                            }
                            var ls44043ValVarSosStatus = new TableDashboardPSLExecutionModel.ValVar();
                            if (checklistSOStatus == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                ls44043ValVarSosStatus.Title = "SO Status";
                                if (string.IsNullOrWhiteSpace(data.SOStatus))
                                {
                                    ls44043ValVarSosStatus.Value = "-";
                                }
                                else if (data.SOStatus.Contains("O - Closed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-success\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Abandoned"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-danger\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Completed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-completed\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Closed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-success\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Planned Event Confirmed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Ready to Execute"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-info\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Released"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Waiting Part"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Work In Progress"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-warning\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                listLS44043.Add(ls44043ValVarSosStatus);
                            }
                            if (SRNo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.SRStatus))
                                {
                                    if (data.FollowUpDuration >= 0 && data.FollowUpDuration <= 45)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-success\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                    else if (data.FollowUpDuration > 45 && data.FollowUpDuration <= 90)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-warning\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                    else if (data.FollowUpDuration > 90)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-danger\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                }
                                else
                                {
                                    var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarSrStatus.Title = "SR No";
                                    ls44043ValVarSrStatus.Value = "-";
                                    ls44043ValVarSrStatus.Divider = divider;
                                    listLS44043.Add(ls44043ValVarSrStatus);
                                }

                            }
                            if (QuotNo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.QuotNo))
                                {
                                    var ls44043ValVarQuotNo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarQuotNo.Title = "Quot No";
                                    ls44043ValVarQuotNo.Value = data.QuotNo;
                                    ls44043ValVarQuotNo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarQuotNo);
                                }
                                else
                                {
                                    var ls44043ValVarQuotNo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarQuotNo.Title = "Quot No";
                                    ls44043ValVarQuotNo.Value = "-";
                                    ls44043ValVarQuotNo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarQuotNo);
                                }

                            }

                            if (SONo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.SONo))
                                {
                                    if (data.WipAge >= 0 && data.WipAge <= 45)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-success\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                    else if (data.WipAge > 45 && data.WipAge <= 90)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-warning\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                    else if (data.WipAge > 90)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-danger\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                }
                                else
                                {
                                    var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarSONo.Title = "SO No";
                                    ls44043ValVarSONo.Value = "-";
                                    ls44043ValVarSONo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarSONo);
                                }
                            }
                        }
                        divider = false;
                    }
                    #endregion

                    listLs44043.Style = "";
                    listLs44043.ValVar = listLS44043;

                    itemData.Id = id;
                    itemData.SerialNo = serialNo;
                    itemData.Area = area;
                    itemData.SalesOffice = salesOffice;
                    itemData.Customer = customer;
                    itemData.Model = model;
                    itemData.Status = listLs44043;
                    listData.Add(itemData);

                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searchValuePSLExecution"].ToString();
                var getListDataTable = pslBS.GetDataForTablePSLExecuton(splitArea, splitModel, splitPSLNo, splitPSLStatus, splitSOStatus, searchSerialNo, searchArea, searchSalesOffice, searchCustomer, searchLS44043, getSearchValue, HID, Rental, Inventory, orderByDir, orderByColumn, start, length, Others);
                countDataTotal = getListDataTable.Count();
                foreach (var item in getListDataTable)
                {
                    var listLs44043 = new TableDashboardPSLExecutionModel.LS44043();
                    var itemData = new TableDashboardPSLExecutionModel.Data();
                    var serialNo = new TableDashboardPSLExecutionModel.ValueData();
                    var area = new TableDashboardPSLExecutionModel.ValueData();
                    var salesOffice = new TableDashboardPSLExecutionModel.ValueData();
                    var customer = new TableDashboardPSLExecutionModel.ValueData();
                    var model = new TableDashboardPSLExecutionModel.ValueData();
                    var id = new TableDashboardPSLExecutionModel.Id();
                    id.Row = item.Row;

                    #region LS44043
                    serialNo.Style = "";
                    serialNo.Value = item.SerialNo;

                    area.Style = "";
                    area.Value = item.Area;

                    salesOffice.Style = "";
                    salesOffice.Value = item.SalesOffice;

                    customer.Style = "";
                    customer.Value = item.Customer;

                    model.Style = "";
                    model.Value = item.Model;

                    var listLS44043 = new List<TableDashboardPSLExecutionModel.ValVar>();
                    bool divider = false;
                    foreach (var data in item.Status)
                    {
                        var ls44043ValVarPSLId = new TableDashboardPSLExecutionModel.ValVar();
                        if (splitPSLStatus.Count() <= 0 && splitSOStatus.Count() <= 0 && SRNo != "on" && QuotNo != "on" && SONo != "on" && checklistPSLStatus != "on" && checklistSOStatus != "on")
                        {
                            if (divider == true)
                            {
                                divider = false;
                            }
                            else
                            {
                                divider = true;
                            }
                            var subsPSLId = data.PSLId.Substring(0, 2);
                            var convDayToExpired = (data.DayToExpired != "Expired") ? Convert.ToInt32(data.DayToExpired) : 0;
                            var subsDate = (data.TerminationDate != null && data.ReleaseDate != null) ? data.TerminationDate - data.ReleaseDate : DateTime.Now - DateTime.Now;
                            if (data.Validation.Contains("Outstanding"))
                            {

                                if (subsPSLId == "PS")
                                {
                                    if (convDayToExpired > 0 && convDayToExpired <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (convDayToExpired > 182 && convDayToExpired <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId == "PI")
                                {
                                    if (data.PSLAge > 0 && data.PSLAge <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 182 && data.PSLAge <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-danger\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId != "PI" && subsPSLId != "PS")
                                {
                                    if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                            }
                            else
                            {
                                ls44043ValVarPSLId.Title = "PSL Id";
                                ls44043ValVarPSLId.Value = data.PSLId;
                                ls44043ValVarPSLId.Divider = divider;
                                listLS44043.Add(ls44043ValVarPSLId);
                            }

                        }
                        else
                        {
                            if (divider == true)
                            {
                                divider = false;
                            }
                            else
                            {
                                divider = true;
                            }
                            var subsPSLId = data.PSLId.Substring(0, 2);
                            var convDayToExpired = (data.DayToExpired != "Expired") ? Convert.ToInt32(data.DayToExpired) : 0;
                            var subsDate = data.TerminationDate - data.ReleaseDate;
                            if (data.Validation.Contains("Outstanding"))
                            {

                                if (subsPSLId == "PS")
                                {
                                    if (convDayToExpired > 0 && convDayToExpired <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (convDayToExpired > 182 && convDayToExpired <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId == "PI")
                                {
                                    if (data.PSLAge > 0 && data.PSLAge <= 182)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 182 && data.PSLAge <= 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (data.PSLAge > 365)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-danger\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                                else if (subsPSLId != "PI" && subsPSLId == "PS")
                                {
                                    if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 0 && subsDate.Value.Hours <= 30)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-success\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else if (subsDate.Value.Days == 0 && subsDate.Value.Hours > 30 && subsDate.Value.Hours <= 59)
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = "<span class=\"badge badge-warning\">" + data.PSLId + "</span>";
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                    else
                                    {
                                        ls44043ValVarPSLId.Title = "PSL Id";
                                        ls44043ValVarPSLId.Value = data.PSLId;
                                        ls44043ValVarPSLId.Divider = divider;
                                        listLS44043.Add(ls44043ValVarPSLId);
                                    }
                                }
                            }
                            else
                            {
                                ls44043ValVarPSLId.Title = "PSL Id";
                                ls44043ValVarPSLId.Value = data.PSLId;
                                ls44043ValVarPSLId.Divider = divider;
                                listLS44043.Add(ls44043ValVarPSLId);
                            }
                            var ls44043ValVarPSLStatus = new TableDashboardPSLExecutionModel.ValVar();
                            if (checklistPSLStatus == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                ls44043ValVarPSLStatus.Title = "PSL Status";
                                if (string.IsNullOrWhiteSpace(data.PSLStatus))
                                {
                                    ls44043ValVarPSLStatus.Value = "-";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Outstanding")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-info\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Completed")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-success\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else if (data.PSLStatus == "Open")
                                {
                                    ls44043ValVarPSLStatus.Value = "<span class=\"badge badge-danger\">" + data.PSLStatus + "</span>";
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                else
                                {
                                    ls44043ValVarPSLStatus.Value = data.PSLStatus;
                                    ls44043ValVarPSLStatus.Divider = divider;
                                }
                                listLS44043.Add(ls44043ValVarPSLStatus);
                            }
                            var ls44043ValVarSosStatus = new TableDashboardPSLExecutionModel.ValVar();
                            if (checklistSOStatus == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                ls44043ValVarSosStatus.Title = "SO Status";
                                if (string.IsNullOrWhiteSpace(data.SOStatus))
                                {
                                    ls44043ValVarSosStatus.Value = "-";
                                }
                                else if (data.SOStatus.Contains("O - Closed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-success\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Abandoned"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-danger\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Completed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-completed\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Closed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-success\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Planned Event Confirmed"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Ready to Execute"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-info\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Released"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Waiting Part"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-purple\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                else if (data.SOStatus.Contains("O - Work In Progress"))
                                {
                                    ls44043ValVarSosStatus.Value = "<span class=\"badge badge-warning\">" + data.SOStatus + "</span>";
                                    ls44043ValVarSosStatus.Divider = divider;
                                }
                                listLS44043.Add(ls44043ValVarSosStatus);
                            }
                            if (SRNo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.SRStatus))
                                {
                                    if (data.FollowUpDuration >= 0 && data.FollowUpDuration <= 45)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-success\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                    else if (data.FollowUpDuration > 45 && data.FollowUpDuration <= 90)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-warning\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                    else if (data.FollowUpDuration > 90)
                                    {
                                        var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSrStatus.Title = "SR No";
                                        ls44043ValVarSrStatus.Value = "<span class=\"badge badge-danger\">" + data.SRStatus + "</span>";
                                        ls44043ValVarSrStatus.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSrStatus);
                                    }
                                }
                                else
                                {
                                    var ls44043ValVarSrStatus = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarSrStatus.Title = "SR No";
                                    ls44043ValVarSrStatus.Value = "-";
                                    ls44043ValVarSrStatus.Divider = divider;
                                    listLS44043.Add(ls44043ValVarSrStatus);
                                }

                            }
                            if (QuotNo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.QuotNo))
                                {
                                    var ls44043ValVarQuotNo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarQuotNo.Title = "Quot No";
                                    ls44043ValVarQuotNo.Value = data.QuotNo;
                                    ls44043ValVarQuotNo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarQuotNo);
                                }
                                else
                                {
                                    var ls44043ValVarQuotNo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarQuotNo.Title = "Quot No";
                                    ls44043ValVarQuotNo.Value = "-";
                                    ls44043ValVarQuotNo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarQuotNo);
                                }

                            }

                            if (SONo == "on")
                            {
                                if (divider == true)
                                {
                                    divider = false;
                                }
                                if (!string.IsNullOrWhiteSpace(data.SONo))
                                {
                                    if (data.WipAge >= 0 && data.WipAge <= 45)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-success\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                    else if (data.WipAge > 45 && data.WipAge <= 90)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-warning\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                    else if (data.WipAge > 90)
                                    {
                                        var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                        ls44043ValVarSONo.Title = "SO No";
                                        ls44043ValVarSONo.Value = "<span class=\"badge badge-danger\">" + data.SONo + "</span>";
                                        ls44043ValVarSONo.Divider = divider;
                                        listLS44043.Add(ls44043ValVarSONo);
                                    }
                                }
                                else
                                {
                                    var ls44043ValVarSONo = new TableDashboardPSLExecutionModel.ValVar();
                                    ls44043ValVarSONo.Title = "SO No";
                                    ls44043ValVarSONo.Value = "-";
                                    ls44043ValVarSONo.Divider = divider;
                                    listLS44043.Add(ls44043ValVarSONo);
                                }
                            }
                        }
                        divider = false;
                    }
                    #endregion

                    listLs44043.Style = "";
                    listLs44043.ValVar = listLS44043;

                    itemData.Id = id;
                    itemData.SerialNo = serialNo;
                    itemData.Area = area;
                    itemData.SalesOffice = salesOffice;
                    itemData.Customer = customer;
                    itemData.Model = model;
                    itemData.Status = listLs44043;
                    listData.Add(itemData);


                }
            }
           
            

            var status = new TableDashboardPSLExecutionModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableDashboardPSLExecutionModel.ResponseJson();
            responseJson.TotalNeedToRespond = countDataTotal;
            responseJson.Draw = Draw;
            responseJson.RecordsFilter = countDataTotal;
            responseJson.RecordTotal = countDataTotal;
            responseJson.Data = listData;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}