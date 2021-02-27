using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Linq;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileMepController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly MepBusinessService _mepBusinessService = Factory.Create<MepBusinessService>("Mep", ClassType.clsTypeBusinessService);
        #endregion

        [HttpGet]
        [Route("api/mobile/mep")]
        public IHttpActionResult GetMep(string sn)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                var mep = _mepBusinessService.GetBySn(sn);

                if (mep == null)
                {
                    _webStatus.code = 404;
                    _webStatus.description = "Not Found";

                    return Ok(new { message = _webStatus, result = new Object() });
                }
                else
                {
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    return Ok(new { message = _webStatus, result = mep });
                }
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpGet]
        [Route("api/mobile/mepsuggestion")]
        public IHttpActionResult GetMepSuggestion(string sn)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                _webStatus.code = 200;
                _webStatus.description = "Success";

                return Ok(new { message = _webStatus, result = _mepBusinessService.GetSuggestion(sn) });
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