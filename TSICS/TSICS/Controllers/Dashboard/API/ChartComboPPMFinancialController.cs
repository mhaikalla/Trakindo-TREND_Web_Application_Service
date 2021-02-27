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
        // GET: ChartComboPPMFinancial
        public ActionResult ChartComboPPMFinancial(string hid, string rental, string inventory, string others, string dateRangeFrom, string dateRangeEnd, string PartNumber, string PartDescription, string ModelDashboard, string PrefixSN)
        {
            var splitPartNumber = new string[] { };
            var splitPartDescription = new string[] { };
            var splitModel = new string[] { };
            var splitPrefixSN = new string[] { };

            if (!string.IsNullOrWhiteSpace(PartNumber))
            {
                splitPartNumber = PartNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PartDescription))
            {
                splitPartDescription = PartDescription.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(ModelDashboard))
            {
                splitModel = ModelDashboard.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PrefixSN))
            {
                splitPrefixSN = PrefixSN.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PrefixSN))
            {
                splitPrefixSN = PrefixSN.Split(',');
            }
            var getListData = PartResponBS.GetData(splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, dateRangeFrom, dateRangeEnd, hid, rental, inventory, others);
            var listDataLabel = new List<List<string>>();
            var dataLine = new List<decimal>();
            var dataBar = new List<decimal>();
            foreach (var item in getListData)
            {
                listDataLabel.Add(item.Model);
                dataLine.Add(item.DataTotalCost);
                dataBar.Add(item.DataTotalSO);
            }

            var dataSets1 = new ChartComboFinancialModel.DataSets();
            dataSets1.Type = "line";
            dataSets1.Label = "Total Cost";
            dataSets1.BorderColor = "red";
            dataSets1.BackgroundColor = "";
            dataSets1.Data = dataLine;

            var dataSets2 = new ChartComboFinancialModel.DataSets();
            dataSets2.Type = "bar";
            dataSets2.Label = "Total SO";
            dataSets2.BackgroundColor = "#cfd20f";
            dataSets2.BorderColor = "";
            dataSets2.Data = dataBar;

            var listDataSet = new List<ChartComboFinancialModel.DataSets>();
            listDataSet.Add(dataSets1);
            listDataSet.Add(dataSets2);

            var comboBar = new ChartComboFinancialModel.ComboBar();
            comboBar.Labels = listDataLabel;
            comboBar.DataSets = listDataSet;

            var data = new ChartComboFinancialModel.Data();
            data.ComboBar = comboBar;

            var status = new ChartComboFinancialModel.Status();
            status.Code = 200;
            status.Message = "Success";

            var responseJson = new ChartComboFinancialModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}