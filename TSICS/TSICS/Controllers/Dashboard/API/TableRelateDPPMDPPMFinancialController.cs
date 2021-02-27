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
        // GET: TableRelateDPPMDPPMFinancial
        [HttpPost]
        public ActionResult TableRelateDPPMDPPMFinancial(FormCollection fc, string test, string cost, string freq, string dateRangeFrom, string dateRangeEnd, string hid, string inventory, string rental, string others, string PartNumber, string PartDescription, string Model, string PrefixSN, string download = "0")
        {
            var getData = PartResponBS.GetDataForTableRelateDPPMFinancial(cost, freq, fc["search[value]"]);
            var draw = (fc["draw"] != null) ? Convert.ToInt32(fc["draw"]) : 1;
            var listData = new List<TableRelateDPPMDPPMFinancialModel.Data>();
            if (download == "0")
            {
                Session["valueSearchRelateDPPM"] = fc["search[value]"];
                foreach (var item in getData.Skip(Convert.ToInt32(fc["start"])).Take(Convert.ToInt32(fc["length"])))
                {
                    var id = new TableRelateDPPMDPPMFinancialModel.Id();
                    id.Row = item.Row;
                    var dppmNo = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dppmNo.Value = item.DPPMNo;
                    dppmNo.Style = "";
                    var title = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "";
                    var desc = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    desc.Value = item.Desc;
                    desc.Style = "";
                    var statusData = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    statusData.Value = item.Status;
                    statusData.Style = "";
                    var submiter = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    submiter.Value = item.DealerContact;
                    submiter.Style = "";
                    var catReps = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    catReps.Value = item.CatReps;
                    catReps.Style = "";
                    var ica = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    ica.Value = item.ICA;
                    ica.Style = "";
                    var pca = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    pca.Value = item.PCA;
                    pca.Style = "";
                    var dateCreated = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dateCreated.Value = item.DateCreated.ToString();
                    dateCreated.Style = "";
                    var dateLastUpdate = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dateLastUpdate.Value = item.DateLastUpdate.ToString();
                    dateLastUpdate.Style = "";
                    var itemData = new TableRelateDPPMDPPMFinancialModel.Data();
                    itemData.Id = id;
                    itemData.DPPMNo = dppmNo;
                    itemData.Title = title;
                    itemData.Desc = desc;
                    itemData.Status = statusData;
                    itemData.Submiter = submiter;
                    itemData.CatReps = catReps;
                    itemData.ICA = ica;
                    itemData.PCA = pca;
                    itemData.DateCreated = dateCreated;
                    itemData.DateLastUpdate = dateLastUpdate;
                    listData.Add(itemData);
                }
            }
            if (download == "1")
            {
                var getSearchValue = Session["valueSearchRelateDPPM"].ToString();
                getData = PartResponBS.GetDataForTableRelateDPPMFinancial(cost, freq, getSearchValue);
                foreach (var item in getData)
                {
                    var id = new TableRelateDPPMDPPMFinancialModel.Id();
                    id.Row = item.Row;
                    var dppmNo = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dppmNo.Value = item.DPPMNo;
                    dppmNo.Style = "";
                    var title = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    title.Value = item.Title;
                    title.Style = "";
                    var desc = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    desc.Value = item.Desc;
                    desc.Style = "";
                    var statusData = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    statusData.Value = item.Status;
                    statusData.Style = "";
                    var submiter = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    submiter.Value = item.DealerContact;
                    submiter.Style = "";
                    var catReps = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    catReps.Value = item.CatReps;
                    catReps.Style = "";
                    var ica = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    ica.Value = item.ICA;
                    ica.Style = "";
                    var pca = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    pca.Value = item.PCA;
                    pca.Style = "";
                    var dateCreated = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dateCreated.Value = item.DateCreated.ToString();
                    dateCreated.Style = "";
                    var dateLastUpdate = new TableRelateDPPMDPPMFinancialModel.ValueData();
                    dateLastUpdate.Value = item.DateLastUpdate.ToString();
                    dateLastUpdate.Style = "";
                    var itemData = new TableRelateDPPMDPPMFinancialModel.Data();
                    itemData.Id = id;
                    itemData.DPPMNo = dppmNo;
                    itemData.Title = title;
                    itemData.Desc = desc;
                    itemData.Status = statusData;
                    itemData.Submiter = submiter;
                    itemData.CatReps = catReps;
                    itemData.ICA = ica;
                    itemData.PCA = pca;
                    itemData.DateCreated = dateCreated;
                    itemData.DateLastUpdate = dateLastUpdate;
                    listData.Add(itemData);
                }
            }
            

            var status = new TableRelateDPPMDPPMFinancialModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new TableRelateDPPMDPPMFinancialModel.ResponseJson();
            responseJson.Status = status;
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