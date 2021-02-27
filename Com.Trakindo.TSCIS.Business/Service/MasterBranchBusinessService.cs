using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class MasterBranchBusinessService
    {
        private readonly TsicsContext _tsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
          
        public List<MasterBranch> GetList()
        {
            List<MasterBranch> result = _tsicsContext.MasterBranch
                .Where(s => s.Status != 2)
                .ToList();

            return result;
        }
        public List<MasterBranch> GetListbyArea(int id)
        {
            List<MasterBranch> result = _tsicsContext.MasterBranch
                .Where(s => s.Status != 2)
                .Where(s => s.MasterAreaId == id)
                .OrderBy(s => s.Name)
                .ToList();

            return result;
        }
        public MasterBranch GetDetail(int id)
        {
            MasterBranch result = _tsicsContext.MasterBranch
                .SingleOrDefault(s => s.MasterBranchId.Equals(id));

            return result;
        }
        public MasterBranch Add(MasterBranch masterBranch)
        {
            _tsicsContext.MasterBranch.Add(masterBranch);
            _tsicsContext.SaveChanges();
            return masterBranch;
        }
        public MasterBranch Edit(MasterBranch masterBranch)
        {
            _tsicsContext.Entry(masterBranch).State = EntityState.Modified;
            _tsicsContext.SaveChanges();
            return masterBranch;
        }

        public IQueryable<MasterBranch> GetQueryableList()
        {
            return _tsicsContext.MasterBranch.Where(s => s.Status != 2);
        }
    }
}
