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
    public class DPPMSummaryBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public Tuple<List<DPPMSummary>,int> getDataDPPMSummaryOverview(string[] serialnumber)
        {
            if (serialnumber.Length > 0)
            {
                DPPMAffectedUnit DPPMAU = new DPPMAffectedUnit();
                List<String> ListDPPM = new List<String>();
                var getData = _ctx.DPPMAffectedUnit.Where(item => true).Select(item => new { Serial_Number = item.SerialNumber, DealerPPM = item.DealerPPM }).Distinct();
                foreach (var sn in getData)
                {
                    if (serialnumber.Contains(sn.Serial_Number))
                    {
                        ListDPPM.Add(sn.DealerPPM);
                    }
                }
                String[] DealerDPPM = ListDPPM.Distinct().ToArray();
                var FinalData = _ctx.DPPMSummary.Where(item => DealerDPPM.Contains(item.SRNumber));
                return Tuple.Create(FinalData.ToList(), FinalData.Count());
            }
            else
            {
                return Tuple.Create(_ctx.DPPMSummary.Take(0).ToList(),0);
            }
        }
    }
}
