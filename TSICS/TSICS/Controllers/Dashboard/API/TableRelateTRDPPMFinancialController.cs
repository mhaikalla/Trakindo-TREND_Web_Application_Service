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
        // GET: TableRelateTRDPPMFinancial
        [HttpPost]
        public ActionResult TableRelateTRDPPMFinancial(FormCollection fc, string test, string cost, string freq, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN)
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var listData = new List<TableRelateTRDPPMFinancialModel.Data>();
            var getData = PartResponBS.GetDataForTableRelateTRDPPMFinancial(cost, freq);
            foreach(var item in getData)
            {
                var id = new TableRelateTRDPPMFinancialModel.Id();
                id.Row = item.Row;
                var ticketNo = new TableRelateTRDPPMFinancialModel.ValueData();
                ticketNo.Value = item.TicketNo;
                ticketNo.Style = "";
                var industry = new TableRelateTRDPPMFinancialModel.ValueData();
                industry.Value = item.Industry;
                industry.Style = "";
                var family = new TableRelateTRDPPMFinancialModel.ValueData();
                family.Value = item.Familiy;
                family.Style = "";
                var model = new TableRelateTRDPPMFinancialModel.ValueData();
                model.Value = item.Model;
                model.Style = "";
                var category = new TableRelateTRDPPMFinancialModel.ValueData();
                category.Value = item.Category;
                category.Style = "";
                var title = new TableRelateTRDPPMFinancialModel.ValueData();
                title.Value = item.Title;
                title.Style = "";
                var desc = new TableRelateTRDPPMFinancialModel.ValueData();
                desc.Value = item.Desc;
                desc.Style = "";
                var resolution = new TableRelateTRDPPMFinancialModel.ValueData();
                resolution.Value = item.Resolution;
                resolution.Style = "";
                var dateCreated = new TableRelateTRDPPMFinancialModel.ValueData();
                dateCreated.Value = item.DateCreated;
                dateCreated.Style = "";
                var dateClosed = new TableRelateTRDPPMFinancialModel.ValueData();
                dateClosed.Value = item.DateClosed;
                dateClosed.Style = "";
                var statusTR = new TableRelateTRDPPMFinancialModel.ValueData();
                var statustr = "";
                if(item.StatusTR == 1)
                {
                    statustr = "Draft";
                }
                else if(item.StatusTR == 2)
                {
                    statustr = "Submit";
                }
                else if(item.StatusTR == 3)
                {
                    statustr = "Closed";
                }
                else if(item.StatusTR == 4)
                {
                    statustr = "Re-Open";
                }
                else if(item.StatusTR == 5)
                {
                    statustr = "Remove";
                }
                else if(item.StatusTR == 6)
                {
                    statustr = "Solved";
                }
                statusTR.Value = statustr;
                statusTR.Style = "";
                var data = new TableRelateTRDPPMFinancialModel.Data();
                data.Id = id;
                data.TicketNo = ticketNo;
                data.Industry = industry;
                data.Family = family;
                data.Model = model;
                data.Category = category;
                data.Title = title;
                data.Desc = desc;
                data.Resolution = resolution;
                data.DateCreated = dateCreated;
                data.DateClosed = dateClosed;
                data.StatusTicket = statusTR;
                listData.Add(data);
            }

            var status = new TableRelateTRDPPMFinancialModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableRelateTRDPPMFinancialModel.ResponseJson();
            responseJson.Data = listData;
            responseJson.Draw = draw;
            responseJson.RecordsFiltered = getData.Count();
            responseJson.RecordsTotal = getData.Count();
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}