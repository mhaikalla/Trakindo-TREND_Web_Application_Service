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
        // GET: TableRelatePSLDPPMFinancial
        [HttpPost]
        public ActionResult TableRelatePSLDPPMFinancial(FormCollection fc, string test, string cost, string freq, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN, string download = "0")
        {
            //var getData = PartResponBS.
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var getTablePSL = PartResponBS.GetDataForTableRelatePSLDPPMFinancial(cost, freq, fc["search[value]"]);
            var listData = new List<TableRelatePSLDPPMFinancialModel.Data>();
            if(download == "0")
            {
                Session["searcValueRelatePSL"] = fc["search[value]"];
                foreach (var item in getTablePSL.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableRelatePSLDPPMFinancialModel.Id();
                    id.Row = item.Row;
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDate - item.IssueDate;
                    var pslNo = new TableRelatePSLDPPMFinancialModel.ValueData();
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
                    var description = new TableRelatePSLDPPMFinancialModel.ValueData();
                    description.Value = item.Desc;
                    description.Style = "";
                    var pslType = new TableRelatePSLDPPMFinancialModel.ValueData();
                    pslType.Value = item.PSLType;
                    pslType.Style = "";
                    var issueDate = new TableRelatePSLDPPMFinancialModel.ValueData();
                    issueDate.Value = item.IssueDateString;
                    issueDate.Style = "";
                    var terminationDate = new TableRelatePSLDPPMFinancialModel.ValueData();
                    terminationDate.Value = item.TerminationDateString;
                    terminationDate.Style = "";
                    var completion = new TableRelatePSLDPPMFinancialModel.ValueData();
                    completion.Value = item.Completion.ToString();
                    completion.Style = "";
                    var itemData = new TableRelatePSLDPPMFinancialModel.Data();
                    itemData.Id = id;
                    itemData.PSLNo = pslNo;
                    itemData.Description = description;
                    itemData.PSLType = pslType;
                    itemData.IssueDate = issueDate;
                    itemData.TerminationDate = terminationDate;
                    itemData.Completion = completion;

                    listData.Add(itemData);
                }
            }

            if(download == "1")
            {
                var getSearchValue = Session["searcValueRelatePSL"].ToString();
                getTablePSL = PartResponBS.GetDataForTableRelatePSLDPPMFinancial(cost, freq, getSearchValue);
                foreach (var item in getTablePSL)
                {
                    var id = new TableRelatePSLDPPMFinancialModel.Id();
                    id.Row = item.Row;
                    var subsPSLId = item.PSLNo.Substring(0, 2);
                    var convDayToExpired = (item.DaysToExpired != "Expired") ? Convert.ToInt32(item.DaysToExpired) : 0;
                    var subsDate = item.TerminationDate - item.IssueDate;
                    var pslNo = new TableRelatePSLDPPMFinancialModel.ValueData();
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
                    var description = new TableRelatePSLDPPMFinancialModel.ValueData();
                    description.Value = item.Desc;
                    description.Style = "";
                    var pslType = new TableRelatePSLDPPMFinancialModel.ValueData();
                    pslType.Value = item.PSLType;
                    pslType.Style = "";
                    var issueDate = new TableRelatePSLDPPMFinancialModel.ValueData();
                    issueDate.Value = item.IssueDate.ToString();
                    issueDate.Style = "";
                    var terminationDate = new TableRelatePSLDPPMFinancialModel.ValueData();
                    terminationDate.Value = item.TerminationDate.ToString();
                    terminationDate.Style = "";
                    var completion = new TableRelatePSLDPPMFinancialModel.ValueData();
                    completion.Value = item.Completion.ToString();
                    completion.Style = "";
                    var itemData = new TableRelatePSLDPPMFinancialModel.Data();
                    itemData.Id = id;
                    itemData.PSLNo = pslNo;
                    itemData.Description = description;
                    itemData.PSLType = pslType;
                    itemData.IssueDate = issueDate;
                    itemData.TerminationDate = terminationDate;
                    itemData.Completion = completion;

                    listData.Add(itemData);
                }
            }
            

            var status = new TableRelatePSLDPPMFinancialModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableRelatePSLDPPMFinancialModel.ResponseJson();
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