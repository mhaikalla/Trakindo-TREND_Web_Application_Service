using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileEmployeeController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        #endregion

        [HttpGet]
        [Route("api/mobile/respondersuggestion")]
        public IHttpActionResult GetResponderSuggestion(string xupj)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                _webStatus.code = 200;
                _webStatus.description = "Success";

                var userList = _userBusinessService.GetSuggestion(user.UserId, xupj);
                List<UserSuggestionAPI> userListResult = new List<UserSuggestionAPI>();

                if (userList != null)
                {
                    foreach(var userItem in userList)
                    {
                        UserSuggestionAPI userObj = new UserSuggestionAPI()
                        {
                            UserName = userItem.Username,
                            Name = userItem.Name
                        };
                        userListResult.Add(userObj);
                    }
                }

                return Ok(new { status = _webStatus, data = userListResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpGet]
        [Route("api/mobile/participantssuggestion")]
        public IHttpActionResult GetParticipantsSuggestion(string xupj)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                _webStatus.code = 200;
                _webStatus.description = "Success";

                var userList = _userBusinessService.GetSuggestionParticipant(user.UserId, xupj);
                List<UserSuggestionAPI> userListResult = new List<UserSuggestionAPI>();

                if (userList != null)
                {
                    foreach (var userItem in userList)
                    {
                        UserSuggestionAPI userObj = new UserSuggestionAPI()
                        {
                            UserName = userItem.Username,
                            Name = userItem.Name
                        };
                        userListResult.Add(userObj);
                    }
                }

                return Ok(new { status = _webStatus, data = userListResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpPost]
        [Route("api/mobile/getemployeename")]
        public IHttpActionResult GetName(dynamic usersId)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                _webStatus.code = 200;
                _webStatus.description = "Success";

                List<TicketTagsAPI> listName = new List<TicketTagsAPI>();
                if(usersId != null)
                {
                    foreach(int userId in usersId)
                    {
                        TicketTagsAPI userObj = new TicketTagsAPI()
                        {
                            Name = _userBusinessService.GetDetail(userId).Name
                        };
                        listName.Add(userObj);
                    }

                    return Ok(new { status = _webStatus, data = listName });
                }

                _webStatus.code = 400;
                _webStatus.description = "Empty parameter";
                return Ok(new { status = _webStatus, data = new object() });
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