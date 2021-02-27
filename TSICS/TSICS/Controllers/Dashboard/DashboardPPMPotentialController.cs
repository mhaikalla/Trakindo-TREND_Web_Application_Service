using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Trakindo.TSICS.Data.Model;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: DashboardPPMPotential
        public ActionResult DashboardPPMPotential()
        {
            this.setViewBag();
            var hid = string.Empty;
            var inventory = string.Empty;
            var rental = string.Empty;
            var others = string.Empty;
            var dateRangeFrom = "";
            var dateRangeEnd = "";
            ViewBag.DateFrom = dateRangeFrom;
            ViewBag.DateEnd = dateRangeEnd;
            var partNumber = string.Empty;
            var partDescription = string.Empty;
            var model = string.Empty;
            var prefixSN = string.Empty;

            var modelCollection = new GetFormCollectionDPPMPotential();
            modelCollection.DateRangeFrom = dateRangeFrom;
            modelCollection.DateRangeEnd = dateRangeEnd;
            modelCollection.HID = hid;
            modelCollection.Rental = rental;
            modelCollection.Others = others;
            modelCollection.Inventory = inventory;
            modelCollection.PartNumber = partNumber;
            modelCollection.PartDescription = partDescription;
            modelCollection.Model = model;
            modelCollection.PrefixSN = prefixSN;

            ViewBag.ModelCollectionPPMPotential = modelCollection;
          
            return View();
        }
         
        [HttpPost]
        public ActionResult DashboardPPMPotential(FormCollection fc)
        {
            this.setViewBag();
            var dateRangeFrom = "";
            var dateRangeEnd = "";
            if (fc["dateRange"] != null)
            {
                var strList = fc["dateRange"].Split(' ', 't', 'o', ' ');
                if (strList.Count() == 5)
                {
                    dateRangeFrom = strList[0];
                    dateRangeEnd = strList[4];
                }
                else
                {
                    dateRangeFrom = strList[0];
                }
            }
            else
            {
                dateRangeFrom = "";
                dateRangeEnd = "";
            }
            var hid = fc["hid"];
            var inventory = fc["inventory"];
            var rental = fc["rental"];
            var others = fc["others"];
            var partNumber = fc["part-number"];
            var partDescription = fc["part-description"];
            var model = fc["model"];
            var prefixSN = fc["prefix-sn"];
            ViewBag.DateFrom = dateRangeFrom;
            ViewBag.DateEnd = dateRangeEnd;
            var modelCollection = new GetFormCollectionDPPMPotential();
            modelCollection.DateRangeFrom = dateRangeFrom;
            modelCollection.DateRangeEnd = dateRangeEnd;
            modelCollection.HID = hid;
            modelCollection.Rental = rental;
            modelCollection.Others = others;
            modelCollection.Inventory = inventory;
            modelCollection.PartNumber = partNumber;
            modelCollection.PartDescription = partDescription;
            modelCollection.Model = model;
            modelCollection.PrefixSN = prefixSN;

            ViewBag.ModelCollectionPPMPotential = modelCollection;
            return View();
        }
    }
}