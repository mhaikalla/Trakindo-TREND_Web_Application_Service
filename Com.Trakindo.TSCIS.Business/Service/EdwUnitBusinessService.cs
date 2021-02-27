using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class EdwUnitBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public EdwUnit GetByUnitSn(string sn)
        {
            return _ctx.EdwUnit.FirstOrDefault(q => q.UnitSN == sn);
        }
    }
}
