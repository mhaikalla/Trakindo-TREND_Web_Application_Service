using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class MasterAreaBusinessService
    {
        private readonly TsicsContext _tsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
          
        public List<MasterArea> GetList()
        {
            List<MasterArea> result = _tsicsContext.MasterArea
                .Where(s => s.Status !=2)
                .OrderBy(s => s.Name)
                .ToList();

            return result;
        }
        public List<MasterArea> GetListActive()
        {
            List<MasterArea> result = _tsicsContext.MasterArea
                .Where(s => s.Status == 1)
                .OrderBy(s => s.Name)
                .ToList();

            return result;
        }
       
        public MasterArea GetDetail(int id)
        {
            MasterArea result = _tsicsContext.MasterArea
                .Where(s => s.Status != 2)
                .SingleOrDefault(s => s.MasterAreaId.Equals(id));

            return result;
        }
        public MasterArea Add(MasterArea masterArea)
        {
            _tsicsContext.MasterArea.Add(masterArea);
            _tsicsContext.SaveChanges();
            return masterArea;
        }
        public MasterArea Edit(MasterArea masterArea)
        {
            _tsicsContext.Entry(masterArea).State = EntityState.Modified;
            _tsicsContext.SaveChanges();
            return masterArea;
        }
        public void DeleteBranchInvolved(int Areaid)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE MasterBranch set Status = 2 where MasterAreaId = " + Areaid);
            };
        }

        public void setIncativeBranchInvolved(int Areaid)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE MasterBranch set Status = 0 where MasterAreaId = " + Areaid);
            };
        }
        public void setActiveBranchInvolved(int Areaid)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE MasterBranch set Status = 1 where MasterAreaId = " + Areaid);
            };
        }
    }
}
