using System.Web.Http;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;

namespace TSICS.Controllers
{
    public class WarrantyController : ApiController
    {
        private readonly WarrantyTypeBusinessService _warrantyTypeBusinessService = Factory.Create<WarrantyTypeBusinessService>("WarrantyType", ClassType.clsTypeBusinessService);

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            return Ok(_warrantyTypeBusinessService.Get(id));
        }
    }
}
