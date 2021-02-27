using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class UserMessageBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public UserMessage Add(UserMessage usr, string typeMessage = "Registering")
        {
            usr.MessageType = usr.MessageType + " - " + typeMessage;
            _dBtsics.UserMessage.Add(usr);
            _dBtsics.SaveChanges();
            return usr;
        }
        public List<UserMessage> GetListByEmployeeId(int id)
        {
            List<UserMessage> result = _dBtsics.UserMessage
                .Where(q => q.ToEmployeeId == id)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return result;
        }
        public UserMessage GetDetail(int EmployeeId , int Objectid = 0, string Type = "Registering")
        {
            UserMessage Result = _dBtsics.UserMessage.OrderByDescending(sort =>sort.CreatedAt).FirstOrDefault(m => m.FromUserType.Equals(Objectid) && m.MessageType.Contains(Type.ToLower()) && m.FromEmployeeId == EmployeeId);
            return Result;
        }
    }
}
