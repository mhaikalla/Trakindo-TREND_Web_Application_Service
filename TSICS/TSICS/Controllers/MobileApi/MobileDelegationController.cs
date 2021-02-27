using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Helper;
using System.Reflection;

namespace TSICS.Controllers.MobileApi
{

    public class MobileDelegationController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);
        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _ticketNoteBusinessService = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        #endregion
        
        [HttpGet]
        [Route("api/mobile/Delegation")]
        public IHttpActionResult checkDelegationUser()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });
            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());
            if(user !=null)
            {
                Delegation delegationData = _userBusinessService.GetDetailDelegationByUserFrom(user.UserId);
                if(delegationData != null)
                {
                    return Ok(new { status = _webStatus, result = new object() });
                }
                else
                {
                    return Ok(new { status = _webStatus, result = new object() });
                }
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }

        }
        [HttpPost]
        [Route("api/mobile/getDelegation")]
        public IHttpActionResult GetDelegation()
        {
            return Ok();

        }
    }
}