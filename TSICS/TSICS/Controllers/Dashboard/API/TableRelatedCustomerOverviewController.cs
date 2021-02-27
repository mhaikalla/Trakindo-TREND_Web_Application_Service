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
        // GET: TableRelatedCustomerOverview
        [HttpPost]
        public ActionResult TableRelatedCustomerOverview(FormCollection fc, int type, string Area, string SalesOffice, string Customer, string Family, string Industry,string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Others, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST = false, int download = 0, String [] columns =null)
        {
            var orderByColumn = fc["columns[" + fc["order[0][column]"] + "][data]"];
            var orderByDir = fc["order[0][dir]"];
            var start = Convert.ToInt32(fc["start"]);
            var length = Convert.ToInt32(fc["length"]);
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
            if (type != 1)
            {
                var getData = V_TR_MEP_Bs.GetRelatedCustomerfromMEPOverview(LimitforSerialNumberFiltered, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitModel, SplitSerialNumber, SmuRangeFrom, SmuRangeTo, DeliveryDateFrom, DeliveryDateEnd, PurchaseDateFrom, PurchaseDateEnd, RepairDateFrom, RepairDateEnd, Hid,RentalVal,Others, orderByDir, orderByColumn, start, length, paramsModel, paramsSerialNumber, btnPOST, download, columns);
                var listData = new List<TableRelatedCustomerOverviewModel.Data>();
                foreach (var item in getData.Item1)
                {
                    var custId = new TableRelatedCustomerOverviewModel.ValueData();
                    custId.Value =  String.IsNullOrWhiteSpace(item.CustomerId) ? "-" : item.CustomerId;
                    custId.Style = "";
                    var custName = new TableRelatedCustomerOverviewModel.ValueData();
                    custName.Value = String.IsNullOrWhiteSpace(item.CustomerName) ? "-" : item.CustomerName;
                    custName.Style = "";
                    var location = new TableRelatedCustomerOverviewModel.ValueData();
                    location.Value = String.IsNullOrWhiteSpace(item.Location) ? "-" : _ctx.Organization.Where(data => data.SalesOfficeCode.Equals(item.Location)).FirstOrDefault().SalesOfficeDescription;
                    location.Style = "";
                    var modelRelatedCustomer = new TableRelatedCustomerOverviewModel.ValueData();
                    modelRelatedCustomer.Value = String.IsNullOrWhiteSpace(item.Model) ? "-" : item.Model;
                    modelRelatedCustomer.Style = "";
                    var countofsn = new TableRelatedCustomerOverviewModel.ValueData();
                    countofsn.Value = item.CountOfSN.ToString();
                    countofsn.Style = "";
                    var itemData = new TableRelatedCustomerOverviewModel.Data();
                    itemData.CustomerId = custId;
                    itemData.CustomerName = custName;
                    itemData.Location = location;
                    itemData.Model = modelRelatedCustomer;
                    itemData.CountOfSN = countofsn;
                    listData.Add(itemData);
                }

                var status = new TableRelatedCustomerOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";

                var responseJson = new TableRelatedCustomerOverviewModel.ResponseJson();
                responseJson.Data = listData;
                responseJson.Status = status;   
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item3;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            else
            {
              //  string[] dataFiltered = getListSerialNumberFiltered(type, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily, SplitModel, SplitSerialNumber, RepairDateFrom, RepairDateEnd, PurchaseDateFrom, PurchaseDateEnd, DeliveryDateFrom, DeliveryDateEnd, SmuRangeFrom, SmuRangeTo, SplitProduct, SplitPlant,  Hid, Rental, Others, paramsModel, paramsSerialNumber);
                var getData = _InventoryWeeklyBs.GetRelatedCustomerfromInventoryOverview(SplitCustomer,SplitModel,SplitSerialNumber, SplitProduct,SplitPlant, orderByDir, orderByColumn, start, length, paramsModel, paramsSerialNumber, download);
                var listData = new List<TableRelatedCustomerOverviewModel.Data>();
                foreach (var item in getData.Item1)
                {
                    var custId = new TableRelatedCustomerOverviewModel.ValueData();
                    custId.Value = item.CustomerId;
                    custId.Style = "";
                    var custName = new TableRelatedCustomerOverviewModel.ValueData();
                    custName.Value = item.CustomerName;
                    custName.Style = "";
                    var location = new TableRelatedCustomerOverviewModel.ValueData();
                    location.Value = item.Location;
                    location.Style = "";
                    var modelRelatedCustomer = new TableRelatedCustomerOverviewModel.ValueData();
                    modelRelatedCustomer.Value = item.Model;
                    modelRelatedCustomer.Style = "";
                    var countofsn = new TableRelatedCustomerOverviewModel.ValueData();
                    countofsn.Value = item.CountOfSN.ToString();
                    countofsn.Style = "";
                    var itemData = new TableRelatedCustomerOverviewModel.Data();
                    itemData.CustomerId = custId;
                    itemData.CustomerName = custName;
                    itemData.Location = location;
                    itemData.Model = modelRelatedCustomer;
                    itemData.CountOfSN = countofsn;
                    listData.Add(itemData);
                }

                var status = new TableRelatedCustomerOverviewModel.Status();
                status.Code = 200;
                status.Message = "Sukses";

                var responseJson = new TableRelatedCustomerOverviewModel.ResponseJson();
                responseJson.Data = listData;
                responseJson.Status = status;
                responseJson.RecordsFiltered = getData.Item2;
                responseJson.RecordsTotal = getData.Item3;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
          
        }
    }
}