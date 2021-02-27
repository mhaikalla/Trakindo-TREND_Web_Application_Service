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
    public class DPPMBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public List<string> GetListPartNumber()
        {
            var result = _ctx.DPPM.Select(m => m.PartNumber).Distinct().ToList();
            return result;
        }

        public List<string> GetListModel()
        {
            var result = _ctx.DPPM.Select(m => m.PrimeProductModel).Distinct().ToList();
            return result;
        }
        
        public List<string> GetListPartDescription()
        {
            var result = _ctx.DPPM.Select(m => m.PartNumber).Distinct().ToList();
            return result;
        }

        public List<DataChartComboDPPM> GetDataForChartComboFinancial(string[] partNumber, string[] partDesc, string[] Model, string[] prefixSN)
        {
            var getData = (from item in _ctx.DPPM
                           where item.PrimeProductModel != null && item.PrimeProductModel != ""
                           select new
                           {
                               TotalSo = item.ServiceOrderCount,
                               TotalSoCost = item.TotalAmount,
                               Model = item.PrimeProductModel,
                               SRNumber = item.SRNumber,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartNumber,
                               PrefixSN = item.PartNumber
                           }).OrderBy(odb => odb.Model);
            if (partNumber.Count() > 0)
            {
                getData = (from item in getData
                           where partNumber.Contains(item.PartNumber)
                           select new
                           {
                               TotalSo = item.TotalSo,
                               TotalSoCost = item.TotalSoCost,
                               Model = item.Model,
                               SRNumber = item.SRNumber,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartNumber,
                               PrefixSN = item.PartNumber
                           }).OrderBy(odb => odb.Model);
            }

            if (partDesc.Count() > 0)
            {
                getData = (from item in getData
                           where partNumber.Contains(item.PartNumber)
                           select new
                           {
                               TotalSo = item.TotalSo,
                               TotalSoCost = item.TotalSoCost,
                               Model = item.Model,
                               SRNumber = item.SRNumber,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartNumber,
                               PrefixSN = item.PartNumber
                           }).OrderBy(odb => odb.Model);
            }

            if (Model.Count() > 0)
            {
                getData = (from item in getData
                           where Model.Contains(item.Model)
                           select new
                           {
                               TotalSo = item.TotalSo,
                               TotalSoCost = item.TotalSoCost,
                               Model = item.Model,
                               SRNumber = item.SRNumber,
                               PartNumber = item.PartNumber,
                               PartDesc = item.PartNumber,
                               PrefixSN = item.PartNumber
                           }).OrderBy(odb => odb.Model);
            }
            var listItem = new List<DataChartComboDPPM>();
            var model = "";
            foreach (var item in getData.ToList())
            {
                var listLabel = new List<string>();
                var list = new DataChartComboDPPM();
                if (model != item.Model)
                {
                    model = item.Model;
                    listLabel.Add(item.SRNumber);
                    listLabel.Add(item.Model);
                    listLabel.Add("new");
                    list.Model = listLabel;
                    list.DataTotalCost = item.TotalSoCost;
                    list.DataTotalSO = item.TotalSo;
                }
                else
                {
                    model = item.Model;
                    listLabel.Add(item.SRNumber);
                    listLabel.Add(item.Model);
                    list.Model = listLabel;
                    list.DataTotalCost = item.TotalSoCost;
                    list.DataTotalSO = item.TotalSo;
                }
                listItem.Add(list);
            }
            return listItem;
        }

        public List<TableDPPMFinance> GetDataTableDPPMFinance(string dateFrom, string dateEnd,string searchValue)
        {
            var listItem = new List<TableDPPMFinance>();
            var getData = (from item in _ctx.DPPM
                           select new
                           {
                               DPPMId = item.DPPMId, 
                               SRNumber = item.SRNumber,
                               Model = item.PrimeProductModel,
                               ProductProblemDescription = item.ProblemDescription,
                               GetData = _ctx.RCIA.Where(w => w.Part_Causing_Failure.Contains(item.PartNumber)).FirstOrDefault(),
                               PartNo = item.PartNumber
                           });
            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                getData = getData.Where(w => w.GetData.Comments.Contains(searchValue));
            }
            var idRow = 0;
            foreach(var item in getData.ToList())
            {
                var list = new TableDPPMFinance();
                list.Row = idRow++;
                list.SerialNumber = item.SRNumber;
                list.Model = item.Model;
                list.ProductProblemDescription = item.ProductProblemDescription;
                list.PartNo = item.PartNo;
                if (item.GetData != null) {
                    if (item.GetData.Comments != null && item.GetData.Comments != "")
                    {
                        list.Comment = item.GetData.Comments;
                    }
                    else
                    {
                        list.Comment = "-";
                    }

                    if (item.GetData.Srv_Order != null && item.GetData.Srv_Order != "")
                    {
                        list.ServiceOrder = item.GetData.Srv_Order;
                    }
                    else
                    {
                        list.ServiceOrder = "-";
                    }

                    if (item.GetData.SMU != 0)
                    {
                        list.ServiceMeterMeasurement = item.GetData.SMU;
                    }
                    else
                    {
                        list.ServiceMeterMeasurement = 0;
                    }

                    if (item.GetData.SMU_Unit != null && item.GetData.SMU_Unit != "")
                    {
                        list.UnitMes = item.GetData.SMU_Unit;
                    }
                    else
                    {
                        list.UnitMes = "-";
                    }

                    if (item.GetData.Sales_Office_Desc != null && item.GetData.Sales_Office_Desc != "")
                    {
                        list.SalesOffice = item.GetData.Sales_Office_Desc;
                    }
                    else
                    {
                        list.SalesOffice = "-";
                    }

                    if (item.GetData.Repair_Date != null )
                    {
                        list.RepairDate = item.GetData.Repair_Date.ToString();
                    }
                    else
                    {
                        list.RepairDate = "-";
                    }
                    if (item.GetData.Part_Causing_Failure_Desc != null && item.GetData.Part_Causing_Failure_Desc != "")
                    {
                        list.PartDescription = item.GetData.Part_Causing_Failure_Desc;
                    }
                    else
                    {
                        list.PartDescription = "-";
                    }
                    if(item.GetData.Tot_Amount != 0)
                    {
                        list.TotalSoCost = item.GetData.Tot_Amount;
                    }
                    else
                    {
                        list.TotalSoCost = 0;
                    }
                    if(item.GetData.Currency != null)
                    {
                        list.Currency = item.GetData.Currency;
                    }
                    else
                    {
                        list.Currency = "-";
                    }
                }
                else
                {
                    list.Comment = "-";
                    list.ServiceOrder = "-";
                    list.ServiceMeterMeasurement = 0;
                    list.UnitMes = "-";
                    list.SalesOffice = "-";
                    list.RepairDate = "-";
                    list.PartDescription = "-";
                    list.Currency = "-";
                    list.TotalSoCost = 0;

                } 
                
                
                listItem.Add(list);
            }
            return listItem;
        }

        public ChartPPMStatus CountStatus(string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            decimal convToDecimalStatusOpen = 0;
            decimal convToDecimalStatusClose = 0;
            decimal convToDecimalStatusIssuePending = 0;
            decimal convToDecimalStatusUnsubmitted = 0;
            decimal convToDecimalStatuAll = 0;
            var vargetCountStatusAll = (from item in _ctx.DPPMSummary
                                     where item.Status != null && item.Status != "" && item.ParentDealerCode.Contains("J210")
                                     select new
                                     {
                                         Id = item.DPPMSummaryId,
                                         status = item.Status,
                                         TechReps = item.TechRep,
                                         DealerContact = item.DealerContactName,
                                         FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault()
                                     });
            var getCountStatusAll = (from item in vargetCountStatusAll
                                      select new
                                      {
                                          Id = item.Id,
                                          status = item.status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact,
                                          FromUnit = item.FromUnit
                                      });

            var vargetCountStatusOpen = (from item in _ctx.DPPMSummary
                                      where item.Status.Contains("Dealer Open") && item.ParentDealerCode.Contains("J210")
                                      select new
                                      {
                                          status = item.Status,
                                          TechReps = item.TechRep,
                                          DealerContact = item.DealerContactName,
                                          FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault()
                                      });

            var getCountStatusOpen = (from item in vargetCountStatusOpen
                                         select new
                                         {
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
            var vargetCountStatusClose = (from item in _ctx.DPPMSummary
                                       where item.Status.Contains("Dealer Closed") && item.ParentDealerCode.Contains("J210")
                                       select new
                                       {
                                           status = item.Status,
                                           TechReps = item.TechRep,
                                           DealerContact = item.DealerContactName,
                                           FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault()
                                       });
            var getCountStatusClose = (from item in vargetCountStatusClose
                                      select new
                                      {
                                          status = item.status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact,
                                          FromUnit = item.FromUnit
                                      });

            var vargetCountStatusIssuPending = (from item in _ctx.DPPMSummary
                                             where item.Status.Contains("Issue Pending") && item.ParentDealerCode.Contains("J210")
                                             select new
                                             {
                                                 status = item.Status,
                                                 TechReps = item.TechRep,
                                                 DealerContact = item.DealerContactName,
                                                 FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault()
                                             });
            var getCountStatusIssuPending = (from item in vargetCountStatusIssuPending
                                       select new
                                       {
                                           status = item.status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact,
                                           FromUnit = item.FromUnit
                                       });

            var vargetCountStatusUnsubmitted = (from item in _ctx.DPPMSummary
                                             where item.Status.Contains("Unsubmitted") && item.ParentDealerCode.Contains("J210")
                                             select new
                                             {
                                                 status = item.Status,
                                                 TechReps = item.TechRep,
                                                 DealerContact = item.DealerContactName,
                                                 FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault()
                                             });
            var getCountStatusUnsubmitted = (from item in vargetCountStatusUnsubmitted
                                             select new
                                             {
                                                 status = item.status,
                                                 TechReps = item.TechReps,
                                                 DealerContact = item.DealerContact,
                                                 FromUnit = item.FromUnit
                                             });

            if (!string.IsNullOrWhiteSpace(tech_reps))
            {
                //var split = new string[] { };
                //split = tech_reps.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (tech_reps == "Blank")
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.TechReps.Contains("")
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.TechReps.Contains("")
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.TechReps.Contains("")
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.TechReps.Contains("")
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.TechReps.Contains("")
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
                else if(tech_reps == "Others")
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
                else
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.TechReps == tech_reps
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.TechReps == tech_reps
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.TechReps == tech_reps
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.TechReps == tech_reps
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.TechReps == tech_reps
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
                
            }
            if (!string.IsNullOrWhiteSpace(register))
            {
                getCountStatusAll = (from item in getCountStatusAll
                                     where item.FromUnit.PrimeProductModel == register
                                     select new
                                     {
                                         Id = item.Id,
                                         status = item.status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact,
                                         FromUnit = item.FromUnit
                                     });
                getCountStatusOpen = (from item in getCountStatusOpen
                                      where item.FromUnit.PrimeProductModel == register
                                      select new
                                      {
                                          status = item.status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact,
                                          FromUnit = item.FromUnit
                                      });
                getCountStatusClose = (from item in getCountStatusClose
                                       where item.FromUnit.PrimeProductModel == register
                                       select new
                                       {
                                           status = item.status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact,
                                           FromUnit = item.FromUnit
                                       });
                getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                             where item.FromUnit.PrimeProductModel == register
                                             select new
                                             {
                                                 status = item.status,
                                                 TechReps = item.TechReps,
                                                 DealerContact = item.DealerContact,
                                                 FromUnit = item.FromUnit
                                             });
                getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                             where item.FromUnit.PrimeProductModel == register
                                             select new
                                             {
                                                 status = item.status,
                                                 TechReps = item.TechReps,
                                                 DealerContact = item.DealerContact,
                                                 FromUnit = item.FromUnit
                                             });
            }
            if (!string.IsNullOrWhiteSpace(industry))
            {
                //var split = new string[] { };
                //split = industry.Split('(', ' ');
                //var result = "";
                //if(split.Count() == 10)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString() + " " + split[3].ToString();
                //}
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if(split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (industry == "Blank")
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.FromUnit.PrimeProductFamily.Contains("")
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.FromUnit.PrimeProductFamily.Contains("")
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.FromUnit.PrimeProductFamily.Contains("")
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.FromUnit.PrimeProductFamily.Contains("")
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.FromUnit.PrimeProductFamily.Contains("")
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
                else if(industry == "Others")
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
                else
                {
                    getCountStatusAll = (from item in getCountStatusAll
                                         where item.FromUnit.PrimeProductFamily.Contains(industry)
                                         select new
                                         {
                                             Id = item.Id,
                                             status = item.status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact,
                                             FromUnit = item.FromUnit
                                         });
                    getCountStatusOpen = (from item in getCountStatusOpen
                                          where item.FromUnit.PrimeProductFamily.Contains(industry)
                                          select new
                                          {
                                              status = item.status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact,
                                              FromUnit = item.FromUnit
                                          });
                    getCountStatusClose = (from item in getCountStatusClose
                                           where item.FromUnit.PrimeProductFamily.Contains(industry)
                                           select new
                                           {
                                               status = item.status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact,
                                               FromUnit = item.FromUnit
                                           });
                    getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                                 where item.FromUnit.PrimeProductFamily.Contains(industry)
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                    getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                                 where item.FromUnit.PrimeProductFamily.Contains(industry)
                                                 select new
                                                 {
                                                     status = item.status,
                                                     TechReps = item.TechReps,
                                                     DealerContact = item.DealerContact,
                                                     FromUnit = item.FromUnit
                                                 });
                }
            }
            if (!string.IsNullOrWhiteSpace(dealer_contact))
            {
                getCountStatusAll = (from item in getCountStatusAll
                                     where item.DealerContact.Contains(dealer_contact)
                                     select new
                                     {
                                         Id = item.Id,
                                         status = item.status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact,
                                         FromUnit = item.FromUnit
                                     });
                getCountStatusOpen = (from item in getCountStatusOpen
                                      where item.DealerContact.Contains(dealer_contact)
                                      select new
                                      {
                                          status = item.status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact,
                                          FromUnit = item.FromUnit
                                      });
                getCountStatusClose = (from item in getCountStatusClose
                                       where item.DealerContact.Contains(dealer_contact)
                                       select new
                                       {
                                           status = item.status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact,
                                           FromUnit = item.FromUnit
                                       });
                getCountStatusIssuPending = (from item in getCountStatusIssuPending
                                             where item.DealerContact.Contains(dealer_contact)
                                             select new
                                             {
                                                 status = item.status,
                                                 TechReps = item.TechReps,
                                                 DealerContact = item.DealerContact,
                                                 FromUnit = item.FromUnit
                                             });
                getCountStatusUnsubmitted = (from item in getCountStatusUnsubmitted
                                             where item.DealerContact.Contains(dealer_contact)
                                             select new
                                             {
                                                 status = item.status,
                                                 TechReps = item.TechReps,
                                                 DealerContact = item.DealerContact,
                                                 FromUnit = item.FromUnit
                                             });
            }


            convToDecimalStatusOpen = Convert.ToDecimal(getCountStatusOpen.Count());
            convToDecimalStatusClose = Convert.ToDecimal(getCountStatusClose.Count());
            convToDecimalStatusIssuePending = Convert.ToDecimal(getCountStatusIssuPending.Count());
            convToDecimalStatusUnsubmitted = Convert.ToDecimal(getCountStatusUnsubmitted.Count());
            convToDecimalStatuAll = Convert.ToDecimal(getCountStatusAll.Count());

            var resultStatusOpen = (convToDecimalStatuAll != 0 && convToDecimalStatusOpen != 0) ? (convToDecimalStatusOpen / convToDecimalStatuAll) * 100 : 0;
            var resultStatusClose = (convToDecimalStatuAll != 0 && convToDecimalStatusClose != 0) ? (convToDecimalStatusClose / convToDecimalStatuAll) * 100 : 0;
            var resultStatusIssuPending = (convToDecimalStatuAll != 0 && convToDecimalStatusIssuePending != 0) ? (convToDecimalStatusIssuePending / convToDecimalStatuAll) * 100 : 0;
            var resultStatusUnsubmitted = (convToDecimalStatuAll != 0 && convToDecimalStatusUnsubmitted != 0) ? (convToDecimalStatusUnsubmitted / convToDecimalStatuAll) * 100 : 0; ;
            var data = new ChartPPMStatus();
            data.CountStatusClose = getCountStatusClose.Count();
            data.CountStatusOpen = getCountStatusOpen.Count();
            data.CountStatusIssuePending = getCountStatusIssuPending.Count();
            data.CountStatusUnsubmitted = getCountStatusUnsubmitted.Count();
            data.StatusClose = resultStatusClose;
            data.StatusOpen = resultStatusOpen;
            data.StatusIssuePending = resultStatusIssuPending;
            data.StatusUnsubmitted = resultStatusUnsubmitted;
            return data;
        }

        public ChartPPMIndustry CountIndustry(string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            var getAllData = (from item in _ctx.DPPMSummary
                              where item.ParentDealerCode == "J210"
                              select new
                              {
                                  Id = item.DPPMSummaryId,
                                  FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                  Status = item.Status,
                                  TechReps = item.TechRep,
                                  DealerContact = item.DealerContactName
                              });
            var getDataCompactMachine = (from item in _ctx.DPPMSummary
                                         where item.ParentDealerCode == "J210"
                                         select new
                                         {
                                             Id = item.DPPMSummaryId,
                                             FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                             Status = item.Status,
                                             TechReps = item.TechRep,
                                             DealerContact = item.DealerContactName
                                         });
            var vargetDataCompactMachine = (from item in getDataCompactMachine
                                            where item.FromUnit.PrimeProductFamily.Contains("Small Off-Highway Trucks")
                                            select new
                                            {
                                                Id = item.Id,
                                                FromUnit = item.FromUnit,
                                                Status = item.Status,
                                                TechReps = item.TechReps,
                                                DealerContact = item.DealerContact
                                            });

            var getDataEarthMove = (from item in _ctx.DPPMSummary
                                    where item.ParentDealerCode == "J210"
                                    select new
                                    {
                                        Id = item.DPPMSummaryId,
                                        FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                        Status = item.Status,
                                        TechReps = item.TechRep,
                                        DealerContact = item.DealerContactName
                                    });

            var vargetDataEarthMove = (from item in getDataEarthMove
                                       where item.FromUnit.PrimeProductFamily.Contains("Motor Graders")
                                            select new
                                            {
                                                Id = item.Id,
                                                FromUnit = item.FromUnit,
                                                Status = item.Status,
                                                TechReps = item.TechReps,
                                                DealerContact = item.DealerContact
                                            });

            var getDataMarineEngAux = (from item in _ctx.DPPMSummary
                                       where item.ParentDealerCode == "J210"
                                       select new
                                       {
                                           Id = item.DPPMSummaryId,
                                           FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                           Status = item.Status,
                                           TechReps = item.TechRep,
                                           DealerContact = item.DealerContactName
                                       });
            var vargetDataMarineEngAux = (from item in getDataMarineEngAux
                                          where item.FromUnit.PrimeProductFamily.Contains("Medium Track Type Tractors")
                                          select new
                                          {
                                              Id = item.Id,
                                              FromUnit = item.FromUnit,
                                              Status = item.Status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact
                                          });

            var getDataMining = (from item in _ctx.DPPMSummary
                                 where item.ParentDealerCode == "J210"
                                 select new
                                 {
                                     Id = item.DPPMSummaryId,
                                     FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                     Status = item.Status,
                                     TechReps = item.TechRep,
                                     DealerContact = item.DealerContactName
                                 });
            var vargetDataMining = (from item in getDataMining
                                    where item.FromUnit.PrimeProductFamily.Contains("Rotary Drills")
                                    select new
                                    {
                                        Id = item.Id,
                                        FromUnit = item.FromUnit,
                                        Status = item.Status,
                                        TechReps = item.TechReps,
                                        DealerContact = item.DealerContact
                                    });
            
            var getDataBlank = (from item in _ctx.DPPMSummary
                                where item.ParentDealerCode == "J210"
                                select new
                                {
                                    Id = item.DPPMSummaryId,
                                    FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                    Status = item.Status,
                                    TechReps = item.TechRep,
                                    DealerContact = item.DealerContactName
                                });
            var vargetDataBlank = (from item in getDataBlank
                                   where item.FromUnit.PrimeProductFamily == ""
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });

            var getDataOther = (from item in _ctx.DPPMSummary
                                where item.ParentDealerCode == "J210"
                                select new
                                {
                                    Id = item.DPPMSummaryId,
                                    FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                    Status = item.Status,
                                    TechReps = item.TechRep,
                                    DealerContact = item.DealerContactName
                                });
            var vargetDataOther = (from item in getDataOther
                                   where item.FromUnit.PrimeProductFamily != "" && item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills"
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });

            if (!string.IsNullOrWhiteSpace(register))
            {
                getAllData = (from item in getAllData
                              where item.FromUnit.PrimeProductModel == register
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                            where item.FromUnit.PrimeProductModel == register
                                            select new
                                                {
                                                    Id = item.Id,
                                                    FromUnit = item.FromUnit,
                                                    Status = item.Status,
                                                    TechReps = item.TechReps,
                                                    DealerContact = item.DealerContact
                                                });
                vargetDataEarthMove = (from item in vargetDataEarthMove
                                       where item.FromUnit.PrimeProductModel == register
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                          where item.FromUnit.PrimeProductModel == register
                                          select new
                                          {
                                              Id = item.Id,
                                              FromUnit = item.FromUnit,
                                              Status = item.Status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact
                                          });
                vargetDataMining = (from item in vargetDataMining
                                    where item.FromUnit.PrimeProductModel == register
                                    select new
                                    {
                                        Id = item.Id,
                                        FromUnit = item.FromUnit,
                                        Status = item.Status,
                                        TechReps = item.TechReps,
                                        DealerContact = item.DealerContact
                                    });
                vargetDataBlank = (from item in vargetDataBlank
                                where item.FromUnit.PrimeProductModel == register
                                select new
                                {
                                    Id = item.Id,
                                    FromUnit = item.FromUnit,
                                    Status = item.Status,
                                    TechReps = item.TechReps,
                                    DealerContact = item.DealerContact
                                });
                vargetDataOther = (from item in vargetDataOther
                                   where item.FromUnit.PrimeProductModel == register
                                select new
                                {
                                    Id = item.Id,
                                    FromUnit = item.FromUnit,
                                    Status = item.Status,
                                    TechReps = item.TechReps,
                                    DealerContact = item.DealerContact
                                });
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                //var splitStatus = new string[] { };
                //splitStatus = status.Split('(', ' ');
                //var result = splitStatus[0].ToString() + " " + splitStatus[1].ToString();
                getAllData = (from item in getAllData
                              where item.Status == status
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                            where item.Status == status
                                            select new
                                            {
                                                Id = item.Id,
                                                FromUnit = item.FromUnit,
                                                Status = item.Status,
                                                TechReps = item.TechReps,
                                                DealerContact = item.DealerContact
                                            });
                vargetDataEarthMove = (from item in vargetDataEarthMove
                                       where item.Status == status
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                          where item.Status == status
                                          select new
                                          {
                                              Id = item.Id,
                                              FromUnit = item.FromUnit,
                                              Status = item.Status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact
                                          });
                vargetDataMining = (from item in vargetDataMining
                                    where item.Status == status
                                    select new
                                    {
                                        Id = item.Id,
                                        FromUnit = item.FromUnit,
                                        Status = item.Status,
                                        TechReps = item.TechReps,
                                        DealerContact = item.DealerContact
                                    });
                vargetDataBlank = (from item in vargetDataBlank
                                   where item.Status == status
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                vargetDataOther = (from item in vargetDataOther
                                   where item.Status == status
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
            }
            if (!string.IsNullOrWhiteSpace(tech_reps))
            {
                //var split = new string[] { };
                //split = tech_reps.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (tech_reps == "Blank")
                {
                    getAllData = (from item in getAllData
                                  where item.TechReps == ""
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                                where item.TechReps == ""
                                                select new
                                                {
                                                    Id = item.Id,
                                                    FromUnit = item.FromUnit,
                                                    Status = item.Status,
                                                    TechReps = item.TechReps,
                                                    DealerContact = item.DealerContact
                                                });
                    vargetDataEarthMove = (from item in vargetDataEarthMove
                                           where item.TechReps == ""
                                           select new
                                           {
                                               Id = item.Id,
                                               FromUnit = item.FromUnit,
                                               Status = item.Status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact
                                           });
                    vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                              where item.TechReps == ""
                                              select new
                                              {
                                                  Id = item.Id,
                                                  FromUnit = item.FromUnit,
                                                  Status = item.Status,
                                                  TechReps = item.TechReps,
                                                  DealerContact = item.DealerContact
                                              });
                    vargetDataMining = (from item in vargetDataMining
                                        where item.TechReps == ""
                                        select new
                                        {
                                            Id = item.Id,
                                            FromUnit = item.FromUnit,
                                            Status = item.Status,
                                            TechReps = item.TechReps,
                                            DealerContact = item.DealerContact
                                        });
                    vargetDataBlank = (from item in vargetDataBlank
                                       where item.TechReps == ""
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetDataOther = (from item in vargetDataOther
                                       where item.TechReps == ""
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                }
                else if(tech_reps == "Others")
                {
                    getAllData = (from item in getAllData
                                  where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                                where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                                select new
                                                {
                                                    Id = item.Id,
                                                    FromUnit = item.FromUnit,
                                                    Status = item.Status,
                                                    TechReps = item.TechReps,
                                                    DealerContact = item.DealerContact
                                                });
                    vargetDataEarthMove = (from item in vargetDataEarthMove
                                           where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                           select new
                                           {
                                               Id = item.Id,
                                               FromUnit = item.FromUnit,
                                               Status = item.Status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact
                                           });
                    vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                              where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                              select new
                                              {
                                                  Id = item.Id,
                                                  FromUnit = item.FromUnit,
                                                  Status = item.Status,
                                                  TechReps = item.TechReps,
                                                  DealerContact = item.DealerContact
                                              });
                    vargetDataMining = (from item in vargetDataMining
                                        where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                        select new
                                        {
                                            Id = item.Id,
                                            FromUnit = item.FromUnit,
                                            Status = item.Status,
                                            TechReps = item.TechReps,
                                            DealerContact = item.DealerContact
                                        });
                    vargetDataBlank = (from item in vargetDataBlank
                                       where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetDataOther = (from item in vargetDataOther
                                       where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                }
                else
                {
                    getAllData = (from item in getAllData
                                  where item.TechReps.Contains(tech_reps)
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                                where item.TechReps.Contains(tech_reps)
                                                select new
                                                {
                                                    Id = item.Id,
                                                    FromUnit = item.FromUnit,
                                                    Status = item.Status,
                                                    TechReps = item.TechReps,
                                                    DealerContact = item.DealerContact
                                                });
                    vargetDataEarthMove = (from item in vargetDataEarthMove
                                           where item.TechReps.Contains(tech_reps)
                                           select new
                                           {
                                               Id = item.Id,
                                               FromUnit = item.FromUnit,
                                               Status = item.Status,
                                               TechReps = item.TechReps,
                                               DealerContact = item.DealerContact
                                           });
                    vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                              where item.TechReps.Contains(tech_reps)
                                              select new
                                              {
                                                  Id = item.Id,
                                                  FromUnit = item.FromUnit,
                                                  Status = item.Status,
                                                  TechReps = item.TechReps,
                                                  DealerContact = item.DealerContact
                                              });
                    vargetDataMining = (from item in vargetDataMining
                                        where item.TechReps.Contains(tech_reps)
                                        select new
                                        {
                                            Id = item.Id,
                                            FromUnit = item.FromUnit,
                                            Status = item.Status,
                                            TechReps = item.TechReps,
                                            DealerContact = item.DealerContact
                                        });
                    vargetDataBlank = (from item in vargetDataBlank
                                       where item.TechReps.Contains(tech_reps)
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetDataOther = (from item in vargetDataOther
                                       where item.TechReps.Contains(tech_reps)
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                }
                
            }
            if (!string.IsNullOrWhiteSpace(dealer_contact))
            {
                getAllData = (from item in getAllData
                              where item.DealerContact.Contains(dealer_contact)
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetDataCompactMachine = (from item in vargetDataCompactMachine
                                            where item.DealerContact.Contains(dealer_contact)
                                            select new
                                            {
                                                Id = item.Id,
                                                FromUnit = item.FromUnit,
                                                Status = item.Status,
                                                TechReps = item.TechReps,
                                                DealerContact = item.DealerContact
                                            });
                vargetDataEarthMove = (from item in vargetDataEarthMove
                                       where item.DealerContact.Contains(dealer_contact)
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                vargetDataMarineEngAux = (from item in vargetDataMarineEngAux
                                          where item.DealerContact.Contains(dealer_contact)
                                          select new
                                          {
                                              Id = item.Id,
                                              FromUnit = item.FromUnit,
                                              Status = item.Status,
                                              TechReps = item.TechReps,
                                              DealerContact = item.DealerContact
                                          });
                vargetDataMining = (from item in vargetDataMining
                                    where item.DealerContact.Contains(dealer_contact)
                                    select new
                                    {
                                        Id = item.Id,
                                        FromUnit = item.FromUnit,
                                        Status = item.Status,
                                        TechReps = item.TechReps,
                                        DealerContact = item.DealerContact
                                    });
                vargetDataBlank = (from item in vargetDataBlank
                                   where item.DealerContact.Contains(dealer_contact)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                vargetDataOther = (from item in vargetDataOther
                                   where item.DealerContact.Contains(dealer_contact)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
            }

            var convToDecimalAllData = Convert.ToDecimal(getAllData.Count());
            var convToDecimalCompactMachine = Convert.ToDecimal(vargetDataCompactMachine.Count());
            var convToDecimalEarthMove = Convert.ToDecimal(vargetDataEarthMove.Count());
            var convToDecimalMarineEngAux = Convert.ToDecimal(vargetDataMarineEngAux.Count());
            var convToDecimalMining = Convert.ToDecimal(vargetDataMining.Count());
            var convToDecimalBlank = Convert.ToDecimal(vargetDataBlank.Count());
            var convToDecimalOthers = Convert.ToDecimal(vargetDataOther.Count());

            var resultCompactMachine = (convToDecimalCompactMachine != 0) ? (convToDecimalCompactMachine / convToDecimalAllData) * 100 : 0;
            var resultEarthMove = (convToDecimalEarthMove != 0) ? (convToDecimalEarthMove / convToDecimalAllData) * 100 : 0;
            var resultMarineEngAux = (convToDecimalMarineEngAux != 0) ? (convToDecimalMarineEngAux / convToDecimalAllData) * 100 : 0;
            var resultMining = (convToDecimalMining != 0) ? (convToDecimalMining / convToDecimalAllData) * 100 : 0;
            var resultBlank = (convToDecimalBlank != 0) ? (convToDecimalBlank / convToDecimalAllData) * 100 : 0;
            var resultOthers = (convToDecimalOthers != 0) ? (convToDecimalOthers / convToDecimalAllData) * 100 : 0;

            var data = new ChartPPMIndustry();
            data.CountCompactMachines = vargetDataCompactMachine.Count();
            data.CompactMachines = resultCompactMachine;
            data.CountEarthMove = vargetDataEarthMove.Count();
            data.EarthMove = resultEarthMove;
            data.CountMarineEngAux = vargetDataMarineEngAux.Count();
            data.MarineEngAux = resultMarineEngAux;
            data.CountMining = vargetDataMining.Count();
            data.Mining = resultMining;
            data.CountBlank = vargetDataBlank.Count();
            data.Blank = resultBlank;
            data.CountOther = vargetDataOther.Count();
            data.Other = resultOthers;

            return data;
        }

        public ChartPPMTechReps CountTechReps(string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            var getAllData = (from item in _ctx.DPPMSummary
                              where item.ParentDealerCode == "J210"
                              select new
                              {
                                  Id = item.DPPMSummaryId,
                                  FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                  Status = item.Status,
                                  TechReps = item.TechRep,
                                  DealerContact = item.DealerContactName
                              });
            var getChenShuehSy = (from item in _ctx.DPPMSummary
                                  where item.ParentDealerCode == "J210"
                                  select new
                                  {
                                      Id = item.DPPMSummaryId,
                                      FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                      Status = item.Status,
                                      TechReps = item.TechRep,
                                      DealerContact = item.DealerContactName
                                  });
            var vargetChenShuehSy = (from item in getChenShuehSy
                                     where item.TechReps == "Chen, Shueh Sy"
                                     select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
            var getChouglan = (from item in _ctx.DPPMSummary
                               where item.ParentDealerCode == "J210"
                               select new
                               {
                                   Id = item.DPPMSummaryId,
                                   FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                   Status = item.Status,
                                   TechReps = item.TechRep,
                                   DealerContact = item.DealerContactName
                               });
            var vargetChouglan = (from item in getChouglan
                                  where item.TechReps.Contains("Coughlan, Zane Stephen")
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });

            var getJoniYohanis = (from item in _ctx.DPPMSummary
                                  where item.ParentDealerCode == "J210"
                                  select new
                                  {
                                      Id = item.DPPMSummaryId,
                                      FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                      Status = item.Status,
                                      TechReps = item.TechRep,
                                      DealerContact = item.DealerContactName
                                  });
            var vargetJoniYohanis = (from item in getJoniYohanis
                                     where item.TechReps.Contains("Joni, Yohanis")
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });


            var getTamaAnton = (from item in _ctx.DPPMSummary
                                where item.ParentDealerCode == "J210"
                                select new
                                {
                                    Id = item.DPPMSummaryId,
                                    FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                    Status = item.Status,
                                    TechReps = item.TechRep,
                                    DealerContact = item.DealerContactName
                                });
            var vargetTamaAnton = (from item in getTamaAnton
                                   where item.TechReps.Contains("Tama, Anton Firman")
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });

            var getBlank = (from item in _ctx.DPPMSummary
                            where item.ParentDealerCode == "J210"
                            select new
                            {
                                Id = item.DPPMSummaryId,
                                FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                Status = item.Status,
                                TechReps = item.TechRep,
                                DealerContact = item.DealerContactName
                            });
            var vargetBlank = (from item in getBlank
                               where item.TechReps == ""
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });

            var getOther = (from item in _ctx.DPPMSummary
                            where item.ParentDealerCode == "J210"
                            select new
                            {
                                Id = item.DPPMSummaryId,
                                FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                Status = item.Status,
                                TechReps = item.TechRep,
                                DealerContact = item.DealerContactName
                            });
            var vargetOther = (from item in getBlank
                               where item.TechReps != "" && item.TechReps != "Tama, Anton Firman" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Chen, Shueh Sy"
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });

            if (!string.IsNullOrWhiteSpace(register))
            {
                getAllData = (from item in getAllData
                              where item.FromUnit.PrimeProductModel == register
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetChenShuehSy = (from item in vargetChenShuehSy
                                     where item.FromUnit.PrimeProductModel == register
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });
                vargetChouglan = (from item in vargetChouglan
                                  where item.FromUnit.PrimeProductModel == register
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                vargetJoniYohanis = (from item in vargetJoniYohanis
                                     where item.FromUnit.PrimeProductModel == register
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                vargetTamaAnton = (from item in vargetTamaAnton
                                   where item.FromUnit.PrimeProductModel == register
                                select new
                                {
                                    Id = item.Id,
                                    FromUnit = item.FromUnit,
                                    Status = item.Status,
                                    TechReps = item.TechReps,
                                    DealerContact = item.DealerContact
                                });
                vargetBlank = (from item in vargetBlank
                               where item.FromUnit.PrimeProductModel == register
                            select new
                            {
                                Id = item.Id,
                                FromUnit = item.FromUnit,
                                Status = item.Status,
                                TechReps = item.TechReps,
                                DealerContact = item.DealerContact
                            });
                vargetOther = (from item in vargetOther
                               where item.FromUnit.PrimeProductModel == register
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                //var splitStatus = new string[] { };
                //splitStatus = status.Split('(', ' ');
                //var resultSplit = splitStatus[0].ToString() + " " + splitStatus[1].ToString();
                getAllData = (from item in getAllData
                              where item.Status.Contains(status)
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetChenShuehSy = (from item in vargetChenShuehSy
                                     where item.Status.Contains(status)
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });
                vargetChouglan = (from item in vargetChouglan
                                  where item.Status.Contains(status)
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                vargetJoniYohanis = (from item in vargetJoniYohanis
                                     where item.Status.Contains(status)
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });
                vargetTamaAnton = (from item in vargetTamaAnton
                                   where item.Status.Contains(status)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                vargetBlank = (from item in vargetBlank
                               where item.Status.Contains(status)
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });
                vargetOther = (from item in vargetOther
                               where item.Status.Contains(status)
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });
            }

            if (!string.IsNullOrWhiteSpace(industry))
            {
                //var split = new string[] { };
                //split = industry.Split('(', ' ');
                //var resultSplit = "";
                //if (split.Count() == 10)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString() + " " + split[3].ToString();
                //}
                //if (split.Count() == 9)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    resultSplit = split[0].ToString();
                //}
                if (industry == "Blank")
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily == ""
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetChenShuehSy = (from item in vargetChenShuehSy
                                         where item.FromUnit.PrimeProductFamily == ""
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetChouglan = (from item in vargetChouglan
                                      where item.FromUnit.PrimeProductFamily == ""
                                      select new
                                      {
                                          Id = item.Id,
                                          FromUnit = item.FromUnit,
                                          Status = item.Status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact
                                      });
                    vargetJoniYohanis = (from item in vargetJoniYohanis
                                         where item.FromUnit.PrimeProductFamily == ""
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetTamaAnton = (from item in vargetTamaAnton
                                       where item.FromUnit.PrimeProductFamily == ""
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetBlank = (from item in vargetBlank
                                   where item.FromUnit.PrimeProductFamily == ""
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                    vargetOther = (from item in vargetOther
                                   where item.FromUnit.PrimeProductFamily == ""
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                }
                else if(industry == "Others")
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetChenShuehSy = (from item in vargetChenShuehSy
                                         where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetChouglan = (from item in vargetChouglan
                                      where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                      select new
                                      {
                                          Id = item.Id,
                                          FromUnit = item.FromUnit,
                                          Status = item.Status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact
                                      });
                    vargetJoniYohanis = (from item in vargetJoniYohanis
                                         where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetTamaAnton = (from item in vargetTamaAnton
                                       where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetBlank = (from item in vargetBlank
                                   where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                    vargetOther = (from item in vargetOther
                                   where item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills" && item.FromUnit.PrimeProductFamily != ""
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                }
                else
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily.Contains(industry)
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                    vargetChenShuehSy = (from item in vargetChenShuehSy
                                         where item.FromUnit.PrimeProductFamily.Contains(industry)
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetChouglan = (from item in vargetChouglan
                                      where item.FromUnit.PrimeProductFamily.Contains(industry)
                                      select new
                                      {
                                          Id = item.Id,
                                          FromUnit = item.FromUnit,
                                          Status = item.Status,
                                          TechReps = item.TechReps,
                                          DealerContact = item.DealerContact
                                      });
                    vargetJoniYohanis = (from item in vargetJoniYohanis
                                         where item.FromUnit.PrimeProductFamily.Contains(industry)
                                         select new
                                         {
                                             Id = item.Id,
                                             FromUnit = item.FromUnit,
                                             Status = item.Status,
                                             TechReps = item.TechReps,
                                             DealerContact = item.DealerContact
                                         });
                    vargetTamaAnton = (from item in vargetTamaAnton
                                       where item.FromUnit.PrimeProductFamily.Contains(industry)
                                       select new
                                       {
                                           Id = item.Id,
                                           FromUnit = item.FromUnit,
                                           Status = item.Status,
                                           TechReps = item.TechReps,
                                           DealerContact = item.DealerContact
                                       });
                    vargetBlank = (from item in vargetBlank
                                   where item.FromUnit.PrimeProductFamily.Contains(industry)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                    vargetOther = (from item in vargetOther
                                   where item.FromUnit.PrimeProductFamily.Contains(industry)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                }
                
            }

            if (!string.IsNullOrWhiteSpace(dealer_contact))
            {
                getAllData = (from item in getAllData
                              where item.DealerContact.Contains(dealer_contact)
                              select new
                              {
                                  Id = item.Id,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechReps = item.TechReps,
                                  DealerContact = item.DealerContact
                              });
                vargetChenShuehSy = (from item in vargetChenShuehSy
                                     where item.DealerContact.Contains(dealer_contact)
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });
                vargetChouglan = (from item in vargetChouglan
                                  where item.DealerContact.Contains(dealer_contact)
                                  select new
                                  {
                                      Id = item.Id,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechReps = item.TechReps,
                                      DealerContact = item.DealerContact
                                  });
                vargetJoniYohanis = (from item in vargetJoniYohanis
                                     where item.DealerContact.Contains(dealer_contact)
                                     select new
                                     {
                                         Id = item.Id,
                                         FromUnit = item.FromUnit,
                                         Status = item.Status,
                                         TechReps = item.TechReps,
                                         DealerContact = item.DealerContact
                                     });
                vargetTamaAnton = (from item in vargetTamaAnton
                                   where item.DealerContact.Contains(dealer_contact)
                                   select new
                                   {
                                       Id = item.Id,
                                       FromUnit = item.FromUnit,
                                       Status = item.Status,
                                       TechReps = item.TechReps,
                                       DealerContact = item.DealerContact
                                   });
                vargetBlank = (from item in vargetBlank
                               where item.DealerContact.Contains(dealer_contact)
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });
                vargetOther = (from item in vargetOther
                               where item.DealerContact.Contains(dealer_contact)
                               select new
                               {
                                   Id = item.Id,
                                   FromUnit = item.FromUnit,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   DealerContact = item.DealerContact
                               });
            }


            var convDataAll = Convert.ToDecimal(getAllData.Count());
            var convChenShuehSy = Convert.ToDecimal(vargetChenShuehSy.Count());
            var convChouglan = Convert.ToDecimal(vargetChouglan.Count());
            var convJoniYohanis = Convert.ToDecimal(vargetJoniYohanis.Count());
            var convTamaAnton = Convert.ToDecimal(vargetTamaAnton.Count());
            var convBlank = Convert.ToDecimal(vargetBlank.Count());
            var convOther = Convert.ToDecimal(vargetOther.Count());

            var resultChenShuehSy = (convChenShuehSy != 0) ? (convChenShuehSy / convDataAll) * 100 : 0;
            var resultChouglan = (convChouglan != 0) ? (convChouglan / convDataAll) * 100 : 0;
            var resultJoniYohanis = (convJoniYohanis != 0) ? (convJoniYohanis / convDataAll) * 100 : 0;
            var resultTamaAnton = (convTamaAnton != 0) ? (convTamaAnton / convDataAll) * 100 : 0;
            var resultBlank = (convBlank != 0) ? (convBlank / convDataAll) * 100 : 0;
            var resultOther = (convOther != 0) ? (convOther / convDataAll) * 100 : 0;

            var data = new ChartPPMTechReps();
            data.CountChenShuehSy = vargetChenShuehSy.Count();
            data.ChenShuehSy = resultChenShuehSy;
            data.CountChouglanZaneStephen = vargetChouglan.Count();
            data.ChouglanZaneStephen = resultChouglan;
            data.CountJoniYohanis = vargetJoniYohanis.Count();
            data.JoniYohanis = resultJoniYohanis;
            data.CountTamaAntonFirman = vargetTamaAnton.Count();
            data.TamaAntonFirman = resultTamaAnton;
            data.CountBlank = vargetBlank.Count();
            data.Blank = resultBlank;
            data.CountOther = vargetOther.Count();
            data.Other = resultOther;

            return data;
        }

        public ChartPPMDealerContact CountDealerContact(string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            var getAllData = (from item in _ctx.DPPMSummary
                              where item.ParentDealerCode.Contains("J210")
                              select new
                              {
                                  Name = item.DealerContactName,
                                  FromUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                                  Status = item.Status,
                                  TechRep = item.TechRep
                              });
            if (!string.IsNullOrWhiteSpace(register))
            {
                getAllData = (from item in getAllData
                              where item.FromUnit.PrimeProductModel == register
                              select new
                              {
                                  Name = item.Name,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechRep = item.TechRep,
                              });
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                //var splitStatus = new string[] { };
                //splitStatus = status.Split('(', ' ');
                //var stringStatus = splitStatus[0].ToString() + " " + splitStatus[1].ToString();
                getAllData = (from item in getAllData
                              where item.Status == status
                              select new
                              {
                                  Name = item.Name,
                                  FromUnit = item.FromUnit,
                                  Status = item.Status,
                                  TechRep = item.TechRep,
                              });
            }
            if (!string.IsNullOrWhiteSpace(industry))
            {
                //var split = new string[] { };
                //split = industry.Split('(', ' ');
                //var resultSplit = "";
                //if (split.Count() == 10)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString() + " " + split[3].ToString();
                //}
                //if (split.Count() == 9)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    resultSplit = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    resultSplit = split[0].ToString();
                //}
                if (industry == "Blank")
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily == ""
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                else if(industry == "Others")
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily != "" && item.FromUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.FromUnit.PrimeProductFamily != "Motor Graders" && item.FromUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.FromUnit.PrimeProductFamily != "Rotary Drills"
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                else
                {
                    getAllData = (from item in getAllData
                                  where item.FromUnit.PrimeProductFamily == industry
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                
            }
            if (!string.IsNullOrWhiteSpace(tech_reps))
            {
                //var split = new string[] { };
                //split = tech_reps.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (tech_reps == "Blank")
                {
                    getAllData = (from item in getAllData
                                  where item.TechRep == ""
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                else if(tech_reps == "Others")
                {
                    getAllData = (from item in getAllData
                                  where item.TechRep != "" && item.TechRep != "Chen, Shueh Sy" && item.TechRep != "Coughlan, Zane Stephen" && item.TechRep != "Joni, Yohanis" && item.TechRep != "Tama, Anton Firman"
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                else
                {
                    getAllData = (from item in getAllData
                                  where item.TechRep.Contains(tech_reps)
                                  select new
                                  {
                                      Name = item.Name,
                                      FromUnit = item.FromUnit,
                                      Status = item.Status,
                                      TechRep = item.TechRep,
                                  });
                }
                
            }
            var getData = (from item in getAllData
                           group item by item.Name into a
                           select new
                           {
                               Name = a.FirstOrDefault().Name,
                               Value = a.Count()
                           }).OrderByDescending(odb => odb.Value).ToList();
            var name = new List<string>();
            var value = new List<int>();
            foreach (var item in getData)
            {
                var splitCommaName = new string[]{ };
                splitCommaName = item.Name.Split(',');
                var dealerName = "";
                if(splitCommaName.Count() > 1)
                {
                    dealerName = splitCommaName[0];
                }
                name.Add(dealerName);
                value.Add(item.Value);
            }
            var data = new ChartPPMDealerContact();
            data.Name = name;
            data.Value = value;
            return data;
        }

        public ChartPPMUpdateBar CountBarUpdatePPM(string register, string status, string industry, string tech_reps, string dealer_contact)
        {

            var getData = (from unit in _ctx.DPPMAffectedUnit
                           from dppm in _ctx.DPPM.Where(w => w.SRNumber == unit.DealerPPM)
                           where dppm.ParentDealershipCode == "J210"
                           select new
                           {
                               UnitId = unit.DPPMAffectedUnitId,
                               Model = unit.PrimeProductModel,
                               DealerPPM = unit.DealerPPM,
                               Status = dppm.Status,
                               TechReps = dppm.TechRep,
                               Family = unit.PrimeProductFamily,
                               DealerContact = dppm.DealerContactName,
                               SerialNumber = unit.SerialNumber
                           });

            if (!string.IsNullOrWhiteSpace(status))
            {
                //var splitStatus = new string[] { };
                //splitStatus = status.Split('(', ' ');
                //var result = splitStatus[0].ToString() + " " + splitStatus[1].ToString();
                getData = (from item in getData
                           where item.Status == status
                           select new
                           {
                               UnitId = item.UnitId,
                               Model = item.Model,
                               DealerPPM = item.DealerPPM,
                               Status = item.Status,
                               TechReps = item.TechReps,
                               Family = item.Family,
                               DealerContact = item.DealerContact,
                               SerialNumber = item.SerialNumber
                           });
            }

            if (!string.IsNullOrWhiteSpace(industry))
            {
                //var split = new string[] { };
                //split = industry.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 10)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString() + " " + split[3].ToString();
                //}
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (industry == "Blank")
                {
                    getData = (from item in getData
                               where item.Family == ""
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                else if(industry == "Others")
                {
                    getData = (from item in getData
                               where item.Family != "" && item.Family != "Small Off-Highway Trucks" && item.Family != "Motor Graders" && item.Family != "Medium Track Type Tractors" && item.Family != "Rotary Drills"
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                else
                {
                    getData = (from item in getData
                               where item.Family == industry
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                
            }

            if (!string.IsNullOrWhiteSpace(tech_reps))
            {
                //var split = new string[] { };
                //split = tech_reps.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (tech_reps == "Blank")
                {
                    getData = (from item in getData
                               where item.TechReps == ""
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                else if(tech_reps == "Others")
                {
                    getData = (from item in getData
                               where item.TechReps != "" && item.TechReps != "Chen, Shueh Sy" && item.TechReps != "Coughlan, Zane Stephen" && item.TechReps != "Joni, Yohanis" && item.TechReps != "Tama, Anton Firman"
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                else
                {
                    getData = (from item in getData
                               where item.TechReps == tech_reps
                               select new
                               {
                                   UnitId = item.UnitId,
                                   Model = item.Model,
                                   DealerPPM = item.DealerPPM,
                                   Status = item.Status,
                                   TechReps = item.TechReps,
                                   Family = item.Family,
                                   DealerContact = item.DealerContact,
                                   SerialNumber = item.SerialNumber
                               });
                }
                
            }

            if (!string.IsNullOrWhiteSpace(dealer_contact))
            {
                getData = (from item in getData
                           where item.DealerContact.Contains(dealer_contact)
                           select new
                           {
                               UnitId = item.UnitId,
                               Model = item.Model,
                               DealerPPM = item.DealerPPM,
                               Status = item.Status,
                               TechReps = item.TechReps,
                               Family = item.Family,
                               DealerContact = item.DealerContact,
                               SerialNumber = item.SerialNumber
                           });
            }

            var getDataGroup = (from item in getData
                                where item.Model != "" && item.Model != null
                                group item by item.Model into model
                                select new
                                {
                                    Model = model.FirstOrDefault().Model,
                                    Value = model.Select(s => s.SerialNumber).Distinct().Count()
                                }).OrderByDescending(odb => odb.Value);

            var listLabel = new List<string>();
            var listValue = new List<int>();
            foreach(var item in getDataGroup.ToList())
            {
                //var getCountSN = CountSN(item.Model);
                listLabel.Add(item.Model);
                listValue.Add(item.Value);
            }
            var data = new ChartPPMUpdateBar();
            data.Model = listLabel;
            data.ValueModel = listValue;

            return data;
        }

        public int CountSN(string model)
        {
            int result = 0;
            var getData = _ctx.DPPMAffectedUnit.Where(w => w.PrimeProductModel == model).Select(s => s.SerialNumber).Distinct().Count();
            result = getData;
            return result;
        }

        public List<TableDPPMUpdate> TableDPPMUpdate (string register, string status, string industry, string tech_reps, string dealer_contact, string valueSearch, int column, string order)
        {
            var listItem = new List<TableDPPMUpdate>();
            var getData = (from item in _ctx.DPPMSummary
                           select new
                           {
                               Status = item.Status,
                               ParentDealershipCode = item.ParentDealerCode,
                               DPPMAffectedUnit = _ctx.DPPMAffectedUnit.Where(w => w.DealerPPM == item.SRNumber).FirstOrDefault(),
                               TechRep = item.TechRep,
                               DealerContactName = item.DealerContactName,
                               SRNumber = item.SRNumber,
                               Title = item.Title,
                               Description = item.ProblemDescription,
                               ICA = item.ICA,
                               PCA = item.PCA,
                               DateDealerOpen = item.DateDealerOpen,
                               DateLastUpdated = item.DateLastUpdated
                           });
            if (!string.IsNullOrWhiteSpace(register))
            {
                getData = getData.Where(item => item.DPPMAffectedUnit.PrimeProductModel == register && item.ParentDealershipCode.Contains("J210"));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                //var splitStatus = new string[] { };
                //splitStatus = status.Split('(', ' ');
                //var stringStatus = splitStatus[0].ToString() + " " + splitStatus[1].ToString();
                getData = getData.Where(item => item.Status == status && item.ParentDealershipCode.Contains("J210"));
            }
            if (!string.IsNullOrWhiteSpace(industry))
            {
                //var split = new string[] { };
                //split = industry.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 10)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString() + " " + split[3].ToString();
                //}
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (industry == "Blank")
                {
                    getData = getData.Where(item => item.DPPMAffectedUnit.PrimeProductFamily == "" && item.ParentDealershipCode.Contains("J210"));
                }
                else if(industry == "Others")
                {
                    getData = getData.Where(item => item.DPPMAffectedUnit.PrimeProductFamily != "" && item.DPPMAffectedUnit.PrimeProductFamily != "Small Off-Highway Trucks" && item.DPPMAffectedUnit.PrimeProductFamily != "Motor Graders" && item.DPPMAffectedUnit.PrimeProductFamily != "Medium Track Type Tractors" && item.DPPMAffectedUnit.PrimeProductFamily != "Rotary Drills" && item.ParentDealershipCode.Contains("J210"));
                }
                else
                {
                    getData = getData.Where(item => item.DPPMAffectedUnit.PrimeProductFamily.Contains(industry) && item.ParentDealershipCode.Contains("J210"));
                }
                
            }
            if (!string.IsNullOrWhiteSpace(tech_reps))
            {
                //var split = new string[] { };
                //split = tech_reps.Split('(', ' ');
                //var result = "";
                //if (split.Count() == 9)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString() + " " + split[2].ToString();
                //}
                //if (split.Count() == 8)
                //{
                //    result = split[0].ToString() + " " + split[1].ToString();
                //}
                //if (split.Count() == 7)
                //{
                //    result = split[0].ToString();
                //}
                if (tech_reps == "Blank")
                {
                    getData = getData.Where(item => item.TechRep == "" && item.ParentDealershipCode.Contains("J210"));
                }
                else if(tech_reps == "Others")
                {
                    getData = getData.Where(item => item.ParentDealershipCode.Contains("J210") && item.TechRep != "" && item.TechRep != "Tama, Anton Firman" && item.TechRep != "Joni, Yohanis" && item.TechRep != "Coughlan, Zane Stephen" && item.TechRep != "Chen, Shueh Sy");
                }
                else
                {
                    getData = getData.Where(item => item.TechRep == tech_reps && item.ParentDealershipCode.Contains("J210"));
                }
                
            }
            if (!string.IsNullOrWhiteSpace(dealer_contact))
            {
                getData = getData.Where(item => item.DealerContactName.Contains(dealer_contact) && item.ParentDealershipCode.Contains("J210"));
            }

            var varGetDataModel = (
                 from item in getData
                 where item.ParentDealershipCode.Contains("J210")
                 select new
                 {
                     TicketId = 0,
                     Submiter = 0,
                     DPPMno = "",
                     item.SRNumber,
                     item.Title,
                     item.Description,
                     item.Status,
                     item.DPPMAffectedUnit.PrimeProductModel,
                     item.PCA,
                     item.ICA,
                     item.DateDealerOpen,
                     item.DateLastUpdated,
                     item.TechRep,
                     item.DealerContactName
                 }
            )
                .GroupBy(g => new { g.TicketId, g.PrimeProductModel, g.DPPMno, g.SRNumber, g.Title, g.Description, g.Status, g.PCA, g.ICA, g.DateDealerOpen, g.DateLastUpdated, g.TechRep, g.Submiter, g.DealerContactName })
                .Select(s => new { s.Key.TicketId, s.Key.PrimeProductModel, s.Key.SRNumber, s.Key.Title, s.Key.Description, s.Key.Status, s.Key.PCA, s.Key.ICA, s.Key.DateDealerOpen, s.Key.DateLastUpdated, s.Key.TechRep, s.Key.Submiter, s.Key.DealerContactName });
            

            if (!string.IsNullOrWhiteSpace(valueSearch))
                {
                    varGetDataModel = varGetDataModel.Where(w => w.SRNumber.ToLower().Contains(valueSearch.ToLower()) || w.Title.ToLower().Contains(valueSearch.ToLower()) || w.Description.ToLower().Contains(valueSearch.ToLower()) || w.Status.ToLower().Contains(valueSearch.ToLower()) || w.TechRep.ToLower().Contains(valueSearch.ToLower()) || w.ICA.ToLower().Contains(valueSearch.ToLower()) || w.PCA.ToLower().Contains(valueSearch.ToLower()) || w.DateDealerOpen.ToString().ToLower().Contains(valueSearch.ToLower()) || w.DateLastUpdated.ToString().ToLower().Contains(valueSearch.ToLower()));
                }
            if(column == 0)
            {
                if(order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.Title);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.Title);
                }
            }
            if (column == 1)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.Description);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.Description);
                }
            }
            if (column == 2)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.ICA);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.ICA);
                }
            }
            if (column == 3)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.PCA);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.PCA);
                }
            }
            if (column == 4)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.SRNumber);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.SRNumber);
                }
            }
            if (column == 5)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.Status);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.Status);
                }
            }
            if (column == 6)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.DealerContactName);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.DealerContactName);
                }
            }
            if (column == 7)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.TechRep);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.TechRep);
                }
            }
            if (column == 8)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.DateDealerOpen);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.DateDealerOpen);
                }
            }
            if (column == 9)
            {
                if (order == "asc")
                {
                    varGetDataModel = varGetDataModel.OrderBy(odb => odb.DateLastUpdated);
                }
                else
                {
                    varGetDataModel = varGetDataModel.OrderByDescending(odb => odb.DateLastUpdated);
                }
            }
            var ida = 0;
                foreach (var item in varGetDataModel.ToList())
                {
                    var list = new TableDPPMUpdate();
                    list.Row = ida++;
                    if (string.IsNullOrWhiteSpace(item.DealerContactName))
                    {
                        list.DealerContact = "-";
                    }
                    else
                    {
                        list.DealerContact = item.DealerContactName;
                    }
                    if (string.IsNullOrWhiteSpace(item.SRNumber))
                    {
                        list.DPPMNo = "-";
                    }
                    else
                    {
                        list.DPPMNo = item.SRNumber;
                    }
                    if (string.IsNullOrWhiteSpace(item.Title))
                    {
                        list.Title = "-";
                    }
                    else
                    {
                        list.Title = item.Title;
                    }
                    if (string.IsNullOrWhiteSpace(item.Description))
                    {
                        list.Desc = "-";
                    }
                    else
                    {
                        list.Desc = item.Description;
                    }
                    if (string.IsNullOrWhiteSpace(item.Status))
                    {
                        list.Status = "-";
                    }
                    else
                    {
                        list.Status = item.Status;
                    }
                    if (string.IsNullOrWhiteSpace(item.TechRep))
                    {
                        list.CatReps = "-";
                    }
                    else
                    {
                        list.CatReps = item.TechRep;
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
                    if (item.DateDealerOpen != null)
                    {
                        list.DateCreated = item.DateDealerOpen.ToString();
                    }
                    else
                    {
                        list.DateCreated = "-";
                    }

                    if (item.DateLastUpdated != null)
                    {
                        list.DateLastUpdate = item.DateLastUpdated.ToString();
                    }
                    else
                    {
                        list.DateLastUpdate = "-";
                    }
                    listItem.Add(list);
                }
            return listItem;
        }

    }
}
