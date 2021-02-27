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
        // GET: ChartPPMPie
        public ActionResult ChartPPMPie(string type, string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            if(type == "status")
            {
                var backgroundColor = new List<string>();
                backgroundColor.Add("rgba(112, 124, 169, .8)");
                backgroundColor.Add("rgba(141, 168, 215, .8)");
                backgroundColor.Add("rgba(186, 205, 228, .8)");
                backgroundColor.Add("rgba(42, 178, 166, .8)");

                var dataDataSets = new List<decimal>();
                var getData = dppmBS.CountStatus(register, status, industry, tech_reps, dealer_contact);
                dataDataSets.Add(Math.Round(getData.StatusClose, 2));
                dataDataSets.Add(Math.Round(getData.StatusOpen, 2));
                dataDataSets.Add(Math.Round(getData.StatusIssuePending, 2));
                dataDataSets.Add(Math.Round(getData.StatusUnsubmitted, 2));

                var labels = new List<string>();
                labels.Add("Dealer Closed " + " (" + getData.CountStatusClose + " , "+ Math.Round(getData.StatusClose, 2) + "%" +" )");
                labels.Add("Dealer Open " + "( " + getData.CountStatusOpen + " , " + Math.Round(getData.StatusOpen, 2) + "%" + " )");
                labels.Add("Issue Pending " + "( " + getData.CountStatusIssuePending + " , " + Math.Round(getData.StatusIssuePending, 2) + "%" + " )");
                labels.Add("Unsubmitted " + "( " + getData.CountStatusUnsubmitted + " , " + Math.Round(getData.StatusUnsubmitted, 2) + "%" + " )");
                //labels.Add("Dealer Closed");
                //labels.Add("Dealer Open");
                //labels.Add("Issue Pending");
                //labels.Add("Unsubmitted");

                var dataSets = new ChartPPMUpdatePieModel.DataSets();
                dataSets.BackgroundColor = backgroundColor;
                dataSets.data = dataDataSets;

                var listDataSets = new List<ChartPPMUpdatePieModel.DataSets>();
                listDataSets.Add(dataSets);

                var pie = new ChartPPMUpdatePieModel.PIE();
                pie.Label = labels;
                pie.PercentLabel = true;
                pie.DataSets = listDataSets;

                var data = new ChartPPMUpdatePieModel.Data();
                data.Pie = pie;

                var statusNetwork = new ChartPPMUpdatePieModel.Status();
                statusNetwork.Code = 200;
                statusNetwork.Message = "Success";

                var responseJson = new ChartPPMUpdatePieModel.ResponseJson();
                responseJson.Data = data;
                responseJson.Status = statusNetwork;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            if(type == "industry")
            {

                var backgroundColor = new List<string>();
                backgroundColor.Add("rgba(112, 124, 169, .8)");
                backgroundColor.Add("rgba(141, 168, 215, .8)");
                backgroundColor.Add("rgba(186, 205, 228, .8)"); 
                backgroundColor.Add("rgba(42, 178, 166, .8)");
                backgroundColor.Add("rgb(44, 176, 163)");
                backgroundColor.Add("rgb(12, 238, 95)"); 

                var dataDataSets = new List<decimal>();
                var getData = dppmBS.CountIndustry(register, status, industry, tech_reps, dealer_contact);
                dataDataSets.Add(Math.Round(getData.CompactMachines, 2));
                dataDataSets.Add(Math.Round(getData.EarthMove, 2));
                dataDataSets.Add(Math.Round(getData.MarineEngAux, 2));
                dataDataSets.Add(Math.Round(getData.Mining, 2));
                dataDataSets.Add(Math.Round(getData.Blank, 2));
                dataDataSets.Add(Math.Round(getData.Other, 2));

                var labels = new List<string>();
                labels.Add("Small Off-Highway Trucks " + "( " + getData.CountCompactMachines + " , " + Math.Round(getData.CompactMachines, 2) + "%" + " )");
                labels.Add("Motor Graders " + "( " + getData.CountEarthMove + " , " + Math.Round(getData.EarthMove, 2) + "%" + " )");
                labels.Add("Medium Track Type Tractors " + "( " + getData.CountMarineEngAux + " , " + Math.Round(getData.MarineEngAux, 2) + "%" + " )");
                labels.Add("Rotary Drills " + "( " + getData.CountMining + " , " + Math.Round(getData.Mining, 2) + "%" + " )");
                labels.Add("Blank " + "( " + getData.CountBlank + " , " + Math.Round(getData.Blank, 2) + "%" + " )");
                labels.Add("Others " + "( " + getData.CountOther + " , " + Math.Round(getData.Other, 2) + "%" + " )");

                //labels.Add("Small Off-Highway Trucks");
                //labels.Add("Motor Graders");
                //labels.Add("Medium Track Type Tractors");
                //labels.Add("Rotary Drills");
                //labels.Add("(Blank)");
                //labels.Add("(Others)");

                var dataSets = new ChartPPMUpdatePieModel.DataSets();
                dataSets.BackgroundColor = backgroundColor;
                dataSets.data = dataDataSets;

                var listDataSets = new List<ChartPPMUpdatePieModel.DataSets>();
                listDataSets.Add(dataSets);

                var pie = new ChartPPMUpdatePieModel.PIE();
                pie.Label = labels;
                pie.PercentLabel = true;
                pie.DataSets = listDataSets;

                var data = new ChartPPMUpdatePieModel.Data();
                data.Pie = pie;

                var statusNetwork = new ChartPPMUpdatePieModel.Status();
                statusNetwork.Code = 200;
                statusNetwork.Message = "Success";

                var responseJson = new ChartPPMUpdatePieModel.ResponseJson();
                responseJson.Data = data;
                responseJson.Status = statusNetwork;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            if(type == "tech-reps")
            {

                var backgroundColor = new List<string>();
                backgroundColor.Add("rgba(112, 124, 169, .8)");
                backgroundColor.Add("rgba(141, 168, 215, .8)");
                backgroundColor.Add("rgba(186, 205, 228, .8)");
                backgroundColor.Add("rgba(42, 178, 166, .8)");
                backgroundColor.Add("rgb(44, 176, 163)");
                backgroundColor.Add("rgb(12, 238, 95)");

                var dataDataSets = new List<decimal>();
                var getData = dppmBS.CountTechReps(register, status, industry, tech_reps, dealer_contact);
                dataDataSets.Add(Math.Round(getData.ChenShuehSy,2));
                dataDataSets.Add(Math.Round(getData.ChouglanZaneStephen, 2));
                dataDataSets.Add(Math.Round(getData.JoniYohanis, 2));
                dataDataSets.Add(Math.Round(getData.TamaAntonFirman,2 ));
                dataDataSets.Add(Math.Round(getData.Blank, 2));
                dataDataSets.Add(Math.Round(getData.Other, 2));

                var labels = new List<string>();
                labels.Add("Chen, Shueh Sy " + "( " + getData.CountChenShuehSy + " , " + Math.Round(getData.ChenShuehSy, 2) + "%" + " )");
                labels.Add("Coughlan, Zane Stephen " + "( " + getData.CountChouglanZaneStephen + " , " + Math.Round(getData.ChouglanZaneStephen, 2) + "%" + " )");
                labels.Add("Joni, Yohanis " + "( " + getData.CountJoniYohanis + " , " + Math.Round(getData.JoniYohanis, 2) + "%" + " )");
                labels.Add("Tama, Anton Firman " + "( " + getData.CountTamaAntonFirman + " , " + Math.Round(getData.TamaAntonFirman, 2) + "%" + " )");
                labels.Add("Blank " + "( " + getData.CountBlank + " , " + Math.Round(getData.Blank, 2) + "%" + " )");
                labels.Add("Others " + "( " + getData.CountOther + " , " + Math.Round(getData.Other, 2) + "%" + " )");
                //labels.Add("Chen, Shueh Sy");
                //labels.Add("Coughlan, Zane Stephen");
                //labels.Add("Joni, Yohanis");
                //labels.Add("Tama, Anton Firman");
                //labels.Add("(Blank)");
                //labels.Add("(Others)");

                var dataSets = new ChartPPMUpdatePieModel.DataSets();
                dataSets.BackgroundColor = backgroundColor;
                dataSets.data = dataDataSets;

                var listDataSets = new List<ChartPPMUpdatePieModel.DataSets>();
                listDataSets.Add(dataSets);

                var pie = new ChartPPMUpdatePieModel.PIE();
                pie.Label = labels;
                pie.PercentLabel = true;
                pie.DataSets = listDataSets;

                var data = new ChartPPMUpdatePieModel.Data();
                data.Pie = pie;

                var statusNetwork = new ChartPPMUpdatePieModel.Status();
                statusNetwork.Code = 200;
                statusNetwork.Message = "Success";

                var responseJson = new ChartPPMUpdatePieModel.ResponseJson();
                responseJson.Data = data;
                responseJson.Status = statusNetwork;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            return View();
        }
    }
}