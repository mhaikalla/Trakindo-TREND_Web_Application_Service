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
        // GET: MonthlyTurnAround
        public ActionResult MonthlyTurnAround(string type, int m, int y)
        {
            var getTicketCloseLessThanThree = ticketBS.GetValueTurnAroundMonthLessThanTree(Convert.ToInt32(Session["userid"]), type, m, y);
            var getTicketCloseLessThanFourteen = ticketBS.GetValueTurnAroundMonthLessThanFourteen(Convert.ToInt32(Session["userid"]), type, m, y);
            var getTicketCloseLessThanFourthyTwo = ticketBS.GetValueTurnAroundMonthLessThanFourthyTwo(Convert.ToInt32(Session["userid"]), type, m, y);
            var getTicketCloseLessThanEightyFour = ticketBS.GetValueTurnAroundMonthLessThanEightyFour(Convert.ToInt32(Session["userid"]), type, m, y);
            var getTicketCloseLessThanTwoQuarter = ticketBS.GetValueTurnAroundMonthLessThanTwoQuarter(Convert.ToInt32(Session["userid"]), type, m, y);
            var getTicketCloseMoreThanTwoQuarter = ticketBS.GetValueTurnAroundMonthMoreThanTwoQuarter(Convert.ToInt32(Session["userid"]), type, m, y);
            #region FormatMonth
            var nameMonth = "";
            if(m == 1)
            {
                nameMonth = "Januari";
            }else if(m == 2)
            {
                nameMonth = "Februari";
            }else if(m == 3)
            {
                nameMonth = "Maret";
            }
            else if (m == 4)
            {
                nameMonth = "April";
            }
            else if (m == 5)
            {
                nameMonth = "Mei";
            }
            else if (m == 6)
            {
                nameMonth = "Juni";
            }
            else if (m == 7)
            {
                nameMonth = "Juli";
            }
            else if (m == 8)
            {
                nameMonth = "Agustus";
            }
            else if (m == 9)
            {
                nameMonth = "September";
            }
            else if (m == 10)
            {
                nameMonth = "Oktober";
            }
            else if (m == 11)
            {
                nameMonth = "November";
            }
            else{
                nameMonth = "Desember";
            }
            #endregion 
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

            var lineHorizontalBar = new MonthlyTurnAroundModel.Line();
            lineHorizontalBar.Label = labelHorizontalBar;

            var yAxisHorizontalBar = new MonthlyTurnAroundModel.YAxisHorizontalBar();
            yAxisHorizontalBar.Line = lineHorizontalBar;

            var horizontalBar = new MonthlyTurnAroundModel.HorizontalBar();
            horizontalBar.Data = dataHorizontalBar;
            horizontalBar.BgColor = "orange";
            horizontalBar.Legend = "Periode " + nameMonth +" " + y + "";
            horizontalBar.YAxis = yAxisHorizontalBar;

            var status = new MonthlyTurnAroundModel.Status();
            status.Message = "success";
            status.Code = 200;

            var data = new MonthlyTurnAroundModel.Data();
            data.HorizontalBar = horizontalBar;

            var responseJson = new MonthlyTurnAroundModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));

            return new EmptyResult();
        }
    }
}