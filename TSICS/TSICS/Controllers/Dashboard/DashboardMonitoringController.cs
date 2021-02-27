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
        // GET: DashboardMonitoring
        public ActionResult DashboardMonitoring()
        {
            this.setViewBag();
            var getCloseTicketResponder = ticketBS.getvalueResponder(Convert.ToInt32(Session["userid"]));
            var getValueResponderPRA = ticketBS.getValueResponderPRA(Convert.ToInt32(Session["userid"]));
            var getValueResponderSolved = ticketBS.getvalueResponderEscalated(Convert.ToInt32(Session["userid"]));  

             
            var labelsDataList = new List<string>();
            labelsDataList.Add("0-2 days");
            labelsDataList.Add("3 days");
            labelsDataList.Add(">3 days");

            #region Post Data
            #region PostData1
            var postDataSolved = new List<DashboardMonitoringModel.PostData>();
            for (var i = 1; i <= getValueResponderPRA.Count(); i++)
            {
                if (i == 1)
                {
                    var value1 = new DashboardMonitoringModel.PostData();
                    value1.Val = getValueResponderPRA[0];
                    value1.Key = "PRA";
                    postDataSolved.Add(value1);
                    for (var k = 1; k <= getValueResponderSolved.Count(); k++)
                    {
                        
                        if (k == 1)
                        {
                            var value2 = new DashboardMonitoringModel.PostData();
                            value2.Val = getValueResponderSolved[0];
                            value2.Key = "Escalated";
                            postDataSolved.Add(value2);
                        }
                    }
                }
            }
            #endregion
            #region PostData2
            var postDataSolved2 = new List<DashboardMonitoringModel.PostData>();
            for (var i = 1; i <= getValueResponderPRA.Count(); i++)
            {
                 if (i == 2)
                {
                    var value2 = new DashboardMonitoringModel.PostData();
                    value2.Val = getValueResponderPRA[1];
                    value2.Key = "PRA";
                    postDataSolved2.Add(value2);
                    for (var k = 1; k <= getValueResponderSolved.Count(); k++)
                    {
                        
                        if (k == 2)
                        {
                            var value = new DashboardMonitoringModel.PostData();
                            value.Val = getValueResponderSolved[1];
                            value.Key = "Escalated";
                            postDataSolved2.Add(value);
                        }
                    }
                }
            }
            #endregion
            #region PostData3
            var postDataSolved3 = new List<DashboardMonitoringModel.PostData>();
            for (var i = 1; i <= getValueResponderPRA.Count(); i++)
            {
                if (i == 3)
                {
                    var value3 = new DashboardMonitoringModel.PostData();
                    value3.Val = getValueResponderPRA[2];
                    value3.Key = "PRA";
                    postDataSolved3.Add(value3);
                    for (var k = 1; k <= getValueResponderSolved.Count(); k++)
                    {
                        if (k == 3)
                        {
                            var value = new DashboardMonitoringModel.PostData();
                            value.Val = getValueResponderSolved[2];
                            value.Key = "Escalated";
                            postDataSolved3.Add(value);
                        }
                    }
                }
            }
            #endregion

            var postData = new List<List<DashboardMonitoringModel.PostData>>();
            postData.Add(postDataSolved);
            postData.Add(postDataSolved2);
            postData.Add(postDataSolved3);
            #endregion
            

            var backgroundColor = new List<string>();
            backgroundColor.Add("#6bf854");
            backgroundColor.Add("#fafe45");
            backgroundColor.Add("#f3461c");

            var dataDataSets = new DashboardMonitoringModel.DataSets();
            dataDataSets.Data = getCloseTicketResponder;
            dataDataSets.PostData = postData;
            dataDataSets.BackgroundColor = backgroundColor;

            var unitLegend = new DashboardMonitoringModel.Legend();
            unitLegend.Unit = "TR";

            var apiChangeDT = new List<string>();
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=responder&filter=psa");
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=responder&filter=escalated");
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=responder&filter=other");

            var changeDT = new DashboardMonitoringModel.ChangeDT();
            changeDT.Target = "#table--all-tickets";
            changeDT.API = apiChangeDT;

            var eventSubmitter = new DashboardMonitoringModel.Event();
            eventSubmitter.ChangeDT = changeDT;

            var pieHalf = new DashboardMonitoringModel.PieHalf();
            pieHalf.Label = labelsDataList;
            pieHalf.DataSets = dataDataSets;
            pieHalf.Legend = unitLegend;
            pieHalf.Event = eventSubmitter;

            var data = new DashboardMonitoringModel.Data();
            data.PieHalf = pieHalf;

            var status = new DashboardMonitoringModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new DashboardMonitoringModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}