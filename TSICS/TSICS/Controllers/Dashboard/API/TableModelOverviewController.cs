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
        // GET: TableModelOverview
        public ActionResult TableModelOverview(FormCollection collection, int type, string Area, string SalesOffice, string Customer, string Family, string Industry, string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Others, string paramsModel, string paramsSerialNumber, bool btnPOST = false)
        {
            var orderByColumn = collection["columns[" + collection["order[0][column]"] + "][data]"];
            var orderByDir = collection["order[0][dir]"];
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
            if (type == 1)
            {
                //var dataFiltered = getListSerialNumberFiltered(type, customer : SplitCustomer, plant :SplitPlant, serialnumber : SplitSerialNumber, model : SplitModel,product : SplitProduct);
                var getData = _InventoryWeeklyBs.GetMEPTechnicalOverviewBySN(type, SplitCustomer, SplitModel, SplitSerialNumber, SplitPlant, SplitProduct, paramsModel, paramsSerialNumber, orderByDir, orderByColumn ,start, length);
                var listItem = new List<TableModelOverviewModel.Data>();
                //Custom Query
                foreach (var item in getData.Item1)
                {
                    var model = new TableModelOverviewModel.ValueData();
                    model.Value = item.Model;
                    model.Style = paramsModel == model.Value ? "<i class='fa fa-arrow-right mr-2'></i>" : "";
                    var countModel = new TableModelOverviewModel.ValueData();
                    countModel.Value = item.CountModel.ToString();
                    countModel.Style = "";
                    var itemData = new TableModelOverviewModel.Data();
                    itemData.Model = model;
                    itemData.CountModel = countModel;
                    listItem.Add(itemData);
                }
                var status = new TableModelOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";
                var responseJson = new TableModelOverviewModel.ResponseJson();
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
               var getData = V_TR_MEP_Bs.GetMEPTechnicalOverview(LimitforSerialNumberFiltered,SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitModel, SplitSerialNumber, SmuRangeFrom, SmuRangeTo, DeliveryDateFrom,DeliveryDateEnd, PurchaseDateFrom, PurchaseDateEnd,RepairDateFrom, RepairDateEnd, Hid, RentalVal, Others, orderByDir,orderByColumn,start,length, paramsModel, paramsSerialNumber, btnPOST);
                
                var listItem = new List<TableModelOverviewModel.Data>();
                //Custom Query
                foreach (var item in getData.Item1)
                {
                    var model = new TableModelOverviewModel.ValueData();
                    model.Value = item.Model;
                    model.Style = paramsModel == model.Value ? "<i class='fa fa-arrow-right mr-2'></i>" : "";
                    var countModel = new TableModelOverviewModel.ValueData();
                    countModel.Value = item.CountModel.ToString();
                    countModel.Style = "";
                    var itemData = new TableModelOverviewModel.Data();
                    itemData.Model = model;
                    itemData.CountModel = countModel;
                    listItem.Add(itemData);
                }
                var status = new TableModelOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";
                var responseJson = new TableModelOverviewModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = listItem;
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item2;
                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
        }
      
    }
}