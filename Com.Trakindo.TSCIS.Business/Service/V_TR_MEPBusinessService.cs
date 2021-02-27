using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;


namespace Com.Trakindo.TSICS.Business.Service
{
    public class V_TR_MEPBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        #region===============| Dropdown Logic |============================
        public List<V_TR_MEP> GetAreaOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var getData = _ctx.V_TR_MEP.Where(w => w.Area.Contains(filter));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.Area)
                                            .Select(select => new
                                            {
                                                Area = select.FirstOrDefault().Area
                                            }).OrderBy(odb => odb.Area).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.Area = item.Area;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<V_TR_MEP> GetSalesOfficeOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var LOCdata = _ctx.Organization.Where(item => item.SalesOfficeDescription.Contains(filter)).Select(item => item.SalesOfficeCode).ToArray();
            var getData = _ctx.V_TR_MEP.Where(w => LOCdata.Contains(w.LOC));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.LOC)
                                            .Select(select => new
                                            {
                                                SalesOffice = select.FirstOrDefault().LOC
                                            }).OrderBy(odb => odb.SalesOffice).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.LOC = _ctx.Organization.FirstOrDefault(o => o.SalesOfficeCode == item.SalesOffice).SalesOfficeDescription;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<V_TR_MEP> GetCustomerMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var getData = _ctx.V_TR_MEP.Where(w => w.Customer_Name.Contains(filter));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.Customer_Name)
                                            .Select(select => new
                                            {
                                                cust = select.FirstOrDefault().Customer_Name
                                            }).OrderBy(odb => odb.cust).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.Customer_Name = item.cust;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<MEP> GetIndustryOverview(string filter)
        {
            var listData = _ctx.MEP.Where(w => w.DefinitionIndustry.Contains(filter))
                .Select(s=>s.DefinitionIndustry).Distinct().OrderBy(odb => true).ToList();

            var listitem = new List<MEP>();
            foreach (var item in listData)
            {
                var data = new MEP();
                data.DefinitionIndustry = item;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<V_TR_MEP> GetFamilyOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var getData = _ctx.V_TR_MEP.Where(w => w.Product_Family_Description.Contains(filter));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.Product_Family_Description)
                                            .Select(select => new
                                            {
                                                Family = select.FirstOrDefault().Product_Family_Description
                                            }).OrderBy(odb => odb.Family).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.Product_Family = item.Family;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<V_TR_MEP> GetModelMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var getData = _ctx.V_TR_MEP.Where(w => w.Product_Model.Contains(filter));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.Product_Model)
                                            .Select(select => new
                                            {
                                                Model = select.FirstOrDefault().Product_Model
                                            }).OrderBy(odb => odb.Model).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.Product_Model = item.Model;
                listitem.Add(data);
            }
            return listitem;
        }
        public List<V_TR_MEP> GetSerialNumberMEPOverview(string filter, string area, string salesoffice, string customer, string family, string model, string serialnumber, string Rental, string Hid, string Others)
        {
            var getData = _ctx.V_TR_MEP.Where(w => w.Serial_Number.Contains(filter));
            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
            if (!string.IsNullOrWhiteSpace(salesoffice))
            {
                var getDataSalesOffice = _ctx.Organization.Where(item => salesoffice.Contains(item.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => getDataSalesOffice.Contains(item.LOC));
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                getData = getData.Where(w => area.Contains(w.Area));
            }
            if (!string.IsNullOrWhiteSpace(customer))
            {
                getData = getData.Where(w => customer.Contains(w.Customer_Name));
            }
            if (!string.IsNullOrWhiteSpace(family))
            {
                getData = getData.Where(w => family.Contains(w.Product_Family));
            }
            if (!string.IsNullOrWhiteSpace(model))
            {
                getData = getData.Where(w => model.Contains(w.Product_Model));
            }
            if (!string.IsNullOrWhiteSpace(serialnumber))
            {
                getData = getData.Where(w => serialnumber.Contains(w.Serial_Number));
            }
            //if (smurange != "0")
            //{
            //    getData = getData.Where(w =>);
            //}
            var listData = getData.GroupBy(g => g.Serial_Number)
                                            .Select(select => new
                                            {
                                                sn = select.FirstOrDefault().Serial_Number
                                            }).OrderBy(odb => odb.sn).ToList();
            var listitem = new List<V_TR_MEP>();
            foreach (var item in listData)
            {
                var data = new V_TR_MEP();
                data.Serial_Number = item.sn;
                listitem.Add(data);
            }
            return listitem;
        }
        
     
        #endregion

        #region===============| Bussiness Service Logic |===================
        public IQueryable<V_TR_MEP> LogicFilterQuery(IQueryable<V_TR_MEP> FirstQuery, string[] area = null, string[] salesoffice = null, string[] customer = null, string[] family = null, string[] industry=null, string[] model = null, string[] serialnumber = null, string smuFrom = null, string smuTo = null, string deliveryDateFrom = null, string deliveryDateTo = null, string PurchaseDateFrom = null, string PurchaseDateTo = null, string LastRepairDateFrom = null, string LastRepairDateTo = null, string Hid = null, string Rental = null, string Others = null, string paramsModel = null, string paramsSerialNumber = null)
        {
            double SMU_From = String.IsNullOrWhiteSpace(smuFrom) ? 0 : Convert.ToDouble(smuFrom);
            double SMU_To = String.IsNullOrWhiteSpace(smuTo) ? 0 : Convert.ToDouble(smuTo);
            var getData = FirstQuery;

            #region Checkbox Filter
            if (Hid == "3" && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0 && item.RentStatus == "") || (item.HidTaskId != 0 && item.RentStatus != ""));
            }
            else if (Hid == "3" && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0 && item.RentStatus != "");
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => (item.HidTaskId == 0) || (item.HidTaskId != 0 && item.RentStatus == ""));
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && Others == "4")
            {
                getData = getData.Where(item => (item.RentStatus != "") || (item.HidTaskId == 0 && item.RentStatus == ""));
            }
            else if (Hid == "3" && String.IsNullOrWhiteSpace(Rental) && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.HidTaskId != 0);
            }
            else if (String.IsNullOrWhiteSpace(Hid) && Rental == "2" && String.IsNullOrWhiteSpace(Others))
            {
                getData = getData.Where(item => item.RentStatus != "");
            }
            else if (String.IsNullOrWhiteSpace(Hid) && String.IsNullOrWhiteSpace(Rental) && Others == "4")
            {
                getData = getData.Where(item => item.HidTaskId == 0 && item.RentStatus == "");
            }
            #endregion
           
            #region filter
            if (area.Count() > 0)
            {
                getData = getData.Where(item => area.Contains(item.Area));
            }
          
            if (customer.Count() > 0 )
            {
                getData = getData.Where(item => customer.Contains(item.Customer_Name));
            }
           
            if (family.Count() > 0)
            {
                getData = getData.Where(item => family.Contains(item.Product_Family_Description));
            }

            if (industry.Count() > 0)
            {
                string[] MEPData = _ctx.MEP.Where(item => industry.Contains(item.DefinitionIndustry)).Select(s => s.SerialNumber).Distinct().Take(1000).ToArray();
                getData = getData.Where(item => MEPData.Contains(item.Serial_Number));
            }
          
            if (model.Count() > 0)
            {
                getData = getData.Where(item => model.Contains(item.Product_Model));
            }
          
            if (serialnumber.Count() > 0)
            {
                getData = getData.Where(item => serialnumber.Contains(item.Serial_Number));
            }
         
            if (salesoffice.Count() > 0)
            {
                var locData = _ctx.Organization.Where(i => salesoffice.Contains(i.SalesOfficeDescription)).Select(item => item.SalesOfficeCode);
                getData = getData.Where(item => locData.Contains(item.LOC));
            }
            
            if (SMU_From != 0 && SMU_To != 0)
            {
               getData = getData.Where(item => item.SMU >= SMU_From && item.SMU <= SMU_To);
            }
            else if (SMU_From != 0 && SMU_To == 0)
            {
                getData = getData.Where(item => item.SMU >= SMU_From);
            }
            else if (SMU_From == 0 && SMU_To != 0)
            {
                getData = getData.Where(item => item.SMU <= SMU_To);
            }

            if (!String.IsNullOrWhiteSpace(deliveryDateFrom) && !String.IsNullOrWhiteSpace(deliveryDateTo))
            {
                deliveryDateFrom = deliveryDateFrom.Substring(6, 4) + "-" + deliveryDateFrom.Substring(3, 2) + "-" + deliveryDateFrom.Substring(0, 2);
                deliveryDateTo = deliveryDateTo.Substring(6, 4) + "-" + deliveryDateTo.Substring(3, 2) + "-" + deliveryDateTo.Substring(0, 2);
                getData = getData.Where(item => item.Delivery_Date.CompareTo(deliveryDateFrom) >= 0 && item.Delivery_Date.CompareTo(deliveryDateTo) <= 0);
            }
            else if (!String.IsNullOrWhiteSpace(deliveryDateFrom) && String.IsNullOrWhiteSpace(deliveryDateTo))
            {
                deliveryDateFrom = deliveryDateFrom.Substring(6, 4) + "-" + deliveryDateFrom.Substring(3, 2) + "-" + deliveryDateFrom.Substring(0, 2);
                getData = getData.Where(item => item.Delivery_Date.CompareTo(deliveryDateFrom) >= 0);
            }
            else if (String.IsNullOrWhiteSpace(deliveryDateFrom) && !String.IsNullOrWhiteSpace(deliveryDateTo))
            {
                deliveryDateTo = deliveryDateTo.Substring(0, 2) + "-" + deliveryDateTo.Substring(3, 2) + "-" + deliveryDateTo.Substring(6, 4);
                getData = getData.Where(item => item.Delivery_Date.CompareTo(deliveryDateTo) <= 0);
            }
          
            if (!String.IsNullOrWhiteSpace(PurchaseDateFrom) && !String.IsNullOrWhiteSpace(PurchaseDateTo))
            {
                PurchaseDateFrom = PurchaseDateFrom.Substring(6, 4) + "-" + PurchaseDateFrom.Substring(3, 2) + "-" + PurchaseDateFrom.Substring(0, 2);
                PurchaseDateTo = PurchaseDateTo.Substring(6, 4) + "-" + PurchaseDateTo.Substring(3, 2) + "-" + PurchaseDateTo.Substring(0, 2);
                getData = getData.Where(item => item.Purchase_Date.CompareTo(PurchaseDateFrom) >= 0 && item.Purchase_Date.CompareTo(PurchaseDateTo) <= 0);
            }
            else if (!String.IsNullOrWhiteSpace(PurchaseDateFrom) && String.IsNullOrWhiteSpace(PurchaseDateTo))
            {
                PurchaseDateFrom = PurchaseDateFrom.Substring(6, 4) + "-" + PurchaseDateFrom.Substring(3, 2) + "-" + PurchaseDateFrom.Substring(0, 2);
                getData = getData.Where(item => item.Purchase_Date.CompareTo(PurchaseDateFrom) >= 0);
            }
            else if (String.IsNullOrWhiteSpace(PurchaseDateFrom) && !String.IsNullOrWhiteSpace(PurchaseDateTo))
            {
                PurchaseDateTo = PurchaseDateTo.Substring(6, 4) + "-" + PurchaseDateTo.Substring(3, 2) + "-" + PurchaseDateTo.Substring(0, 2);
                getData = getData.Where(item => item.Purchase_Date.CompareTo(PurchaseDateTo) <= 0);
            }
           
            if (!String.IsNullOrWhiteSpace(LastRepairDateFrom) && !String.IsNullOrWhiteSpace(LastRepairDateTo))
            {
                LastRepairDateFrom = LastRepairDateFrom.Substring(6, 4) + "-" + LastRepairDateFrom.Substring(3, 2) + "-" + LastRepairDateFrom.Substring(0, 2);
                LastRepairDateTo = LastRepairDateTo.Substring(6, 4) + "-" + LastRepairDateTo.Substring(3, 2) + "-" + LastRepairDateTo.Substring(0, 2);
                getData = getData.Where(item => item.Last_Work_Order.CompareTo(LastRepairDateFrom) >= 0 && item.Last_Work_Order.CompareTo(LastRepairDateTo) <= 0);
            }
            else if (!String.IsNullOrWhiteSpace(LastRepairDateFrom) && String.IsNullOrWhiteSpace(LastRepairDateTo))
            {
                LastRepairDateFrom = LastRepairDateFrom.Substring(6, 4) + "-" + LastRepairDateFrom.Substring(3, 2) + "-" + LastRepairDateFrom.Substring(0, 2);
                 getData = getData.Where(item => item.Last_Work_Order.CompareTo(LastRepairDateFrom) >= 0);
            }
            else if (String.IsNullOrWhiteSpace(LastRepairDateFrom) && !String.IsNullOrWhiteSpace(LastRepairDateTo))
            {
                LastRepairDateTo = LastRepairDateTo.Substring(6, 4) + "-" + LastRepairDateTo.Substring(3, 2) + "-" + LastRepairDateTo.Substring(0, 2);
                getData = getData.Where(item => item.Last_Work_Order.CompareTo(LastRepairDateTo) <= 0);
            }
           
            #endregion

            #region paramsFilter
            if (!String.IsNullOrWhiteSpace(paramsModel))
            {
                getData = getData.Where(data => data.Product_Model.Equals(paramsModel));
            }
            if (!String.IsNullOrWhiteSpace(paramsSerialNumber))
            {
                getData = getData.Where(data => data.Serial_Number.Equals(paramsSerialNumber));
            }
            #endregion
            return getData;
        }

        public Tuple<List<TableModelOverview>,int> GetMEPTechnicalOverview(string limitData, string[] area, string[] salesoffice, string[] customer, string[] family, string[]industry ,string[] model, string[] serialnumber, string smuFrom, string smuTo, string deliveryDateFrom, string deliveryDateTo, string PurchaseDateFrom, string PurchaseDateTo, string LastRepairDateFrom, string LastRepairDateTo, string Hid, String Rental, string Others, string orderByDir, string orderByColumn, int start, int length, String paramsModel = null, String paramsSerialNumber = null,bool btnPOST =false)
        {
            orderByColumn = orderByColumn.ToLower();
            start = (start == null) ? 0 : start;
            length = (length == null) ? 10 : length;
           
            if (btnPOST == true || area.Count() > 0 || customer.Count() > 0 || family.Count() > 0 || model.Count() > 0 || serialnumber.Count() > 0 || salesoffice.Count() > 0 || smuFrom != "0" || smuTo != "0" || !String.IsNullOrWhiteSpace(PurchaseDateFrom) || !String.IsNullOrWhiteSpace(PurchaseDateTo) || !String.IsNullOrWhiteSpace(deliveryDateTo) || !String.IsNullOrWhiteSpace(deliveryDateFrom) || !String.IsNullOrWhiteSpace(LastRepairDateFrom) || !String.IsNullOrWhiteSpace(PurchaseDateTo) || Rental == "2" || Hid == "3" || Others == "4" || !String.IsNullOrWhiteSpace(paramsModel) || !String.IsNullOrWhiteSpace(paramsSerialNumber))
            {
                var getData = _ctx.V_TR_MEP.Where(item => item.Product_Model != string.Empty && item.Product_Model != null);
                getData = LogicFilterQuery(FirstQuery: getData, area: area, salesoffice: salesoffice, customer: customer, family: family, industry: industry, model: model, serialnumber: serialnumber, smuFrom: smuFrom, smuTo: smuTo, deliveryDateFrom: deliveryDateFrom, deliveryDateTo: deliveryDateTo, PurchaseDateFrom: PurchaseDateFrom, PurchaseDateTo: PurchaseDateTo, LastRepairDateFrom: LastRepairDateFrom, LastRepairDateTo: LastRepairDateTo, Hid: Hid, Rental: Rental, Others: Others, paramsModel: null, paramsSerialNumber: null);

                var getDataGroup = getData.GroupBy(gb => new { gb.Product_Model }).Select(s => new
                {
                    productModel = s.FirstOrDefault().Product_Model,
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
           
                return Tuple.Create(finalData.ToList(), countDataGroup);
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
        public Tuple<List<TableRelatedCustomerOverview>, int, int> GetRelatedCustomerfromMEPOverview(string limitData, string[] area, string[] salesoffice, string[] customer, string[] family, string [] industry , string[] model, string[] serialnumber, string smuFrom, string smuTo, string deliveryDateFrom, string deliveryDateTo, string PurchaseDateFrom, string PurchaseDateTo, string LastRepairDateFrom, string LastRepairDateTo, string Hid, string Rental, string Others, string orderByDir, string orderByColumn, int start, int length, string paramsModel = null, string paramsSerialNumber = null, bool btnPOST=false, int download = 0, String[] ExportColumns = null)
        {
            orderByColumn = !String.IsNullOrWhiteSpace(orderByColumn) ? orderByColumn.ToLower():String.Empty;
            start = (start == null) ? 0 : start;
            length = (length == null) ? 10 : length;
            var finalData = new List<TableRelatedCustomerOverview>();

            if (btnPOST == true || area.Count() > 0 || salesoffice.Count() > 0 || customer.Count() > 0 || family.Count() > 0 || industry.Count() > 0 || model.Count() > 0 || serialnumber.Count() > 0 || LastRepairDateFrom != "" || LastRepairDateTo != "" || PurchaseDateFrom != "" || PurchaseDateTo != "" || deliveryDateFrom != "" || deliveryDateTo != "" || smuFrom != "0" || smuTo != "0" || !String.IsNullOrWhiteSpace(Hid) || !String.IsNullOrWhiteSpace(Rental) || !String.IsNullOrWhiteSpace(Others))
            {
                var getData = _ctx.V_TR_MEP.Where(item => true);
                getData = LogicFilterQuery(FirstQuery: getData, area: area, salesoffice: salesoffice, customer: customer, family: family, industry: industry, model: model, serialnumber: serialnumber, smuFrom: smuFrom, smuTo: smuTo, deliveryDateFrom: deliveryDateFrom, deliveryDateTo: deliveryDateTo, PurchaseDateFrom: PurchaseDateFrom, PurchaseDateTo: PurchaseDateTo, LastRepairDateFrom: LastRepairDateFrom, LastRepairDateTo: LastRepairDateTo, Hid: Hid, Rental: Rental, Others: Others, paramsModel: paramsModel, paramsSerialNumber: paramsSerialNumber);

                var getDataGroup = getData.GroupBy(item => new { item.Customer_Name, item.Customer_ID, item.Product_Model, item.LOC })
               .Select(item => new {
                   Customer_Id = item.FirstOrDefault().Customer_ID,
                   Customer_Name = item.FirstOrDefault().Customer_Name,
                   Location = item.FirstOrDefault().LOC,
                   Model = item.FirstOrDefault().Product_Model,
                   CountSN = item.Select(select => select.Serial_Number).Distinct().Count()
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
                else
                {
                    getDataGroup = getDataGroup.OrderByDescending(ob => ob.CountSN);
                }
                foreach (var item in getDataGroup)
                {
                    var data = new TableRelatedCustomerOverview();
                    data.CustomerId = String.IsNullOrWhiteSpace(item.Customer_Id) ? "-" : item.Customer_Id;
                    data.CustomerName = String.IsNullOrWhiteSpace(item.Customer_Name) ? "-" : item.Customer_Name;
                    data.Location =  item.Location;
                    data.Model = String.IsNullOrWhiteSpace(item.Model) ? "-" : item.Model;
                    data.CountOfSN = item.CountSN;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData,countRealData, countCustomerData);
            }
            else
            {
                var getData = _ctx.V_TR_MEP.Take(0);
               
                var getDataGroup = (from item in getData
                                    group item by item.Customer_ID into data
                                    select new
                                    {
                                        Customer_Id = data.FirstOrDefault().Customer_ID,
                                        Customer_Name = data.FirstOrDefault().Customer_Name,
                                        Location = data.FirstOrDefault().LOC,
                                        Model = data.FirstOrDefault().Product_Model,
                                    }
                                );
                foreach (var item in getDataGroup)
                {
                    var data = new TableRelatedCustomerOverview();
                    int countOfSN = 0;
                    data.CustomerId = item.Customer_Id;
                    data.CustomerName = item.Customer_Name;
                    data.Location = item.Location;
                    data.Model = item.Model;
                    data.CountOfSN = countOfSN;
                    finalData.Add(data);
                }
                return Tuple.Create(finalData, 0, 0);
            }
        }
        public List<V_TR_MEP> GetSerialNumberFiltered(string limitData, string[] area, string[] salesoffice, string[] customer, string[] family, string[] industry, string[] model, string[] serialnumber, string LastRepairDateFrom, string LastRepairDateTo, string PurchaseDateFrom, string PurchaseDateTo, string deliveryDateFrom, string deliveryDateTo, string smurangefrom, string smurangeto, string Hid, string Rental, string Others, string paramsModel = null, string paramsSerialNumber = null)
        {
            var getData = _ctx.V_TR_MEP.Where(item => true);
            getData = LogicFilterQuery(FirstQuery: getData, area: area, salesoffice: salesoffice, customer: customer, family: family, industry: industry, model: model, serialnumber: serialnumber, smuFrom: smurangefrom, smuTo: smurangeto, deliveryDateFrom: deliveryDateFrom, deliveryDateTo: deliveryDateTo, PurchaseDateFrom: PurchaseDateFrom, PurchaseDateTo: PurchaseDateTo, LastRepairDateFrom: LastRepairDateFrom, LastRepairDateTo: LastRepairDateTo, Hid: Hid, Rental: Rental, Others: Others, paramsModel: paramsModel, paramsSerialNumber: paramsSerialNumber);
            var getGroupData = getData.Select(s => s.Serial_Number).Distinct();
            var ListData = new List<V_TR_MEP>();

            foreach (var item in getGroupData)
            {
                var data = new V_TR_MEP();
                data.Serial_Number = item;
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
        }
        public Tuple<List<TableMEPOverviewModel>, int> getListMEPbyModel(string model, string[] area, string[] salesoffice, string[] customer, string[] family, string[] industry, string[] serialnumber, string smuFrom, string smuTo, string PurchaseDateFrom, string PurchaseDateTo, string deliveryDateFrom, string deliveryDateTo, string LastRepairDateFrom, string LastRepairDateTo, string[] Plant, string[] Product, string Hid, string Rental, string orderByDir, string orderByColumn, int start, int length, bool isDistinct, string Others, int download=0)
        {
            if (!String.IsNullOrWhiteSpace(orderByDir)) {
                orderByColumn = orderByColumn.ToLower();
            }
            start = (start == null) ? 0 : start;
            length = (length == null) ? 10 : length;
            var finalData = new List<TableMEPOverviewModel>();
            if (!String.IsNullOrWhiteSpace(model))
            {
                var getData = _ctx.V_TR_MEP.Where(item => true);
                string[] modelData = { };
                getData = LogicFilterQuery(FirstQuery:getData, area:area, salesoffice:salesoffice, customer:customer, family:family, industry: industry, model:modelData,serialnumber:serialnumber,smuFrom:smuFrom,smuTo:smuTo,deliveryDateFrom:deliveryDateFrom, deliveryDateTo:deliveryDateTo, PurchaseDateFrom:PurchaseDateFrom, PurchaseDateTo:PurchaseDateTo, LastRepairDateFrom:LastRepairDateFrom, LastRepairDateTo:LastRepairDateTo, Hid:Hid, Rental:Rental, Others:Others,paramsModel:null,paramsSerialNumber:null);
                var getDataGroup = (from item in getData
                                    from joinMep in _ctx.MEP.Where(mepw => mepw.SerialNumber.Equals(item.Serial_Number)).DefaultIfEmpty()
                                    where item.Product_Model == model
                                    group new { item, joinMep } by item.Serial_Number into dataModel
                                    select new
                                    {
                                        SerialNumber = dataModel.FirstOrDefault().item.Serial_Number,
                                        Smu = dataModel.FirstOrDefault().item.SMU == null ? 0 : dataModel.FirstOrDefault().item.SMU,
                                        smu_update = dataModel.FirstOrDefault().item.SMU_Update,
                                        delivery_date = dataModel.FirstOrDefault().item.Delivery_Date,
                                        purchase_date = dataModel.FirstOrDefault().item.Purchase_Date,
                                        last_serviced = dataModel.FirstOrDefault().item.Last_Work_Order,
                                        industry = (dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry == null && dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry.ToString() == "") ? "-" : dataModel.FirstOrDefault().joinMep.DefinitionSubIndustry.ToString()
                                    }
                                );
                if (isDistinct)
                {
                    getDataGroup = getDataGroup.Distinct();
                }

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
                    data.SMU_Update = String.IsNullOrWhiteSpace(item.smu_update) ? "N/A" :item.smu_update;
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
        public Tuple<List<Organization2>, int> GetDataMapDistribution(bool btnPOST, string[] area, string[] salesoffice, string[] customer, string[] family, string[] industry, string[] model, string[] serialnumber, string smuFrom, string smuTo, string deliveryDateFrom, string deliveryDateTo, string PurchaseDateFrom, string PurchaseDateTo, string LastRepairDateFrom, string LastRepairDateTo, string Hid, string Rental, string Others, string paramsModel = null, string paramsSerialNumber = null)
        {
            if (btnPOST == true || area.Count() > 0 || customer.Count() > 0 || family.Count() > 0 || industry.Count() > 0 || model.Count() > 0 || serialnumber.Count() > 0 || salesoffice.Count() > 0 || smuFrom != "0"|| smuTo != "0" || !String.IsNullOrWhiteSpace(PurchaseDateFrom) || !String.IsNullOrWhiteSpace(PurchaseDateTo) || !String.IsNullOrWhiteSpace(deliveryDateTo) || !String.IsNullOrWhiteSpace(deliveryDateFrom) || !String.IsNullOrWhiteSpace(LastRepairDateFrom) || !String.IsNullOrWhiteSpace(PurchaseDateTo) || Rental == "2" || Hid == "3" || Others == "4")
            {
                var getData = _ctx.V_TR_MEP.Where(item => true);
                getData = LogicFilterQuery(FirstQuery:getData, area:area, salesoffice:salesoffice, customer:customer, family:family, industry: industry, model:model,serialnumber:serialnumber,smuFrom:smuFrom,smuTo:smuTo,deliveryDateFrom:deliveryDateFrom, deliveryDateTo:deliveryDateTo, PurchaseDateFrom:PurchaseDateFrom, PurchaseDateTo:PurchaseDateTo, LastRepairDateFrom:LastRepairDateFrom, LastRepairDateTo:LastRepairDateTo, Hid:Hid, Rental:Rental, Others:Others,paramsModel:paramsModel,paramsSerialNumber:paramsSerialNumber);
                var LocationData = getData.Select(item => item.LOC).Distinct().ToArray();
                var getLocData = _ctx.Organization2.Where(item => LocationData.Contains(item.SalesOfficeCode)).ToList();

                return Tuple.Create(getLocData.ToList(), getLocData.Count());
            }
            else
            {
                return Tuple.Create(_ctx.Organization2.Take(0).ToList(), 0);
            }

        }
        #endregion

        #region===============| getDetail Logic |===========================
        public String getDetailDeliveryDate(string SerialNumber)
        {
            String result = _ctx.V_TR_MEP.Where(item => item.Serial_Number == SerialNumber).Select(s => s.Delivery_Date).FirstOrDefault();
            return String.IsNullOrWhiteSpace(result) ? "N/A" : result;
        }
        public String getDetailPurchasingDate(string SerialNumber)
        {
            String result = _ctx.V_TR_MEP.Where(item => item.Serial_Number == SerialNumber).Select(s => s.Purchase_Date).FirstOrDefault();
            return String.IsNullOrWhiteSpace(result) ? "N/A" : result;
        }
        public String getDetailLastUpdate(string SerialNumber)
        {
            String result = _ctx.V_TR_MEP.Where(item => item.Serial_Number == SerialNumber).Select(s => s.Last_Work_Order).FirstOrDefault();
            return String.IsNullOrWhiteSpace(result) ? "N/A" : result;
        }
        public String getDetailSMUDate(string SerialNumber)
        {
            String result = _ctx.V_TR_MEP.Where(item => item.Serial_Number == SerialNumber).Select(s => s.SMU_Update).FirstOrDefault();
            return String.IsNullOrWhiteSpace(result) ? "N/A" : result;
        }
        public String getDetailSMU(string SerialNumber)
        {
            try
            {
                var result = _ctx.V_TR_MEP.Where(item => item.Serial_Number == SerialNumber).Select(s => s.SMU).FirstOrDefault().ToString();
                return result;
            }
            catch (InvalidOperationException)
            {
                return "-";
            }
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
        #endregion
    }
}