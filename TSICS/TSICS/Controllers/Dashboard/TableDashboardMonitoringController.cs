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
        // GET: TableDashboardMonitoring
        [HttpPost]
        public ActionResult TableDashboardMonitoring(FormCollection formCollection, string type, string filter, string download = "0")
        {
            this.setViewBag();
            var draw = (formCollection["draw"] != null) ? Convert.ToInt32(formCollection["draw"]) : 1;
            var orderForm = formCollection["order[0][column]"];
            var searchColomTicketNo = Convert.ToBoolean(formCollection["columns[0][searchable]"]);
            var searchColomCategory = Convert.ToBoolean(formCollection["columns[1][searchable]"]);
            var searchColomTitle = Convert.ToBoolean(formCollection["columns[2][searchable]"]);
            var searchColomSubmitter = Convert.ToBoolean(formCollection["columns[3][searchable]"]);
            var searchColomResponder = Convert.ToBoolean(formCollection["columns[4][searchable]"]);
            var searchColomDateCreated = Convert.ToBoolean(formCollection["columns[5][searchable]"]);
            var searchColomDateClosed = Convert.ToBoolean(formCollection["columns[6][searchable]"]);
            var searchColomDateUpdate = Convert.ToBoolean(formCollection["columns[7][searchable]"]);
            var searchColomStatusTR = Convert.ToBoolean(formCollection["columns[8][searchable]"]);
            var searchColomStatusUser = Convert.ToBoolean(formCollection["columns[9][searchable]"]);

            var listData = new List<TableMonitoringModel.Data>();
            var search = "";
            if(formCollection["search[value]"].Length >= 2)
            {
                search = formCollection["search[value]"];
            }
            var listDataTable = ticketBS.getDataDashboardMonitoring(Convert.ToInt32(Session["userid"]), type, filter, Convert.ToInt32(formCollection["order[0][column]"]), formCollection["order[0][dir]"], searchColomTicketNo, searchColomCategory, searchColomTitle, searchColomSubmitter, searchColomResponder, searchColomDateCreated, searchColomDateClosed, searchColomDateUpdate, searchColomStatusTR, searchColomStatusUser, search);
            
            var countData = listDataTable.Count();
            if (download == "0")
            {
                if(formCollection["search[value]"].Length >= 2)
                {
                    Session["searchValue"] = formCollection["search[value]"];
                }
                foreach (var item in listDataTable.Skip(Convert.ToInt32(formCollection["start"])).Take(Convert.ToInt32(formCollection["length"])))
                {
                    var id = new TableMonitoringModel.Id();
                    id.Row = item.Row;
                    var ticketId = new TableMonitoringModel.ValueData();
                    ticketId.Value = item.TicketId.ToString();
                    ticketId.Style = "text - secondary small text-truncate";
                    var ticketNo = new TableMonitoringModel.ValueData();
                    ticketNo.Value = item.TicketNo;
                    ticketNo.Style = "text - secondary small text-truncate";
                    var category = new TableMonitoringModel.ValueData();
                    category.Value = Convert.ToString(item.Category);
                    category.Style = "text - secondary small text-truncate";
                    var title = new TableMonitoringModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "text - secondary small text-truncate";
                    var submitter = new TableMonitoringModel.ValueData();
                    submitter.Value = Convert.ToString(item.Submitter);
                    submitter.Style = "text - secondary small text-truncate";
                    var responder = new TableMonitoringModel.ValueData();
                    responder.Value = Convert.ToString(item.Responder); ;
                    responder.Style = "text - secondary small text-truncate";
                    var dateCreated = new TableMonitoringModel.ValueData();
                    dateCreated.Value = item.TicketCreated;
                    dateCreated.Style = "text - secondary small text-truncate";
                    var dateClosed = new TableMonitoringModel.ValueData();
                    dateClosed.Value = item.TicketClosed;
                    dateClosed.Style = "text - secondary small text-truncate";
                    var dateUpdated = new TableMonitoringModel.ValueData();
                    dateUpdated.Value = item.TicketUpdated;
                    dateUpdated.Style = "text - secondary small text-truncate";
                    var statusTR = new TableMonitoringModel.ValueData();
                    statusTR.Value = item.TicketStatus;
                    statusTR.Style = "text - secondary small text-truncate";
                    var statusUser = new TableMonitoringModel.ValueData();
                    statusUser.Value = item.UserStatus;
                    statusUser.Style = "text - secondary small text-truncate";
                    var itemData = new TableMonitoringModel.Data();
                    itemData.Id = id;
                    itemData.TicketId = ticketId;
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

            }
            if (download == "1")
            {
                var getSearchFromSession = Session["searchValue"].ToString();
                listDataTable = ticketBS.getDataDashboardMonitoring(Convert.ToInt32(Session["userid"]), type, filter, Convert.ToInt32(formCollection["order[0][column]"]), formCollection["order[0][dir]"], searchColomTicketNo, searchColomCategory, searchColomTitle, searchColomSubmitter, searchColomResponder, searchColomDateCreated, searchColomDateClosed, searchColomDateUpdate, searchColomStatusTR, searchColomStatusUser, getSearchFromSession);
                foreach (var item in listDataTable)
                {
                    var id = new TableMonitoringModel.Id();
                    id.Row = item.Row;
                    var ticketId = new TableMonitoringModel.ValueData();
                    ticketId.Value = item.TicketId.ToString();
                    ticketId.Style = "text - secondary small text-truncate";
                    var ticketNo = new TableMonitoringModel.ValueData();
                    ticketNo.Value = item.TicketNo;
                    ticketNo.Style = "text - secondary small text-truncate";
                    var category = new TableMonitoringModel.ValueData();
                    category.Value = Convert.ToString(item.Category);
                    category.Style = "text - secondary small text-truncate";
                    var title = new TableMonitoringModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "text - secondary small text-truncate";
                    var submitter = new TableMonitoringModel.ValueData();
                    submitter.Value = Convert.ToString(item.Submitter);
                    submitter.Style = "text - secondary small text-truncate";
                    var responder = new TableMonitoringModel.ValueData();
                    responder.Value = Convert.ToString(item.Responder); ;
                    responder.Style = "text - secondary small text-truncate";
                    var dateCreated = new TableMonitoringModel.ValueData();
                    dateCreated.Value = item.TicketCreated;
                    dateCreated.Style = "text - secondary small text-truncate";
                    var dateClosed = new TableMonitoringModel.ValueData();
                    dateClosed.Value = item.TicketClosed;
                    dateClosed.Style = "text - secondary small text-truncate";
                    var dateUpdated = new TableMonitoringModel.ValueData();
                    dateUpdated.Value = item.TicketUpdated;
                    dateUpdated.Style = "text - secondary small text-truncate";
                    var statusTR = new TableMonitoringModel.ValueData();
                    statusTR.Value = item.TicketStatus;
                    statusTR.Style = "text - secondary small text-truncate";
                    var statusUser = new TableMonitoringModel.ValueData();
                    statusUser.Value = item.UserStatus;
                    statusUser.Style = "text - secondary small text-truncate";
                    var itemData = new TableMonitoringModel.Data();
                    itemData.Id = id;
                    itemData.TicketId = ticketId;
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

            }


            var targetColumnDefs = new List<int>();
            targetColumnDefs.Add(1);
            targetColumnDefs.Add(2);

            var columnDefs = new TableMonitoringModel.ColumnDefs();
            columnDefs.Visible = false;
            columnDefs.Target = targetColumnDefs;


            var status = new TableMonitoringModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableMonitoringModel.ResponseJson();
            responseJson.Status = status;
            responseJson.TotalNeedToRespond = countData;
            responseJson.RecordsTotal = listDataTable.Count();
            responseJson.RecordsFiltered = listDataTable.Count();
            responseJson.Draw = draw;
            responseJson.Data = listData;
            responseJson.ColumnnDefs = columnDefs;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}