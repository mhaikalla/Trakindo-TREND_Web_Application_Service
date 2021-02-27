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
        // GET: TablePSLCompletionPIP
        [HttpPost]
        public ActionResult TablePSLCompletionPIP(FormCollection fc, string TerminationDateFrom, string TerminationDateEnd, string Inventory, string Rental, string HID, string area, string storeName,string others, string pslType, string download = "0")
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var Month = (fc["month"] != null) ? fc["month"] : "";
            var Type = (fc["type"] != null) ? fc["type"] : "";
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
            var splitPSLType = new string[] { };
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitSalesOffice = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            var listData = new List<TablePSLCompletionPIPModel.Data>();
            var getData = pslBS.GetDataForTablePSLCompletionPIP(splitArea, splitSalesOffice, splitPSLType, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], fc["search[value]"], searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, Month, Type, HID, Rental, Inventory, others);

            if(download == "0")
            {
                Session["searchValueCompletionPIP"] = fc["search[value]"];
                foreach (var item in getData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TablePSLCompletionPIPModel.Id();
                    id.Row = item.Row;
                    var Area = new TablePSLCompletionPIPModel.ValueData();
                    Area.Value = item.Area;
                    Area.Style = "";
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDate - item.ReleaseDate;
                    var pslNo = new TablePSLCompletionPIPModel.ValueData();
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
                    var StoreName = new TablePSLCompletionPIPModel.ValueData();
                    StoreName.Value = item.StoreName;
                    StoreName.Style = "";
                    var model = new TablePSLCompletionPIPModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TablePSLCompletionPIPModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }
                    srNo.Style = "";
                    var quotNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TablePSLCompletionPIPModel.ValueData();
                    if(item.PSLStatus  == "Outstanding")
                    {
                        pslStatus.Value = "<span class=\"badge badge-info\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if(item.PSLStatus == "Completed")
                    {
                        pslStatus.Value = "<span class=\"badge badge-success\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Open")
                    {
                        pslStatus.Value = "<span class=\"badge badge-danger\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Released")
                    {
                        pslStatus.Value = "<span class=\"badge badge-purple\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "In Progress")
                    {
                        pslStatus.Value = "<span class=\"badge badge-warning\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Completed")
                    {
                        pslStatus.Value = "<span class=\"badge badge-completed\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Over Limit")
                    {
                        pslStatus.Value = "<span class=\"badge badge-success\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Ignored")
                    {
                        pslStatus.Value = "<span class=\"badge badge-success\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else if (item.PSLStatus == "Customer Rejection")
                    {
                        pslStatus.Value = "<span class=\"badge badge-success\">" + item.PSLStatus + "</span>";
                        pslStatus.Style = "";
                    }
                    else
                    {
                        pslStatus.Value = item.PSLStatus;
                        pslStatus.Style = "";
                    }

                    var issueDate = new TablePSLCompletionPIPModel.ValueData();
                    issueDate.Value = item.LetterDate.ToString();
                    issueDate.Style = "";
                    var terminationDate = new TablePSLCompletionPIPModel.ValueData();
                    terminationDate.Value = item.TerminationDate.ToString();
                    terminationDate.Style = "";
                    var catpslstatus = new TablePSLCompletionPIPModel.ValueData();
                    if(item.CatPSLStatus == "Complete")
                    {
                        catpslstatus.Value = "<span class=\"badge badge-success\">" + item.CatPSLStatus + "</span>";
                        catpslstatus.Style = "";
                    }
                    else
                    {
                        catpslstatus.Value = "<span class=\"badge badge-danger\">" + item.CatPSLStatus + "</span>";
                        catpslstatus.Style = "";
                    }
                    
                    var itemData = new TablePSLCompletionPIPModel.Data();
                    itemData.Id = id;
                    itemData.Area = Area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = StoreName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;
                    itemData.IssueDate = issueDate;
                    itemData.TerminationDate = terminationDate;
                    itemData.CatPSLStatus = catpslstatus;

                    listData.Add(itemData);
                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searchValueCompletionPIP"].ToString();
                getData = pslBS.GetDataForTablePSLCompletionPIP(splitArea, splitSalesOffice, splitPSLType, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"], getSearchValue, searchArea, searchPSLNo, searchStoreName, searchModel, searchSerialNumber, searchSRNo, searchQuotNo, searchSoNo, searchSAPPSLStatus, Month, Type, HID, Rental, Inventory, others);
                foreach (var item in getData)
                {
                    var id = new TablePSLCompletionPIPModel.Id();
                    id.Row = item.Row;
                    var Area = new TablePSLCompletionPIPModel.ValueData();
                    Area.Value = item.Area;
                    Area.Style = "";
                    var pslNo = new TablePSLCompletionPIPModel.ValueData();
                    pslNo.Value = item.PSLNo;
                    pslNo.Style = "";
                    var StoreName = new TablePSLCompletionPIPModel.ValueData();
                    StoreName.Value = item.StoreName;
                    StoreName.Style = "";
                    var model = new TablePSLCompletionPIPModel.ValueData();
                    model.Value = item.Model;
                    model.Style = "";
                    var serialNumber = new TablePSLCompletionPIPModel.ValueData();
                    serialNumber.Value = item.SerialNo;
                    serialNumber.Style = "";
                    var srNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SRNo) && item.SRNo != "0")
                    {
                        srNo.Value = item.SRNo;
                    }
                    else
                    {
                        srNo.Value = "-";
                    }
                    srNo.Style = "";
                    var quotNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.QuotNo) && item.QuotNo != "0")
                    {
                        quotNo.Value = item.QuotNo;
                    }
                    else
                    {
                        quotNo.Value = "-";
                    }

                    quotNo.Style = "";
                    var soNo = new TablePSLCompletionPIPModel.ValueData();
                    if (!string.IsNullOrWhiteSpace(item.SoNo) && item.SoNo != "0")
                    {
                        soNo.Value = item.SoNo;
                    }
                    else
                    {
                        soNo.Value = "-";
                    }
                    soNo.Style = "";
                    var pslStatus = new TablePSLCompletionPIPModel.ValueData();
                    pslStatus.Value = item.PSLStatus;
                    pslStatus.Style = "badge badge-warning";
                    var itemData = new TablePSLCompletionPIPModel.Data();
                    itemData.Id = id;
                    itemData.Area = Area;
                    itemData.PSLNo = pslNo;
                    itemData.StoreName = StoreName;
                    itemData.Model = model;
                    itemData.SerialNumber = serialNumber;
                    itemData.SRNo = srNo;
                    itemData.QuotNo = quotNo;
                    itemData.SoNo = soNo;
                    itemData.PSLStatus = pslStatus;

                    listData.Add(itemData);
                }
            }
            

            var status = new TablePSLCompletionPIPModel.Status();
            status.Code = 200;
            status.Message = "sukses";

            var responseJson = new TablePSLCompletionPIPModel.ResponseJson();
            responseJson.Draw = draw;
            responseJson.RecordsFiltered = getData.Count();
            responseJson.RecordsTotal = getData.Count();
            responseJson.TotalNeedToRespond = getData.Count();
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}