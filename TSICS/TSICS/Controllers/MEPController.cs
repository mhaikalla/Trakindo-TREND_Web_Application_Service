using System.Collections.Generic;
using System.Web.Http;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;

namespace TSICS.Controllers
{
    public class MepController : ApiController
    {
        private readonly MepBusinessService _mepBusinessService = Factory.Create<MepBusinessService>("Mep", ClassType.clsTypeBusinessService);

        [HttpPost]
        public IHttpActionResult FindSuggestion(dynamic param)
        {
            string sn = param.input_val;
            Dictionary<string, string> dict = new Dictionary<string, string> {{"code", "200"}, {"message", ""}};

            return Ok(new { status = dict, data = _mepBusinessService.GetSuggestion(sn) });
        }

        [HttpGet]
        public IHttpActionResult GetBySn(string sn)
        {
            return Ok(_mepBusinessService.GetBySnCustom(sn));
        }
    }
}
