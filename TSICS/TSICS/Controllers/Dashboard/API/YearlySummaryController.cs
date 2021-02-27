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
        // GET: YearlySummary
        public ActionResult YearlySummary(string type, int y)
        {
            var getCount = ticketBS.GetCountTicketDashboardYearly(Convert.ToInt32(Session["userid"]), type, y);
            var getLastDiscuss = ticketDiscussionBusinessService.GetValuePercentageYearly(Convert.ToInt32(Session["userid"]), type, y);

            #region ValueMonthInYearAverage
            var dataComboBarJan = ticketBS.GetCountTicketDashboardYearlyJan(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarFeb = ticketBS.GetCountTicketDashboardYearlyFeb(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarMar = ticketBS.GetCountTicketDashboardYearlyMar(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarApr = ticketBS.GetCountTicketDashboardYearlyApr(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarMei = ticketBS.GetCountTicketDashboardYearlyMei(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarJun = ticketBS.GetCountTicketDashboardYearlyJun(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarJul = ticketBS.GetCountTicketDashboardYearlyJul(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarAgu = ticketBS.GetCountTicketDashboardYearlyAgu(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarSep = ticketBS.GetCountTicketDashboardYearlySep(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarOkt = ticketBS.GetCountTicketDashboardYearlyOkt(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarNov = ticketBS.GetCountTicketDashboardYearlyNov(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarDes = ticketBS.GetCountTicketDashboardYearlyDes(Convert.ToInt32(Session["userid"]), type, y);
            #endregion

            #region ValueMonthInYearTicketPerformance
            var dataComboBarTicketPerformanceJan = ticketDiscussionBusinessService.GetValuePercentageYearlyJan(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceFeb = ticketDiscussionBusinessService.GetValuePercentageYearlyFeb(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceMar = ticketDiscussionBusinessService.GetValuePercentageYearlyMar(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceApr = ticketDiscussionBusinessService.GetValuePercentageYearlyApr(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceMei = ticketDiscussionBusinessService.GetValuePercentageYearlyMei(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceJun = ticketDiscussionBusinessService.GetValuePercentageYearlyJun(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceJul = ticketDiscussionBusinessService.GetValuePercentageYearlyJul(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceAgu = ticketDiscussionBusinessService.GetValuePercentageYearlyAgu(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceSep = ticketDiscussionBusinessService.GetValuePercentageYearlySep(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceOkt = ticketDiscussionBusinessService.GetValuePercentageYearlyOkt(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceNov = ticketDiscussionBusinessService.GetValuePercentageYearlyNov(Convert.ToInt32(Session["userid"]), type, y);
            var dataComboBarTicketPerformanceDes = ticketDiscussionBusinessService.GetValuePercentageYearlyDes(Convert.ToInt32(Session["userid"]), type, y);
            #endregion

            if(y == 0)
            { 
                #region ValueMonthInYearAverage
                dataComboBarJan = 0;
                dataComboBarFeb = 0;
                dataComboBarMar = 0;
                dataComboBarApr = 0;
                dataComboBarMei = 0;
                dataComboBarJun = 0;
                dataComboBarJul = 0;
                dataComboBarAgu = 0;
                dataComboBarSep = 0;
                dataComboBarOkt = 0;
                dataComboBarNov = 0;
                dataComboBarDes = 0;
                #endregion

                #region ValueMonthInYearTicketPerformance
                dataComboBarTicketPerformanceJan = 0;
                dataComboBarTicketPerformanceFeb = 0;
                dataComboBarTicketPerformanceMar = 0;
                dataComboBarTicketPerformanceApr = 0;
                dataComboBarTicketPerformanceMei = 0;
                dataComboBarTicketPerformanceJun = 0;
                dataComboBarTicketPerformanceJul = 0;
                dataComboBarTicketPerformanceAgu = 0;
                dataComboBarTicketPerformanceSep = 0;
                dataComboBarTicketPerformanceOkt = 0;
                dataComboBarTicketPerformanceNov = 0;
                dataComboBarTicketPerformanceDes = 0;
                #endregion 
            }
            //ViewBag.GetTRYearResponder = ticketBS.GetCountTRYearResponder(Convert.ToInt32(Session["userid"]), type, y);
            //ViewBag.GetTRYearSubmitter = ticketBS.GetCountTRYearSubmitter(Convert.ToInt32(Session["userid"]), type, y);
            //if(getCount > 0 && getLastDiscuss > 0)
            //{
            #region DoughnutGauge
            var listBackgroundColorDoughnutGauge = new List<string>();
                listBackgroundColorDoughnutGauge.Add("red");
                listBackgroundColorDoughnutGauge.Add("yellow");
                listBackgroundColorDoughnutGauge.Add("yellow");
                listBackgroundColorDoughnutGauge.Add("green");

                var listGaugeLimitsDoughnutGauge = new List<decimal>();
                listGaugeLimitsDoughnutGauge.Add(1);
                listGaugeLimitsDoughnutGauge.Add(2);
                listGaugeLimitsDoughnutGauge.Add(3);
                listGaugeLimitsDoughnutGauge.Add(4);
                listGaugeLimitsDoughnutGauge.Add(5);

                var doughnutGauge = new YearlySubmitterModel.DoughnutGauge();
                doughnutGauge.BackgroundColor = listBackgroundColorDoughnutGauge;
                doughnutGauge.Value = Convert.ToDouble(getCount);
                doughnutGauge.GaugeLimits = listGaugeLimitsDoughnutGauge;
                #endregion

                #region ComboBar
                var listLabelXaxisComboBar = new List<string>();
                listLabelXaxisComboBar.Add("Bulan 1");
                listLabelXaxisComboBar.Add("Bulan 2");
                listLabelXaxisComboBar.Add("Bulan 3");
                listLabelXaxisComboBar.Add("Bulan 4");
                listLabelXaxisComboBar.Add("Bulan 5");
                listLabelXaxisComboBar.Add("Bulan 6");
                listLabelXaxisComboBar.Add("Bulan 7");
                listLabelXaxisComboBar.Add("Bulan 8");
                listLabelXaxisComboBar.Add("Bulan 9");
                listLabelXaxisComboBar.Add("Bulan 10");
                listLabelXaxisComboBar.Add("Bulan 11");
                listLabelXaxisComboBar.Add("Bulan 12");

                var minTargetXaxisComboBar = new YearlySubmitterModel.MinTargetXaxisComboBar();
                minTargetXaxisComboBar.BorderColor = "rgb(75, 192, 192)";
                minTargetXaxisComboBar.BorderWidth = 2;
                minTargetXaxisComboBar.Value = 4;

                var xaxisComboBar = new YearlySubmitterModel.XaxisComboBar();
                xaxisComboBar.MinTarget = minTargetXaxisComboBar;
                xaxisComboBar.Label = listLabelXaxisComboBar;

                var leftYaxisComboBar = new YearlySubmitterModel.LeftYaxisComboBar();
                leftYaxisComboBar.Min = 0;
                leftYaxisComboBar.Max = 5;

                var rightYaxisComboBar = new YearlySubmitterModel.RightYaxisComboBar();
                rightYaxisComboBar.Min = 0;
                rightYaxisComboBar.Max = 100;

                var yaxisComboBar = new YearlySubmitterModel.YaxisComboBar();
                yaxisComboBar.Left = leftYaxisComboBar;
                yaxisComboBar.Right = rightYaxisComboBar;

                var listValueLineDataComboBar = new List<decimal>();
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceJan);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceFeb);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceMar);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceApr);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceMei);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceJun);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceJul);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceAgu);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceSep);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceOkt);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceNov);
                listValueLineDataComboBar.Add(dataComboBarTicketPerformanceDes);

            var lineDataComboBar = new YearlySubmitterModel.LineDataComboBar();
                lineDataComboBar.Label = "Ticket Performance";
                lineDataComboBar.BorderWidth = 1;
                lineDataComboBar.Color = "red";
                lineDataComboBar.Value = listValueLineDataComboBar;

                var listValueBarDataComboBar = new List<decimal>();
                listValueBarDataComboBar.Add(dataComboBarJan);
                listValueBarDataComboBar.Add(dataComboBarFeb);
                listValueBarDataComboBar.Add(dataComboBarMar);
                listValueBarDataComboBar.Add(dataComboBarApr);
                listValueBarDataComboBar.Add(dataComboBarMei);
                listValueBarDataComboBar.Add(dataComboBarJun);
                listValueBarDataComboBar.Add(dataComboBarJul);
                listValueBarDataComboBar.Add(dataComboBarAgu);
                listValueBarDataComboBar.Add(dataComboBarSep);
                listValueBarDataComboBar.Add(dataComboBarOkt);
                listValueBarDataComboBar.Add(dataComboBarNov);
                listValueBarDataComboBar.Add(dataComboBarDes);

            var barDataComboBar = new YearlySubmitterModel.BarDataComboBar();
                barDataComboBar.Label = "Average Rating";
                barDataComboBar.BorderWidth = 1;
                barDataComboBar.Color = "olive";
                barDataComboBar.Value = listValueBarDataComboBar;

                var dataComboBar = new YearlySubmitterModel.DataComboBar();
                dataComboBar.Line = lineDataComboBar;
                dataComboBar.Bar = barDataComboBar;

                var comboBar = new YearlySubmitterModel.ComboBar();
                comboBar.Xaxis = xaxisComboBar;
                comboBar.Yaxis = yaxisComboBar;
                comboBar.Data = dataComboBar;
                #endregion

                #region PercentBar
                var lineYaxisPercentBar = new YearlySubmitterModel.LineYaxisPercentBar();
                lineYaxisPercentBar.Div = 5;
                lineYaxisPercentBar.Min = 0;
                lineYaxisPercentBar.Max = 100;

                var yaxisPercentBar = new YearlySubmitterModel.YaxisPercentBar();
                yaxisPercentBar.LineYaxisPercentBar = lineYaxisPercentBar;
                yaxisPercentBar.Legend = "Responded in < 3 days (%)";

                var dataPercentBar = new YearlySubmitterModel.DataPercentBar();
                dataPercentBar.Min = 80;
                dataPercentBar.Value = getLastDiscuss;

                var tooltipPercentBar = new YearlySubmitterModel.TooltipPercentBar();
                tooltipPercentBar.Value = "below min. target";

                var percentBar = new YearlySubmitterModel.PercentBar();
                percentBar.YaxisPercentBar = yaxisPercentBar;
                percentBar.DataPercentBar = dataPercentBar;
                percentBar.TooltipPercentBar = tooltipPercentBar;
                #endregion

                #region Status
                var status = new YearlySubmitterModel.Status();
                status.Code = 200;
                status.Message = "";
                #endregion

                #region Data
                var data = new YearlySubmitterModel.Data();
                data.DoughnutGauge = doughnutGauge;
                data.ComboBar = comboBar;
                data.PercentBar = percentBar;
                #endregion

                #region Response
                var response = new YearlySubmitterModel.ResponseJson();
                response.Status = status;
                response.Data = data;
                #endregion

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(response));

                return new EmptyResult();
            //}
            //else
            //{
            //    #region DoughnutGauge
            //    var listBackgroundColorDoughnutGauge = new List<string>();
            //    listBackgroundColorDoughnutGauge.Add("red");
            //    listBackgroundColorDoughnutGauge.Add("yellow");
            //    listBackgroundColorDoughnutGauge.Add("yellow");
            //    listBackgroundColorDoughnutGauge.Add("green");

            //    var listGaugeLimitsDoughnutGauge = new List<int>();
            //    listGaugeLimitsDoughnutGauge.Add(1);
            //    listGaugeLimitsDoughnutGauge.Add(2);
            //    listGaugeLimitsDoughnutGauge.Add(3);
            //    listGaugeLimitsDoughnutGauge.Add(4);
            //    listGaugeLimitsDoughnutGauge.Add(5);

            //    var doughnutGauge = new YearlySubmitterModel.DoughnutGauge();
            //    doughnutGauge.BackgroundColor = listBackgroundColorDoughnutGauge;
            //    doughnutGauge.Value = 0;
            //    doughnutGauge.GaugeLimits = listGaugeLimitsDoughnutGauge;
            //    #endregion

            //    #region ComboBar
            //    var listLabelXaxisComboBar = new List<string>();
            //    listLabelXaxisComboBar.Add("Bulan 1");
            //    listLabelXaxisComboBar.Add("Bulan 2");
            //    listLabelXaxisComboBar.Add("Bulan 3");
            //    listLabelXaxisComboBar.Add("Bulan 4");
            //    listLabelXaxisComboBar.Add("Bulan 5");
            //    listLabelXaxisComboBar.Add("Bulan 6");
            //    listLabelXaxisComboBar.Add("Bulan 7");
            //    listLabelXaxisComboBar.Add("Bulan 8");
            //    listLabelXaxisComboBar.Add("Bulan 9");
            //    listLabelXaxisComboBar.Add("Bulan 10");
            //    listLabelXaxisComboBar.Add("Bulan 11");
            //    listLabelXaxisComboBar.Add("Bulan 12");

            //    var minTargetXaxisComboBar = new YearlySubmitterModel.MinTargetXaxisComboBar();
            //    minTargetXaxisComboBar.BorderColor = "rgb(75, 192, 192)";
            //    minTargetXaxisComboBar.BorderWidth = 2;
            //    minTargetXaxisComboBar.Value = 4;

            //    var xaxisComboBar = new YearlySubmitterModel.XaxisComboBar();
            //    xaxisComboBar.MinTarget = minTargetXaxisComboBar;
            //    xaxisComboBar.Label = listLabelXaxisComboBar;

            //    var leftYaxisComboBar = new YearlySubmitterModel.LeftYaxisComboBar();
            //    leftYaxisComboBar.Min = 0;
            //    leftYaxisComboBar.Max = 5;

            //    var rightYaxisComboBar = new YearlySubmitterModel.RightYaxisComboBar();
            //    rightYaxisComboBar.Min = 0;
            //    rightYaxisComboBar.Max = 100;

            //    var yaxisComboBar = new YearlySubmitterModel.YaxisComboBar();
            //    yaxisComboBar.Left = leftYaxisComboBar;
            //    yaxisComboBar.Right = rightYaxisComboBar;

            //    var listValueLineDataComboBar = new List<int>();
            //    listValueLineDataComboBar.Add(2);
            //    listValueLineDataComboBar.Add(4);
            //    listValueLineDataComboBar.Add(5);
            //    listValueLineDataComboBar.Add(3);

            //    var lineDataComboBar = new YearlySubmitterModel.LineDataComboBar();
            //    lineDataComboBar.Label = "Ticket Performance";
            //    lineDataComboBar.BorderWidth = 1;
            //    lineDataComboBar.Color = "red";
            //    lineDataComboBar.Value = listValueLineDataComboBar;

            //    var listValueBarDataComboBar = new List<int>();
            //    listValueBarDataComboBar.Add(4);
            //    listValueBarDataComboBar.Add(5);
            //    listValueBarDataComboBar.Add(3);
            //    listValueBarDataComboBar.Add(1);

            //    var barDataComboBar = new YearlySubmitterModel.BarDataComboBar();
            //    barDataComboBar.Label = "Average Rating";
            //    barDataComboBar.BorderWidth = 1;
            //    barDataComboBar.Color = "olive";
            //    barDataComboBar.Value = listValueBarDataComboBar;

            //    var dataComboBar = new YearlySubmitterModel.DataComboBar();
            //    dataComboBar.Line = lineDataComboBar;
            //    dataComboBar.Bar = barDataComboBar;

            //    var comboBar = new YearlySubmitterModel.ComboBar();
            //    comboBar.Xaxis = xaxisComboBar;
            //    comboBar.Yaxis = yaxisComboBar;
            //    comboBar.Data = dataComboBar;
            //    #endregion

            //    #region PercentBar
            //    var lineYaxisPercentBar = new YearlySubmitterModel.LineYaxisPercentBar();
            //    lineYaxisPercentBar.Div = 5;
            //    lineYaxisPercentBar.Min = 0;
            //    lineYaxisPercentBar.Max = 100;

            //    var yaxisPercentBar = new YearlySubmitterModel.YaxisPercentBar();
            //    yaxisPercentBar.LineYaxisPercentBar = lineYaxisPercentBar;
            //    yaxisPercentBar.Legend = "Responded in < 3 days (%)";

            //    var dataPercentBar = new YearlySubmitterModel.DataPercentBar();
            //    dataPercentBar.Min = 80;
            //    dataPercentBar.Value = 0;

            //    var tooltipPercentBar = new YearlySubmitterModel.TooltipPercentBar();
            //    tooltipPercentBar.Value = "below min. target";

            //    var percentBar = new YearlySubmitterModel.PercentBar();
            //    percentBar.YaxisPercentBar = yaxisPercentBar;
            //    percentBar.DataPercentBar = dataPercentBar;
            //    percentBar.TooltipPercentBar = tooltipPercentBar;
            //    #endregion

            //    #region Status
            //    var status = new YearlySubmitterModel.Status();
            //    status.Code = 200;
            //    status.Message = "";
            //    #endregion

            //    #region Data
            //    var data = new YearlySubmitterModel.Data();
            //    data.DoughnutGauge = doughnutGauge;
            //    data.ComboBar = comboBar;
            //    data.PercentBar = percentBar;
            //    #endregion

            //    #region Response
            //    var response = new YearlySubmitterModel.ResponseJson();
            //    response.Status = status;
            //    response.Data = data;
            //    #endregion

            //    Response.ContentType = "application/json";
            //    Response.Write(JsonConvert.SerializeObject(response));

            //    return new EmptyResult();
            //}

            
        }
    }
}