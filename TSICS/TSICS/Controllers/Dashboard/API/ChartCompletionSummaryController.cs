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
        // GET: ChartCompletionSummary
        public ActionResult ChartCompletionSummary(string dateRangeFrom, string dateRangeEnd, string dateRangeFromIssue, string dateRangeEndIssue, string area, string inventory, string rental, string hid, string others, string PSLType, string SalesOffice, string Model)
        {
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
            var dataChart = pslBS.GetDataForChartPSLSummary(splitArea, splitSalesOffice, splitMode, splitPSLType, dateRangeFrom, dateRangeEnd, dateRangeFromIssue, dateRangeEndIssue, hid, rental, inventory, others, splitMonthFrom, splitMonthEnd, splitYearFrom, splitYearEnd, splitMonthFromIssue, splitMonthEndIssue, splitYearFromIssue, splitYearEndIssue);

            var listMonth = new List<string>();
            var listPercentDataPipSafety = new List<decimal>();
            var listPercentDataPipPriority = new List<decimal>();
            var listPercentDataPspProActive = new List<decimal>(); 
            var listPercenDataPspAfterFailure = new List<decimal>();

            var listCountdataPipSafetyCompOuts = new List<decimal>();
            var listCountdataPipPriorityCompOuts = new List<decimal>();
            var listCountdataPspProActiveCompOuts = new List<decimal>();
            var listCountdataPspAfterFailureCompOuts = new List<decimal>();
            var listInListRawDataPipSafety = new List<List<decimal>>();
            var listInListRawDataPipPriority = new List<List<decimal>>();
            var listInListRawDataPspProActive = new List<List<decimal>>();
            var listInListRawDataPspAfterFailure = new List<List<decimal>>();

            foreach (var item in dataChart)
            {
                listMonth = item.MonthName;
                listPercentDataPipSafety = item.DataPercentPipSafety;
                listPercentDataPipPriority = item.DataPercentPipPriority;
                listPercentDataPspProActive = item.DataPercentPspProActive;
                listPercenDataPspAfterFailure = item.DataPercentPspAfterFailure;
                listInListRawDataPipSafety.Add(item.RawDataPipSafetyCompAndOuts);
                listInListRawDataPipPriority.Add(item.RawDataPipPriorityCompAndOuts);
                listInListRawDataPspProActive.Add(item.RawDataPspProActiveCompAndOuts);
                listInListRawDataPspAfterFailure.Add(item.RawDataPspAfterFailureCompAndOuts);
            }

            var yAxes = new ChartCompletionSummaryModel.YAxes();
            yAxes.Min = 0;
            yAxes.Max = 100;
            yAxes.Step = 20;


            var listDataSets = new List<ChartCompletionSummaryModel.DataSets>();
            var dataSets1 = new ChartCompletionSummaryModel.DataSets();
            dataSets1.Label = "PIP Safety";
            dataSets1.DataPercent = listPercentDataPipSafety;
            dataSets1.RawData = listInListRawDataPipSafety;
            dataSets1.BackgroundColor = "rgb(255, 142, 35)";
            dataSets1.BorderColor = "transparent";
            var dataSets2 = new ChartCompletionSummaryModel.DataSets();
            dataSets2.Label = "PIP Priority";
            dataSets2.DataPercent = listPercentDataPipPriority;
            dataSets2.RawData = listInListRawDataPipPriority;
            dataSets2.BackgroundColor = "rgb(92, 155, 147)";
            dataSets2.BorderColor = "transparent";
            var dataSets3 = new ChartCompletionSummaryModel.DataSets();
            dataSets3.Label = "PSP Pro Active";
            dataSets3.DataPercent = listPercentDataPspProActive;
            dataSets3.RawData = listInListRawDataPspProActive;
            dataSets3.BackgroundColor = "rgb(67, 206, 43)";
            dataSets3.BorderColor = "transparent";
            var dataSets4 = new ChartCompletionSummaryModel.DataSets();
            dataSets4.Label = "PSP After Failure";
            dataSets4.DataPercent = listPercenDataPspAfterFailure;
            dataSets4.RawData = listInListRawDataPspAfterFailure;
            dataSets4.BackgroundColor = "rgb(230, 232, 28)";
            dataSets4.BorderColor = "transparent";
            listDataSets.Add(dataSets1);
            listDataSets.Add(dataSets2);
            listDataSets.Add(dataSets3);
            listDataSets.Add(dataSets4);

            var verticalBar = new ChartCompletionSummaryModel.VerticalBar();
            verticalBar.Label = listMonth;
            verticalBar.BarSize = Convert.ToDecimal(0.5);
            verticalBar.YAxes = yAxes;
            verticalBar.DataSets = listDataSets;

            var data = new ChartCompletionSummaryModel.Data();
            data.VerticalBar = verticalBar;

            var status = new ChartCompletionSummaryModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new ChartCompletionSummaryModel.ResponseJson();
            responseJson.Data = data;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}