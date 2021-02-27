using Com.Trakindo.Framework;
using System.Web.Http;
using Com.Trakindo.TSICS.Business.Service;
using TSICS.Helper;
// ReSharper disable IdentifierTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileUnitController : ApiController
    {
        // GET: api/MobileUnit
        private readonly EdwUnitBusinessService _edwUnitBs = Factory.Create<EdwUnitBusinessService>("EdwUnit", ClassType.clsTypeBusinessService);
        public IHttpActionResult Get()
        {
            return Ok(new { Code = 404, Message = "not Found" });
        }

        // GET: api/MobileUnit/5
        public IHttpActionResult Get(string id)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            return Ok(new { Code = 200, Authorization = "true", Data = _edwUnitBs.GetByUnitSn(id) });
        }

        // POST: api/MobileUnit
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/MobileUnit/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MobileUnit/5
        public void Delete(int id)
        {
        }
    }
}
