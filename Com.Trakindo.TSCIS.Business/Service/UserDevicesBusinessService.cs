using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
   public class UserDevicesBusinessService
    {
        private readonly TsicsContext _tsicsContext = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public UserDevices InsertData(UserDevices userDevices)
        {
           var cek = GetPlayerId(userDevices.UserId, userDevices.PlayerId);
            if (cek == null)
            {
                _tsicsContext.UserDevices.Add(userDevices);
                _tsicsContext.SaveChanges();
                
            }

            return userDevices;
        }

        public UserDevices GetPlayerId(int userid, string playerId)
        {
            UserDevices result = _tsicsContext.UserDevices.
                Where(uid => uid.UserId == userid).
                SingleOrDefault(pid => pid.PlayerId == playerId);
            return result;
        }
    }
}
