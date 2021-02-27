using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Com.Trakindo.TSICS.Business.Service
{
    public class InventoryWeeklyBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        private readonly V_TR_MEPBusinessService V_TR_MEPBusinessService = Factory.Create<V_TR_MEPBusinessService>("V_TR_MEP", ClassType.clsTypeBusinessService);
        public List<PPSC_Inventory_Weekly> GetCustomerOverview(string filter, string customer = null, string model=null, string product=null, string plant =null, string serialnumber=null)
        {
            var getData = _ctx.Inventory_Weekly.Where(item => item.Customer_Name.Contains(filter));
            if (!String.IsNullOrWhiteSpace(customer))
            {
                getData.Where(item => customer.Contains(item.Customer_Name));
            }
            if (!String.IsNullOrWhiteSpace(model))
            {
                getData.Where(item => model.Contains(item.Model));
            }
            if (!String.IsNullOrWhiteSpace(product))
            {
                getData.Where(item => product.Contains(item.Product));
            }
            if (!String.IsNullOrWhiteSpace(plant))
            {
                getData.Where(item => plant.Contains(item.Plant));
            }
            if (!String.IsNullOrWhiteSpace(serialnumber))
            {
                getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
            var listData = getData.GroupBy(g => g.Customer_Name)
                                           .Select(select => new
                                           {
                                               Customer = select.FirstOrDefault().Customer_Name
                                           }).OrderBy(odb => odb.Customer).ToList();
            var listitem = new List<PPSC_Inventory_Weekly>();
            foreach (var item in listData)
            {
                var data = new PPSC_Inventory_Weekly();
                data.Customer_Name = item.Customer;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<PPSC_Inventory_Weekly> GetModelOverview(string filter, string customer = null, string model = null, string product = null, string plant = null, string serialnumber = null)
        {
            var getData = _ctx.Inventory_Weekly.Where(item => item.Model.Contains(filter));
            if (!String.IsNullOrWhiteSpace(customer))
            {
                getData.Where(item => customer.Contains(item.Customer_Name));
            }
            if (!String.IsNullOrWhiteSpace(model))
            {
                getData.Where(item => model.Contains(item.Model));
            }
            if (!String.IsNullOrWhiteSpace(product))
            {
                getData.Where(item => product.Contains(item.Product));
            }
            if (!String.IsNullOrWhiteSpace(plant))
            {
                getData.Where(item => plant.Contains(item.Plant));
            }
            if (!String.IsNullOrWhiteSpace(serialnumber))
            {
                getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
            var listData = getData.GroupBy(g => g.Model)
                                           .Select(select => new
                                           {
                                               Model = select.FirstOrDefault().Model
                                           }).OrderBy(odb => odb.Model).ToList();
            var listitem = new List<PPSC_Inventory_Weekly>();
            foreach (var item in listData)
            {
                var data = new PPSC_Inventory_Weekly();
                data.Model = item.Model;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<PPSC_Inventory_Weekly> GetProductOverview(string filter, string customer = null, string model = null, string product = null, string plant = null, string serialnumber = null)
        {
            var getData = _ctx.Inventory_Weekly.Where(item => item.Product.Contains(filter));
            //var getData = _ctx.Inventory_Weekly.Where(item =>true);
            if (!String.IsNullOrWhiteSpace(customer))
            {
                getData.Where(item => customer.Contains(item.Customer_Name));
            }
            if (!String.IsNullOrWhiteSpace(model))
            {
                getData.Where(item => model.Contains(item.Model));
            }
            if (!String.IsNullOrWhiteSpace(product))
            {
                getData.Where(item => product.Contains(item.Product));
            }
            if (!String.IsNullOrWhiteSpace(plant))
            {
                getData.Where(item => plant.Contains(item.Plant));
            }
            if (!String.IsNullOrWhiteSpace(serialnumber))
            {
                getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
            var listData = getData.GroupBy(g => g.Product)
                                           .Select(select => new
                                           {
                                               Product = select.FirstOrDefault().Product
                                           }).Take(100).OrderBy(odb => odb.Product).ToList();
            var listitem = new List<PPSC_Inventory_Weekly>();
            foreach (var item in listData)
            {
                var data = new PPSC_Inventory_Weekly();
                data.Product = item.Product;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<PPSC_Inventory_Weekly> GetPlantOverview(string filter, string customer = null, string model = null, string product = null, string plant = null, string serialnumber = null)
        {
            var getData = _ctx.Inventory_Weekly.Where(item => item.Plant_Desc.Contains(filter));
            if (!String.IsNullOrWhiteSpace(customer))
            {
                getData.Where(item => customer.Contains(item.Customer_Name));
            }
            if (!String.IsNullOrWhiteSpace(model))
            {
                getData.Where(item => model.Contains(item.Model));
            }
            if (!String.IsNullOrWhiteSpace(product))
            {
                getData.Where(item => product.Contains(item.Product));
            }
            if (!String.IsNullOrWhiteSpace(plant))
            {
                getData.Where(item => plant.Contains(item.Plant_Desc));
            }
            if (!String.IsNullOrWhiteSpace(serialnumber))
            {
                getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
            var listData = getData.GroupBy(g => g.Plant_Desc)
                                           .Select(select => new
                                           {
                                               PlantDesc = select.FirstOrDefault().Plant_Desc
                                           }).Take(100).OrderBy(odb => odb.PlantDesc).ToList();
            var listitem = new List<PPSC_Inventory_Weekly>();
            foreach (var item in listData)
            {
                var data = new PPSC_Inventory_Weekly();
                data.Plant_Desc = item.PlantDesc;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<PPSC_Inventory_Weekly> GetSerialNumberOverview(string filter, string customer = null, string model = null, string product = null, string plant = null, string serialnumber = null)
        {
            var getData = _ctx.Inventory_Weekly.Where(item => item.Serial_Number.Contains(filter));
            if (!String.IsNullOrWhiteSpace(customer))
            {
                getData.Where(item => customer.Contains(item.Customer_Name));
            }
            if (!String.IsNullOrWhiteSpace(model))
            {
                getData.Where(item => model.Contains(item.Model));
            }
            if (!String.IsNullOrWhiteSpace(product))
            {
                getData.Where(item => product.Contains(item.Product));
            }
            if (!String.IsNullOrWhiteSpace(plant))
            {
                getData.Where(item => plant.Contains(item.Plant));
            }
            if (!String.IsNullOrWhiteSpace(serialnumber))
            {
                getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
            var listData = getData.GroupBy(g => g.Serial_Number)
                                           .Select(select => new
                                           {
                                               SerialNumber = select.FirstOrDefault().Serial_Number
                                           }).Take(100).OrderBy(odb => odb.SerialNumber).ToList();
            var listitem = new List<PPSC_Inventory_Weekly>();
            foreach (var item in listData)
            {
                var data = new PPSC_Inventory_Weekly();
                data.Serial_Number = item.SerialNumber;
                listitem.Add(data);
            }
            return listitem;
        }
        public IQueryable<PPSC_Inventory_Weekly> LogicFilterQuery(IQueryable<PPSC_Inventory_Weekly> Firstquery, String [] Customer=null, string[] Model=null, string[] SerialNumber=null, string[] Plant=null, string[] Product=null, String paramsModel=null, String paramsSerialNumber=null)
        {
            if (Customer.Count() > 0)
            {
                Firstquery = Firstquery.Where(item => Customer.Contains(item.Customer_Name));
            }
            if (Model.Count() > 0)
            {
                Firstquery = Firstquery.Where(item => Model.Contains(item.Model));
            }
            if (Plant.Count() > 0)
            {
                Firstquery = Firstquery.Where(item => Plant.Contains(item.Plant_Desc));
            }
            if (Product.Count() > 0)
            {
                Firstquery = Firstquery.Where(item => Product.Contains(item.Product));
            }
            if (SerialNumber.Count() > 0)
            {
                Firstquery = Firstquery.Where(item => SerialNumber.Contains(item.Serial_Number));
            }
            #region paramsFilter
            if (!String.IsNullOrWhiteSpace(paramsModel))
            {
                Firstquery = Firstquery.Where(item => item.Model.Equals(paramsModel));
            }
            if (!String.IsNullOrWhiteSpace(paramsSerialNumber))
            {
                Firstquery = Firstquery.Where(item => item.Serial_Number.Equals(paramsSerialNumber));
            }
            #endregion
            return Firstquery;
        }
        public Tuple<List<TableModelOverview>,int> GetMEPTechnicalOverviewBySN(int type, string[] customer, string[] model, string[] SerialNumber, string[] Plant, string[]Product, string paramsModel, string paramsSerialNumber, string orderByDir, string orderByColumn, int start, int length)
        {
            orderByColumn = orderByColumn.ToLower();
            start = (start == null) ? 0 : start;
            length = (length == null) ? 10 : length;
            if (type == 1 || customer.Count() > 0 || model.Count() > 0 || SerialNumber.Count() > 0 || Plant.Count() > 0 || Product.Count() > 0 || !String.IsNullOrWhiteSpace(paramsModel) )
            {
                var getData = _ctx.Inventory_Weekly.Where(item => item.Model != string.Empty && item.Model != null);
                getData = LogicFilterQuery(getData, customer, model, SerialNumber, Plant, Product);
                var getDataGroup = getData.GroupBy(gb => new { gb.Model }).Select(s => new
                {
                    productModel = s.FirstOrDefault().Model,
                    Count = s.Select(select => select.Serial_Number).Distinct().Count()
                });
                #region OrderByCondition
                if (!string.IsNullOrWhiteSpace(orderByColumn))
                {
                    if (orderByColumn == "model")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.productModel);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.productModel);
                        }
                    }
                    else if (orderByColumn == "count")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Count);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Count);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Count);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Count);
                        }
                    }
                }

                #endregion

                var countDataGroup = getDataGroup.Count();
                getDataGroup = getDataGroup.Skip(start).Take(length);

                var finalData = new List<TableModelOverview>();
                foreach (var item in getDataGroup)
                {
                    var data = new TableModelOverview();
                    data.Model = item.productModel;
                    data.CountModel = item.Count;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData.ToList(),countDataGroup);
            }
            else
            {
                var getDataGroup = _ctx.V_TR_MEP.Take(0);
                var finalData = new List<TableModelOverview>();
                foreach (var item in getDataGroup)
                {
                    var data = new TableModelOverview();
                    data.Model = item.Product_Model;
                    data.CountModel = 0;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData.ToList(), 0);
            }
        }
      
        public Tuple<List<TableMEPOverviewModel>, int> getListMEPbyModel(string model, string[] customer, string[] SerialNumber, string[] Plant, string[] Product, string orderByDir, string orderByColumn, int start, int length, int download=0)
        {
            if (!String.IsNullOrWhiteSpace(orderByDir))
            {
                orderByColumn = orderByColumn.ToLower();
            }
            start = (start == null) ? 0 : start;
            length = (length == null) ? 10 : length;
            var finalData = new List<TableMEPOverviewModel>();
            if (!String.IsNullOrWhiteSpace(model))
            {
                var getData = _ctx.Inventory_Weekly.Where(item => true);
                string[] modelData = { };
                getData = LogicFilterQuery(Firstquery: getData, Customer: customer, Model: modelData, SerialNumber: SerialNumber, Plant: Plant, Product: Product, paramsModel: model, paramsSerialNumber:null);
                var getDataGroup = (from item in getData
                                    from joinMep in _ctx.MEP.Where(mepw => mepw.SerialNumber.Equals(item.Serial_Number)).DefaultIfEmpty()
                                    from join_v_tr_mep in _ctx.V_TR_MEP.Where(v_tr_mep => v_tr_mep.Serial_Number.Equals(item.Serial_Number)).DefaultIfEmpty()
                                    where item.Model == model
                                    group new { item, joinMep, join_v_tr_mep } by item.Serial_Number into dataModel
                                    select new
                                    {
                                        SerialNumber = dataModel.FirstOrDefault().item.Serial_Number,
                                        Smu = dataModel.FirstOrDefault().join_v_tr_mep.SMU == null ? 0 : dataModel.FirstOrDefault().join_v_tr_mep.SMU,
                                        smu_update = dataModel.FirstOrDefault().join_v_tr_mep.SMU_Update,
                                        delivery_date = dataModel.FirstOrDefault().join_v_tr_mep.Delivery_Date,
                                        purchase_date = dataModel.FirstOrDefault().join_v_tr_mep.Purchase_Date,
                                        last_serviced = dataModel.FirstOrDefault().join_v_tr_mep.Last_Work_Order,
                                        industry = (dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry == null && dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry.ToString() == "") ? "-" : dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry.ToString()
                                    }
                                );
                //if (isDistinct)
                //{
                //    getDataGroup = getDataGroup.Distinct();
                //}

                #region OrderByCondition
                if (!string.IsNullOrWhiteSpace(orderByColumn))
                {
                    if (orderByColumn == "smu")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Smu);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Smu);
                        }
                    }
                    else if (orderByColumn == "smu_update")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.smu_update);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.smu_update);
                        }
                    }
                    else if (orderByColumn == "last_serviced")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.last_serviced);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.last_serviced);
                        }
                    }
                    else if (orderByColumn == "delivery_date")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.delivery_date);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.delivery_date);
                        }
                    }
                    else if (orderByColumn == "purchase_date")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.purchase_date);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.purchase_date);
                        }
                    }
                    else if (orderByColumn == "industry")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.industry);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.industry);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.SerialNumber);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.SerialNumber);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                    {
                        getDataGroup = getDataGroup.OrderByDescending(ob => ob.SerialNumber);
                    }
                    else
                    {
                        getDataGroup = getDataGroup.OrderBy(ob => ob.SerialNumber);
                    }
                }
                #endregion
                var countDataGroup = getDataGroup.Count();
                var listDataGroup = getDataGroup.ToList();
                if (download != 1)
                {
                    listDataGroup = getDataGroup.Skip(start).Take(length).ToList();
                }
                foreach (var item in listDataGroup)
                {
                    var data = new TableMEPOverviewModel();
                    data.Serial_Number = item.SerialNumber;
                    data.DeliveryDate = String.IsNullOrWhiteSpace(item.delivery_date) ? "N/A" : item.delivery_date;
                    data.PurchaseDate = String.IsNullOrWhiteSpace(item.purchase_date) ? "N/A" : item.purchase_date;
                    data.SMU_Update = String.IsNullOrWhiteSpace(item.smu_update) ? "N/A" : item.smu_update;
                    data.SMU = String.IsNullOrWhiteSpace(item.Smu.ToString()) ? "-" : item.Smu.ToString();
                    data.LastServiced = String.IsNullOrWhiteSpace(item.last_serviced) ? "N/A" : item.last_serviced;
                    data.Industry = String.IsNullOrWhiteSpace(item.industry) ? "-" : item.industry;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData, countDataGroup);
            }
            else
            {

                var listDataGroup = _ctx.V_TR_MEP.Take(0);
                foreach (var item in listDataGroup)
                {
                    var data = new TableMEPOverviewModel();
                    data.Serial_Number = item.Serial_Number;
                    data.DeliveryDate = item.Delivery_Date;
                    data.PurchaseDate = item.Purchase_Date;
                    data.SMU_Update = item.SMU_Update;
                    data.SMU = item.SMU.ToString();
                    data.LastServiced = item.Last_Work_Order;
                    data.Industry = item.Area;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData, finalData.Count());
            }
        }
        public Tuple<List<TableRelatedCustomerOverview>, int, int> GetRelatedCustomerfromInventoryOverview(string[] Customer, string[]Model, string[] serialnumber, string[]product, string []plant , string orderByDir, string orderByColumn, int start, int length,string paramsModel = null, string paramsSerialNumber = null, int download = 0)
        {
            orderByColumn = !String.IsNullOrWhiteSpace(orderByColumn) ? orderByColumn.ToLower() : String.Empty ;
          
            var finalData = new List<TableRelatedCustomerOverview>();
            
            var getData = _ctx.Inventory_Weekly.Where(item => true);

            getData = LogicFilterQuery(getData, Customer, Model, serialnumber, plant, product, paramsModel, paramsSerialNumber);
        
            var getDataGroup = getData.GroupBy(item => new { item.Customer_Number, item.Customer_Name, item.Model, item.Plant })
                .Select(item => new {
                    Customer_Id = item.FirstOrDefault().Customer_Number,
                    Customer_Name = item.FirstOrDefault().Customer_Name,
                    Location = item.FirstOrDefault().Plant_Desc,
                    Model = item.FirstOrDefault().Model,
                    CountSN = item.Count()
                });
            var countRealData = getDataGroup.Count();
            var countCustomerData = getDataGroup.Select(s => s.Customer_Id).Distinct().Count();
            if (download != 1)
            {
                #region OrderByCondition
                if (!string.IsNullOrWhiteSpace(orderByColumn))
                {
                    if (orderByColumn == "customer_name")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Customer_Name);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Customer_Name);
                        }
                    }
                    else if (orderByColumn == "location")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Location);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Location);
                        }
                    }
                    else if (orderByColumn == "model")
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Model);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Model);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            getDataGroup = getDataGroup.OrderByDescending(ob => ob.Customer_Id);
                        }
                        else
                        {
                            getDataGroup = getDataGroup.OrderBy(ob => ob.Customer_Id);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                    {
                        getDataGroup = getDataGroup.OrderByDescending(ob => ob.Customer_Id);
                    }
                    else
                    {
                        getDataGroup = getDataGroup.OrderBy(ob => ob.Customer_Id);
                    }
                }
                #endregion
                getDataGroup = getDataGroup.Skip(start).Take(length);
            }
            else {
                getDataGroup = getDataGroup.OrderByDescending(ob => ob.CountSN);
            }
            foreach (var item in getDataGroup)
            {
                var data = new TableRelatedCustomerOverview();
                data.CustomerId = String.IsNullOrWhiteSpace(item.Customer_Id) ? "-" : item.Customer_Id ;
                data.CustomerName = String.IsNullOrWhiteSpace(item.Customer_Name) ? "-" : item.Customer_Name;
                data.Location = String.IsNullOrWhiteSpace(item.Location)? "-" : item.Location;
                data.Model = String.IsNullOrWhiteSpace(item.Model) ? "-" : item.Model;
                data.CountOfSN = item.CountSN;
                finalData.Add(data);
            }
            return Tuple.Create(finalData, countRealData,countCustomerData);
           
        }
        public List<PPSC_Inventory_Weekly> GetSerialNumberFiltered(string limitData, string[] customer, string[] model, string[] serialnumber, string[] plant, string [] product, string paramsModel = null, string paramsSerialNumber = null)
        {
            var InventoryFiltered = _ctx.Inventory_Weekly.Where(item => true);

            var ListData = new List<PPSC_Inventory_Weekly>();
            InventoryFiltered = LogicFilterQuery(InventoryFiltered, customer, model, serialnumber, plant, product, paramsModel,paramsSerialNumber);
            var getdata = (from item in InventoryFiltered
                           group item by item.Serial_Number into sn
                           select new
                           {
                               serialNumber = sn.FirstOrDefault().Serial_Number
                           });

                foreach (var item in getdata)
                {
                    var data = new PPSC_Inventory_Weekly();
                    data.Serial_Number = item.serialNumber;
                    ListData.Add(data);
                }
            if (!String.IsNullOrWhiteSpace(limitData))
            {
                return ListData.Take(Convert.ToInt32(limitData)).ToList();
            }
            else
            {
                return ListData.ToList();
            }
            //}
            //else
            //{
            //    InventoryFiltered = InventoryFiltered.Take(0);
            //    foreach (var item in InventoryFiltered)
            //    {
            //        var data = new PPSC_Inventory_Weekly();
            //        data.Serial_Number = item.Serial_Number;
            //        ListData.Add(data);
            //    }
            //    return ListData;
            //}
        }
    
        public string getIndustryData(string SerialNumber)
        {
            String data = _ctx.MEP.Where(item => item.SerialNumber.Equals(SerialNumber)).Select(item => item.DefinitionSubIndustry).FirstOrDefault();
            if (!String.IsNullOrWhiteSpace(data))
            {
                return data;
            }
            else
            {
                return "-";
            }
        }
        public Tuple<List<Organization2>, int> GetDataMapDistribution(string[] SerialNumber)
        {
            if (SerialNumber.Length > 0)
            {
                List<String> mapData = new List<String>();
                var LocationData = _ctx.Inventory_Weekly.Where(item => true).Select(item =>new { Plant_Desc = item.Plant_Desc, Serial_Number = item.Serial_Number }).Distinct().ToArray();
                foreach(var item in LocationData)
                {
                    if (SerialNumber.Contains(item.Serial_Number))
                    {
                        mapData.Add(item.Plant_Desc);
                    }
                }
                List<Organization2> getLocData = new List<Organization2>();
                foreach (var item in _ctx.Organization2)
                {
                    if (mapData.Contains(item.SalesOfficeDescription))
                    {
                        getLocData.Add(item);
                    }
                }
                return Tuple.Create(getLocData.ToList(), getLocData.Count());
            }
            else
            {
                return Tuple.Create(_ctx.Organization2.Take(0).ToList(), 0);
            }

        }
    }
}
