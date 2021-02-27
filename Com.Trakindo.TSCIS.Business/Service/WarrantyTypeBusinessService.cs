using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class WarrantyTypeBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public List<WarrantyType> Get()
        {
            return _ctx.WarrantyType.ToList();
        }

        public WarrantyType Get(int id)
        {
            return _ctx.WarrantyType.FirstOrDefault(q => q.WarrantyTypeId == id);
        }
    }
}
