using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System.Linq;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileWarrantyController : ApiController
    {
        #region Business Services
        private readonly  WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly WarrantyTypeBusinessService _warrantyTypeBusinessService = Factory.Create<WarrantyTypeBusinessService>("WarrantyType", ClassType.clsTypeBusinessService);
        #endregion

        [HttpGet]
        [Route("api/mobile/warrantytype")]
        public IHttpActionResult GetWarrantyType()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                return Ok(_warrantyTypeBusinessService.Get());
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }
    }
}