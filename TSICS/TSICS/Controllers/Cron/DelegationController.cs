using System.Web.Http;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Helper;

namespace TSICS.Controllers.Cron
{
    public class DelegationController : ApiController
    {
        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _UserBs = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly ApiJsonStatus _mStatus = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);
        [HttpGet]
        [Route("job/setDelegation")]
        // GET: Delegation
        public IHttpActionResult setDelegation()
        {
            _mStatus.code = 200;
            _mStatus.message = "Activated";

            return Ok(new { Status = _mStatus });
        }

        [HttpGet]
        [Route("job/endDelegation")]
        // GET: Delegation
        public IHttpActionResult endDelegation()
        {
            _mStatus.code = 200;
            _mStatus.message = "Ended";

            return Ok(new { Status = _mStatus });
        }
    }
}