using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class UserTsManagerBusinessService
    {
        private readonly TsicsContext _contextTsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public UserTsManager GetDetail(int id)
        {
            UserTsManager userTs = _contextTsics.UserTsManager.Find(id);
            return (userTs);
        }
        public List<UserTsManager> GetList()
        {
            List<UserTsManager> result = _contextTsics.UserTsManager                
                .ToList();

            return result;
        }
        public string GetListEmail(List<int> ipd)
        {
            string result = "";
            List<UserTsManager> lUser = _contextTsics.UserTsManager
                .Where(r => ipd.Contains(r.LevelTs))
                .ToList();

            foreach (var item in lUser)
            {
                result += ";" + item.Email;
            }
            return result;
        }
        public UserTsManager GetDetailLevel(int levelTs)
        {
            UserTsManager result = _contextTsics.UserTsManager
                .FirstOrDefault(t => t.LevelTs == levelTs);

            return result;
        }
    }
}
