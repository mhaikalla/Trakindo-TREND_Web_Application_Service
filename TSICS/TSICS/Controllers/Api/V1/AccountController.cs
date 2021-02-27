using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using System.Collections.Generic;
using System.Web.Http;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.Api.V1
{
    public class AccountController : ApiController
    {
       private readonly  UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        
        // GET: api/Account
        public IEnumerable<string> Get()
        {
            return new[] { "valasdue1", "valuasde2" };
        }

        [HttpPost]
        public IHttpActionResult FindSuggestionRole(dynamic param)
        {
            string sn = param.input_val;
            Dictionary<string, string> dict = new Dictionary<string, string> {{"code", "200"}, {"message", ""}};

            return Ok(new { status = dict, data = _userBService.GetSuggestionRoleDesc(sn) });
        }
    }
}
