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
        // GET: DashboardMonitoringSubmitter
        public ActionResult DashboardMonitoringSubmitter()
        {
            this.setViewBag();
            var getCloseTicketSubmitter = ticketBS.getvalueSubmitter(Convert.ToInt32(Session["userid"]));
            var getCountSubmitterPSA = ticketBS.getvalueSubmitterPSA(Convert.ToInt32(Session["userid"]));
            var getCountSubmitterSolved = ticketBS.getvalueSubmitterSolved(Convert.ToInt32(Session["userid"]));

            var labelsDataList = new List<string>();
            labelsDataList.Add("0-2 days");
            labelsDataList.Add("3 days");
            labelsDataList.Add(">3 days");

            #region Post Data
            #region PostData1
            var postDataSolved = new List<DashboardMonitoringSubmitterModel.PostData>();
            for (var i = 1; i <= getCountSubmitterPSA.Count(); i++)
            {
                if (i == 1)
                {
                    var value1 = new DashboardMonitoringSubmitterModel.PostData();
                    value1.Val = getCountSubmitterPSA[0];
                    value1.Key = "PSA";
                    postDataSolved.Add(value1);
                    for(var k=1; k<=getCountSubmitterSolved.Count(); k++)
                    {
                        if(k == 1)
                        {
                            var value = new DashboardMonitoringSubmitterModel.PostData();
                            value.Val = getCountSubmitterSolved[0];
                            value.Key = "Solved";
                            postDataSolved.Add(value);
                        }
                    }
                }
            }
            #endregion
            #region PostData2
            var postDataSolved2 = new List<DashboardMonitoringSubmitterModel.PostData>();
            for (var i = 1; i <= getCountSubmitterPSA.Count(); i++)
            {
                if (i == 2)
                {
                    var value2 = new DashboardMonitoringSubmitterModel.PostData();
                    value2.Val = getCountSubmitterPSA[1];
                    value2.Key = "PSA";
                    postDataSolved2.Add(value2);
                    for (var k = 1; k <= getCountSubmitterSolved.Count(); k++)
                    {
                        if (k == 2)
                        {
                            var value = new DashboardMonitoringSubmitterModel.PostData();
                            value.Val = getCountSubmitterSolved[1];
                            value.Key = "Solved";
                            postDataSolved2.Add(value);
                        }
                    }
                }
            }
            #endregion
            #region PostData3
            var postDataSolved3 = new List<DashboardMonitoringSubmitterModel.PostData>();
            for (var i = 1; i <= getCountSubmitterPSA.Count(); i++)
            {
                if (i == 3)
                {
                    var value2 = new DashboardMonitoringSubmitterModel.PostData();
                    value2.Val = getCountSubmitterPSA[2];
                    value2.Key = "PSA";
                    postDataSolved3.Add(value2);
                }
            }
            #endregion

            var postData = new List<List<DashboardMonitoringSubmitterModel.PostData>>();
            postData.Add(postDataSolved);
            postData.Add(postDataSolved2);
            postData.Add(postDataSolved3);
            #endregion


            var backgroundColor = new List<string>();
            backgroundColor.Add("#6bf854");
            backgroundColor.Add("#fafe45");
            backgroundColor.Add("#f3461c");

            var dataDataSets = new DashboardMonitoringSubmitterModel.DataSets();
            dataDataSets.Data = getCloseTicketSubmitter;
            dataDataSets.PostData = postData;
            dataDataSets.BackgroundColor = backgroundColor;

            var unitLegend = new DashboardMonitoringSubmitterModel.Legend();
            unitLegend.Unit = "TR";

            var apiChangeDT = new List<string>();
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=submitter&filter=psa");
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=submitter&filter=escalated");
            apiChangeDT.Add(Url.Action("TableDashboardMonitoring", "Dashboard") + "?type=submitter&filter=other");

            var changeDT = new DashboardMonitoringSubmitterModel.ChangeDT();
            changeDT.Target = "#table--all-tickets";
            changeDT.API = apiChangeDT;

            var eventSubmitter = new DashboardMonitoringSubmitterModel.Event();
            eventSubmitter.ChangeDT = changeDT;

            var pieHalf = new DashboardMonitoringSubmitterModel.PieHalf();
            pieHalf.Label = labelsDataList;
            pieHalf.DataSets = dataDataSets;
            pieHalf.Legend = unitLegend;
            pieHalf.Event = eventSubmitter;

            var data = new DashboardMonitoringSubmitterModel.Data();
            data.PieHalf = pieHalf;

            var status = new DashboardMonitoringSubmitterModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new DashboardMonitoringSubmitterModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}