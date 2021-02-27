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
        [HttpPost]
        public ActionResult GetTicketOverview(FormCollection collection, int type, string Area, string SalesOffice, string Customer, string Family, string Industry, string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Other, string paramsModel = null, string paramsSerialNumber=null, bool btnPOST = false, int download = 0, String[] columns = null)
        {
            var orderByColumn = collection["columns[" + collection["order[0][column]"] + "][data]"];
            var orderByDir = collection["order[0][dir]"];
            var isDistinct = true;
            var start = Convert.ToInt32(collection["start"]);
            var length = Convert.ToInt32(collection["length"]);
            string[] SplitArea = new string[] { };
            string[] SplitCustomer = new string[] { };
            string[] SplitFamily = new string[] { };
            string[] SplitSalesOffice = new string[] { };
            string[] SplitModel = new string[] { };
            string[] SplitSerialNumber = new string[] { };
            string[] SplitProduct = new string[] { };
            string[] SplitPlant = new string[] { };
            string[] SplitIndustry = new string[] { };

            if (!string.IsNullOrWhiteSpace(Area))
            {
                SplitArea = Area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Customer))
            {
                SplitCustomer = Customer.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Model))
            {
                SplitModel = Model.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                SplitSerialNumber = SerialNumber.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Family))
            {
                SplitFamily = Family.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Industry))
            {
                SplitIndustry = Industry.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Plant))
            {
                SplitPlant = Plant.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(Product))
            {
                SplitProduct = Product.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                SplitSalesOffice = SalesOffice.Split(',');
            }
            var RentalVal = Rental.Replace("?model=1", "");
            var modelData = paramsModel.Replace("?model=1", "");
            paramsSerialNumber = paramsSerialNumber.Replace("?model=1", "");
            if (type == 1)
            {
                //var dataFiltered = getListSerialNumberFiltered(type, customer: SplitCustomer, plant: SplitPlant, serialnumber: SplitSerialNumber, model: SplitModel, product: SplitProduct, paramsModel : paramsModel);
                var getData = _InventoryWeeklyBs.getListMEPbyModel(modelData, SplitCustomer,SplitSerialNumber,SplitPlant,SplitProduct, orderByDir, orderByColumn, start, length, download);
               
                var listItem = new List<TableMEPOverviewModel.Data>();
                foreach (var item in getData.Item1)
                {
                    var serialnumber = new TableMEPOverviewModel.ValueData();
                    serialnumber.Value = item.Serial_Number;
                    serialnumber.Style = paramsSerialNumber == serialnumber.Value ? "<i class='fa fa-arrow-right'></i>" : "";
                    var smu = new TableMEPOverviewModel.ValueData();
                    smu.Value = Convert.ToString(item.SMU);
                    smu.Style = "";
                    var smuUpdate = new TableMEPOverviewModel.ValueData();
                    smuUpdate.Value = item.SMU_Update;
                    smuUpdate.Style = "";
                    var lastServiced = new TableMEPOverviewModel.ValueData();
                    lastServiced.Value = !String.IsNullOrWhiteSpace(item.LastServiced) ? item.LastServiced : "-";
                    lastServiced.Style = "";
                    var deliveryDate = new TableMEPOverviewModel.ValueData();
                    deliveryDate.Value = (item.DeliveryDate != null ? item.DeliveryDate : "-");
                    deliveryDate.Style = "";
                    var purchaseDate = new TableMEPOverviewModel.ValueData();
                    purchaseDate.Value = (item.PurchaseDate != null ? item.PurchaseDate : "-");
                    purchaseDate.Style = "";
                    var industry = new TableMEPOverviewModel.ValueData();
                    industry.Value = (!String.IsNullOrWhiteSpace(item.Industry) ? item.Industry : "-");
                    industry.Style = "";

                    var itemData = new TableMEPOverviewModel.Data();
                    itemData.SerialNumber = serialnumber;
                    itemData.SMU = smu;
                    itemData.SMU_Update = smuUpdate;
                    itemData.DeliveryDate = deliveryDate;
                    itemData.Purchase_Date = purchaseDate;
                    itemData.LastServiced = lastServiced;
                    itemData.Industry = industry;
                    listItem.Add(itemData);
                }
                var status = new TableMEPOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";
                var responseJson = new TableMEPOverviewModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = listItem;
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item2;
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            else
            {
                var getData = V_TR_MEP_Bs.getListMEPbyModel(modelData, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitSerialNumber, SmuRangeFrom, SmuRangeTo, PurchaseDateFrom, PurchaseDateEnd, DeliveryDateFrom, DeliveryDateEnd, RepairDateFrom, RepairDateEnd, SplitPlant, SplitProduct, Hid, RentalVal, orderByDir, orderByColumn, start, length, isDistinct, Other, download);

                var listItem = new List<TableMEPOverviewModel.Data>();
                foreach (var item in getData.Item1)
                {
                    var serialnumber = new TableMEPOverviewModel.ValueData();
                    serialnumber.Value = item.Serial_Number;
                    serialnumber.Style = paramsSerialNumber == serialnumber.Value ? "<i class='fa fa-arrow-right'></i>" : "";
                    var smu = new TableMEPOverviewModel.ValueData();
                    smu.Value = Convert.ToString(item.SMU);
                    smu.Style = "";
                    var smuUpdate = new TableMEPOverviewModel.ValueData();
                    smuUpdate.Value = item.SMU_Update;
                    smuUpdate.Style = "";
                    var lastServiced = new TableMEPOverviewModel.ValueData();
                    lastServiced.Value = !String.IsNullOrWhiteSpace(item.LastServiced) ? item.LastServiced : "-";
                    lastServiced.Style = "";
                    var deliveryDate = new TableMEPOverviewModel.ValueData();
                    deliveryDate.Value = (item.DeliveryDate != null ? item.DeliveryDate : "-" );
                    deliveryDate.Style = "";
                    var purchaseDate = new TableMEPOverviewModel.ValueData();
                    purchaseDate.Value = (item.PurchaseDate !=null ? item.PurchaseDate : "-");
                    purchaseDate.Style = "";
                    var industry = new TableMEPOverviewModel.ValueData();
                    industry.Value = (!String.IsNullOrWhiteSpace(item.Industry) ? item.Industry : "-");
                    industry.Style = "";

                    var itemData = new TableMEPOverviewModel.Data();
                    itemData.SerialNumber = serialnumber;
                    itemData.SMU = smu;
                    itemData.SMU_Update = smuUpdate;
                    itemData.DeliveryDate = deliveryDate;
                    itemData.Purchase_Date = purchaseDate;
                    itemData.LastServiced = lastServiced;
                    itemData.Industry = industry;
                    listItem.Add(itemData);
                }
                var status = new TableMEPOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";
                var responseJson = new TableMEPOverviewModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = listItem;
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item2;
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
        }
        
        public string [] getSerialNumberbyModel (String model)
        {
            var sn = _ctx.V_TR_MEP.Where(item => item.Product_Model.Equals(model)).Select(item => item.Serial_Number);
            return sn.Distinct().ToArray();
        } 
    }
}