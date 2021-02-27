using System.Web.Http;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Helper;
// ReSharper disable IdentifierTypo

namespace TSICS.Controllers.Cron
{
    public class RatingController : ApiController
    {
        private  readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBs = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly ApiJsonStatus _mStatus = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);

        [HttpGet]
        [Route("job/rating")]
        public IHttpActionResult Index()
        {
            _mStatus.code = 200;
            _mStatus.message = "Ok";
            _userBs.EndDelegate();
            _userBs.StartDelegate();
            return Ok(new { status = _mStatus, autoClosedTickets = _ticketBs.AutoClose(WebConfigure.GetAutoCloseRulesDay()) });
        }

    }
}