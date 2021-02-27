using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{

    public class MobileSearchController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        #endregion
         
        [HttpGet]
        [Route("api/mobile/search")]
        public IHttpActionResult GetAnything(string searchString)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (String.IsNullOrEmpty(searchString))
                return BadRequest();

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                var tickets = _ticketBs.GetAnythingLike(searchString);

                if(tickets.Count > 0)
                {
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    List<TicketAPI> result = new List<TicketAPI>();
                    MobileTicketController mobileTicketController = new MobileTicketController();
                    foreach (var ticket in tickets)
                    {
                        result.Add(mobileTicketController.GenerateDetail(ticket.TicketId, user.UserId, false));
                    }

                    return Ok(new { message = _webStatus, result = result.OrderByDescending(ticket => ticket.RecentDate) });
                }
                else
                {
                    _webStatus.code = 404;
                    _webStatus.description = "Not found";

                    return Ok(new { message = _webStatus, result = new object() });
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
        [Route("api/mobile/searchMyTR")]
        public IHttpActionResult GetMtr(string searchString)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (String.IsNullOrEmpty(searchString))
                return BadRequest();

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                var tickets = _ticketBs.GetMyTr(searchString, user.UserId);

                if (tickets.Count > 0)
                {
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    List<TicketAPI> result = new List<TicketAPI>();
                    MobileTicketController mobileTicketController = new MobileTicketController();
                    foreach (var ticket in tickets)
                    {
                        result.Add(mobileTicketController.GenerateDetail(ticket.TicketId, user.UserId, false));
                    }

                    return Ok(new { message = _webStatus, result = result.OrderByDescending(ticket => ticket.RecentDate) });
                }
                else
                {
                    _webStatus.code = 404;
                    _webStatus.description = "Not found";

                    return Ok(new { message = _webStatus, result = new object() });
                }
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