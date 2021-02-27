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
using Com.Trakindo.TSICS.Data.Model;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        [HttpPost]
        public Tuple<List<DPPMSummary>, int> GetTableRelatedDPPMOverview(FormCollection collection, string Hid, string Rental, string Others, string paramsModel=null, string paramsSerialNumber=null, bool btnPOST = false)
        {
            string[] SplitArea = new string[] { };
            string[] SplitCustomer = new string[] { };
            string[] SplitFamily = new string[] { };
            string[] SplitIndustry = new string[] { };
            string[] SplitSalesOffice = new string[] { };
            string[] SplitModel = new string[] { };
            string[] SplitSerialNumber = new string[] { };
            string[] SplitProduct = new string[] { };
            string[] SplitPlant = new string[] { };
            var purchasedatefrom = "";
            var purchasedateto = "";
            var deliverydatefrom = "";
            var deliverydateto = "";
            var repairdatefrom = "";
            var repairdateto = "";
            if (Convert.ToInt32(collection["filterType"]) == 1)
            {
                if (!string.IsNullOrWhiteSpace(collection["SerialNumber"]))
                {
                    SplitSerialNumber = collection["SerialNumber"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Customer"]))
                {
                    SplitCustomer = collection["Customer"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Model"]))
                {
                    SplitModel = collection["Model"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Product"]))
                {
                    SplitProduct = collection["Product"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["PlantDescription"]))
                {
                    SplitPlant = collection["PlantDescription"].Split(',');
                }
            }
            else
            {
                
                if (collection["purchaseDate"] != null)
                {
                    var strList = collection["purchaseDate"].Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        purchasedatefrom = strList[0];
                        purchasedateto = strList[4];
                    }
                    else
                    {
                        purchasedatefrom = strList[0];
                    }
                }
                else
                {
                    purchasedatefrom = "";
                    purchasedateto = "";
                }

                if (collection["deliveryDate"] != null)
                {
                    var strList = collection["deliveryDate"].Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        deliverydatefrom = strList[0];
                        deliverydateto = strList[4];
                    }
                    else
                    {
                        deliverydatefrom = strList[0];
                    }
                }
                else
                {
                    deliverydatefrom = "";
                    deliverydateto = "";
                }

                if (collection["repairDate"] != null)
                {
                    var strList = collection["repairDate"].Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        repairdatefrom = strList[0];
                        repairdateto = strList[4];
                    }
                    else
                    {
                        repairdatefrom = strList[0];
                    }
                }
                else
                {
                    repairdatefrom = "";
                    repairdateto = "";
                }

                if (!string.IsNullOrWhiteSpace(collection["SerialNumberMEP"]))
                {
                    SplitSerialNumber = collection["SerialNumberMEP"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["CustomerMEP"]))
                {
                    SplitCustomer = collection["CustomerMEP"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["ModelMEP"]))
                {
                    SplitModel = collection["ModelMEP"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Area"]))
                {
                   SplitArea = collection["Area"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["SalesOffice"]))
                {
                    SplitSalesOffice = collection["SalesOffice"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Family"]))
                {
                    SplitFamily = collection["Family"].Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection["Industry"]))
                {
                    SplitIndustry = collection["Industry"].Split(',');
                }

            }
            string[] dataFiltered = getListSerialNumberFiltered(Convert.ToInt32(collection["filterType"]), SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily, SplitIndustry, SplitModel, SplitSerialNumber, repairdatefrom, repairdateto, purchasedatefrom, purchasedateto, deliverydatefrom, deliverydateto, collection["SMURangeFrom"], collection["SMURangeTo"], SplitProduct, SplitPlant, Hid, Rental, Others, paramsModel, paramsSerialNumber, btnPOST);
          
            var getData = _DPPMSummaryBs.getDataDPPMSummaryOverview(dataFiltered);
            return Tuple.Create(getData.Item1, getData.Item2);
        }

        public Tuple<List<DPPMSummary>, int> GetTableRelatedDPPMOverviewGet(GetFormCollectionOverview collection, string Hid, string Rental, string Others, string paramsModel=null, string paramsSerialNumber=null, bool btnPOST = false)
        {
            string[] SplitArea = new string[] { };
            string[] SplitCustomer = new string[] { };
            string[] SplitFamily = new string[] { };
            string[] SplitIndustry = new string[] { };
            string[] SplitSalesOffice = new string[] { };
            string[] SplitModel = new string[] { };
            string[] SplitSerialNumber = new string[] { };
            string[] SplitProduct = new string[] { };
            string[] SplitPlant = new string[] { };
            var purchasedatefrom = "";
            var purchasedateto = "";
            var deliverydatefrom = "";
            var deliverydateto = "";
            var repairdatefrom = "";
            var repairdateto = "";
            if (collection.FilterType == 1)
            {
                if (!string.IsNullOrWhiteSpace(collection.SerialNumber))
                {
                    SplitSerialNumber = collection.SerialNumber.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Customer_Name))
                {
                    SplitCustomer = collection.Customer_Name.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Model))
                {
                    SplitModel = collection.Model.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Product))
                {
                    SplitProduct = collection.Product.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Product))
                {
                    SplitPlant = collection.Product.Split(',');
                }
            }
            else
            {

                if (collection.PurchaseDateEnd != null && collection.PurchaseDateFrom != null)
                {
                    var strList = collection.PurchaseDateFrom.Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        purchasedatefrom = strList[0];
                        purchasedateto = strList[4];
                    }
                    else
                    {
                        purchasedatefrom = strList[0];
                    }
                }
                else
                {
                    purchasedatefrom = "";
                    purchasedateto = "";
                }

                if (collection.DeliveryDateFrom != null)
                {
                    var strList = collection.DeliveryDateFrom.Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        deliverydatefrom = strList[0];
                        deliverydateto = strList[4];
                    }
                    else
                    {
                        deliverydatefrom = strList[0];
                    }
                }
                else
                {
                    deliverydatefrom = "";
                    deliverydateto = "";
                }

                if (collection.LastRepairDateFrom != null)
                {
                    var strList = collection.LastRepairDateFrom.Split(' ', 't', 'o', ' ');
                    if (strList.Count() == 5)
                    {
                        repairdatefrom = strList[0];
                        repairdateto = strList[4];
                    }
                    else
                    {
                        repairdatefrom = strList[0];
                    }
                }
                else
                {
                    repairdatefrom = "";
                    repairdateto = "";
                }

                if (!string.IsNullOrWhiteSpace(collection.SerialNumber))
                {
                    SplitSerialNumber = collection.SerialNumber.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Customer))
                {
                    SplitCustomer = collection.Customer.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Model))
                {
                    SplitModel = collection.Model.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Area))
                {
                    SplitArea = collection.Area.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.SalesOffice))
                {
                    SplitSalesOffice = collection.SalesOffice.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Family))
                {
                    SplitFamily = collection.Family.Split(',');
                }
                if (!string.IsNullOrWhiteSpace(collection.Industry))
                {
                    SplitIndustry = collection.Industry.Split(',');
                }
            }
            string[] dataFiltered = getListSerialNumberFiltered(collection.FilterType, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitModel, SplitSerialNumber, repairdatefrom, repairdateto, purchasedatefrom, purchasedateto, deliverydatefrom, deliverydateto, collection.SMURangeFrom.ToString(), collection.SMURangeTo.ToString(), SplitProduct, SplitPlant, Hid, Rental, Others, paramsModel, paramsSerialNumber, btnPOST);

            var getData = _DPPMSummaryBs.getDataDPPMSummaryOverview(dataFiltered);
            return Tuple.Create(getData.Item1, getData.Item2);
        }
    }
}