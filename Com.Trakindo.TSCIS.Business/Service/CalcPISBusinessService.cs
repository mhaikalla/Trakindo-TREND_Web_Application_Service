using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class CalcPISBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<CalcPISTemp> GetDataPIS()
        {
            var getData = (from pis in _ctx.PIS
                           select new
                           {
                               PSLSN = pis.ProgramNo + " " + pis.SerialNumber,
                               programNo = pis.ProgramNo,
                               description = pis.Description,
                               serialNumber = pis.SerialNumber,
                               model = pis.Model,
                               assignDealer = pis.AssignDealer,
                               catCustomer = pis.CatCustomer,
                               pslType = pis.PslType,
                               catPSLStatus = pis.CatPslStatus,
                               letterDate = pis.LetterDate,
                               terminationDate = pis.TerminationDate,
                               repairDate = pis.RepairDate,
                               laborHours = pis.LaborHours,
                               catDeliveryDate = pis.CatDeliveryDate,
                               serviceClaimAllowedGroup = pis.ServiceClaimAllowanceGroup,
                               productSMUAgeRange1 = pis.ProductSmuAgeRange1,
                               productSMUAgeRange2 = pis.ProductSmuAgeRange2,
                               productSMUAgeRange3 = pis.ProductSmuAgeRange3,
                               productSMUAgeRange4 = pis.ProductSmuAgeRange4
                           });

            var getDataRebuild = (from pis in getData
                                  select new
                                  {
                                      PSLSN = pis.PSLSN,
                                      programNo = pis.programNo,
                                      description = pis.description,
                                      serialNumber = pis.serialNumber,
                                      model = pis.model,
                                      assignDealer = pis.assignDealer,
                                      catCustomer = pis.catCustomer,
                                      location = _ctx.Location2.Where(w => w.JCode == pis.assignDealer).FirstOrDefault(),
                                      pslType = pis.pslType,
                                      catPSLStatus = pis.catPSLStatus,
                                      letterDate = pis.letterDate,
                                      terminationDate = pis.terminationDate,
                                      repairDate = pis.repairDate,
                                      laborHours = pis.laborHours,
                                      catDeliveryDate = pis.catDeliveryDate,
                                      serviceClaimAllowedGroup = pis.serviceClaimAllowedGroup,
                                      productSMUAgeRange1 = pis.productSMUAgeRange1,
                                      productSMUAgeRange2 = pis.productSMUAgeRange2,
                                      productSMUAgeRange3 = pis.productSMUAgeRange3,
                                      productSMUAgeRange4 = pis.productSMUAgeRange4
                                  });

            var listData = (from pis in getDataRebuild
                            select new
                            {
                                PSLSN = pis.PSLSN,
                                programNo = pis.programNo,
                                description = pis.description,
                                serialNumber = pis.serialNumber,
                                model = pis.model,
                                assignDealer = pis.assignDealer,
                                catCustomer = pis.catCustomer,
                                area = (pis.location.Area != null) ? pis.location.Area : "",
                                region = (pis.location.Region != null) ? pis.location.Region : "",
                                salesOffice = (pis.location.SalesOffice != null) ? pis.location.SalesOffice : "",
                                pslType = pis.pslType,
                                catPSLStatus = pis.catPSLStatus,
                                letterDate = pis.letterDate,
                                terminationDate = pis.terminationDate,
                                repairDate = pis.repairDate,
                                laborHours = pis.laborHours,
                                catDeliveryDate = pis.catDeliveryDate,
                                serviceClaimAllowedGroup = pis.serviceClaimAllowedGroup,
                                productSMUAgeRange1 = pis.productSMUAgeRange1,
                                productSMUAgeRange2 = pis.productSMUAgeRange2,
                                productSMUAgeRange3 = pis.productSMUAgeRange3,
                                productSMUAgeRange4 = pis.productSMUAgeRange4
                            });

            var listItem = new List<CalcPISTemp>();
            foreach (var item in listData.ToList())
            {
                var list = new CalcPISTemp();
                var splitRange1 = new string[] { };
                var splitRange2 = new string[] { };
                var splitRange3 = new string[] { };
                var splitRange4 = new string[] { };
                if (item.productSMUAgeRange1 != null && item.productSMUAgeRange1 != "")
                {
                    splitRange1 = item.productSMUAgeRange1.Split(',', '-', 'h', 'r', 's', 'm', 'o');
                }
                if (item.productSMUAgeRange2 != null && item.productSMUAgeRange2 != "")
                {
                    splitRange2 = item.productSMUAgeRange2.Split(',', '-', 'h', 'r', 's', 'm', 'o');
                }
                if (item.productSMUAgeRange3 != null && item.productSMUAgeRange3 != "")
                {
                    splitRange3 = item.productSMUAgeRange3.Split(',', '-', 'h', 'r', 's', 'm', 'o');
                }
                if (item.productSMUAgeRange4 != null && item.productSMUAgeRange4 != "")
                {
                    splitRange4 = item.productSMUAgeRange4.Split(',', '-', 'h', 'r', 's', 'm', 'o');
                }

                list.PSLSN = item.PSLSN;
                list.ProgramNo = item.programNo;
                list.Description = item.description;
                list.SerialNumber = item.serialNumber;
                list.Model = item.model;
                list.AssignDealer = item.assignDealer;
                list.CatCustomer = item.catCustomer;
                list.Area = item.area;
                list.Region = item.region;
                list.SalesOffice = item.salesOffice;
                list.PSLType = item.pslType;
                list.CatPSLStatus = item.catPSLStatus;
                list.LetterDate = item.letterDate;
                list.TerminationDate = item.terminationDate;
                list.RepairDate = item.repairDate;
                list.LaborHours = item.laborHours;
                list.CatDeliveryDate = item.catDeliveryDate;
                list.ServiceClaimAllowanceGroup = item.serviceClaimAllowedGroup;
                if (item.productSMUAgeRange1 != null && item.productSMUAgeRange1 != "")
                {
                    list.ProductSMURange1 = splitRange1[1];
                    list.ProductAgeRange1 = splitRange1[6];
                }
                else
                {
                    list.ProductAgeRange1 = "";
                    list.ProductSMURange1 = "";
                }

                if (item.productSMUAgeRange2 != null && item.productSMUAgeRange2 != "")
                {
                    list.ProductSMURange2 = splitRange2[1];
                    list.ProductAgeRange2 = splitRange2[6];
                }
                else
                {
                    list.ProductAgeRange2 = "";
                    list.ProductSMURange2 = "";
                }

                if (item.productSMUAgeRange3 != null && item.productSMUAgeRange3 != "")
                {
                    list.ProductSMURange3 = splitRange3[1];
                    list.ProductAgeRange3 = splitRange3[6];
                }
                else
                {
                    list.ProductAgeRange3 = "";
                    list.ProductSMURange3 = "";
                }

                if (item.productSMUAgeRange4 != null && item.productSMUAgeRange4 != "")
                {
                    list.ProductSMURange4 = splitRange4[1];
                    list.ProductAgeRange4 = splitRange4[6];
                }
                else
                {
                    list.ProductAgeRange4 = "";
                    list.ProductSMURange4 = "";
                }

                listItem.Add(list);
            }
            return listItem;
        }

        public List<CalcPIS> Add (List<CalcPIS> data)
        {
            GetDataPIS();
            //_ctx.CalcPIS.Add(data);
            _ctx.SaveChanges();
           
            return data;
        }
    }
}
