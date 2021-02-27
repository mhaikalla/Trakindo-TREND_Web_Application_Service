using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using TSICS.Helper;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController 
    {
        public string LimitforSerialNumberFiltered = "";

        // GET: DashboardTechnicalOverview
        public ActionResult DashboardTechnicalOverview(int filterType = 0, string Area = null, string Model = null, string Family = null, string Industry = null, string SerialNumber = null, string Customer = null, string SalesOffice = null, string deliveryDate = null, string purchaseDate = null, string repairDate = null, string SMURangeFrom = null, string SMURangeTo = null, string ModelMEP = null, string SerialNumberMEP = null, string CustomerMEP = null, string Plant = null, string Product = null, string hid = null, string rental = null, string other = null, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST = false)
        {  if (Session["userid"] == null)
           {
               return RedirectToAction("Login", "Account");
           }
           else
           {
               if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
               {
                   return RedirectToAction("Login", "Account");
               }
                this.setViewBag();
                var modelFormCollection = new GetFormCollectionOverview();
                var strArea = Area;          
                var strSalesOffice = SalesOffice;
                var strCustomer = CustomerMEP;
                var strFamily = Family;
                var strIndustry = Industry;
                var strModel = ModelMEP;
                var strSerialNumber = SerialNumberMEP;
                var strSMURangeFrom = SMURangeFrom;
                var strSMURangeTo = SMURangeTo;
                var strpurchaseDateFrom = String.Empty;
                var strdeliveryDateFrom = String.Empty;
                var strrepairDateFrom =String.Empty;
                var strpurchaseDateEnd = String.Empty;
                var strdeliveryDateEnd = String.Empty;
                var strrepairDateEnd= String.Empty;
                var Hid = hid;
                var Rental = rental;
                var Others = other;
                var strcustomer = "";
                var strmodel = "";
                var strserialnumber = "";
                var strplant = "";
                var strproduct = "";
                if (filterType == 1)
                {
                    strcustomer = Customer;
                    strmodel = Model;
                    strserialnumber = SerialNumber;
                    strplant = Plant;
                    strproduct = Product;
                }
                else
                {
                    strcustomer = CustomerMEP;
                    strmodel = ModelMEP;
                    strserialnumber = SerialNumberMEP;
                    if (purchaseDate != null)
                    {
                        var strList = purchaseDate.Split(' ', 't', 'o', ' ');
                        if (strList.Count() == 5)
                        {
                            strpurchaseDateFrom = strList[0];
                            strpurchaseDateEnd = strList[4];

                        }
                        else
                        {
                        strpurchaseDateFrom = strList[0];
                        }
                    }
                    else
                    {
                        strpurchaseDateFrom = String.Empty;
                        strpurchaseDateEnd = String.Empty;
                    }

                    if (deliveryDate != null)
                    {
                        var strList = deliveryDate.Split(' ', 't', 'o', ' ');
                        if (strList.Count() == 5)
                        {
                            strdeliveryDateFrom = strList[0];
                            strdeliveryDateEnd = strList[4];
                        }
                        else
                        {
                            strdeliveryDateFrom = strList[0];
                        }
                    }
                    else
                    {
                        strdeliveryDateFrom = "";
                        strdeliveryDateEnd = "";
                    }

                    if (repairDate != null)
                    {
                        var strList = repairDate.Split(' ', 't', 'o', ' ');
                        if (strList.Count() == 5)
                        {
                            strrepairDateFrom = strList[0];
                            strrepairDateEnd = strList[4];
                        }
                        else
                        {
                            strrepairDateFrom = strList[0];
                        }
                    }
                    else
                    {
                        strrepairDateFrom = String.Empty;
                        strrepairDateEnd = String.Empty;
                    }
                }

                modelFormCollection.HID = Hid;
                modelFormCollection.Rental = Rental;
                modelFormCollection.Others = Others;
                modelFormCollection.Area = strArea;
                modelFormCollection.SalesOffice = strSalesOffice;
                modelFormCollection.Customer = strCustomer;
                modelFormCollection.Family = strFamily;
                modelFormCollection.Industry = strIndustry;
                modelFormCollection.Model = strModel;
                modelFormCollection.SerialNumber = strSerialNumber;
                modelFormCollection.SMURangeFrom = String.IsNullOrWhiteSpace(strSMURangeFrom) ? 0 : Convert.ToInt32(strSMURangeFrom);
                modelFormCollection.SMURangeTo = String.IsNullOrWhiteSpace(strSMURangeTo) ? 0 : Convert.ToInt32(strSMURangeTo);
                modelFormCollection.PurchaseDateFrom = strpurchaseDateFrom;
                modelFormCollection.DeliveryDateFrom = strdeliveryDateFrom;
                modelFormCollection.LastRepairDateFrom = strrepairDateFrom;
                modelFormCollection.PurchaseDateEnd = strpurchaseDateEnd;
                modelFormCollection.DeliveryDateEnd= strdeliveryDateEnd;
                modelFormCollection.LastRepairDateEnd = strrepairDateEnd;
                modelFormCollection.Customer = strcustomer;
                modelFormCollection.Model = strmodel;
                modelFormCollection.SerialNumber = strserialnumber;
                modelFormCollection.Product = strproduct;
                modelFormCollection.Plant = strplant;

                ViewBag.FilterType = filterType;
                ViewBag.urlModel = WebConfigure.GetDomain() + "/Dashboard/DashboardTechnicalOverview?filterType=" + filterType
                   + "&Area=" + Area
                   + "&Model=" + Model
                   + "&Family=" + Family
                   + "&Industry=" + Industry
                   + "&SerialNumber=" + SerialNumber
                   + "&Customer=" + Customer
                   + "&SalesOffice=" + SalesOffice
                   + "&deliveryDate=" + deliveryDate
                   + "&purchaseDate=" + purchaseDate
                   + "&repairDate=" + repairDate
                   + "&SMURangeFrom=" + SMURangeFrom
                   + "&SMURangeTo=" + SMURangeTo
                   + "&ModelMEP=" + ModelMEP
                   + "&SerialNumberMEP=" + SerialNumberMEP
                   + "&CustomerMEP=" + CustomerMEP
                   + "&Plant=" + Plant
                   + "&Product=" + Product
                   + "&hid=" + hid
                   + "&rental=" + rental
                   + "&other=" + other
                   + "&btnPOST=" + btnPOST
                   ;

                var ModelParams = "";
                var SerialNumberParams = "";
                if (!String.IsNullOrWhiteSpace(paramsModel))
                {
                    ModelParams = paramsModel;
                    modelFormCollection.paramsModel = ModelParams;
                    ViewBag.Model = ModelParams;
                }
                if (!String.IsNullOrWhiteSpace(paramsSerialNumber))
                {
                    SerialNumberParams = paramsSerialNumber;
                    modelFormCollection.paramsSerialNumber = SerialNumberParams;
                    ViewBag.SerialNumber = SerialNumberParams;
                }
                var RelatedDPPM = GetTableRelatedDPPMOverviewGet(modelFormCollection, Hid, Rental, Others, modelFormCollection.paramsModel, modelFormCollection.paramsSerialNumber, btnPOST);
                var RelatedTR = getListTableRelatedTROverviewGet(modelFormCollection, Hid, Rental, Others, modelFormCollection.paramsModel, modelFormCollection.paramsSerialNumber, btnPOST);
                ViewBag.RelatedDPPMData = RelatedDPPM.Item1;
                ViewBag.RelatedDPPMDataCount = RelatedDPPM.Item2;
                ViewBag.TicketDataOverview = RelatedTR.Item1;
                ViewBag.TicketDataCountOverview = RelatedTR.Item2;
                ViewBag.FilterType = filterType;
                ViewBag.ModelFormCollectionOverview = modelFormCollection;
                return View();
            }
        }
        [HttpPost]
        public ActionResult DashboardTechnicalOverview(FormCollection fc)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }
                this.setViewBag();
                var modelFormCollection = new GetFormCollectionOverview();

                bool btnPOST = true;
                ViewBag.urlModel = WebConfigure.GetDomain() + "/Dashboard/DashboardTechnicalOverview?filterType=" + fc["filterType"]
                    + "&Area=" + fc["Area"]
                    + "&Model=" + fc["Model"]
                    + "&Family=" + fc["Family"]
                     + "&Industry=" + fc["Industry"]
                    + "&SerialNumber=" + fc["SerialNumber"]
                    + "&Customer=" + fc["Customer"]
                    + "&SalesOffice=" + fc["SalesOffice"]
                    + "&deliveryDate=" + fc["deliveryDate"]
                    + "&purchaseDate=" + fc["purchaseDate"]
                    + "&repairDate=" + fc["repairDate"]
                    + "&SMURangeFrom=" + fc["SMURangeFrom"]
                    + "&SMURangeTo=" + fc["SMURangeTo"]
                    + "&ModelMEP=" + fc["ModelMEP"]
                    + "&SerialNumberMEP=" + fc["SerialNumberMEP"]
                    + "&CustomerMEP=" + fc["CustomerMEP"]
                    + "&Plant=" + fc["PlantDescription"]
                    + "&Product=" + fc["Product"]
                    + "&hid=" + fc["hid"]
                    + "&rental=" + fc["rental"]
                    + "&other=" + fc["other"]
                    + "&btnPOST=" + btnPOST
                    ;


                if (Convert.ToInt32(fc["filterType"]) == 1)
                {
                    var strSerialNumber = (fc["SerialNumber"] != null) ? fc["SerialNumber"] : String.Empty;
                    var strCustomer = (fc["Customer"] != null) ? fc["Customer"] : String.Empty;
                    var strModel = (fc["Model"] != null) ? fc["Model"] : String.Empty;
                    var strplantDescription = (fc["PlantDescription"] != null) ? fc["PlantDescription"] : String.Empty;
                    var strplantProduct = (fc["Product"] != null) ? fc["Product"] : String.Empty;
                    modelFormCollection.Customer = strCustomer;
                    modelFormCollection.Model = strModel;
                    modelFormCollection.SerialNumber = strSerialNumber;
                    modelFormCollection.Plant = strplantDescription;
                    modelFormCollection.Product = strplantProduct;
                }
                else
                {
                    var purchasedatefrom = "";
                    var purchasedateto = "";
                    var deliverydatefrom = "";
                    var deliverydateto = "";
                    var repairdatefrom = "";
                    var repairdateto = "";
                    if (fc["purchaseDate"] != null)
                    {
                        var strList = fc["purchaseDate"].Split(' ', 't', 'o', ' ');
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

                    if (fc["deliveryDate"] != null)
                    {
                        var strList = fc["deliveryDate"].Split(' ', 't', 'o', ' ');
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

                    if (fc["repairDate"] != null)
                    {
                        var strList = fc["repairDate"].Split(' ', 't', 'o', ' ');
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
                    var strArea = (fc["Area"] != null) ? fc["Area"] : String.Empty;
                    var strSalesOffice = (fc["SalesOffice"] != null) ? fc["SalesOffice"] : String.Empty;
                    var strCustomer = (fc["CustomerMEP"] != null) ? fc["CustomerMEP"] : String.Empty;
                    var strFamily = (fc["Family"] != null) ? fc["Family"] : String.Empty;
                    var strIndustry = (fc["Industry"] != null) ? fc["Industry"] : String.Empty;
                    var strModel = (fc["ModelMEP"] != null) ? fc["ModelMEP"] : String.Empty;
                    var strSerialNumber = (fc["SerialNumberMEP"] != null) ? fc["SerialNumberMEP"] : String.Empty;
                    var strSMURangeFrom = (!String.IsNullOrWhiteSpace(fc["SMURangeFrom"])) ? Convert.ToInt32(fc["SMURangeFrom"]) : 0;
                    var strSMURangeTo = (!String.IsNullOrWhiteSpace(fc["SMURangeTo"])) ? Convert.ToInt32(fc["SMURangeTo"]) : 0;

                    modelFormCollection.Area = strArea;
                    modelFormCollection.SalesOffice = strSalesOffice;
                    modelFormCollection.Customer = strCustomer;
                    modelFormCollection.Family = strFamily;
                    modelFormCollection.Industry = strIndustry;
                    modelFormCollection.Model = strModel;
                    modelFormCollection.SerialNumber = strSerialNumber;
                    modelFormCollection.SMURangeFrom = strSMURangeFrom;
                    modelFormCollection.SMURangeTo = strSMURangeTo;
                    modelFormCollection.PurchaseDateFrom = purchasedatefrom;
                    modelFormCollection.PurchaseDateEnd = purchasedateto;
                    modelFormCollection.DeliveryDateEnd = deliverydateto;
                    modelFormCollection.DeliveryDateFrom = deliverydatefrom;
                    modelFormCollection.LastRepairDateEnd = repairdateto;
                    modelFormCollection.LastRepairDateFrom = repairdatefrom;
                }
                var Hid = (fc["hid"] != null) ? fc["hid"] : "";
                var Rental = (fc["rental"] != null) ? fc["rental"] : "";
                var Others = (fc["other"] != null) ? fc["other"] : "";
                modelFormCollection.HID = Hid;
                modelFormCollection.Rental = Rental;
                modelFormCollection.Others = Others;
                modelFormCollection.paramsModel = String.Empty;
                modelFormCollection.paramsSerialNumber = String.Empty;
                modelFormCollection.btnPOST = btnPOST;
                var DataRelatedTicketOverview = getListTableRelatedTROverview(fc, Hid, Rental, Others, fc["model"], null, btnPOST);
                var DataRelatedDPPMOverview = GetTableRelatedDPPMOverview(fc, Hid, Rental, Others, fc["model"], null, btnPOST);
                ViewBag.TicketDataOverview = DataRelatedTicketOverview.Item1;
                ViewBag.TicketDataCountOverview = DataRelatedTicketOverview.Item2;
                ViewBag.RelatedDPPMData = DataRelatedDPPMOverview.Item1;
                ViewBag.RelatedDPPMDataCount = DataRelatedDPPMOverview.Item2;
                ViewBag.ModelFormCollectionOverview = modelFormCollection;
                ViewBag.FilterType = Convert.ToInt32(fc["filterType"]);
              
                return View();
            }
        }
        #region //===========|    MEP    |=========================================

        public JsonResult GetAreaOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetAreaOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.label = item.Area;
                Data.value = item.Area;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSalesOfficeOverview(string filter, string area = null, string salesoffice = null, string customer = null, string family = null, string model = null, string serialnumber = null, int smurange = 0, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetSalesOfficeOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.LOC;
                Data.label = item.LOC;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetCustomerMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetCustomerMEPOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Customer_Name;
                Data.label = item.Customer_Name;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetIndustryOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetIndustryOverview(filter);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.DefinitionIndustry;
                Data.label = item.DefinitionIndustry;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetFamilyOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetFamilyOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Product_Family;
                Data.label = item.Product_Family;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetModelMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetModelMEPOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Product_Model;
                Data.label = item.Product_Model;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSerialNumberMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string smurange, string rental = null, string hid = null, string other = null)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = V_TR_MEP_Bs.GetSerialNumberMEPOverview(filter, area, salesoffice, customer, family, model, serialnumber, rental, hid, other);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Serial_Number;
                Data.label = item.Serial_Number;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region //===========|    Inventory     |=====================================================================
        [HttpGet]
        public JsonResult GetCustomerOverview(string filter, string customer, string model, string serialnumber, string product, string plant)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = _InventoryWeeklyBs.GetCustomerOverview(filter, customer, model, product, plant, serialnumber);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Customer_Name;
                Data.label = item.Customer_Name;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetModelOverview(string filter, string customer, string model, string serialnumber, string product, string plant)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = _InventoryWeeklyBs.GetModelOverview(filter, customer, model, product, plant, serialnumber);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Model;
                Data.label = item.Model;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSNOverview(string filter, string customer, string model, string serialnumber, string product, string plant)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = _InventoryWeeklyBs.GetSerialNumberOverview(filter, customer, model, product, plant, serialnumber);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Serial_Number;
                Data.label = item.Serial_Number;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetPlantOverview(string filter, string customer, string model, string serialnumber, string product, string plant)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = _InventoryWeeklyBs.GetPlantOverview(filter, customer, model, product, plant, serialnumber);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Plant_Desc;
                Data.label = item.Plant_Desc;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetProductOverview(string filter, string customer, string model, string serialnumber, string product, string plant)
        {
            var RespondJson = new AutoSuggestOverviewModel.ResponseJson();
            var Status = new AutoSuggestOverviewModel.status(); ;

            var ListData = new List<AutoSuggestOverviewModel.data>();
            Status.code = 200;
            Status.message = "Success";
            var Result = _InventoryWeeklyBs.GetProductOverview(filter, customer, model, product, plant, serialnumber);
            foreach (var item in Result)
            {
                var Data = new AutoSuggestOverviewModel.data();
                Data.value = item.Product;
                Data.label = item.Product;
                Data.style = "";
                ListData.Add(Data);
            }
            RespondJson.status = Status;
            RespondJson.data = ListData;
            return Json(RespondJson, JsonRequestBehavior.AllowGet);

        }
        #endregion

        public string[] getListSerialNumberFiltered(int type, string[] area = null, string[] salesoffice = null, string[] customer = null, string[] family = null, string[] industry=null,string[] model = null, string[] serialnumber = null, string repairdatefrom = null, string repairdateto = null, string purchasedatefrom = null, string purchasedateto = null, string deliverydatefrom = null, string deliverydateto = null, string smurangefrom = null, string smurangeto = null, string[] product = null, string[] plant = null, string Hid = "", string Rental = "", string Others = "", string paramsModel = null, string paramsSerialNumber = null, bool btnPOST =false)
        {
            string[] getFinalData;
            if (type == 1)
            {
                getFinalData = _InventoryWeeklyBs.GetSerialNumberFiltered(LimitforSerialNumberFiltered, customer, model, serialnumber, plant, product, paramsModel, paramsSerialNumber).Select(item => item.Serial_Number).Distinct().ToArray();
            }
            else
            {
                if (area.Count() > 0 || salesoffice.Count() > 0 || customer.Count() > 0 || family.Count() > 0 || industry.Count() > 0 || model.Count() > 0 || serialnumber.Count() > 0 || repairdatefrom != "" || repairdateto != "" || purchasedatefrom != "" || purchasedateto != "" || deliverydatefrom != "" || repairdateto != "" || (!String.IsNullOrWhiteSpace(smurangefrom) && smurangefrom != "0") || (!String.IsNullOrWhiteSpace(smurangeto) && smurangeto != "0") || !String.IsNullOrWhiteSpace(Hid) || !String.IsNullOrWhiteSpace(Rental) || !String.IsNullOrWhiteSpace(Others) || !String.IsNullOrWhiteSpace(paramsModel) || !String.IsNullOrWhiteSpace(paramsSerialNumber))
                {                    
                    getFinalData = V_TR_MEP_Bs.GetSerialNumberFiltered(LimitforSerialNumberFiltered, area, salesoffice, customer, family,industry, model, serialnumber, repairdatefrom, repairdateto, purchasedatefrom, purchasedateto, deliverydatefrom, deliverydateto, smurangefrom, smurangeto, Hid, Rental, Others, paramsModel, paramsSerialNumber).Select(item => item.Serial_Number).ToArray();
                }
                else
                {
                    if (btnPOST == false)
                    {
                        getFinalData = V_TR_MEP_Bs.GetSerialNumberFiltered("0", area, salesoffice, customer, family, industry, model, serialnumber, repairdatefrom, repairdateto, purchasedatefrom, purchasedateto, deliverydatefrom, deliverydateto, smurangefrom, smurangeto, Hid, Rental, Others, paramsModel, paramsSerialNumber).Select(item => item.Serial_Number).Take(0).ToArray();
                    }
                    else
                    {
                        getFinalData = V_TR_MEP_Bs.GetSerialNumberFiltered(LimitforSerialNumberFiltered, area, salesoffice, customer, family, industry, model, serialnumber, repairdatefrom, repairdateto, purchasedatefrom, purchasedateto, deliverydatefrom, deliverydateto, smurangefrom, smurangeto, Hid, Rental, Others, paramsModel, paramsSerialNumber).Select(item => item.Serial_Number).ToArray();
                    }
                }
               
            }
            return getFinalData;
        }
    }
}
