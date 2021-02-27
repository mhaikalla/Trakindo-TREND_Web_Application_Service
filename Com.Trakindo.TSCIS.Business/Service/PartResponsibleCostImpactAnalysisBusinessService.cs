using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class PartResponsibleCostImpactAnalysisBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public String[] data = { "49", "06", "00", "X", "AA", "28", "38", "50", "74", "75", "76", "08", "RD", "20", "98" };

        public List<string> GetListPartNo()
        {
            var result = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure != null && item.PartCausingFailure != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98").Select(m => m.PartCausingFailure).Distinct().ToList();
            return result;
        }

        public List<string> GetListModel()
        {
            var result = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.Model != null && item.Model != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98").Select(m => m.Model).Distinct().ToList();
            return result;
        }

        public List<string> GetListPartDesc()
        {
            var result = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailureDescription != null && item.PartCausingFailureDescription != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98").Select(m => m.PartCausingFailureDescription).Distinct().ToList();
            return result;
        }

        public List<string> GetListPrefixSN()
        {
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           group item by item.PrefixSN into a
                           select new
                           {
                               PrefixSN = a.FirstOrDefault().PrefixSN
                           }).OrderBy(odb => odb.PrefixSN);
            var listString = new List<string>();
            foreach(var item in getData.ToList())
            {
                listString.Add(item.PrefixSN);
            }
            return listString;
        }

        public List<TablePPMPotentialByCost> GetDataForTablePPMByCost(string dateRangeFrom, string dateRangeEnd, string[] splitPartNumber, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, bool searchSerialNo, bool searchModel, bool searchProductProblemDescription, bool searchComment, bool searchServiceOrder, bool searchServiceMeter, bool searchUnitMes, bool searchSalesOffice, bool searchPartNo, bool searchPartDesc, bool searchRepairDate, bool searchCurrency, bool searchTotalCostSO, string valueSearch, int column, string order, string hid, string rental, string inventory, string others)
        {
            var listItem = new List<TablePPMPotentialByCost>();
            var getListData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                               where item.PartCausingFailure != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comments,
                                   ServceOrder = item.ServiceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartCausingFailure,
                                   PartDesc = item.PartCausingFailureDescription,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalSO,
                                   GroupNo = item.GroupNumber,
                                   GroupDesc = item.GroupDescription,
                                   SoClaim = item.TotalClaim,
                                   SoSettled = item.TotalSettled,
                                   HidTaskId = item.HIDTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            if (hid == "on" && rental == "" && inventory == "")
            {
                getListData = (from item in getListData
                               where item.HidTaskId != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getListData = (from item in getListData
                               where item.HidTaskId != null || item.RentStatus != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getListData = (from item in getListData
                               where item.HidTaskId != null || item.Plant != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getListData = (from item in getListData
                               where item.RentStatus == null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getListData = (from item in getListData
                               where item.Plant != null || item.RentStatus != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getListData = (from item in getListData
                               where item.Plant != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getListData = (from item in getListData
                               where item.HidTaskId != null || item.Plant != null || item.RentStatus != null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getListData = (from item in getListData
                               where item.HidTaskId == null || item.Plant == null|| item.RentStatus == null
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if(splitPartNumber.Count() > 0 && splitPartNumber != null)
            {
                getListData = (from item in getListData
                               where splitPartNumber.Contains(item.PartNo)
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }
            if(splitPartDescription.Count() > 0 && splitPartDescription != null)
            {
                getListData = (from item in getListData
                               where splitPartDescription.Contains(item.PartDesc)
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }
            if(splitModel.Count() > 0 && splitModel != null)
            {
                getListData = (from item in getListData
                               where splitModel.Contains(item.Model)
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }
            if(splitPrefixSN.Count() > 0)
            {
                getListData = (from item in getListData
                               where splitPrefixSN.Contains(item.PrefixSN)
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                getListData = (from item in getListData
                               where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }
            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                getListData = (from item in getListData
                               where item.RepairDate.Value == convertToDateTimeFrom
                               select new
                               {
                                   SerialNumber = item.SerialNumber,
                                   Model = item.Model,
                                   ProductProblemDescription = item.ProductProblemDescription,
                                   Comment = item.Comment,
                                   ServceOrder = item.ServceOrder,
                                   ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                   UnitMes = item.UnitMes,
                                   SalesOffice = item.SalesOffice,
                                   PartNo = item.PartNo,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   Currency = item.Currency,
                                   TotalCostSo = item.TotalCostSo,
                                   GroupNo = item.GroupNo,
                                   GroupDesc = item.GroupDesc,
                                   SoClaim = item.SoClaim,
                                   SoSettled = item.SoSettled,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }

            if (!string.IsNullOrWhiteSpace(valueSearch))
            {
                if(searchSerialNo == true || searchModel == true || searchProductProblemDescription == true || searchComment == true || searchServiceOrder == true || searchServiceMeter == true || searchUnitMes == true || searchSalesOffice == true || searchPartNo == true || searchPartDesc == true || searchRepairDate == true || searchCurrency == true || searchTotalCostSO == true)
                {
                    getListData = (from item in getListData
                                   where item.ProductProblemDescription.Contains(valueSearch) || item.PartNo.Contains(valueSearch) || item.PartDesc.Contains(valueSearch) || item.TotalCostSo.ToString().Contains(valueSearch) || item.GroupNo.Contains(valueSearch.ToLower()) || item.GroupDesc.ToLower().Contains(valueSearch.ToLower())
                                   select new
                                   {
                                       SerialNumber = item.SerialNumber,
                                       Model = item.Model,
                                       ProductProblemDescription = item.ProductProblemDescription,
                                       Comment = item.Comment,
                                       ServceOrder = item.ServceOrder,
                                       ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                       UnitMes = item.UnitMes,
                                       SalesOffice = item.SalesOffice,
                                       PartNo = item.PartNo,
                                       PartDesc = item.PartDesc,
                                       RepairDate = item.RepairDate,
                                       Currency = item.Currency,
                                       TotalCostSo = item.TotalCostSo,
                                       GroupNo = item.GroupNo,
                                       GroupDesc = item.GroupDesc,
                                       SoClaim = item.SoClaim,
                                       SoSettled = item.SoSettled,
                                       HidTaskId = item.HidTaskId,
                                       RentStatus = item.RentStatus,
                                       Plant = item.Plant,
                                       PrefixSN = item.PrefixSN
                                   });
                }
            }

            var getData = (from item in getListData
                           select new
                           {
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductPRoblemDescription = item.ProductProblemDescription,
                               Comment = item.Comment,
                               GroupNo = item.GroupNo,
                               GroupDesc = item.GroupDesc,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Currency = item.Currency,
                               TotalSOCost = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == item.PartNo).Select(s => s.TotalSO).Sum(),
                               TotalClaim = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == item.PartNo).Select(s => s.TotalClaim).Sum(),
                               TotalSettled = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == item.PartNo).Select(s => s.TotalSettled).Sum()
                           });

            var listDataAll = (from item in getData
                               group item by item.PartNo into partno
                               select new
                               {
                                   SerialNumber = partno.FirstOrDefault().SerialNumber,
                                   Model = partno.FirstOrDefault().Model,
                                   ProductProblemDescription = partno.FirstOrDefault().ProductPRoblemDescription,
                                   Comment = partno.FirstOrDefault().Comment,
                                   PartNo = partno.FirstOrDefault().PartNo,
                                   PartDesc = partno.FirstOrDefault().PartDesc,
                                   Currency = partno.FirstOrDefault().Currency,
                                   GroupNo = partno.FirstOrDefault().GroupNo,
                                   GroupDesc = partno.FirstOrDefault().GroupDesc,
                                   TotalSo = partno.FirstOrDefault().TotalSOCost,
                                   TotalClaim = partno.FirstOrDefault().TotalClaim,
                                   TotalSettled = partno.FirstOrDefault().TotalSettled
                               }).OrderByDescending(odb => odb.TotalSo);


            foreach (var item in listDataAll.ToList().Take(100))
            {
                var list = new TablePPMPotentialByCost();
                var getTotalSoCost = CountTotalSO(item.PartNo, hid, inventory, rental, others, splitPartDescription, splitModel, splitPrefixSN, dateRangeFrom, dateRangeEnd);
                var getTotalClaim = CountTotalClaim(item.PartNo, hid, inventory, rental, others, splitPartDescription, splitModel, splitPrefixSN, dateRangeFrom, dateRangeEnd);
                var getTotalSettled = CountTotalSettled(item.PartNo, hid, inventory, rental, others, splitPartDescription, splitModel, splitPrefixSN, dateRangeFrom, dateRangeEnd);
                list.Row = item.PartNo;
                list.SerialNumber = item.SerialNumber;
                list.Model = item.Model;
                if (!string.IsNullOrWhiteSpace(item.ProductProblemDescription))
                {
                    list.ProductProblemDescription = item.ProductProblemDescription;
                }
                else
                {
                    list.ProductProblemDescription = "-";
                }
                
                list.Comment = item.Comment;
                if (!string.IsNullOrWhiteSpace(item.PartNo))
                {
                    list.PartNo = item.PartNo;
                }
                else
                {
                    list.PartNo ="-";
                }

                if (!string.IsNullOrWhiteSpace(item.PartDesc))
                {
                    list.PartDescription = item.PartDesc;
                }
                else{
                    list.PartDescription = "-";
                }
                list.Currency = item.Currency;
                list.TotalSoCost = getTotalSoCost;
                list.SoClaim = getTotalClaim;
                list.SOSettled = getTotalSettled;
                if (!string.IsNullOrWhiteSpace(item.GroupNo))
                {
                    list.GroupNo = item.GroupNo;
                }
                else
                {
                    list.GroupNo = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.GroupDesc))
                {
                    list.GroupDesc = item.GroupDesc;
                }
                else
                {
                    list.GroupDesc = "-";
                }
                
                
                listItem.Add(list);
            }
            listItem = listItem.OrderByDescending(odb => odb.TotalSoCost).ToList();
            return listItem;
        }

        public decimal CountTotalSO(string partNo, string hid, string inventory, string rental, string others, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, string dateRangeFrom, string dateRangeEnd )
        {
            decimal result = 0;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == partNo);
            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.RentStatus != "" || item.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.RentStatus != "" || item.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId == "" || item.Plant == "" || item.RentStatus == "");
            }
            if(splitPartDescription.Count() > 0)
            {
                data = data.Where(w => splitPartDescription.Contains(w.PartCausingFailureDescription));
            }
            if(splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Contains(w.PrefixSN));
            }
            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }

            result = data.Select(s => s.TotalAmount).Sum();
            return result;
        }

        public decimal CountTotalClaim(string partNo, string hid, string inventory, string rental, string others, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, string dateRangeFrom, string dateRangeEnd)
        {
            decimal result = 0;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == partNo);
            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.RentStatus != "" || item.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.RentStatus != "" || item.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId == "" || item.Plant == "" || item.RentStatus == "");
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => splitPartDescription.Contains(w.PartCausingFailureDescription));
            }
            if (splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Contains(w.PrefixSN));
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }

            result = data.Select(s => s.TotalClaim).Sum();
            return result;
        }

        public decimal CountTotalSettled(string partNo, string hid, string inventory, string rental, string others, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, string dateRangeFrom, string dateRangeEnd)
        {
            decimal result = 0;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == partNo);
            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.RentStatus != "" || item.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.RentStatus != "" || item.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId == "" || item.Plant == "" || item.RentStatus == "");
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => splitPartDescription.Contains(w.PartCausingFailureDescription));
            }
            if (splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Contains(w.PrefixSN));
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }

            result = data.Select(s => s.TotalSettled).Sum();
            return result;
        }

        public List<TablePPMPotentialByFrequency> GetDataForTablePPMByFrequency(string dateRangeFrom, string dateRangeEnd, string[] splitPartNumber, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, bool searchSerialNo, bool searchModel, bool searchProductProblemDescription, bool searchComment, bool searchServiceOrder, bool searchServiceMeter, bool searchUnitMes, bool searchSalesOffice, bool searchPartNo, bool searchPartDesc, bool searchRepairDate, bool searchCurrency, bool searchTotalCostSO, string valueSearch, int column, string order, string hid, string rental, string inventory, string others)
        {
            var listItem = new List<TablePPMPotentialByFrequency>();
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               Comment = item.Comments,
                               //ServceOrder = _ctx.PartResponsibleCostImpactAnalysis.Where(w => w.PartCausingFailure == item.PartCausingFailure && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98").Select(s => s.ServiceOrder).Distinct().Count(),
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               PartNo = item.PartCausingFailure,
                               PartDesc = item.PartCausingFailureDescription,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               TotalCostSo = item.TotalSO,
                               GroupNo = item.GroupNumber,
                               GroupDesc = item.GroupDescription,
                               CountOfRepair = item.RepairingDealer,
                               HidTaskId = item.HIDTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            var getDataRebuild = (from item in getData
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });

            if (hid == "on" && rental == "" && inventory == "")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.HidTaskId != null 
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.HidTaskId != null || item.RentStatus != null 
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.HidTaskId != null || item.Plant != null
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.RentStatus != null 
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.RentStatus != null || item.Plant != null
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.Plant != null
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (splitPartNumber.Count() > 0 && splitPartNumber != null)
            {
                getDataRebuild = (from item in getDataRebuild
                                  where splitPartNumber.Contains(item.PartNo)
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }
            if(splitPartDescription.Count() > 0 && splitPartDescription != null)
            {
                getDataRebuild = (from item in getDataRebuild
                                  where splitPartDescription.Contains(item.PartDesc)
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }
            if(splitModel.Count() > 0 && splitModel != null)
            {
                getDataRebuild = (from item in getDataRebuild
                                  where splitModel.Contains(item.Model)
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }
            if(splitPrefixSN.Count() > 0 && splitPrefixSN != null)
            {
                getDataRebuild = (from item in getDataRebuild
                                  where splitPrefixSN.Contains(item.PrefixSN)
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }
            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                getDataRebuild = (from item in getDataRebuild
                                  where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }
            if(!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                getDataRebuild = (from item in getDataRebuild
                                  where item.RepairDate.Value == convertToDateTimeFrom
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            if (!string.IsNullOrWhiteSpace(valueSearch))
            {
                getDataRebuild = (from item in getDataRebuild
                                  where item.ProductProblemDescription.Contains(valueSearch) || item.PartNo.Contains(valueSearch) || item.PartDesc.Contains(valueSearch) || item.GroupNo.Contains(valueSearch) || item.GroupDesc.ToLower().Contains(valueSearch.ToLower())
                                  select new
                                  {
                                      PartResponsibleId = item.PartResponsibleId,
                                      SerialNumber = item.SerialNumber,
                                      Model = item.Model,
                                      ProductProblemDescription = item.ProductProblemDescription,
                                      Comment = item.Comment,
                                      //ServceOrder = item.ServceOrder,
                                      ServiceOrder = item.ServiceOrder,
                                      ServiceMeterMeasurement = item.ServiceMeterMeasurement,
                                      UnitMes = item.UnitMes,
                                      SalesOffice = item.SalesOffice,
                                      PartNo = item.PartNo,
                                      PartDesc = item.PartDesc,
                                      RepairDate = item.RepairDate,
                                      Currency = item.Currency,
                                      TotalCostSo = item.TotalCostSo,
                                      GroupNo = item.GroupNo,
                                      GroupDesc = item.GroupDesc,
                                      //CountOfRepair = item.ServceOrder,
                                      HidTaskId = item.HidTaskId,
                                      RentStatus = item.RentStatus,
                                      Plant = item.Plant,
                                      PrefixSN = item.PrefixSN
                                  });
            }

            var vardataRebuild = (from item in getDataRebuild
                                  where item.PartNo != "" && item.PartNo != "-"
                                  group item by item.PartNo into partNo
                                  select new
                                  {
                                      PartResponsibleId = partNo.FirstOrDefault().PartResponsibleId,
                                      SerialNumber = partNo.FirstOrDefault().SerialNumber,
                                      Model = partNo.FirstOrDefault().Model,
                                      ProductProblemDescription = partNo.FirstOrDefault().ProductProblemDescription,
                                      Comment = partNo.FirstOrDefault().Comment,
                                      //ServceOrder = partNo.FirstOrDefault().ServceOrder,
                                      ServiceOrder = partNo.FirstOrDefault().ServiceOrder,
                                      ServiceMeterMeasurement = partNo.FirstOrDefault().ServiceMeterMeasurement,
                                      UnitMes = partNo.FirstOrDefault().UnitMes,
                                      SalesOffice = partNo.FirstOrDefault().SalesOffice,
                                      PartNo = partNo.FirstOrDefault().PartNo,
                                      PartDesc = partNo.FirstOrDefault().PartDesc,
                                      RepairDate = partNo.FirstOrDefault().RepairDate,
                                      Currency = partNo.FirstOrDefault().Currency,
                                      TotalCostSo = partNo.FirstOrDefault().TotalCostSo,
                                      GroupNo = partNo.FirstOrDefault().GroupNo,
                                      GroupDesc = partNo.FirstOrDefault().GroupDesc,
                                      //CountOfRepair = partNo.FirstOrDefault().ServceOrder,
                                      HidTaskId = partNo.FirstOrDefault().HidTaskId,
                                      RentStatus = partNo.FirstOrDefault().RentStatus,
                                      Plant = partNo.FirstOrDefault().Plant,
                                      PrefixSN = partNo.FirstOrDefault().PrefixSN
                                  });

            
            foreach(var item in vardataRebuild.Take(100).ToList())
            {
                var listGroupNo = new List<string>();
                var list = new TablePPMPotentialByFrequency();
                var countSO = CountTotalServiceOrder(item.PartNo, hid, inventory, rental, others, splitPartDescription, splitModel, splitPrefixSN, dateRangeFrom, dateRangeEnd);
                list.Row = item.PartNo;
                list.SerialNumber = item.SerialNumber;
                list.Model = item.Model;
                if (string.IsNullOrWhiteSpace(item.ProductProblemDescription))
                {
                    list.ProductProblemDescription = "-";
                }
                else
                {
                    list.ProductProblemDescription = item.ProductProblemDescription;
                }

                list.Comment = item.Comment;
                list.ServiceOrder = item.ServiceOrder;
                list.UnitMes = item.UnitMes;
                list.SalesOffice = item.SalesOffice;
                if (string.IsNullOrWhiteSpace(item.PartNo))
                {
                    list.PartNo = "-";
                }
                else
                {
                    list.PartNo = item.PartNo;
                }

                if (string.IsNullOrWhiteSpace(item.PartDesc))
                {
                    list.PartDescription = "-";
                }
                else
                {
                    list.PartDescription = item.PartDesc;
                }
                list.CountOfRepair = countSO;
                listGroupNo.Add(item.GroupNo);
                list.GroupNo = listGroupNo;

                if (string.IsNullOrWhiteSpace(item.GroupDesc))
                {
                    list.GroupDesc = "-";
                }
                else
                {
                    list.GroupDesc = item.GroupDesc;
                }
                list.RepairDate = item.RepairDate;
                list.Currency = item.Currency;
                list.TotalSoCost = item.TotalCostSo;
                listItem.Add(list);
            }
            listItem = listItem.OrderByDescending(odb => odb.CountOfRepair).ToList();
               
            return listItem;
        }

        public int CountTotalServiceOrder(string partNo, string hid, string inventory, string rental, string others, string[] splitPartDescription, string[] splitModel, string[] splitPrefixSN, string dateRangeFrom, string dateRangeEnd)
        {
            int result = 0;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PartCausingFailure == partNo && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != "");
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.Plant != "");
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.RentStatus != "");
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.RentStatus != "" || item.Plant != "");
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.Plant != "");
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != "" || item.RentStatus != "" || item.Plant != "");
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId == "" || item.Plant == "" || item.RentStatus == "");
            }
            if (splitPartDescription.Count() > 0)
            {
                data = data.Where(w => splitPartDescription.Contains(w.PartCausingFailureDescription));
            }
            if (splitModel.Count() > 0)
            {
                data = data.Where(w => splitModel.Contains(w.Model));
            }
            if (splitPrefixSN.Count() > 0)
            {
                data = data.Where(w => splitPrefixSN.Contains(w.PrefixSN));
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && !string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateRangeEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateRangeFrom) && string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateRangeFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }

            result = data.Select(s => s.ServiceOrder).Distinct().Count();
            return result;
        }

        public List<DataChartComboDPPM> GetData(string[] partNumber, string[] partDesc, string[] Model, string[] prefixSN, string dateFrom, string dateEnd, string hid, string rental, string inventory, string others)
        {
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.Model != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SerialNumber.Substring(0,3),
                               TotalSoCost = item.TotalAmount,
                               TotalSo = item.TotalSO,
                               PartNumber = item.PartCausingFailure,
                               PartDesc = item.PartCausingFailureDescription,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HIDTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null 
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != null 
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value == convertToDateTimeFrom
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (partNumber.Count() > 0)
            {
                 getData = (from item in getData
                               where partNumber.Contains(item.PartNumber)
                               select new
                               {
                                   PartResponsibleId = item.PartResponsibleId,
                                   Model = item.Model,
                                   SRnumber = item.SRnumber,
                                   TotalSoCost = item.TotalSoCost,
                                   TotalSo = item.TotalSo,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   RepairDate = item.RepairDate,
                                   ProductProblem = item.ProductProblem,
                                   HidTaskId = item.HidTaskId,
                                   RentStatus = item.RentStatus,
                                   Plant = item.Plant,
                                   PrefixSN = item.PrefixSN
                               });
            }
            if (Model.Count() > 0)
            {
                getData = (from item in getData
                           where Model.Contains(item.Model)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }
            if (partDesc.Count() > 0)
            {
                getData = (from item in getData
                           where partDesc.Contains(item.PartDesc)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }
            if (prefixSN.Count() > 0)
            {
                getData = (from item in getData
                           where prefixSN.Contains(item.PrefixSN)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               Model = item.Model,
                               SRnumber = item.SRnumber,
                               TotalSoCost = item.TotalSoCost,
                               TotalSo = item.TotalSo,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               RepairDate = item.RepairDate,
                               ProductProblem = item.ProductProblem,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            var getDataSubstringSN = (from item in getData
                                      group item by item.Model into sn
                                      select new
                                      {
                                          PartResponsibleId = sn.FirstOrDefault().PartResponsibleId,
                                          Model = sn.FirstOrDefault().Model,
                                          SRnumber = sn.FirstOrDefault().PrefixSN,
                                          PartNumber = sn.FirstOrDefault().PartNumber
                                      });
            if(partNumber.Count() > 0 || partDesc.Count() > 0 || Model.Count() > 0 || prefixSN.Count() > 0 || dateFrom != "" || hid != "" || rental != "" || inventory != "" || others != "")
            {
                getDataSubstringSN = (from item in getDataSubstringSN
                                      select new
                                      {
                                          PartResponsibleId = item.PartResponsibleId,
                                          Model = item.Model,
                                          SRnumber = item.SRnumber,
                                          PartNumber = item.PartNumber
                                      }).OrderBy(model => model.Model).Take(15);
            }
            else
            {
                getDataSubstringSN = (from item in getDataSubstringSN
                                      select new
                                      {
                                          PartResponsibleId = item.PartResponsibleId,
                                          Model = item.Model,
                                          SRnumber = item.SRnumber,
                                          PartNumber = item.PartNumber
                                      }).OrderBy(model => model.Model).Take(0);
            }

            var listItem = new List<DataChartComboDPPM>();
            foreach (var item in getDataSubstringSN)
            {
                
                CountSOChartFinance GetDataPrefixSN = GetPrefixSN(item.Model, item.SRnumber, partNumber, partDesc, prefixSN, dateFrom, dateEnd, hid, rental, inventory, others);
                foreach(var dataItem in GetDataPrefixSN.PrefixSN)
                {
                    var listLabel = new List<string>();
                    var list = new DataChartComboDPPM();
                    CountSOChartFinance CountSo = CountSO(dataItem, partNumber, partDesc, item.Model, dateFrom, dateEnd, hid, rental, inventory, others);
                    listLabel.Add(dataItem);
                    listLabel.Add(item.Model);
                    listLabel.Add("new");
                    list.Model = listLabel;
                    list.DataTotalCost = CountSo.TotalSoCost;
                    list.DataTotalSO = CountSo.TotalSO;
                    list.SerialNumber = item.SRnumber;
                    listItem.Add(list);
                }
            }
            return listItem;
        }

        public CountSOChartFinance CountSO(string sn, string[] partNumber, string[] partDesc, string Model, string dateFrom, string dateEnd, string hid, string rental, string inventory, string others)
        {
            var data = new CountSOChartFinance();
            var countTotalSo = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PrefixSN.Contains(sn) && item.Model == Model && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            var countTotalSoCost = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.PrefixSN.Contains(sn) && item.Model == Model && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if (hid == "on" && rental == "" && inventory == "")
            {
                countTotalSo = countTotalSo.Where(w => w.HIDTaskId != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.HIDTaskId != null);
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                countTotalSo = countTotalSo.Where(w => w.HIDTaskId != null || w.RentStatus != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.HIDTaskId != null || w.RentStatus != null);
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                countTotalSo = countTotalSo.Where(w => w.HIDTaskId != null || w.Plant != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.HIDTaskId != null || w.Plant != null);
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                countTotalSo = countTotalSo.Where(w => w.RentStatus != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.RentStatus != null);
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                countTotalSo = countTotalSo.Where(w => w.RentStatus != null || w.Plant != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.RentStatus != null || w.Plant != null);
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                countTotalSo = countTotalSo.Where(w => w.Plant != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.Plant != null);
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                countTotalSo = countTotalSo.Where(w => w.HIDTaskId != null || w.RentStatus != null || w.Plant != null);
                countTotalSoCost = countTotalSoCost.Where(w => w.HIDTaskId != null || w.RentStatus != null || w.Plant != null);
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                countTotalSo = countTotalSo.Where(w => w.HIDTaskId == null || w.RentStatus == null || w.Plant == null);
                countTotalSoCost = countTotalSoCost.Where(w => w.HIDTaskId == null || w.RentStatus == null || w.Plant == null);
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                countTotalSo = countTotalSo.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
                countTotalSoCost = countTotalSoCost.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                countTotalSo = countTotalSo.Where(w => w.RepairDate.Value == convertToDateTimeFrom );
                countTotalSoCost = countTotalSoCost.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }
            if(partNumber.Count() > 0)
            {
                countTotalSo = countTotalSo.Where(w => partNumber.Contains(w.PartCausingFailure));
                countTotalSoCost = countTotalSoCost.Where(w => partNumber.Contains(w.PartCausingFailure));
            }
            if(partDesc.Count() > 0)
            {
                countTotalSo = countTotalSo.Where(w => partDesc.Contains(w.PartCausingFailureDescription));
                countTotalSoCost = countTotalSoCost.Where(w => partDesc.Contains(w.PartCausingFailureDescription));
            }
            int countTotalSOConv = countTotalSo.Select(s => s.ServiceOrder).Distinct().Count();
            var getCountDataTotalSOCost = (from item in countTotalSoCost
                                           group item by item.ServiceOrder into so
                                           select new
                                           {
                                               TotalAmount = so.FirstOrDefault().TotalAmount
                                           });
            decimal sum = 0;
            foreach (var item in getCountDataTotalSOCost)
            {
                sum += item.TotalAmount;
            }
            //decimal countTotalSoCostConv = countTotalSoCost.Select(s => s.TotalAmount).Sum();
            data.TotalSO = countTotalSOConv;
            data.TotalSoCost = sum;
            return data;
        }

        public CountSOChartFinance GetPrefixSN(string model,string sn, string[] partNumber, string[] partDesc, string[] prefixSN, string dateFrom, string dateEnd, string hid, string rental, string inventory, string others)
        {
            var getData = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.Model == model && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDTaskId != null);
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.HIDTaskId != null || w.RentStatus != null);
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDTaskId != null || w.Plant != null);
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = getData.Where(w => w.RentStatus != null);
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.RentStatus != null || w.Plant != null);
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = getData.Where(w => w.Plant != null);
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = getData.Where(w => w.HIDTaskId != null || w.RentStatus != null || w.Plant != null);
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = getData.Where(w => w.HIDTaskId == null || w.RentStatus == null || w.Plant == null);
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = getData.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                getData = getData.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }
            if (partNumber.Count() > 0)
            {
                getData = getData.Where(w => partNumber.Contains(w.PartCausingFailure));
            }
            if (partDesc.Count() > 0)
            {
                getData = getData.Where(w => partDesc.Contains(w.PartCausingFailureDescription));
            }
            if (prefixSN.Count() > 0)
            {
                getData = getData.Where(w => prefixSN.Contains(w.PrefixSN));
            }
            var getGroupPrefixSN = (from item in getData
                                           group item by item.PrefixSN into serialNo
                                           select new
                                           {
                                               PrefixSN = serialNo.FirstOrDefault().PrefixSN
                                           });

            var dataPrefix = new CountSOChartFinance();
            var listData = new List<string>();
            foreach (var item in getGroupPrefixSN)
            {
                listData.Add(item.PrefixSN);
            }
            dataPrefix.PrefixSN = listData;
            return dataPrefix;
        }

        public List<TableDPPMFinance> GetDataTableDPPMFinance(string dateFrom, string dateEnd, string searchValue, string[] partNo, string[] partDesc, string[] model, string[] prefixSN, string sn, string modelchart, string hid, string rental, string inventory, string others)
        {
            var listItem = new List<TableDPPMFinance>();
            var dateTimeNow = DateTime.Now;
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.Model != "" && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartCausingFailure,
                               PartDesc = item.PartCausingFailureDescription,
                               Comment = item.Comments,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeterMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOfficeDescription,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HIDTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != null 
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value == convertToDateTimeFrom
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (partNo.Count() > 0)
            {
                getData = (from item in getData
                           where partNo.Contains(item.PartNo)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if(partDesc.Count() > 0)
            {
                getData = (from item in getData
                           where partDesc.Contains(item.PartDesc)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if(model.Count() > 0)
            {
                getData = (from item in getData
                           where model.Contains(item.Model)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (prefixSN.Count() > 0)
            {
                getData = (from item in getData
                           where prefixSN.Contains(item.PrefixSN)
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }
            

            if (!string.IsNullOrWhiteSpace(sn) && !string.IsNullOrWhiteSpace(modelchart))
            {
                getData = (from item in getData
                           where item.PrefixSN == sn && item.Model == modelchart
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            }

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                getData = getData.Where(w => w.SerialNumber.ToLower().Contains(searchValue) || w.Model.ToLower().Contains(searchValue) || w.ProductProblemDescription.ToLower().Contains(searchValue) || w.Comment.ToLower().Contains(searchValue) || w.ServiceOrder.ToLower().Contains(searchValue) || w.ServiceMeasurement.ToString().Contains(searchValue) || w.UnitMes.ToLower().Contains(searchValue) || w.SalesOffice.ToLower().Contains(searchValue) || w.PartNo.ToLower().Contains(searchValue) || w.PartDesc.ToLower().Contains(searchValue));
            }

            var AllData = (from item in getData
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           });
            if(partNo.Count() > 0 || partDesc.Count() > 0 || model.Count() > 0 || prefixSN.Count() > 0 || dateFrom != "" || hid != "" || rental != "" || inventory != "" || others != "")
            {
                AllData = (from item in AllData
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           }).OrderBy(odb => odb.Model);
            }
            else
            {
                AllData = (from item in AllData
                           select new
                           {
                               PartResponsibleId = item.PartResponsibleId,
                               SerialNumber = item.SerialNumber,
                               Model = item.Model,
                               ProductProblemDescription = item.ProductProblemDescription,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Comment = item.Comment,
                               ServiceOrder = item.ServiceOrder,
                               ServiceMeasurement = item.ServiceMeasurement,
                               UnitMes = item.UnitMes,
                               SalesOffice = item.SalesOffice,
                               RepairDate = item.RepairDate,
                               Currency = item.Currency,
                               HidTaskId = item.HidTaskId,
                               RentStatus = item.RentStatus,
                               Plant = item.Plant,
                               PrefixSN = item.PrefixSN
                           }).OrderBy(odb => odb.Model).Take(0);
            }

                var idRow = 0;
            foreach (var item in AllData)
            {
                var list = new TableDPPMFinance();
                decimal totalSO = CountTotalSOForTableDPPMFinance(item.ServiceOrder, hid, rental, inventory, others, partNo, partDesc, model, prefixSN, dateFrom, dateEnd);
                var repairDate = "";
                var splitDate = new string[] { };
                if (item.RepairDate != null)
                {
                    repairDate = item.RepairDate.Value.Date.ToString();
                    splitDate = repairDate.Split(' ');
                }
                list.Row = idRow++;
                if (!string.IsNullOrWhiteSpace(item.SerialNumber))
                {
                    list.SerialNumber = item.SerialNumber;
                }
                else
                {
                    list.SerialNumber = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.Model))
                {
                    list.Model = item.Model;
                }
                else
                {
                    list.Model = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.ProductProblemDescription))
                {
                    list.ProductProblemDescription = item.ProductProblemDescription;
                }
                else
                {
                    list.ProductProblemDescription = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.PartNo))
                {
                    list.PartNo = item.PartNo;
                }
                else
                {
                    list.PartNo = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.PartDesc))
                {
                    list.PartDescription = item.PartDesc;
                }
                else
                {
                    list.PartDescription = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.Comment))
                {
                    list.Comment = item.Comment;
                }
                else
                {
                    list.Comment = "-";
                }
                if (!string.IsNullOrWhiteSpace(item.ServiceOrder))
                {
                    list.ServiceOrder = item.ServiceOrder;
                }
                else
                {
                    list.ServiceOrder = "-";
                }

                if (item.ServiceMeasurement != 0)
                {
                    list.ServiceMeterMeasurement = item.ServiceMeasurement;
                }
                else
                {
                    list.ServiceMeterMeasurement = 0;
                }

                if (!string.IsNullOrWhiteSpace(item.UnitMes))
                {
                    list.UnitMes = item.UnitMes;
                }
                else
                {
                    list.UnitMes = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.SalesOffice))
                {
                    list.SalesOffice = item.SalesOffice;
                }
                else
                {
                    list.SalesOffice = "-";
                }
                
                if(item.RepairDate.ToString() != null)
                {
                    list.RepairDate = splitDate[0].ToString();
                }
                else
                {
                    list.RepairDate = "-";
                }

                if (!string.IsNullOrWhiteSpace(item.Currency))
                {
                    list.Currency = item.Currency;
                }
                else
                {
                    list.Currency = "-";
                }

                if (totalSO != 0)
                {
                    list.TotalSoCost = totalSO;
                }
                else
                {
                    list.TotalSoCost = 0;
                }
                
                listItem.Add(list);
            }
            return listItem;
        }

        public decimal CountTotalSOForTableDPPMFinance(string serviceOrder,string hid,string rental, string inventory, string others, string[] partNo, string[] partDesc, string[] model, string[] prefixSN, string dateFrom, string dateEnd)
        {
            decimal total = 0;
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.ServiceOrder == serviceOrder && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");

            if (hid == "on" && rental == "" && inventory == "")
            {
                data = data.Where(w => w.HIDTaskId != null);
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(w => w.HIDTaskId != null || w.RentStatus != null);
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(w => w.HIDTaskId != null || w.Plant != null);

            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(w => w.RentStatus != null);

            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(w => w.RentStatus != null || w.Plant != null);

            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(w => w.Plant != null);

            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(w => w.HIDTaskId != null || w.RentStatus != null || w.Plant != null);
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(w => w.HIDTaskId == null || w.RentStatus == null || w.Plant == null);
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value >= convertToDateTimeFrom && w.RepairDate.Value <= convertToDateTimeEnd);
            }

            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                data = data.Where(w => w.RepairDate.Value == convertToDateTimeFrom);
            }

            if (partNo.Count() > 0)
            {
                data = data.Where(w => partNo.Contains(w.PartCausingFailure));
            }
            if(partDesc.Count() > 0)
            {
                data = data.Where(w => partDesc.Contains(w.PartCausingFailureDescription));
            }
            if(model.Count() > 0)
            {
                data = data.Where(w => model.Contains(w.Model));
            }
            if(prefixSN.Count() > 0)
            {
                data = data.Where(w => prefixSN.Contains(w.PrefixSN));
            }
            total = data.Select(s => s.TotalAmount).Sum();
            return total;
        }

        public CountDataDPPMFinance CountTotalSODPPMFinance(string[] partNo, string[] partDesc, string[] model, string[] prefixSN, string dateFrom, string dateEnd, string hid, string inventory, string rental, string others)
        {
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           select new
                           {
                               TotalSOCost = item.TotalAmount,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartCausingFailure,
                               PartDesc = item.PartCausingFailureDescription,
                               Model = item.Model, 
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value == convertToDateTimeFrom
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDTaskId != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDTaskId != null || item.RentStatus != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDTaskId != null || item.Plant != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != null || item.Plant != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HIDTaskId != null || item.RentStatus != null || item.Plant != null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HIDTaskId == null || item.RentStatus == null || item.Plant == null
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            if (partNo.Count() > 0)
            {
                getData = (from item in getData
                           where partNo.Contains(item.PartNo)
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }
            if (partDesc.Count() > 0)
            {
                getData = (from item in getData
                           where partDesc.Contains(item.PartDesc)
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }
            if (model.Count() > 0)
            {
                getData = (from item in getData
                           where model.Contains(item.Model)
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }
            if (prefixSN.Count() > 0)
            {
                getData = (from item in getData
                           where prefixSN.Contains(item.PrefixSN)
                           select new
                           {
                               TotalSOCost = item.TotalSOCost,
                               ServiceOrder = item.ServiceOrder,
                               PartNo = item.PartNo,
                               PartDesc = item.PartDesc,
                               Model = item.Model,
                               SerialNumber = item.SerialNumber,
                               RepairDate = item.RepairDate,
                               PrefixSN = item.PrefixSN,
                               HIDTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus
                           });
            }

            //var sumData = (from item in getData
            //               select new
            //               {
            //                   TotalSOCost = getData.Where(w => w.SerialNumber != "" && w.SerialNumber != null).Select(s => s.TotalSOCost).Sum(),
            //                   TotalUnitImpacted = getData.Select(s => s.SerialNumber).Distinct().Count(),
            //                   QuantitySummary = getData.Where(w => w.ServiceOrder != "" && w.ServiceOrder != null).Select(s => s.ServiceOrder).Distinct().Count()
            //               });
            var resultData = (from item in getData
                              group item by item.Model into mod
                              select new
                              {
                                  TTotalSOCost = mod.FirstOrDefault().TotalSOCost,
                                  ServiceOrder = mod.FirstOrDefault().ServiceOrder,
                                  PartNo = mod.FirstOrDefault().PartNo,
                                  PartDesc = mod.FirstOrDefault().PartDesc,
                                  Model = mod.FirstOrDefault().Model,
                                  SerialNumber = mod.FirstOrDefault().SerialNumber,
                                  RepairDate = mod.FirstOrDefault().RepairDate
                              }).ToList();
            var data = new CountDataDPPMFinance();
            decimal vatTotalSOCost = 0;
            decimal varTotalSN = 0;
            foreach (var item in resultData)
            {
                var totalSoCost = GetTotalSOCost(item.Model, partNo, partDesc, prefixSN, dateFrom, dateEnd, hid, rental, inventory, others);
                data.TotalServiceOrderCost += Math.Round(totalSoCost.TotalServiceOrderCost, 2);
                data.TotalUnitImpacted += totalSoCost.TotalUnitImpacted;
                vatTotalSOCost += Math.Round(totalSoCost.TotalServiceOrderCost, 2);
                varTotalSN += totalSoCost.TotalUnitImpacted;
                data.QuantitySummary += totalSoCost.QuantitySummary;
            }
            data.FinancialSummary = (vatTotalSOCost != 0 && varTotalSN != 0) ? vatTotalSOCost / varTotalSN : 0;
            return data;
        }

        public CountDataDPPMFinance GetTotalSOCost(string getmodel, string[] partNo, string[] partDesc, string[] prefixSn, string dateFrom, string dateEnd, string hid, string rental, string inventory, string others)
        {
            var data = _ctx.PartResponsibleCostImpactAnalysis.Where(item => item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98");
            if(partNo.Count() > 0)
            {
                data = data.Where(item => partNo.Contains(item.PartCausingFailure));
            }
            if(partDesc.Count() > 0)
            {
                data = data.Where(item => partDesc.Contains(item.PartCausingFailureDescription));
            }
            if(prefixSn.Count() > 0)
            {
                data = data.Where(item => prefixSn.Contains(item.PrefixSN));
            }
            if(!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                data = data.Where(item => item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd);
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                data = data.Where(item => item.RepairDate.Value == convertToDateTimeFrom);
            }
            if (hid == "on" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != null);
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId != null || item.RentStatus != null);
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != null || item.Plant != null);
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                data = data.Where(item => item.RentStatus != null);
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.RentStatus != null || item.Plant != null);
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                data = data.Where(item => item.Plant != null);
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                data = data.Where(item => item.HIDTaskId != null || item.RentStatus != null || item.Plant != null);
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                data = data.Where(item => item.HIDTaskId == null || item.RentStatus == null || item.Plant == null);
            }

            var dataFinal = data.Where(w => w.Model == getmodel);
            var dataCount = new CountDataDPPMFinance();
            dataCount.QuantitySummary = dataFinal.Select(s => s.ServiceOrder).Distinct().Count();
            dataCount.TotalUnitImpacted = dataFinal.Select(s => s.SerialNumber).Distinct().Count();
            dataCount.TotalServiceOrderCost = dataFinal.Select(s => s.TotalAmount).Sum();
            decimal TotalSO = dataFinal.Select(s => s.TotalAmount).Sum();
            decimal TotalSN = dataFinal.Select(s => s.SerialNumber).Distinct().Count();
            decimal financialSummary = TotalSO / TotalSN;
            dataCount.FinancialSummary = Math.Round(financialSummary, 2);
            return dataCount;
        }

        public List<TableAffectedUnitDPPMFinancial> TableAffectedUnit(string cost, string freq, string dateFrom, string dateEnd, string hid, string inventory,string rental, string others, string[] partNo, string[] partDesc, string[] model, string[] prefixSN)
        {
            var listdata = new List<TableAffectedUnitDPPMFinancial>();
            
            if (cost != "all")
            {
                var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                               where item.PartCausingFailure == cost && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartCausingFailure,
                                   PartDesc = item.PartCausingFailureDescription,
                                   HidTaskId = item.HIDTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });

                if (hid == "on" && rental == "" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "on" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.RentStatus != null 
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "on" && inventory == "")
                {
                    getData = (from item in getData
                               where item.RentStatus != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "on" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.RentStatus != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "" && inventory == "on")
                {
                    getData = (from item in getData
                               where  item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "on" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (others == "on" && hid == "" && rental == "" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }
                if(!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
                {
                    var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                    var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                    getData = (from item in getData
                               where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }
                if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
                {
                    var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                    getData = (from item in getData
                               where item.RepairDate.Value == convertToDateTimeFrom
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (partNo.Count() > 0)
                {
                    getData = (from item in getData
                               where partNo.Contains(item.PartNumber)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (partDesc.Count() > 0)
                {
                    getData = (from item in getData
                               where partDesc.Contains(item.PartDesc)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (model.Count() > 0)
                {
                    getData = (from item in getData
                               where model.Contains(item.Model)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (prefixSN.Count() > 0)
                {
                    getData = (from item in getData
                               where prefixSN.Contains(item.PrefixSN)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                var getDataGroup = (from item in getData
                                    group item by item.Model into datemodel
                                    select new
                                    {
                                        Model = datemodel.FirstOrDefault().Model,
                                        PartNumber= datemodel.FirstOrDefault().PartNumber
                                    });

                foreach (var item in getDataGroup.Take(10))
                {
                    var getSN = GetSNForAffectedUnit(item.Model, item.PartNumber, dateFrom, dateEnd, hid, inventory, rental, others, partNo, partDesc, model, prefixSN);
                    var data = new TableAffectedUnitDPPMFinancial();
                    data.Model = item.Model;
                    data.SerialNumber = getSN;
                    listdata.Add(data);
                }
            }
            if(freq != "all")
            {
                var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                               where item.PartCausingFailure == freq && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartCausingFailure,
                                   PartDesc = item.PartCausingFailureDescription,
                                   HidTaskId = item.HIDTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });

                if (hid == "on" && rental == "" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "on" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.RentStatus != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "on" && inventory == "")
                {
                    getData = (from item in getData
                               where item.RentStatus != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "on" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.RentStatus != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "" && rental == "" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (hid == "on" && rental == "on" && inventory == "on")
                {
                    getData = (from item in getData
                               where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (others == "on" && hid == "" && rental == "" && inventory == "")
                {
                    getData = (from item in getData
                               where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }
                if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
                {
                    var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                    var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                    getData = (from item in getData
                               where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }
                if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
                {
                    var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                    getData = (from item in getData
                               where item.RepairDate.Value == convertToDateTimeFrom
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (partNo.Count() > 0)
                {
                    getData = (from item in getData
                               where partNo.Contains(item.PartNumber)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (partDesc.Count() > 0)
                {
                    getData = (from item in getData
                               where partDesc.Contains(item.PartDesc)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (model.Count() > 0)
                {
                    getData = (from item in getData
                               where model.Contains(item.Model)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                if (prefixSN.Count() > 0)
                {
                    getData = (from item in getData
                               where prefixSN.Contains(item.PrefixSN)
                               select new
                               {
                                   Model = item.Model,
                                   PartNumber = item.PartNumber,
                                   PartDesc = item.PartDesc,
                                   HidTaskId = item.HidTaskId,
                                   Plant = item.Plant,
                                   RentStatus = item.RentStatus,
                                   PrefixSN = item.PrefixSN,
                                   RepairDate = item.RepairDate
                               });
                }

                var getDataGroup = (from item in getData
                                    group item by item.Model into datemodel
                                    select new
                                    {
                                        Model = datemodel.FirstOrDefault().Model,
                                        PartNumber = datemodel.FirstOrDefault().PartNumber
                                    });

                foreach (var item in getDataGroup.Take(10))
                {
                    var getSN = GetSNForAffectedUnit(item.Model, item.PartNumber, dateFrom, dateEnd, hid, inventory, rental, others, partNo, partDesc, model, prefixSN);
                    var data = new TableAffectedUnitDPPMFinancial();
                    data.Model = item.Model;
                    data.SerialNumber = getSN;
                    listdata.Add(data);
                }
            }
            
            return listdata;
        }

        public List<string> GetSNForAffectedUnit(string model, string partno, string dateFrom, string dateEnd, string hid, string inventory, string rental, string others, string[] partNo, string[] partDesc, string[] getmodel, string[] prefixSN)
        {
            var getData = (from item in _ctx.PartResponsibleCostImpactAnalysis
                           where item.Model == model && item.PartCausingFailure == partno && item.ProductProblem != "" && item.ProductProblem != "49" && item.ProductProblem != "06" && item.ProductProblem != "00" && item.ProductProblem != "X" && item.ProductProblem != "AA" && item.ProductProblem != "28" && item.ProductProblem != "38" && item.ProductProblem != "50" && item.ProductProblem != "74" && item.ProductProblem != "75" && item.ProductProblem != "76" && item.ProductProblem != "08" && item.ProductProblem != "RD" && item.ProductProblem != "20" && item.ProductProblem != "98"
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartCausingFailure,
                               PartDesc = item.PartCausingFailureDescription,
                               HidTaskId = item.HIDTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });

            if (hid == "on" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "on" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.Plant != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "" && rental == "on" && inventory == "")
            {
                getData = (from item in getData
                           where item.RentStatus != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.RentStatus != null || item.Plant != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "" && rental == "" && inventory == "on")
            {
                getData = (from item in getData
                           where item.Plant != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (hid == "on" && rental == "on" && inventory == "on")
            {
                getData = (from item in getData
                           where item.HidTaskId != null || item.RentStatus != null || item.Plant != null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (others == "on" && hid == "" && rental == "" && inventory == "")
            {
                getData = (from item in getData
                           where item.HidTaskId == null || item.RentStatus == null || item.Plant == null
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && !string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                var convertToDateTimeEnd = DateTime.ParseExact(dateEnd, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value >= convertToDateTimeFrom && item.RepairDate.Value <= convertToDateTimeEnd
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }
            if (!string.IsNullOrWhiteSpace(dateFrom) && string.IsNullOrWhiteSpace(dateEnd))
            {
                var convertToDateTimeFrom = DateTime.ParseExact(dateFrom, "dd-MM-yyyy", null);
                getData = (from item in getData
                           where item.RepairDate.Value == convertToDateTimeFrom
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (partNo.Count() > 0)
            {
                getData = (from item in getData
                           where partNo.Contains(item.PartNumber)
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (partDesc.Count() > 0)
            {
                getData = (from item in getData
                           where partDesc.Contains(item.PartDesc)
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (model.Count() > 0)
            {
                getData = (from item in getData
                           where model.Contains(item.Model)
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (prefixSN.Count() > 0)
            {
                getData = (from item in getData
                           where prefixSN.Contains(item.PrefixSN)
                           select new
                           {
                               Model = item.Model,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartDesc,
                               HidTaskId = item.HidTaskId,
                               Plant = item.Plant,
                               RentStatus = item.RentStatus,
                               PrefixSN = item.PrefixSN,
                               RepairDate = item.RepairDate,
                               SerialNumber = item.SerialNumber
                           });
            }

            var getDataGroup = (from item in getData
                                group item by item.SerialNumber into sn
                                select new
                                {
                                    SerialNumber = sn.FirstOrDefault().SerialNumber
                                }).ToList();
            var listData = new List<string>();
            foreach(var item in getDataGroup)
            {
                listData.Add(item.SerialNumber);
            }
            return listData;
        }

        public List<TableRelateDPPMDPPMFInancial> GetDataForTableRelateDPPMFinancial(string cost, string freq, string searchValue)
        {
            var listData = new List<TableRelateDPPMDPPMFInancial>();
            if (cost != "all")
            {
                var getDataForPieStatus = (from item in _ctx.DPPMAffectedPart
                                           where item.PartNumber.Contains(cost)
                                           select new
                                           {
                                               DPPMPartId = item.DPPMAffectedPartId,
                                               DealerDPPM = item.DealerPPM,
                                               GetDataDPPMSummary = _ctx.DPPMSummary.Where(w => w.SRNumber == item.DealerPPM).FirstOrDefault()
                                           });

                var getDataForPieStatusRebuild = (from item in getDataForPieStatus
                                                  select new
                                                  {
                                                      DPPMNo = item.GetDataDPPMSummary.SRNumber,
                                                      Title = item.GetDataDPPMSummary.Title,
                                                      Desc = item.GetDataDPPMSummary.ProblemDescription,
                                                      DealerContact = item.GetDataDPPMSummary.DealerContactName,
                                                      CatReps = item.GetDataDPPMSummary.TechRep,
                                                      Status = item.GetDataDPPMSummary.Status,
                                                      ICA = item.GetDataDPPMSummary.ICA,
                                                      PCA = item.GetDataDPPMSummary.PCA,
                                                      DateCreated = item.GetDataDPPMSummary.CreateOn,
                                                      DateLastUpdate = item.GetDataDPPMSummary.DateLastUpdated
                                                  });
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    getDataForPieStatusRebuild = getDataForPieStatusRebuild.Where(w => w.DPPMNo.Contains(searchValue) || w.Title.Contains(searchValue) || w.Desc.Contains(searchValue) || w.DealerContact.Contains(searchValue) || w.CatReps.Contains(searchValue) || w.ICA.Contains(searchValue) || w.PCA.Contains(searchValue) || w.DateCreated.ToString().Contains(searchValue) || w.DateLastUpdate.ToString().Contains(searchValue) || w.Status.Contains(searchValue));
                }
                var ida = 0;
                foreach (var item in getDataForPieStatusRebuild.ToList())
                {
                    var list = new TableRelateDPPMDPPMFInancial();
                    list.Row = ida++;
                    if (string.IsNullOrWhiteSpace(item.DPPMNo))
                    {
                        list.DPPMNo = "-";
                    }
                    else
                    {
                        list.DPPMNo = item.DPPMNo;
                    }
                    if (string.IsNullOrWhiteSpace(item.Title))
                    {
                        list.Title = "-";
                    }
                    else
                    {
                        list.Title = item.Title;
                    }
                    if (string.IsNullOrWhiteSpace(item.Desc))
                    {
                        list.Desc = "-";
                    }
                    else
                    {
                        list.Desc = item.Desc;
                    }
                    if (string.IsNullOrWhiteSpace(item.Status))
                    {
                        list.Status = "-";
                    }
                    else
                    {
                        list.Status = item.Status;
                    }

                    if (string.IsNullOrWhiteSpace(item.DealerContact))
                    {
                        list.DealerContact = "-";
                    }
                    else
                    {
                        list.DealerContact = item.DealerContact;
                    }
                    if (string.IsNullOrWhiteSpace(item.CatReps))
                    {
                        list.CatReps = "-";
                    }
                    else
                    {
                        list.CatReps = item.CatReps;
                    }

                    if (string.IsNullOrWhiteSpace(item.ICA))
                    {
                        list.ICA = "-";
                    }
                    else
                    {
                        list.ICA = item.ICA;

                    }
                    if (string.IsNullOrWhiteSpace(item.PCA))
                    {
                        list.PCA = "-";
                    }
                    else
                    {
                        list.PCA = item.PCA;

                    }
                    if (item.DateCreated != null)
                    {
                        list.DateCreated = item.DateCreated.ToString();
                    }
                    else
                    {
                        list.DateCreated = "-";
                    }

                    if (item.DateLastUpdate != null)
                    {
                        list.DateLastUpdate = item.DateLastUpdate.ToString();
                    }
                    else
                    {
                        list.DateLastUpdate = "-";
                    }
                    listData.Add(list);
                }
            }

            if(freq != "all")
            {
                var getDataForPieStatus = (from item in _ctx.DPPMAffectedPart
                                           where item.PartNumber.Contains(freq)
                                           select new
                                           {
                                               DPPMPartId = item.DPPMAffectedPartId,
                                               DealerDPPM = item.DealerPPM,
                                               GetDataDPPMSummary = _ctx.DPPMSummary.Where(w => w.SRNumber == item.DealerPPM).FirstOrDefault()
                                           });

                var getDataForPieStatusRebuild = (from item in getDataForPieStatus
                                                  select new
                                                  {
                                                      DPPMNo = item.GetDataDPPMSummary.SRNumber,
                                                      Title = item.GetDataDPPMSummary.Title,
                                                      Desc = item.GetDataDPPMSummary.ProblemDescription,
                                                      DealerContact = item.GetDataDPPMSummary.DealerContactName,
                                                      CatReps = item.GetDataDPPMSummary.TechRep,
                                                      Status = item.GetDataDPPMSummary.Status,
                                                      ICA = item.GetDataDPPMSummary.ICA,
                                                      PCA = item.GetDataDPPMSummary.PCA,
                                                      DateCreated = item.GetDataDPPMSummary.CreateOn,
                                                      DateLastUpdate = item.GetDataDPPMSummary.DateLastUpdated
                                                  });
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    getDataForPieStatusRebuild = getDataForPieStatusRebuild.Where(w => w.DPPMNo.Contains(searchValue) || w.Title.Contains(searchValue) || w.Desc.Contains(searchValue) || w.DealerContact.Contains(searchValue) || w.CatReps.Contains(searchValue) || w.ICA.Contains(searchValue) || w.PCA.Contains(searchValue) || w.DateCreated.ToString().Contains(searchValue) || w.DateLastUpdate.ToString().Contains(searchValue) || w.Status.Contains(searchValue));
                }

                var ida = 0;
                foreach (var item in getDataForPieStatusRebuild.ToList())
                {
                    var list = new TableRelateDPPMDPPMFInancial();
                    list.Row = ida++;
                    if (string.IsNullOrWhiteSpace(item.DPPMNo))
                    {
                        list.DPPMNo = "-";
                    }
                    else
                    {
                        list.DPPMNo = item.DPPMNo;
                    }
                    if (string.IsNullOrWhiteSpace(item.Title))
                    {
                        list.Title = "-";
                    }
                    else
                    {
                        list.Title = item.Title;
                    }
                    if (string.IsNullOrWhiteSpace(item.Desc))
                    {
                        list.Desc = "-";
                    }
                    else
                    {
                        list.Desc = item.Desc;
                    }
                    if (string.IsNullOrWhiteSpace(item.Status))
                    {
                        list.Status = "-";
                    }
                    else
                    {
                        list.Status = item.Status;
                    }

                    if (string.IsNullOrWhiteSpace(item.DealerContact))
                    {
                        list.DealerContact = "-";
                    }
                    else
                    {
                        list.DealerContact = item.DealerContact;
                    }
                    if (string.IsNullOrWhiteSpace(item.CatReps))
                    {
                        list.CatReps = "-";
                    }
                    else
                    {
                        list.CatReps = item.CatReps;
                    }

                    if (string.IsNullOrWhiteSpace(item.ICA))
                    {
                        list.ICA = "-";
                    }
                    else
                    {
                        list.ICA = item.ICA;

                    }
                    if (string.IsNullOrWhiteSpace(item.PCA))
                    {
                        list.PCA = "-";
                    }
                    else
                    {
                        list.PCA = item.PCA;

                    }
                    if (item.DateCreated != null)
                    {
                        list.DateCreated = item.DateCreated.ToString();
                    }
                    else
                    {
                        list.DateCreated = "-";
                    }

                    if (item.DateLastUpdate != null)
                    {
                        list.DateLastUpdate = item.DateLastUpdate.ToString();
                    }
                    else
                    {
                        list.DateLastUpdate = "-";
                    }
                    listData.Add(list);
                }
            }
            
            return listData;
        }

        public List<TableRelatePSLDPPMFinancial> GetDataForTableRelatePSLDPPMFinancial(string cost, string freq, string searchValue)
        {
            var listitem = new List<TableRelatePSLDPPMFinancial>();
            if (cost != "all")
            {
                var getPartNo = _ctx.PSLPart.Where(w => w.PartNumber.Contains(cost)).Select(s => s.PSLNo).ToList();
                var getListFromDate = (from item in _ctx.PSLMaster
                                       where getPartNo.Contains(item.PSLId)
                                       group item by item.PSLId into pslid
                                       select new
                                       {
                                           PSLMasterId = pslid.FirstOrDefault().PSLMasterId,
                                           Area = pslid.FirstOrDefault().Area,
                                           PSLNo = pslid.FirstOrDefault().PSLId,
                                           StoreName = pslid.FirstOrDefault().SalesOffice,
                                           Model = pslid.FirstOrDefault().Model,
                                           SerialNumber = pslid.FirstOrDefault().SerialNumber,
                                           ServiceRequestNo = pslid.FirstOrDefault().ServiceRequestNo,
                                           QuotationNo = pslid.FirstOrDefault().QuotationNo,
                                           SONo = pslid.FirstOrDefault().ServiceOrderNo,
                                           SapPSLStatus = pslid.FirstOrDefault().SapPSLStatus,
                                           TerminationDate = pslid.FirstOrDefault().TerminationDate,
                                           ReleaseDate = pslid.FirstOrDefault().LetterDate,
                                           PSLType = pslid.FirstOrDefault().PSLType,
                                           PriorityLevel = pslid.FirstOrDefault().PriorityLevel,
                                           AgeIndicator = pslid.FirstOrDefault().PslAge,
                                           RentStatus = pslid.FirstOrDefault().RentStatus,
                                           HIDStatus = pslid.FirstOrDefault().HIDStatus,
                                           Plant = pslid.FirstOrDefault().Plant,
                                           DaysToExpired = pslid.FirstOrDefault().DaysToExpired,
                                           WipAge = (pslid.FirstOrDefault().WipAge != 0) ? pslid.FirstOrDefault().WipAge : 0,
                                           Validation = pslid.FirstOrDefault().Validation,
                                           PslAge = pslid.FirstOrDefault().PslAge,
                                           Completion = pslid.FirstOrDefault().Description,
                                           Desc = pslid.FirstOrDefault().Description,
                                       }).ToList();
                int ida = 0;
                foreach (var item in getListFromDate)
                {
                    var list = new TableRelatePSLDPPMFinancial();
                    list.Row = ida++;
                    var getCompleted = CountCompleted(item.PSLNo);
                    var CountUnit = CountUnitQTy(item.PSLNo);
                    var Completed = (Convert.ToDecimal(getCompleted) / Convert.ToDecimal(CountUnit)) * 100;
                    var resultCompleted = Math.Round(Completed, 2);
                    if (!string.IsNullOrWhiteSpace(item.PSLNo))
                    {
                        list.PSLNo = item.PSLNo;
                    }
                    else
                    {
                        list.PSLNo = "-";
                    }
                    list.Completion = resultCompleted;
                    if (!string.IsNullOrWhiteSpace(item.Desc))
                    {
                        list.Desc = item.Desc;
                    }
                    else
                    {
                        list.Desc = "-";
                    }

                    if (!string.IsNullOrWhiteSpace(item.PSLType))
                    {
                        list.PSLType = item.PSLType;
                    }
                    else
                    {
                        list.PSLType = "-";
                    }
                    if (item.ReleaseDate != null)
                    {
                        var date = item.ReleaseDate.Value.Date.ToString();
                        var splitDate = date.Split(' ');
                        list.IssueDateString = splitDate[0];
                    }
                    else
                    {
                        list.IssueDateString = "-";
                    }
                    if (item.TerminationDate != null)
                    {
                        var date = item.TerminationDate.Value.Date.ToString();
                        var splitDate = date.Split(' ');
                        list.TerminationDateString = splitDate[0];
                    }
                    else
                    {
                        list.TerminationDateString = "-";
                    }
                    list.IssueDate = item.ReleaseDate;
                    list.TerminationDate = item.TerminationDate;
                    list.WipAge = item.WipAge;
                    list.DaysToExpired = item.DaysToExpired;
                    list.Validation = item.Validation;
                    list.PslAge = item.PslAge;
                    listitem.Add(list);
                }
            }
            if(freq != "all")
            {
                var getPartNo = _ctx.PSLPart.Where(w => w.PartNumber.Contains(freq)).Select(s => s.PSLNo).ToList();
                var getListFromDate = (from item in _ctx.PSLMaster
                                       where getPartNo.Contains(item.PSLId)
                                       group item by item.PSLId into pslid
                                       select new
                                       {
                                           PSLMasterId = pslid.FirstOrDefault().PSLMasterId,
                                           Area = pslid.FirstOrDefault().Area,
                                           PSLNo = pslid.FirstOrDefault().PSLId,
                                           StoreName = pslid.FirstOrDefault().SalesOffice,
                                           Model = pslid.FirstOrDefault().Model,
                                           SerialNumber = pslid.FirstOrDefault().SerialNumber,
                                           ServiceRequestNo = pslid.FirstOrDefault().ServiceRequestNo,
                                           QuotationNo = pslid.FirstOrDefault().QuotationNo,
                                           SONo = pslid.FirstOrDefault().ServiceOrderNo,
                                           SapPSLStatus = pslid.FirstOrDefault().SapPSLStatus,
                                           TerminationDate = pslid.FirstOrDefault().TerminationDate,
                                           ReleaseDate = pslid.FirstOrDefault().LetterDate,
                                           PSLType = pslid.FirstOrDefault().PSLType,
                                           PriorityLevel = pslid.FirstOrDefault().PriorityLevel,
                                           AgeIndicator = pslid.FirstOrDefault().PslAge,
                                           RentStatus = pslid.FirstOrDefault().RentStatus,
                                           HIDStatus = pslid.FirstOrDefault().HIDStatus,
                                           Plant = pslid.FirstOrDefault().Plant,
                                           DaysToExpired = pslid.FirstOrDefault().DaysToExpired,
                                           WipAge = (pslid.FirstOrDefault().WipAge != 0) ? pslid.FirstOrDefault().WipAge : 0,
                                           Validation = pslid.FirstOrDefault().Validation,
                                           PslAge = pslid.FirstOrDefault().PslAge,
                                           Completion = pslid.FirstOrDefault().Description,
                                           Desc = pslid.FirstOrDefault().Description,
                                       }).ToList();
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    getListFromDate = getListFromDate.Where(w => w.PSLNo.ToLower().Contains(searchValue.ToLower()) || w.Desc.ToLower().Contains(searchValue.ToLower()) || w.PSLType.ToLower().Contains(searchValue.ToLower()) || w.Completion.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                int ida = 0;
                foreach (var item in getListFromDate)
                {
                    var list = new TableRelatePSLDPPMFinancial();
                    list.Row = ida++;
                    var getCompleted = CountCompleted(item.PSLNo);
                    var CountUnit = CountUnitQTy(item.PSLNo);
                    var Completed = Convert.ToDecimal(getCompleted) / Convert.ToDecimal(CountUnit);
                    var resultCompleted = Math.Round(Completed, 2);
                    if (!string.IsNullOrWhiteSpace(item.PSLNo))
                    {
                        list.PSLNo = item.PSLNo;
                    }
                    else
                    {
                        list.PSLNo = "-";
                    }
                    list.Completion = resultCompleted;
                    if (!string.IsNullOrWhiteSpace(item.Desc))
                    {
                        list.Desc = item.Desc;
                    }
                    else
                    {
                        list.Desc = "-";
                    }

                    if (!string.IsNullOrWhiteSpace(item.PSLType))
                    {
                        list.PSLType = item.PSLType;
                    }
                    else
                    {
                        list.PSLType = "-";
                    }
                    if (item.ReleaseDate != null)
                    {
                        var date = item.ReleaseDate.Value.Date.ToString();
                        var splitDate = date.Split(' ');
                        list.IssueDateString = splitDate[0];
                    }
                    else
                    {
                        list.IssueDateString = "-";
                    }
                    if (item.TerminationDate != null)
                    {
                        var date = item.TerminationDate.Value.Date.ToString();
                        var splitDate = date.Split(' ');
                        list.TerminationDateString = splitDate[0];
                    }
                    else
                    {
                        list.TerminationDateString = "-";
                    }
                    list.IssueDate = item.ReleaseDate;
                    list.TerminationDate = item.TerminationDate;
                    list.WipAge = item.WipAge;
                    list.DaysToExpired = item.DaysToExpired;
                    list.Validation = item.Validation;
                    list.PslAge = item.PslAge;
                    listitem.Add(list);
                }
            }
            return listitem;
        }

        public int CountCompleted (string pslid)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslid) && w.Validation.Contains("Completed"));
            var result = getData.Count();
            return result;
        }

        public int CountUnitQTy(string pslid)
        {
            var getData = _ctx.PSLMaster.Where(w => w.PSLId.Contains(pslid)).Select(s => s.SerialNumber).Distinct().Count();
            return getData;
        }

        public List<TableRelateTRDPPMFinancial> GetDataForTableRelateTRDPPMFinancial(string cost, string freq)
        {
            var listData = new List<TableRelateTRDPPMFinancial>();
            if (cost != "all")
            {
                var getData = (from item in _ctx.Ticket
                               where item.PartCausingFailure.Contains(cost)
                               select new
                               {
                                   TicketId = item.TicketId,
                                   TicketNo = item.TicketNo,
                                   Industry = item.Family,
                                   Family = item.Family,
                                   Model = item.Model,
                                   Category = _ctx.TicketCategory.Where(w => w.TicketCategoryId == item.TicketCategoryId).Select(s => s.Name).FirstOrDefault(),
                                   Title = item.Title,
                                   DataResolution = _ctx.TicketResolution.Where(w => w.TicketId == item.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                   Desc = item.Description,
                                   DateCreated = item.CreatedAt,
                                   StatusTicket = item.Status
                               }).Take(10);
                var varGetData = (from item in getData
                                  select new
                                  {
                                      TicketId = item.TicketId,
                                      TicketNo = item.TicketNo,
                                      Industry = item.Family,
                                      Family = item.Family,
                                      Model = item.Model,
                                      Category = item.Category,
                                      Title = item.Title,
                                      Desc = item.Desc,
                                      DateCreated = item.DateCreated,
                                      Resolution = item.DataResolution.Description,
                                      DateClosed = item.DataResolution.CreatedAt,
                                      StatusTicket = item.StatusTicket
                                  }).ToList();
                var ida = 0;
                foreach (var item in varGetData)
                {
                    var data = new TableRelateTRDPPMFinancial();
                    data.Row = ida++;
                    data.TicketNo = item.TicketNo;
                    data.Industry = item.Industry;
                    data.Familiy = item.Family;
                    data.Model = item.Model;
                    data.Category = item.Category;
                    data.Title = item.Title;
                    data.Desc = item.Desc;
                    data.DateCreated = item.DateCreated.ToString();
                    data.DateClosed = item.DateClosed.ToString();
                    data.Resolution = item.Resolution;
                    data.StatusTR = item.StatusTicket;
                    listData.Add(data);

                }
            }
            if(freq != "all")
            {
                var getData = (from item in _ctx.Ticket
                               where item.PartCausingFailure.Contains(freq)
                               select new
                               {
                                   TicketId = item.TicketId,
                                   TicketNo = item.TicketNo,
                                   Industry = item.Family,
                                   Family = item.Family,
                                   Model = item.Model,
                                   Category = _ctx.TicketCategory.Where(w => w.TicketCategoryId == item.TicketCategoryId).Select(s => s.Name).FirstOrDefault(),
                                   Title = item.Title,
                                   DataResolution = _ctx.TicketResolution.Where(w => w.TicketId == item.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                   Desc = item.Description,
                                   DateCreated = item.CreatedAt,
                                   StatusTicket = item.Status
                               }).Take(10);
                var varGetData = (from item in getData
                                  select new
                                  {
                                      TicketId = item.TicketId,
                                      TicketNo = item.TicketNo,
                                      Industry = item.Family,
                                      Family = item.Family,
                                      Model = item.Model,
                                      Category = item.Category,
                                      Title = item.Title,
                                      Desc = item.Desc,
                                      DateCreated = item.DateCreated,
                                      Resolution = item.DataResolution.Description,
                                      DateClosed = item.DataResolution.CreatedAt,
                                      StatusTicket = item.StatusTicket
                                  }).ToList();
                var ida = 0;
                foreach (var item in varGetData)
                {
                    var data = new TableRelateTRDPPMFinancial();
                    data.Row = ida++;
                    data.TicketNo = item.TicketNo;
                    data.Industry = item.Industry;
                    data.Familiy = item.Family;
                    data.Model = item.Model;
                    data.Category = item.Category;
                    data.Title = item.Title;
                    data.Desc = item.Desc;
                    data.DateCreated = item.DateCreated.ToString();
                    data.DateClosed = item.DateClosed.ToString();
                    data.Resolution = item.Resolution;
                    data.StatusTR = item.StatusTicket;
                    listData.Add(data);
                }
            }
            return listData;
        }

  
        public Tuple<List<TablePPMPotentialByCost>, int> GetDataForTablePPMByCostOverview(string[] serialnumber, string orderByDir = null, string orderByColumn = null, int download = 0, string[] columns= null)
        {
            orderByColumn = orderByColumn == null ? "": orderByColumn.ToLower();
            var listItem = new List<TablePPMPotentialByCost>();
            if (serialnumber.Length > 0)
            {
                List<PartResponsibleCostImpactAnalysis> ListSN = new List<PartResponsibleCostImpactAnalysis>();
                var listSN = _ctx.PartResponsibleCostImpactAnalysis.Where(item => (item.PartCausingFailure != null && item.PartCausingFailure != "" && item.PartCausingFailure != "-") && (!data.Contains(item.ProductProblem) && item.ProductProblem != String.Empty && item.ProductProblem != null));
                foreach (var item in listSN)
                {
                    if (serialnumber.Contains(item.SerialNumber))
                    {
                        ListSN.Add(item);
                    }
                }
             
               var getListData = ListSN.Select(w => new { w.PartCausingFailure, w.TotalAmount, w.TotalClaim, w.TotalSettled, w.ServiceOrder}).Distinct().ToList();
               var getListDataGroup = getListData.GroupBy(gb => new { gb.PartCausingFailure})
                    .Select(g => new { PartCausingFailure = g.Key.PartCausingFailure,
                        TotalAmount = g.Sum(s => s.TotalAmount),
                        TotalClaim = g.Sum(s => s.TotalClaim),
                        TotalSettled = g.Sum(s => s.TotalSettled)
                    })
                    .OrderByDescending(ob => ob.TotalAmount).Take(5).ToList();

                var count = getListData.Count();
                foreach (var item in getListDataGroup)
                {      
                        PartResponsibleCostImpactAnalysis prcia = getDetail(item.PartCausingFailure, serialnumber);
                        var list = new TablePPMPotentialByCost();
                        var getTotalSoCost = item.TotalAmount;/*getListData.Where(w => w.PartCausingFailure == item.PartCausingFailure).GroupBy(gb => gb.ServiceOrder).Select(s => s.Sum(sum => sum.TotalAmount));*/
                        var getTotalClaim = item.TotalClaim;
                        var getTotalSettled = item.TotalSettled;
                        list.Row = item.PartCausingFailure;
                        list.ProductProblemDescription = String.IsNullOrWhiteSpace(prcia.ProductProblemDescription) ? "-" : prcia.ProductProblemDescription;
                        list.PartNo = String.IsNullOrWhiteSpace(item.PartCausingFailure) ? "-" : item.PartCausingFailure;
                        list.PartDescription = String.IsNullOrWhiteSpace(prcia.PartCausingFailureDescription) ? "-" : prcia.PartCausingFailureDescription;

                        list.Currency = prcia.Currency;
                        list.TotalSoCost = Convert.ToDecimal(getTotalSoCost);
                        list.SoClaim = Convert.ToDecimal(getTotalClaim);
                        list.SOSettled = Convert.ToDecimal(getTotalSettled);
                        list.GroupNo = String.IsNullOrWhiteSpace(prcia.GroupNumber) ? "-" : prcia.GroupNumber;
                        list.GroupDesc = String.IsNullOrWhiteSpace(prcia.GroupDescription) ? "-" : prcia.GroupDescription;
                        listItem.Add(list);
                }
                #region OrderByCondition
                if (download != 1) {
                    if (!string.IsNullOrWhiteSpace(orderByColumn))
                    {
                        if (orderByColumn == "part_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.PartDescription).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.PartDescription).ToList();
                            }
                        }
                        else if (orderByColumn == "group_no")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.GroupNo).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.GroupNo).ToList();
                            }
                        }
                        else if (orderByColumn == "group_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.GroupDesc).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.GroupNo).ToList();
                            }
                        }
                        else if (orderByColumn == "prob_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.ProductProblemDescription).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.ProductProblemDescription).ToList();
                            }
                        }
                        else if (orderByColumn == "so_cost")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.TotalSoCost).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.TotalSoCost).ToList();
                            }
                        }
                        else if (orderByColumn == "so_claim")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.SoClaim).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.SoClaim).ToList();
                            }
                        }

                        else if (orderByColumn == "so_settled")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.SOSettled).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.SOSettled).ToList();
                            }
                        }
                        else if (orderByColumn == "currency")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.Currency).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.Currency).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.PartNo).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.PartNo).ToList();
                            }
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            listItem = listItem.OrderByDescending(ob => ob.TotalSoCost).ToList();
                        }
                        else
                        {
                            listItem = listItem.OrderBy(ob => ob.TotalSoCost).ToList();
                        }
                    }
                }
                else
                {
                    listItem = listItem.OrderByDescending(ob => ob.TotalSoCost).ToList();
                }
                #endregion
                return Tuple.Create(listItem, listItem.Count());
            }
            else
            {
                var getData = _ctx.PartResponsibleCostImpactAnalysis.Where(item => serialnumber.Contains(item.SerialNumber)).Take(0);
               
                var countDataGroup =0;
               
                foreach (var item in getData)
                {
                    var list = new TablePPMPotentialByCost();
                    var getTotalSoCost = item.TotalSO;
                    var getTotalClaim = item.TotalClaim;
                    var getTotalSettled = item.TotalSettled;
                    list.Row = item.PartCausingFailure;
                    list.ProductProblemDescription = !string.IsNullOrWhiteSpace(item.ProductProblemDescription) ? item.ProductProblemDescription : "-";
                    list.PartNo = !string.IsNullOrWhiteSpace(item.PartCausingFailure) ? item.PartCausingFailure : "-";
                    list.PartDescription = !string.IsNullOrWhiteSpace(item.PartCausingFailureDescription) ? item.PartCausingFailureDescription : "-";


                    list.Currency = item.Currency;
                    list.TotalSoCost = getTotalSoCost;
                    list.SoClaim = getTotalClaim;
                    list.SOSettled = getTotalSettled;
                    list.GroupNo = !string.IsNullOrWhiteSpace(item.GroupNumber) ? item.GroupNumber : "-";
                    list.GroupDesc = !string.IsNullOrWhiteSpace(item.GroupDescription) ? item.GroupDescription : "-";
                    listItem.Add(list);
                }
                return Tuple.Create(listItem, countDataGroup);
            }
        }

        public PartResponsibleCostImpactAnalysis getDetail(string partNo, string [] SerialNumber)
        {
            PartResponsibleCostImpactAnalysis dataprcia = new PartResponsibleCostImpactAnalysis();
            var result = _ctx.PartResponsibleCostImpactAnalysis.Where(item => (item.PartCausingFailure == partNo) && (!data.Contains(item.ProductProblem) && item.ProductProblem != String.Empty && item.ProductProblem != null)).OrderByDescending(item => item.PartResponsibleId);
            foreach (var item in result)
            {
                if (SerialNumber.Contains(item.SerialNumber))
                {
                    dataprcia.PartCausingFailure = item.PartCausingFailure;
                    dataprcia.PartCausingFailureDescription = item.PartCausingFailureDescription;
                    dataprcia.PartResponsibleId = item.PartResponsibleId;
                    dataprcia.GroupDescription = item.GroupDescription;
                    dataprcia.GroupNumber = item.GroupNumber;
                    dataprcia.ProductProblemDescription = item.ProductProblemDescription;
                }
            }
            return dataprcia;
        }
        public List<TablePPMPotentialByFrequency> GetDataForTablePPMByFrequencyOverview(string [] serialnumber, string orderByDir = null, string orderByColumn = null, int download = 0, string[] columns = null)
        {
            orderByColumn = orderByColumn == null ? "count_of_repair" : orderByColumn.ToLower();
            var listItem = new List<TablePPMPotentialByFrequency>();            
            if (serialnumber.Length > 0)
            {
                List<PartResponsibleCostImpactAnalysis> ListSN = new List<PartResponsibleCostImpactAnalysis>();
                var listSN = _ctx.PartResponsibleCostImpactAnalysis.Where(item => (item.PartCausingFailure != null && item.PartCausingFailure != "" && item.PartCausingFailure != "-") && (!data.Contains(item.ProductProblem) && item.ProductProblem != String.Empty && item.ProductProblem != null) );
                foreach (var item in listSN)
                {
                    if (serialnumber.Contains(item.SerialNumber))
                    {
                        ListSN.Add(item);
                    }
                }
                //var getListData = ListSN.Select(w => new { w.PartCausingFailure, w.TotalAmount, w.TotalClaim, w.TotalSettled }).ToList();
                //var getListDataCount = getListData.GroupBy(gb => gb.PartCausingFailure)
                //    .Select(g => new
                //    {
                //        PartCausingFailure = g.Key,
                //        CountofRepair = g.Where(item => item.PartCausingFailure == g.Key).Select(select => select.TotalAmount).Distinct().Count()
                //    })
                //    .OrderByDescending(ob => ob.CountofRepair)
                //    .Take(5)
                //    .ToList();
                var getListData = ListSN.Select(w => new { w.PartCausingFailure, w.TotalAmount, w.TotalClaim, w.TotalSettled, w.ServiceOrder}).ToList();
                var getListDataCount = getListData.GroupBy(gb => gb.PartCausingFailure)
                    .Select(g => new {
                        PartCausingFailure = g.Key,
                        CountofRepair = g.Count()
                    })
                    .OrderByDescending(ob => ob.CountofRepair)
                    .Take(5)
                    .ToList();

                foreach (var item in getListDataCount)
                {
                    PartResponsibleCostImpactAnalysis prcia = getDetail(item.PartCausingFailure, serialnumber);
                        var listGroupNo = new List<string>();
                        var list = new TablePPMPotentialByFrequency();
                       // list.CountOfRepair = item.CountofRepair; 
                        list.CountOfRepair = getListData.Where(w=> w.PartCausingFailure == item.PartCausingFailure).Select(s=>s.ServiceOrder).Distinct().Count();
                        list.Row = item.PartCausingFailure;
                        list.ProductProblemDescription = String.IsNullOrWhiteSpace(prcia.ProductProblemDescription) ? "-" : prcia.ProductProblemDescription;
                        list.PartNo = String.IsNullOrWhiteSpace(item.PartCausingFailure) ? "-" : prcia.PartCausingFailure;
                        list.PartDescription = String.IsNullOrWhiteSpace(prcia.PartCausingFailureDescription) ? "-" : prcia.PartCausingFailureDescription;
                        listGroupNo.Add(String.IsNullOrWhiteSpace(prcia.GroupNumber) ? "-" : prcia.GroupNumber);
                        list.GroupNo = listGroupNo;
                        list.GroupDesc = String.IsNullOrWhiteSpace(prcia.GroupDescription) ? "-" : prcia.GroupDescription;
                        listItem.Add(list);
                }
                #region OrderByCondition
                if (download != 1)
                {
                    if (!string.IsNullOrWhiteSpace(orderByColumn))
                    {
                        if (orderByColumn == "part_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.PartDescription).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.PartDescription).ToList();
                            }
                        }
                        else if (orderByColumn == "group_no")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.GroupNo).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.GroupNo).ToList();
                            }
                        }
                        else if (orderByColumn == "group_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.GroupDesc).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.GroupNo).ToList();
                            }
                        }
                        else if (orderByColumn == "prob_desc")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.ProductProblemDescription).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.ProductProblemDescription).ToList();
                            }
                        }
                        else if (orderByColumn == "so_cost")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.TotalSoCost).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.TotalSoCost).ToList();
                            }
                        }
                        else if (orderByColumn == "count_of_repair")
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.CountOfRepair).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.CountOfRepair).ToList();
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                            {
                                listItem = listItem.OrderByDescending(ob => ob.PartNo).ToList();
                            }
                            else
                            {
                                listItem = listItem.OrderBy(ob => ob.PartNo).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(orderByDir) && orderByDir == "desc")
                        {
                            listItem = listItem.OrderByDescending(ob => ob.CountOfRepair).ToList();
                        }
                        else
                        {
                            listItem = listItem.OrderBy(ob => ob.CountOfRepair).ToList();
                        }
                    }
                }
                else
                {
                    listItem = listItem.OrderByDescending(ob => ob.CountOfRepair).ToList();
                }
                #endregion
                return listItem;
            }
            else
            {
                var getData = _ctx.PartResponsibleCostImpactAnalysis.Where(item => true).Take(0);
                foreach (var item in getData)
                {
                    var listGroupNo = new List<string>();
                    var list = new TablePPMPotentialByFrequency();
                    var countSO = CountTotalServiceOrder(item.PartCausingFailure,serialnumber);
                    list.Row = item.PartCausingFailure;
                    list.ProductProblemDescription = string.IsNullOrWhiteSpace(item.ProductProblemDescription) ? "-" : item.ProductProblemDescription;
                    list.PartNo = string.IsNullOrWhiteSpace(item.PartCausingFailure) ? "-" : item.PartCausingFailure;
                    list.PartDescription = string.IsNullOrWhiteSpace(item.PartCausingFailureDescription) ? "-" : item.PartCausingFailureDescription;
                    list.CountOfRepair = countSO;
                    listGroupNo.Add(item.GroupNumber);
                    list.GroupNo = listGroupNo;
                    list.GroupDesc = string.IsNullOrWhiteSpace(item.GroupDescription) ? "-" : item.GroupDescription;
                    list.Currency = item.Currency;
                    list.TotalSoCost = item.TotalSO;
                    listItem.Add(list);
                }
                listItem = listItem.ToList();
                return listItem;
            }
        }

        public int CountTotalServiceOrder(string partNo, String[] SerialNumber)
        {
            return _ctx.PartResponsibleCostImpactAnalysis.Where(item => (item.PartCausingFailure == partNo) && (!data.Contains(item.ProductProblem) && item.ProductProblem != String.Empty && item.ProductProblem != null) && SerialNumber.Contains(item.SerialNumber))
              .Select(s => s.ServiceOrder).Count();
        }

        public void CheckIfDecimalTypeisNull()
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("update PartResponsibleCostImpactAnalysis set ActualCostLabor = 0, ActualCostMiscellaneous = 0, ActualCostPart =0, TotalAmount =0 where ActualCostLabor is null and ActualCostMiscellaneous is null and ActualCostPart is null and TotalAmount is null");
            }
        }
    }
 
}
