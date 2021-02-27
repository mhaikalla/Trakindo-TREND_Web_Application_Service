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
        // GET: TableDPPMUpdate
        [HttpPost]
        public ActionResult TableDPPMUpdate(FormCollection fc,string register, string status, string industry, string tech_reps, string dealer_contact, string download="0")
        {
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var searchValue = fc["search[value]"];
            var listData = new List<TableDPPMUpdateModel.Data>();
            var getData = dppmBS.TableDPPMUpdate(register, status, industry, tech_reps, dealer_contact,searchValue, Convert.ToInt32(fc["order[0][column]"]), fc["order[0][dir]"]);
            if(download == "0")
            {
                Session["searchValueDPPMUpdate"] = searchValue;
                Session["column"] = Convert.ToInt32(fc["order[0][column]"]);
                Session["order"] = fc["order[0][dir]"];
                foreach (var item in getData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableDPPMUpdateModel.Id();
                    id.Row = item.Row;
                    var dppmNo = new TableDPPMUpdateModel.ValueData();
                    dppmNo.Value = item.DPPMNo;
                    dppmNo.Style = "text-secondary small";
                    var title = new TableDPPMUpdateModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "text-secondary small";
                    var desc = new TableDPPMUpdateModel.ValueData();
                    desc.Value = item.Desc;
                    desc.Style = "text-secondary small";
                    var statusData = new TableDPPMUpdateModel.ValueData();
                    statusData.Value = item.Status;
                    statusData.Style = "text-secondary small";
                    var dealerContact = new TableDPPMUpdateModel.ValueData();
                    dealerContact.Value = item.DealerContact;
                    dealerContact.Style = "text-secondary small";
                    var catReps = new TableDPPMUpdateModel.ValueData();
                    catReps.Value = item.CatReps;
                    catReps.Style = "text-secondary small";
                    var ica = new TableDPPMUpdateModel.ValueData();
                    ica.Value = item.ICA;
                    ica.Style = "text-secondary small";
                    var pca = new TableDPPMUpdateModel.ValueData();
                    pca.Value = item.PCA;
                    pca.Style = "text-secondary small";
                    var dateCreated = new TableDPPMUpdateModel.ValueData();
                    dateCreated.Value = item.DateCreated.ToString();
                    dateCreated.Style = "text-secondary small";
                    var dateLastUpdate = new TableDPPMUpdateModel.ValueData();
                    dateLastUpdate.Value = item.DateLastUpdate.ToString();
                    dateLastUpdate.Style = "text-secondary small";
                    var itemData = new TableDPPMUpdateModel.Data();
                    itemData.DPPMNo = dppmNo;
                    itemData.Title = title;
                    itemData.Desc = desc;
                    itemData.Status = statusData;
                    itemData.DealerContact = dealerContact;
                    itemData.CatReps = catReps;
                    itemData.ICA = ica;
                    itemData.PCA = pca;
                    itemData.DateCreated = dateCreated;
                    itemData.DateLastUpdate = dateLastUpdate;
                    listData.Add(itemData);
                }
            }
            if(download == "1")
            {
                var getSearchValue = Session["searchValueDPPMUpdate"].ToString();
                var strChart = Session["chart"].ToString();
                var strType = Session["type"].ToString();
                var strLabel = Session["label"].ToString();
                var kolom = Convert.ToInt32(Session["column"]);
                var order = Session["order"].ToString();
                getData = dppmBS.TableDPPMUpdate(register, status, industry, tech_reps, dealer_contact, searchValue, kolom, order);
                foreach (var item in getData)
                {
                    var id = new TableDPPMUpdateModel.Id();
                    id.Row = item.Row;
                    var dppmNo = new TableDPPMUpdateModel.ValueData();
                    dppmNo.Value = item.DPPMNo;
                    dppmNo.Style = "text-secondary small";
                    var title = new TableDPPMUpdateModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "text-secondary small";
                    var desc = new TableDPPMUpdateModel.ValueData();
                    desc.Value = item.Desc;
                    desc.Style = "text-secondary small";
                    var statusData = new TableDPPMUpdateModel.ValueData();
                    statusData.Value = item.Status;
                    statusData.Style = "text-secondary small";
                    var dealerContact = new TableDPPMUpdateModel.ValueData();
                    dealerContact.Value = item.DealerContact;
                    dealerContact.Style = "text-secondary small";
                    var catReps = new TableDPPMUpdateModel.ValueData();
                    catReps.Value = item.CatReps;
                    catReps.Style = "text-secondary small";
                    var ica = new TableDPPMUpdateModel.ValueData();
                    ica.Value = item.ICA;
                    ica.Style = "text-secondary small";
                    var pca = new TableDPPMUpdateModel.ValueData();
                    pca.Value = item.PCA;
                    pca.Style = "text-secondary small";
                    var dateCreated = new TableDPPMUpdateModel.ValueData();
                    dateCreated.Value = item.DateCreated.ToString();
                    dateCreated.Style = "text-secondary small";
                    var dateLastUpdate = new TableDPPMUpdateModel.ValueData();
                    dateLastUpdate.Value = item.DateLastUpdate.ToString();
                    dateLastUpdate.Style = "text-secondary small";
                    var itemData = new TableDPPMUpdateModel.Data();
                    itemData.DPPMNo = dppmNo;
                    itemData.Title = title;
                    itemData.Desc = desc;
                    itemData.Status = statusData;
                    itemData.DealerContact = dealerContact;
                    itemData.CatReps = catReps;
                    itemData.ICA = ica;
                    itemData.PCA = pca;
                    itemData.DateCreated = dateCreated;
                    itemData.DateLastUpdate = dateLastUpdate;
                    listData.Add(itemData);
                }
            }
            

            var statusNetwork = new TableDPPMUpdateModel.Status();
            statusNetwork.Code = 200;
            statusNetwork.Message = "Sukses";

            var listColumnDefs = new List<TableDPPMUpdateModel.ColumnDefs>();
            var listTarget = new List<int>();
            listTarget.Add(1);
            listTarget.Add(2);
            var columnDefs = new TableDPPMUpdateModel.ColumnDefs();
            columnDefs.Visible = false;
            columnDefs.Targets = listTarget;

            var responseJson = new TableDPPMUpdateModel.ResponseJson();
            responseJson.Status = statusNetwork;
            responseJson.Draw = draw;
            responseJson.RecordsFiltered = getData.Count();
            responseJson.RecordsTotal = getData.Count();
            responseJson.TotalNeedToRespond = getData.Count();
            responseJson.Data = listData;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}