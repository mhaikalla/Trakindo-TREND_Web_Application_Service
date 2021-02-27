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
        // GET: TableMonthlySubmitterDetail
        public ActionResult TableMonthlySubmitterDetail(string type, int m, int y)
        {
            var table = new TableMonthlySubmitterDetailModel.Tables();
            table.Order = "tes";

            var feedback = new TableMonthlySubmitterDetailModel.FeedBack();
            feedback.Tables = table;

            var listData = new List<TableMonthlySubmitterDetailModel.Data>();
            var listDataTable = ticketBS.getTicketMonthlySummaryDetail(Convert.ToInt32(Session["userid"]), type, m, y);

            foreach(var item in listDataTable){
                int ida = 0;
                var id = new TableMonthlySubmitterDetailModel.Id();
                id.Row = ida++;
                var ticketNo = new TableMonthlySubmitterDetailModel.ValueData();
                ticketNo.Value = item.TicketNo;
                ticketNo.Style = "text - secondary small text-truncate";
                var category = new TableMonthlySubmitterDetailModel.ValueData();
                category.Value = Convert.ToString(item.TicketCategoryId);
                category.Style = "text - secondary small text-truncate";
                var title = new TableMonthlySubmitterDetailModel.ValueData();
                title.Value = item.Title;
                title.Style = "text - secondary small text-truncate";
                var submitter = new TableMonthlySubmitterDetailModel.ValueData();
                submitter.Value = Convert.ToString(item.Submiter);
                submitter.Style = "text - secondary small text-truncate";
                var responder = new TableMonthlySubmitterDetailModel.ValueData();
                responder.Value = Convert.ToString(item.Responder); ;
                responder.Style = "text - secondary small text-truncate";
                var dateCreated = new TableMonthlySubmitterDetailModel.ValueData();
                dateCreated.Value = Convert.ToString(item.CreatedAt);
                dateCreated.Style = "text - secondary small text-truncate";
                var dateClosed = new TableMonthlySubmitterDetailModel.ValueData();
                dateClosed.Value = "tes2";
                dateClosed.Style = "text - secondary small text-truncate";
                var dateUpdated = new TableMonthlySubmitterDetailModel.ValueData();
                dateUpdated.Value = Convert.ToString(item.UpdatedAt);
                dateUpdated.Style = "text - secondary small text-truncate";
                var statusTR = new TableMonthlySubmitterDetailModel.ValueData();
                #region Status
                var TRstatus = "";
                if (item.Status == 1)
                {
                    TRstatus = "Draft";
                }
                else if (item.Status == 2)
                {
                    TRstatus = "Submit";
                }
                else if (item.Status == 3)
                {
                    TRstatus = "Close";
                }
                else if (item.Status == 4)
                {
                    TRstatus = "Re-Open";
                }
                else if (item.Status == 5)
                {
                    TRstatus = "Remove";
                }
                #endregion
                statusTR.Value = TRstatus;
                statusTR.Style = "text - secondary small text-truncate";
                var statusUser = new TableMonthlySubmitterDetailModel.ValueData();
                statusUser.Value = "tes2";
                statusUser.Style = "text - secondary small text-truncate";
                var itemData = new TableMonthlySubmitterDetailModel.Data();
                itemData.Id = id;
                itemData.TicketNo = ticketNo;
                itemData.Category = category;
                itemData.Title = title;
                itemData.Submitter = submitter;
                itemData.Responder = responder;
                itemData.DateCreated = dateCreated;
                itemData.DateClosed = dateClosed;
                itemData.DateUpdated = dateUpdated;
                itemData.StatusTR = statusTR;
                itemData.StatusUser = statusUser;

                listData.Add(itemData);
            }

            var status = new TableMonthlySubmitterDetailModel.Status();
            status.Message = "Sukses";
            status.Code = 200;

            var responseJson = new TableMonthlySubmitterDetailModel.ResponseJson();
            responseJson.Status = status;
            responseJson.FeedBack = feedback;
            responseJson.Draw = 1;
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}