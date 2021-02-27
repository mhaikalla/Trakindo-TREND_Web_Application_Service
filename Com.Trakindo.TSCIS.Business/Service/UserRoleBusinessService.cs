using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class UserRoleBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<UserRole> GetList()
        {
            List<UserRole> result = _dBtsics.UserRole
                .Where(s => s.Status.Equals(1))
                .ToList();

            return result;
        }
        public UserRole GetDetail(int id)
        {
            UserRole userRoleM = _dBtsics.UserRole.Find(id);
            return (userRoleM);
        }
    }
}
