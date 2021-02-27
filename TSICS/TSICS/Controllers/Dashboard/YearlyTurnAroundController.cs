using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: YearlyTurnAround
        public ActionResult YearlyTurnAround(string type, int y)
        {
            this.setViewBag();
            var getTicketCloseLessThanThree = ticketBS.GetValueTurnAroundYearLessThanTree(Convert.ToInt32(Session["userid"]), type, y);
            var getTicketCloseLessThanFourteen = ticketBS.GetValueTurnAroundYearLessThanFourteen(Convert.ToInt32(Session["userid"]), type, y);
            var getTicketCloseLessThanFourthyTwo = ticketBS.GetValueTurnAroundYearLessThanFourthyTwo(Convert.ToInt32(Session["userid"]), type, y);
            var getTicketCloseLessThanEightyFour = ticketBS.GetValueTurnAroundYearLessThanEightyFour(Convert.ToInt32(Session["userid"]), type, y);
            var getTicketCloseLessThanTwoQuarter = ticketBS.GetValueTurnAroundYearLessThanTwoQuarter(Convert.ToInt32(Session["userid"]), type, y);
            var getTicketCloseMoreThanTwoQuarter = ticketBS.GetValueTurnAroundYearMoreThanTwoQuarter(Convert.ToInt32(Session["userid"]), type, y);
            var dataHorizontalBar = new List<int>();
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseMoreThanTwoQuarter));
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseLessThanTwoQuarter));
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseLessThanEightyFour));
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseLessThanFourthyTwo));
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseLessThanFourteen));
            dataHorizontalBar.Add(Convert.ToInt32(getTicketCloseLessThanThree));

            var labelHorizontalBar = new List<string>();
            labelHorizontalBar.Add("> 2 Q");
            labelHorizontalBar.Add("1 - 2Q");
            labelHorizontalBar.Add("6 - 12 weeks");
            labelHorizontalBar.Add("2 - 6 weeks");
            labelHorizontalBar.Add("3 - 14 days");
            labelHorizontalBar.Add("< 3 days");

            var lineHorizontalBar = new YearlyTurnAroundModel.Line();
            lineHorizontalBar.Label = labelHorizontalBar;

            var yAxisHorizontalBar = new YearlyTurnAroundModel.YAxisHorizontalBar();
            yAxisHorizontalBar.Line = lineHorizontalBar; 

            var horizontalBar = new YearlyTurnAroundModel.HorizontalBar();
            horizontalBar.Data = dataHorizontalBar;
            horizontalBar.BgColor = "orange"; 
            horizontalBar.Legend = "Periode " + y + "";
            horizontalBar.YAxis = yAxisHorizontalBar;

            var status = new YearlyTurnAroundModel.Status();
            status.Message = "success";
            status.Code = 200;

            var data = new YearlyTurnAroundModel.Data();
            data.HorizontalBar = horizontalBar;

            var responseJson = new YearlyTurnAroundModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
           
            return new EmptyResult();
        }
    }
}