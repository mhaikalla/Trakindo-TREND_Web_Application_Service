using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    partial class DashboardController
    {
        // GET: DashboardPPMFinancial
        public ActionResult DashboardPPMFinancial()
        {
            this.setViewBag();
            var strDateRangeFrom = string.Empty;
            var strDateRangeEnd = string.Empty;
            var strHID = string.Empty;
            var strInventory = string.Empty;
            var strRental = string.Empty;
            var partNumber = string.Empty;
            var partDescription = string.Empty;
            var model = string.Empty;
            var prefixSN = string.Empty;
            var dateFrom = "";
            var dateEnd = "";
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateEnd = dateEnd;
            var strPartNo = new string[] { };
            var strPartDesc = new string[] { };
            var strModel = new string[] { };
            var strPrefixSN = new string[] { };
            var others = string.Empty;
            var modelFormCollection = new GetFormCollectionPPMFinancial();
            modelFormCollection.DateRangeFrom = strDateRangeFrom;
            modelFormCollection.DateRangeEnd = strDateRangeEnd;
            modelFormCollection.HID = strHID;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.PartNumber = partNumber;
            modelFormCollection.PartDescription = partDescription;
            modelFormCollection.Model = model;
            modelFormCollection.PrefixSN = prefixSN;
            modelFormCollection.Others = others;
            ViewBag.ModelFormCollectionPPM = modelFormCollection;
            ViewBag.ListPartNumber = PartResponBS.GetListPartNo();
            ViewBag.ListModel = PartResponBS.GetListModel();
            ViewBag.ListPartDescription = PartResponBS.GetListPartDesc();
            ViewBag.ListPrefixSN = PartResponBS.GetListPrefixSN();
            if(strPartNo.Count() > 0 || strPartDesc.Count() > 0 || strModel.Count() > 0 || strPrefixSN.Count() > 0 || dateFrom != "" || strHID != "" || strInventory != "" || strRental != "" || others != "")
            {
                var sumdataFinance = PartResponBS.CountTotalSODPPMFinance(strPartNo, strPartDesc, strModel, strPrefixSN, dateFrom, dateEnd, strHID, strInventory, strRental, others);
                ViewBag.TotalSOCost = sumdataFinance.TotalServiceOrderCost.ToString("N2");
                ViewBag.TotalUnitImpacted = sumdataFinance.TotalUnitImpacted;
                ViewBag.FinancialSummary = sumdataFinance.FinancialSummary.ToString("N2");
                ViewBag.QuantitySummary = sumdataFinance.QuantitySummary;
            }
            else
            {
                ViewBag.TotalSOCost = 0;
                ViewBag.TotalUnitImpacted = 0;
                ViewBag.FinancialSummary = 0;
                ViewBag.QuantitySummary = 0;
            }
            return View();
        }

        [HttpPost]
        public ActionResult DashboardPPMFinancial(FormCollection formCollection)
        {
            this.setViewBag();
            var dateRangeForm = "";
            var dateRangeEnd = "";
            if(formCollection["dateRange"] != null)
            {
                var splitDate = formCollection["dateRange"].Split(' ', 't', 'o', ' ');
                if(splitDate.Count() == 5)
                {
                    dateRangeForm = splitDate[0];
                    dateRangeEnd = splitDate[4];
                }
                else
                {
                    dateRangeForm = splitDate[0];
                }
            }

            var strDateRangeFrom = (!string.IsNullOrWhiteSpace(dateRangeForm)) ? dateRangeForm : "";
            var strDateRangeEnd = (!string.IsNullOrWhiteSpace(dateRangeEnd)) ? dateRangeEnd : "";
            var strHID = (!string.IsNullOrWhiteSpace(formCollection["hid"])) ? formCollection["hid"] : "";
            var strInventory = (!string.IsNullOrWhiteSpace(formCollection["inventory"])) ? formCollection["inventory"] : "";
            var strRental = (!string.IsNullOrWhiteSpace(formCollection["rental"])) ? formCollection["rental"] : "";
            var strOthers = (!string.IsNullOrWhiteSpace(formCollection["others"])) ? formCollection["others"] : "";
            var partNumber = (!string.IsNullOrWhiteSpace(formCollection["part-number"])) ? formCollection["part-number"] : "";
            var partDescription = (!string.IsNullOrWhiteSpace(formCollection["part-description"])) ? formCollection["part-description"] : "";
            var model = (!string.IsNullOrWhiteSpace(formCollection["model"])) ? formCollection["model"] : "";
            var prefixSN = (!string.IsNullOrWhiteSpace(formCollection["prefix-sn"])) ? formCollection["prefix-sn"] : "";
            var splitPartNumber = new string[] { };
            var splitPartDescription = new string[] { };
            var splitModel = new string[] { };
            var splitPrefixSN = new string[] { };

            if (!string.IsNullOrWhiteSpace(partNumber))
            {
                splitPartNumber = partNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(partDescription))
            {
                splitPartDescription = partDescription.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(model))
            {
                splitModel = model.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(prefixSN))
            {
                splitPrefixSN = prefixSN.Split(',');
            }
            ViewBag.DateFrom = strDateRangeFrom;
            ViewBag.DateEnd = strDateRangeEnd;
            var modelFormCollection = new GetFormCollectionPPMFinancial(); 
            modelFormCollection.DateRangeFrom = strDateRangeFrom;
            modelFormCollection.DateRangeEnd = strDateRangeEnd;
            modelFormCollection.HID = strHID;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.PartNumber = partNumber;
            modelFormCollection.PartDescription = partDescription;
            modelFormCollection.Model = model;
            modelFormCollection.PrefixSN = prefixSN;
            modelFormCollection.Others = strOthers;
            ViewBag.ModelFormCollectionPPM = modelFormCollection;
            ViewBag.ListPartNumber = PartResponBS.GetListPartNo();
            ViewBag.ListModel = PartResponBS.GetListModel();
            ViewBag.ListPartDescription = PartResponBS.GetListPartDesc();
            ViewBag.ListPrefixSN = PartResponBS.GetListPrefixSN();
            if (splitPartNumber.Count() > 0 || splitPartDescription.Count() > 0 || splitModel.Count() > 0 || splitPrefixSN.Count() > 0 || dateRangeForm != "" || strHID != "" || strInventory != "" || strRental != "" || strOthers != "")
            {
                var sumdataFinance = PartResponBS.CountTotalSODPPMFinance(splitPartNumber, splitPartDescription, splitModel, splitPrefixSN, dateRangeForm, dateRangeEnd, strHID, strInventory, strRental, strOthers);
                ViewBag.TotalSOCost = sumdataFinance.TotalServiceOrderCost.ToString("N2");
                ViewBag.TotalUnitImpacted = sumdataFinance.TotalUnitImpacted;
                ViewBag.FinancialSummary = sumdataFinance.FinancialSummary.ToString("N2");
                ViewBag.QuantitySummary = sumdataFinance.QuantitySummary;
            }
            else
            {
                ViewBag.TotalSOCost = 0;
                ViewBag.TotalUnitImpacted = 0;
                ViewBag.FinancialSummary = 0;
                ViewBag.QuantitySummary = 0;
            }
            return View();
        }
    }
}