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
        // GET: MapDistribution
        
        public ActionResult MapDistribution(FormCollection collection, int type, string Area, string SalesOffice, string Customer, string Family, string Industry, string Model, string SerialNumber, string SmuRangeFrom, string SmuRangeTo, string PurchaseDateFrom, string PurchaseDateEnd, string DeliveryDateFrom, string DeliveryDateEnd, string RepairDateFrom, string RepairDateEnd, string Plant, string Product, string Hid, string Rental, string Other, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST=false)
        {
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
            Rental = Rental.Replace("?model=1", "");
            paramsModel = paramsModel.Replace("?model=1", "");
            paramsSerialNumber = paramsSerialNumber.Replace("?model=1", "");
            var listMarker = new List<MapDistributionModel.Marker>();

            if (type != 1)
            {
                var GetData = V_TR_MEP_Bs.GetDataMapDistribution(btnPOST, SplitArea, SplitSalesOffice, SplitCustomer, SplitFamily,SplitIndustry, SplitModel, SplitSerialNumber, SmuRangeFrom, SmuRangeTo, DeliveryDateFrom, DeliveryDateEnd, PurchaseDateFrom, PurchaseDateEnd, RepairDateFrom, RepairDateEnd, Hid, Rental, Other, paramsModel, paramsSerialNumber).Item1;
                foreach(var Loc in GetData)
                {
                    var listLatLong = new List<double>();
                    double lat1 = Convert.ToDouble(Loc.Lattitude);
                    double long1 = Convert.ToDouble(Loc.Longitude);
                    listLatLong.Add(lat1);
                    listLatLong.Add(long1);

                    var markers = new MapDistributionModel.Marker();
                    markers.LatLong = listLatLong;
                    markers.Name = Loc.Area;
                    markers.Id = Loc.OrganizationId;
                    listMarker.Add(markers);
                }
                var status = new MapDistributionModel.Status();
                status.Code = 200;
                status.Message = "Success";

                var map = new MapDistributionModel.Map();
                map.Marker = listMarker;

                var data = new MapDistributionModel.Data();
                data.Map = map;

                var responseJson = new MapDistributionModel.ResponseJson();
                responseJson.Data = data;
                responseJson.Status = status;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            else
            {
                var dataFiltered = getListSerialNumberFiltered(type, customer: SplitCustomer, plant: SplitPlant, serialnumber: SplitSerialNumber, model: SplitModel, product: SplitProduct, paramsModel: paramsModel, paramsSerialNumber: SerialNumber);
                var GetData = _InventoryWeeklyBs.GetDataMapDistribution(dataFiltered).Item1;
                foreach (var Loc in GetData)
                {
                    var listLatLong = new List<double>();
                    double lat1 = Convert.ToDouble(Loc.Lattitude);
                    double long1 = Convert.ToDouble(Loc.Longitude);
                    listLatLong.Add(lat1);
                    listLatLong.Add(long1);

                    var markers = new MapDistributionModel.Marker();
                    markers.LatLong = listLatLong;
                    markers.Name = Loc.Area;
                    markers.Id = Loc.OrganizationId;
                    listMarker.Add(markers);
                }
                var status = new MapDistributionModel.Status();
                status.Code = 200;
                status.Message = "Success";

                var map = new MapDistributionModel.Map();
                map.Marker = listMarker;

                var data = new MapDistributionModel.Data();
                data.Map = map;

                var responseJson = new MapDistributionModel.ResponseJson();
                responseJson.Data = data;
                responseJson.Status = status;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }



            //var listLatLong1 = new List<double>();
            //double lat1 = 5.5483;
            //double long1 = 95.3238;
            //listLatLong1.Add(lat1);
            //listLatLong1.Add(long1);
         

            //var markers1 = new MapDistributionModel.Marker();
            //markers1.LatLong = listLatLong1;
            //markers1.Name = "Aceh";
            //markers1.Id = 1;
         

            //var listMarker = new List<MapDistributionModel.Marker>();
            //listMarker.Add(markers1);

            //var map = new MapDistributionModel.Map();
            //map.Marker = listMarker;

            //var data = new MapDistributionModel.Data();
            //data.Map = map;

         
        }
    }
}