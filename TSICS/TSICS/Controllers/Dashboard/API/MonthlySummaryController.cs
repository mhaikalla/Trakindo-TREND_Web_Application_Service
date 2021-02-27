using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System.Collections;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        #region BussinesServices
        RatingBusinessService ratingBS = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        TicketBusinessService ticketBS = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        TicketDiscussionBusinessService ticketDiscussionBusinessService = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        #endregion
        // GET: MonthlySummary
        public ActionResult MonthlySummary(string type, int m, int y)
        {
            //var getTicketId = ticketBS.GetTicketDashboard(Convert.ToInt32(Session["userid"]));
            //ArrayList list = new ArrayList();
            var getLastDiscuss = ticketDiscussionBusinessService.GetValuePercentage(Convert.ToInt32(Session["userid"]), type, m, y);
            var getCount = ticketBS.GetCountTicketDashboardMonthSubmitter(Convert.ToInt32(Session["userid"]), type, m, y);
            var getWeekAverage = ticketBS.GetCountTicketDashboardMonthSubmitterGetWeek(Convert.ToInt32(Session["userid"]), type, m, y);
            var getWeekTicketPerformance = ticketDiscussionBusinessService.GetValuePercentageWeek1(Convert.ToInt32(Session["userid"]), type, m, y);

            //Session["CountTRMonthResponder"] = ticketBS.GetCountTRMonthResponder(Convert.ToInt32(Session["userid"]), type, m);
            //Session["CountTRMonthSubmitter"] = ticketBS.GetCountTRMonthSubmitter(Convert.ToInt32(Session["userid"]), type, m);
            //if (getCount > 0 && getLastDiscuss > 0)
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

                var doughnutGauge = new MonthlySubmitterModel.DoughnutGauge();
                doughnutGauge.BackgroundColor = listBackgroundColorDoughnutGauge;
                doughnutGauge.Value = Convert.ToDouble(getCount);
                //doughnutGauge.Value = 1.65;
                doughnutGauge.GaugeLimits = listGaugeLimitsDoughnutGauge;
                #endregion

                #region ComboBar
                var listLabelXaxisComboBar = new List<string>();
                listLabelXaxisComboBar.Add("Minggu 1");
                listLabelXaxisComboBar.Add("Minggu 2");
                listLabelXaxisComboBar.Add("Minggu 3");
                listLabelXaxisComboBar.Add("Minggu 4");

                var minTargetXaxisComboBar = new MonthlySubmitterModel.MinTargetXaxisComboBar();
                minTargetXaxisComboBar.BorderColor = "rgb(75, 192, 192)";
                minTargetXaxisComboBar.BorderWidth = 2;
                minTargetXaxisComboBar.Value = 4;

                var xaxisComboBar = new MonthlySubmitterModel.XaxisComboBar();
                xaxisComboBar.MinTarget = minTargetXaxisComboBar;
                xaxisComboBar.Label = listLabelXaxisComboBar;

                var leftYaxisComboBar = new MonthlySubmitterModel.LeftYaxisComboBar();
                leftYaxisComboBar.Min = 0;
                leftYaxisComboBar.Max = 5;

                var rightYaxisComboBar = new MonthlySubmitterModel.RightYaxisComboBar();
                rightYaxisComboBar.Min = 0;
                rightYaxisComboBar.Max = 100;

                var yaxisComboBar = new MonthlySubmitterModel.YaxisComboBar();
                yaxisComboBar.Left = leftYaxisComboBar;
                yaxisComboBar.Right = rightYaxisComboBar;

            var listValueLineDataComboBar = getWeekTicketPerformance;

                var lineDataComboBar = new MonthlySubmitterModel.LineDataComboBar();
                lineDataComboBar.Label = "Ticket Performance";
                lineDataComboBar.BorderWidth = 1;
                lineDataComboBar.Color = "red";
                lineDataComboBar.Value = listValueLineDataComboBar;

                var listValueBarDataComboBar = getWeekAverage;

                var barDataComboBar = new MonthlySubmitterModel.BarDataComboBar();
                barDataComboBar.Label = "Average Rating";
                barDataComboBar.BorderWidth = 1;
                barDataComboBar.Color = "olive";
                barDataComboBar.Value = listValueBarDataComboBar;

                var dataComboBar = new MonthlySubmitterModel.DataComboBar();
                dataComboBar.Line = lineDataComboBar;
                dataComboBar.Bar = barDataComboBar;

                var comboBar = new MonthlySubmitterModel.ComboBar();
                comboBar.Xaxis = xaxisComboBar;
                comboBar.Yaxis = yaxisComboBar;
                comboBar.Data = dataComboBar;
                #endregion

                #region PercentBar
                var lineYaxisPercentBar = new MonthlySubmitterModel.LineYaxisPercentBar();
                lineYaxisPercentBar.Div = 5;
                lineYaxisPercentBar.Min = 0;
                lineYaxisPercentBar.Max = 100;

                var yaxisPercentBar = new MonthlySubmitterModel.YaxisPercentBar();
                yaxisPercentBar.LineYaxisPercentBar = lineYaxisPercentBar;
                yaxisPercentBar.Legend = "Responded in < 3 days (%)";

                var dataPercentBar = new MonthlySubmitterModel.DataPercentBar();
                dataPercentBar.Min = 80;
                dataPercentBar.Value = getLastDiscuss;

                var tooltipPercentBar = new MonthlySubmitterModel.TooltipPercentBar();
                tooltipPercentBar.Value = "below min. target";

                var percentBar = new MonthlySubmitterModel.PercentBar();
                percentBar.YaxisPercentBar = yaxisPercentBar;
                percentBar.DataPercentBar = dataPercentBar;
                percentBar.TooltipPercentBar = tooltipPercentBar;
                #endregion

                #region Status
                var status = new MonthlySubmitterModel.Status();
                status.Code = 200;
                status.Message = "";
                #endregion

                #region Data
                var data = new MonthlySubmitterModel.Data();
                data.DoughnutGauge = doughnutGauge;
                data.ComboBar = comboBar;
                data.PercentBar = percentBar;
                #endregion

                #region Response
                var response = new MonthlySubmitterModel.ResponseJson();
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

            //    var listGaugeLimitsDoughnutGauge = new List<decimal>();
            //    listGaugeLimitsDoughnutGauge.Add(1);
            //    listGaugeLimitsDoughnutGauge.Add(2);
            //    listGaugeLimitsDoughnutGauge.Add(3);
            //    listGaugeLimitsDoughnutGauge.Add(4);
            //    listGaugeLimitsDoughnutGauge.Add(5);

            //    var doughnutGauge = new MonthlySubmitterModel.DoughnutGauge();
            //    doughnutGauge.BackgroundColor = listBackgroundColorDoughnutGauge;
            //    doughnutGauge.Value = 0;
            //    //doughnutGauge.Value = 1.65;
            //    doughnutGauge.GaugeLimits = listGaugeLimitsDoughnutGauge;
            //    #endregion

            //    #region ComboBar
            //    var listLabelXaxisComboBar = new List<string>();
            //    listLabelXaxisComboBar.Add("Minggu 1");
            //    listLabelXaxisComboBar.Add("Minggu 2");
            //    listLabelXaxisComboBar.Add("Minggu 3");
            //    listLabelXaxisComboBar.Add("Minggu 4");

            //    var minTargetXaxisComboBar = new MonthlySubmitterModel.MinTargetXaxisComboBar();
            //    minTargetXaxisComboBar.BorderColor = "rgb(75, 192, 192)";
            //    minTargetXaxisComboBar.BorderWidth = 2;
            //    minTargetXaxisComboBar.Value = 4;

            //    var xaxisComboBar = new MonthlySubmitterModel.XaxisComboBar();
            //    xaxisComboBar.MinTarget = minTargetXaxisComboBar;
            //    xaxisComboBar.Label = listLabelXaxisComboBar;

            //    var leftYaxisComboBar = new MonthlySubmitterModel.LeftYaxisComboBar();
            //    leftYaxisComboBar.Min = 0;
            //    leftYaxisComboBar.Max = 5;

            //    var rightYaxisComboBar = new MonthlySubmitterModel.RightYaxisComboBar();
            //    rightYaxisComboBar.Min = 0;
            //    rightYaxisComboBar.Max = 100;

            //    var yaxisComboBar = new MonthlySubmitterModel.YaxisComboBar();
            //    yaxisComboBar.Left = leftYaxisComboBar;
            //    yaxisComboBar.Right = rightYaxisComboBar;

            //    var listValueLineDataComboBar = new List<decimal>();
            //    listValueLineDataComboBar.Add(2);
            //    listValueLineDataComboBar.Add(4);
            //    listValueLineDataComboBar.Add(5);
            //    listValueLineDataComboBar.Add(3);

            //    var lineDataComboBar = new MonthlySubmitterModel.LineDataComboBar();
            //    lineDataComboBar.Label = "Ticket Performance";
            //    lineDataComboBar.BorderWidth = 1;
            //    lineDataComboBar.Color = "red";
            //    lineDataComboBar.Value = listValueLineDataComboBar;

            //    var listValueBarDataComboBar = new List<decimal>();
            //    listValueBarDataComboBar.Add(4);
            //    listValueBarDataComboBar.Add(5);
            //    listValueBarDataComboBar.Add(3);
            //    listValueBarDataComboBar.Add(1);

            //    var barDataComboBar = new MonthlySubmitterModel.BarDataComboBar();
            //    barDataComboBar.Label = "Average Rating";
            //    barDataComboBar.BorderWidth = 1;
            //    barDataComboBar.Color = "olive";
            //    barDataComboBar.Value = listValueBarDataComboBar;

            //    var dataComboBar = new MonthlySubmitterModel.DataComboBar();
            //    dataComboBar.Line = lineDataComboBar;
            //    dataComboBar.Bar = barDataComboBar;

            //    var comboBar = new MonthlySubmitterModel.ComboBar();
            //    comboBar.Xaxis = xaxisComboBar;
            //    comboBar.Yaxis = yaxisComboBar;
            //    comboBar.Data = dataComboBar;
            //    #endregion

            //    #region PercentBar
            //    var lineYaxisPercentBar = new MonthlySubmitterModel.LineYaxisPercentBar();
            //    lineYaxisPercentBar.Div = 5;
            //    lineYaxisPercentBar.Min = 0;
            //    lineYaxisPercentBar.Max = 100;

            //    var yaxisPercentBar = new MonthlySubmitterModel.YaxisPercentBar();
            //    yaxisPercentBar.LineYaxisPercentBar = lineYaxisPercentBar;
            //    yaxisPercentBar.Legend = "Responded in < 3 days (%)";

            //    var dataPercentBar = new MonthlySubmitterModel.DataPercentBar();
            //    dataPercentBar.Min = 80;
            //    dataPercentBar.Value = 0;

            //    var tooltipPercentBar = new MonthlySubmitterModel.TooltipPercentBar();
            //    tooltipPercentBar.Value = "below min. target";

            //    var percentBar = new MonthlySubmitterModel.PercentBar();
            //    percentBar.YaxisPercentBar = yaxisPercentBar;
            //    percentBar.DataPercentBar = dataPercentBar;
            //    percentBar.TooltipPercentBar = tooltipPercentBar;
            //    #endregion

            //    #region Status
            //    var status = new MonthlySubmitterModel.Status();
            //    status.Code = 200;
            //    status.Message = "";
            //    #endregion

            //    #region Data
            //    var data = new MonthlySubmitterModel.Data();
            //    data.DoughnutGauge = doughnutGauge;
            //    data.ComboBar = comboBar;
            //    data.PercentBar = percentBar;
            //    #endregion

            //    #region Response
            //    var response = new MonthlySubmitterModel.ResponseJson();
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