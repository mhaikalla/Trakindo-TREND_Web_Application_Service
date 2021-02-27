using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Helper;
using System.Web;
using System.Text.RegularExpressions;
// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.MobileApi
{
    public class MobileDiscussionController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly TicketDiscussionBusinessService _ticketDiscussionBusinessService = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _ticketNoteBusinessService = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly DiscussionAttachmentBusinessService _discussionAttachmentBusinessService = Factory.Create<DiscussionAttachmentBusinessService>("DiscussionAttachment", ClassType.clsTypeBusinessService);
        private readonly NoteAttachmentBusinessService _noteAttachmentBusinessService = Factory.Create<NoteAttachmentBusinessService>("NoteAttachment", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        #endregion

        private Ticket Ticket = new Ticket();
        private String title = "", content = "",step_1 = "";
        [HttpGet]
        [Route("api/mobile/discussion")]
        public IHttpActionResult GetDiscussion(int ticketId)
        {
            
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());
                int uid = user.UserId;
                if (user != null)
                {
                    List<TicketDiscussion> ticketDiscussions = _ticketDiscussionBusinessService.GetListByTicket(ticketId);

                    List<CustomTicketDiscussion> customTicketDiscussions = new List<CustomTicketDiscussion>();

                    List<TicketNote> ticketNotes = _ticketNoteBusinessService.GetListByTicket(ticketId);

                    ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);

                    List<ApiJsonCommentTR> apiJsonCommentTrml = new List<ApiJsonCommentTR>();

                    if (ticketDiscussions == null && ticketNotes == null)
                    {
                        return NotFound();
                    }

                    if (ticketDiscussions != null)
                    {
                        foreach (TicketDiscussion originCommentObject in ticketDiscussions)
                        {
                            CustomTicketDiscussion customCommentObject = new CustomTicketDiscussion()
                            {
                                TicketDiscussionId = originCommentObject.TicketDiscussionId,
                                TicketId = originCommentObject.TicketId,
                                UserId = originCommentObject.UserId,
                                Description = originCommentObject.Description,
                                CreatedAt = originCommentObject.CreatedAt,
                                TicketNoteId = originCommentObject.TicketNoteId,
                                Status = originCommentObject.Status,

                                IsInserted = false
                            };

                            customTicketDiscussions.Add(customCommentObject);
                        }
                    }

                    Ticket ticket = _ticketBusinessService.GetDetail(ticketId);

                    if (ticketNotes.Count > 0)
                    {
                        foreach (var ticketNote in ticketNotes)
                        {
                            List<ApiJsonCommentTR> noteChild = new List<ApiJsonCommentTR>();

                            if (ticketDiscussions != null)
                            {
                                foreach (CustomTicketDiscussion comment in customTicketDiscussions)
                                {
                                    ApiJsonCommentDateTR apiJsonCommentDateTr1 = new ApiJsonCommentDateTR()
                                    {
                                        day = comment.CreatedAt?.ToString("dd MMM yyyy"),
                                        time = comment.CreatedAt?.ToString("hh:mm tt")
                                    };

                                    ApiJsonCommentSenderTR apiJsonCommentSenderTr1 = new ApiJsonCommentSenderTR()
                                    {
                                        name = _userBusinessService.GetDetail(comment.UserId).Name,
                                        is_verified = true
                                    };
                                    if (ticket.Responder == comment.UserId)
                                    {
                                        apiJsonCommentSenderTr1.type = "Responder";
                                    }
                                    else if(ticket.Submiter == comment.UserId)
                                    {
                                        apiJsonCommentSenderTr1.type = "Submitter";
                                    }
                                    else
                                    {
                                        apiJsonCommentSenderTr1.type = "Participant";
                                    }
                                  
                                    var attachments1 = _discussionAttachmentBusinessService.GetByDiscussionId(comment.TicketDiscussionId);
                                    List<ApiJsonCommentImageTR> listAttachment1 = new List<ApiJsonCommentImageTR>();
                                    string attachmentsPath1 = "/Upload/Discussion/";
                                    if (attachments1 != null)
                                    {
                                        foreach (var attachment in attachments1)
                                        {
                                            listAttachment1.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + attachmentsPath1 + attachment.Name, nama = attachment.Name });
                                        }
                                    }

                                    ApiJsonCommentTR apiJsonCommentTrm1 = new ApiJsonCommentTR
                                    {
                                        id = comment.TicketDiscussionId,
                                        date = apiJsonCommentDateTr1,
                                        sender = apiJsonCommentSenderTr1,
                                        text = comment.Description,
                                        image = listAttachment1,
                                        type = comment.UserId == user.UserId ? "sent" : "received"
                                    };


                                    if (comment.TicketNoteId.Equals(ticketNote.TicketNoteId))
                                    {
                                        noteChild.Add(apiJsonCommentTrm1);
                                    }
                                    else
                                    {
                                        if (comment.TicketNoteId.Equals(0) && comment.IsInserted.Equals(false))
                                        {
                                            apiJsonCommentTrm1.isRemovable = (ticket.Responder == uid);
                                            apiJsonCommentTrml.Add(apiJsonCommentTrm1);
                                            comment.IsInserted = true;
                                        }
                                    }
                                }
                            }

                            ApiJsonCommentDateTR apiJsonCommentDateTr = new ApiJsonCommentDateTR()
                            {
                                day = ticketNote.CreatedAt?.ToString("dd MMM yyyy"),
                                time = ticketNote.CreatedAt?.ToString("hh:mm tt")
                            };

                            ApiJsonCommentSenderTR apiJsonCommentSenderTr = new ApiJsonCommentSenderTR()
                            {
                                name = _userBusinessService.GetDetail(ticketNote.UserId).Name,
                                is_verified = true
                            };
                         
                                if (ticket.TicketCategoryId != 9)
                                {
                                    if (ticket.Responder == ticketNote.UserId)
                                    {
                                        apiJsonCommentSenderTr.type = "Responder";
                                    }
                                    else if(ticket.Submiter == ticketNote.UserId)
                                    {
                                        apiJsonCommentSenderTr.type = "Submitter";
                                    }
                                    else
                                    {
                                        apiJsonCommentSenderTr.type = "Participant";
                                    }
                                }
                                else if (ticket.TicketCategoryId == 9)
                                {
                                    if (ticket.Submiter == ticketNote.UserId)
                                    {
                                        apiJsonCommentSenderTr.type = "Submitter";
                                    }
                                    else
                                    {
                                        apiJsonCommentSenderTr.type = "Responder";
                                    }
                                }
                            var attachments = _noteAttachmentBusinessService.GetByNoteId(ticketNote.TicketNoteId);
                            List<ApiJsonCommentImageTR> listAttachment = new List<ApiJsonCommentImageTR>();
                            string attachmentsPath = "/Upload/Note/";

                            if (attachments != null)
                            {
                                foreach (var attachment in attachments)
                                {
                                    listAttachment.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + attachmentsPath + attachment.Name, nama = attachment.Name });
                                }
                            }

                            ApiJsonCommentTR apiJsonCommentTrm = new ApiJsonCommentTR()
                            {
                                type = "notes",
                                date = apiJsonCommentDateTr,
                                sender = apiJsonCommentSenderTr,
                                text = ticketNote.Description,
                                image = listAttachment,
                                children = noteChild
                            };

                            apiJsonCommentTrml.Add(apiJsonCommentTrm);
                        }
                    }
                    else
                    {
                        if (ticketDiscussions != null)
                            foreach (TicketDiscussion ticketDiscussion in ticketDiscussions)
                            {
                                ApiJsonCommentDateTR apiJsonCommentDateTr1 = new ApiJsonCommentDateTR()
                                {
                                    day = ticketDiscussion.CreatedAt?.ToString("dd MMM yyyy"),
                                    time = ticketDiscussion.CreatedAt?.ToString("hh:mm tt")
                                };

                                ApiJsonCommentSenderTR apiJsonCommentSenderTr1 = new ApiJsonCommentSenderTR()
                                {
                                    name = _userBusinessService.GetDetail(ticketDiscussion.UserId).Name,
                                    is_verified = true
                                };
         
                                    if (ticket.Responder == ticketDiscussion.UserId)
                                    {
                                        apiJsonCommentSenderTr1.type = "Responder";
                                    }
                                    else if(ticket.Submiter == ticketDiscussion.UserId)
                                    {
                                        apiJsonCommentSenderTr1.type = "Submitter";
                                    }
                                    else
                                    {
                                        apiJsonCommentSenderTr1.type = "Participant";
                                    }
                                
                                var attachments1 =
                                    _discussionAttachmentBusinessService.GetByDiscussionId(ticketDiscussion.TicketDiscussionId);
                                List<ApiJsonCommentImageTR> listAttachment1 = new List<ApiJsonCommentImageTR>();
                                string attachmentsPath1 = "/Upload/Discussion/";
                                if (attachments1 != null)
                                {
                                    foreach (var attachment in attachments1)
                                    {
                                        listAttachment1.Add(new ApiJsonCommentImageTR
                                        {
                                            src = WebConfigure.GetDomain() + attachmentsPath1 + attachment.Name,
                                            nama = attachment.Name
                                        });
                                    }
                                }

                                ApiJsonCommentTR apiJsonCommentTrm1 = new ApiJsonCommentTR
                                {
                                    id = ticketDiscussion.TicketDiscussionId,
                                    date = apiJsonCommentDateTr1,
                                    sender = apiJsonCommentSenderTr1,
                                    text = ticketDiscussion.Description,
                                    image = listAttachment1,
                                    isRemovable = (ticket.Responder == uid),
                                    type = ticketDiscussion.UserId == user.UserId ? "sent" : "received"
                                };


                                apiJsonCommentTrml.Add(apiJsonCommentTrm1);
                            }
                    }

                    apiJsonStatusM.code = 200;
                    apiJsonStatusM.message = "ok";

                    return Ok(new { status = apiJsonStatusM, data = apiJsonCommentTrml });
                }
                else
                {
                    _webStatus.code = 403;
                    _webStatus.description = "Invalid AccessToken";

                    return Ok(new { status = _webStatus, result = new object() });
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Route("api/mobile/discussion")]
        public IHttpActionResult PostDiscussion(MobileDiscussionPostAPI data)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });
           

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    string msg = data.Description;
                    int typeMsg = data.Type;
                    int ticketId = data.TicketId;
                    int ticketDiscussionId = 0;
                    int ticketNoteId = 0;
                    DateTime dateTimeNow = DateTime.Now;
                    Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
                    User userData = _userBusinessService.GetDetail(data.UserId);
                    var dataUid = new List<int>();
                    var playerid = new List<string>();
                    var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                    TimeSpan respond = ticket.LastReply == null ? (DateTime.Now.Subtract(ticket.CreatedAt.Value)) : (DateTime.Now.Subtract(ticket.LastReply.Value));
                    if (typeMsg == 1)
                    {
                        TicketDiscussion ticketDiscussion = new TicketDiscussion()
                        {
                            TicketId = ticketId,
                            CreatedAt = dateTimeNow,
                            UserId = data.UserId,
                            Description = msg,
                            TicketNoteId = 0,// -> new comment don't have note
                            Status = 1,
                            RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                        };
                        foreach (var i in participants)
                        {
                            dataUid.Add(i.UserId);
                        }
                        var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                        foreach (var p in iduserdevice)
                        {
                            playerid.Add(p.PlayerId);
                        }
                        
                        playerid.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                        playerid.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);

                        title = ticket.TicketNo + " - " + ticket.Title + " - Reply";
                        content = userData.Name + (ticket.Submiter == data.UserId ? " (Submiter)" : ticket.Responder == data.UserId ? " (Responder)" : " (Participant)") + " : " + msg;
                        Onesignal.PushNotif(content, playerid, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                        ticketDiscussionId = _ticketDiscussionBusinessService.Add(ticketDiscussion).TicketDiscussionId;
                        
                    }
                    else if (typeMsg == 2)
                    {
                        TicketNote ticketNote = new TicketNote()
                        {
                            TicketId = ticketId,
                            CreatedAt = dateTimeNow,
                            UserId = data.UserId,
                            Description = msg,
                            Status = 1,
                            RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                        };
                        foreach (var i in participants)
                        {
                            dataUid.Add(i.UserId);
                        }
                        var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                        foreach (var p in iduserdevice)
                        {
                            playerid.Add(p.PlayerId);
                        }
                        playerid.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                        playerid.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);

                        title = ticket.TicketNo + " - " + ticket.Title + " - Reply As Note";
                        content = userData.Name + (ticket.Submiter == data.UserId ? " (Submiter)" : " (Responder)") + " : " + msg;
                        Onesignal.PushNotif(content, playerid, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                        
                        ticketNoteId = _ticketNoteBusinessService.Add(ticketNote).TicketNoteId;
                        _ticketDiscussionBusinessService.SetNote(ticketId, ticketNoteId);

                    }

                    if (data.Attachments != null)
                    {
                        int fileIndex = 0;
                        foreach (var attachment in data.Attachments)
                        {
                            switch (typeMsg)
                            {
                                case 1:// -> Comment
                                    DiscussionAttachment discussionAttachment = new DiscussionAttachment()
                                    {
                                        TicketDiscussionId = ticketDiscussionId,
                                        Name = Common.AddDiscussionAttachments(Path.GetFileName(attachment.Name), attachment, fileIndex, "Discussion"),
                                        Status = 1
                                    };

                                    _discussionAttachmentBusinessService.Add(discussionAttachment);
                                    break;
                                default:// -> Note
                                    NoteAttachment noteAttachment = new NoteAttachment()
                                    {
                                        TicketNoteId = ticketNoteId,
                                        Name = Common.AddDiscussionAttachments(Path.GetFileName(attachment.Name), attachment, fileIndex, "Note"),
                                        Status = 1
                                    };

                                    _noteAttachmentBusinessService.Add(noteAttachment);
                                    break;
                            }
                        }
                    }

                    List<ApiJsonCommentTR> apiJsonCommentTrList = new List<ApiJsonCommentTR>();

                    ApiJsonCommentDateTR apiJsonCommentDateTr = new ApiJsonCommentDateTR()
                    {
                        day = dateTimeNow.ToString("dddd, dd MMMM yyyy"),
                        time = dateTimeNow.ToString("hh:mm")
                    };

                    ApiJsonCommentSenderTR apiJsonCommentSenderTr = new ApiJsonCommentSenderTR()
                    {
                        name = _userBusinessService.GetDetail(data.UserId).Name,
                        is_verified = true
                    };

                    if (ticket.Responder == data.UserId)
                    {
                        apiJsonCommentSenderTr.type = "Responder";
                        if (typeMsg == 1)
                        {
                            ticket.LastReply = dateTimeNow;
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                        else if (typeMsg == 2)
                        {
                            ticket.NextCommenter = ticket.Submiter;
                            ticket.DueDateAnswer = dateTimeNow.AddDays(Common.NumberOfWorkDays(dateTimeNow, WebConfigure.GetRulesDay()));
                            ticket.LastReply = dateTimeNow;
                            ticket.LastStatusDate = dateTimeNow;
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                    }
                    else
                    {
                        if (ticket.Submiter == data.UserId)
                        {
                            apiJsonCommentSenderTr.type = "Submitter";
                            if(typeMsg == 1)
                            {
                                ticket.LastReply = dateTimeNow;
                                Ticket = _ticketBusinessService.Edit(ticket);
                            }
                            if (typeMsg == 2)
                            {
                                ticket.NextCommenter = ticket.Responder;
                                ticket.DueDateAnswer = dateTimeNow.AddDays(Common.NumberOfWorkDays(dateTimeNow, WebConfigure.GetRulesDay()));
                                ticket.LastReply = dateTimeNow;
                                ticket.LastStatusDate = dateTimeNow;
                                Ticket = _ticketBusinessService.Edit(ticket);
                            }
                        }
                        else
                        {
                            apiJsonCommentSenderTr.type = "Participant";
                            ticket.LastReply = dateTimeNow;
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                    }

                    List<ApiJsonCommentImageTR> listAttachment = new List<ApiJsonCommentImageTR>();

                    var attachments = _discussionAttachmentBusinessService.GetByDiscussionId(ticketDiscussionId);
                    string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Discussion/");

                    if (typeMsg == 2)
                    {
                        attachments = _noteAttachmentBusinessService.GetByNoteId(ticketNoteId);
                        attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Note/");
                    }

                    if (attachments != null)
                        foreach (var attachment in attachments)
                        {
                            listAttachment.Add(new ApiJsonCommentImageTR {
                                src = WebConfigure.GetDomain() + attachmentsPath + attachment.Name,
                                Type = Path.GetExtension(attachment.Name),
                                nama = attachment.Name
                            });
                        }

                    ApiJsonCommentTR apiJsonCommentTr = new ApiJsonCommentTR()
                    {
                        date = apiJsonCommentDateTr,
                        sender = apiJsonCommentSenderTr,
                        text = msg,
                        image = listAttachment
                    };

                    switch (typeMsg)
                    {
                        case 1:
                            apiJsonCommentTr.type = "sent";
                            break;
                        default:
                            apiJsonCommentTr.type = "notes";
                            break;
                    }

                    apiJsonCommentTrList.Add(apiJsonCommentTr);

                    ApiJsonStatus apiJsonStatus = new ApiJsonStatus()
                    {
                        code = 200,
                        message = "ok"
                    };
                    if (typeMsg != 1)
                    {
                        if (ticket.TicketCategoryId != 9)
                        {
                            Email.GetEmailTagTsicsCommentTR(ticket);
                            Email.GetEmailTagTsicsCommentTR(ticket,true);
                        }
                        else
                        {
                            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId, false);
                            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId,true);
                        }
                    }
                    return Json(new { status = apiJsonStatus, data = apiJsonCommentTrList });
                }
                else
                {
                    _webStatus.code = 403;
                    _webStatus.description = "Invalid AccessToken";

                    return Ok(new { status = _webStatus, result = new object() });
                }
            }
            return BadRequest(ModelState);
        }


        [HttpPost]
        [Route("api/mobile/discussionform")]
        public IHttpActionResult PostDiscussion()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });
            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    string msg = HttpContext.Current.Request.Params.Get("Description");
                    int typeMsg = int.Parse(HttpContext.Current.Request.Params.Get("Type"));
                    int ticketId = int.Parse(HttpContext.Current.Request.Params.Get("TicketId"));
                    int ticketDiscussionId = 0;
                    int ticketNoteId = 0;
                    DateTime dateTimeNow = DateTime.Now;
                    Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
                    User userData = _userBusinessService.GetDetail(int.Parse(HttpContext.Current.Request.Params.Get("UserId")));
                    var dataUid = new List<int>();
                    var playerid = new List<string>();
                    var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);

                    TimeSpan respond = ticket.LastReply == null ? (DateTime.Now.Subtract(ticket.CreatedAt.Value)) : (DateTime.Now.Subtract(ticket.LastReply.Value));
                    if (typeMsg == 1)
                    {
                        TicketDiscussion ticketDiscussion = new TicketDiscussion()
                        {
                            TicketId = ticketId,
                            CreatedAt = dateTimeNow,
                            UserId = int.Parse(HttpContext.Current.Request.Params.Get("UserId")),
                            Description = msg,
                            TicketNoteId = 0,// -> new comment don't have note
                            Status = 1,
                            RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                        };
                        foreach (var i in participants)
                        {
                            dataUid.Add(i.UserId);
                        }
                        var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                        foreach (var p in iduserdevice)
                        {
                            playerid.Add(p.PlayerId);
                        }
                        playerid.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                        playerid.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);

                        title = ticket.TicketNo + " - " + ticket.Title + " - Reply";
                        content = userData.Name + (ticket.Submiter == userData.UserId ? " (Submiter)" : ticket.Responder == userData.UserId ? " (Responder)" : " (Participant)") + " : " + msg;
                        Onesignal.PushNotif(content, playerid, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                        ticketDiscussionId = _ticketDiscussionBusinessService.Add(ticketDiscussion).TicketDiscussionId;
                    }
                    else if (typeMsg == 2)
                    {
                        TicketNote ticketNote = new TicketNote()
                        {
                            TicketId = ticketId,
                            CreatedAt = dateTimeNow,
                            UserId = int.Parse(HttpContext.Current.Request.Params.Get("UserId")),
                            Description = msg,
                            Status = 1,
                            RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                        };
                        foreach (var i in participants)
                        {
                            dataUid.Add(i.UserId);
                        }
                        var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                        foreach (var p in iduserdevice)
                        {
                            playerid.Add(p.PlayerId);
                        }
                        playerid.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                        playerid.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);

                        title = ticket.TicketNo + " - " + ticket.Title + " - Reply As Note";
                        content = userData.Name + (ticket.Submiter == userData.UserId ? " (Submiter)" : " (Responder)") + " : " + msg;
                        Onesignal.PushNotif(content, playerid, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);

                        ticketNoteId = _ticketNoteBusinessService.Add(ticketNote).TicketNoteId;
                        _ticketDiscussionBusinessService.SetNote(ticketId, ticketNoteId);
                    }

                    if (HttpContext.Current.Request.Files.Count > 0)
                    {
                        for (int i = 0, iLen = HttpContext.Current.Request.Files.Count; i < iLen; i++)
                        {
                            string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                            var postedFile = HttpContext.Current.Request.Files[i];
                          
                            switch (typeMsg)
                            {
                                case 1:// -> Comment
                                    var fileName = Path.GetFileName(postedFile.FileName);
                                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Discussion/"), fileName);
                                    postedFile.SaveAs(path);
                                    DiscussionAttachment discussionAttachment = new DiscussionAttachment()
                                    {
                                        TicketDiscussionId = ticketDiscussionId,
                                        Name = fileName,
                                        Status = 1
                                    };

                                    _discussionAttachmentBusinessService.Add(discussionAttachment);
                                    break;
                                default:// -> Note
                                    var fileNameNote = Path.GetFileName(postedFile.FileName);
                                    var pathNote = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Note/"), fileNameNote);
                                    postedFile.SaveAs(pathNote);
                                    NoteAttachment noteAttachment = new NoteAttachment()
                                    {
                                        TicketNoteId = ticketNoteId,
                                        Name = fileNameNote,
                                        Status = 1
                                    };

                                    _noteAttachmentBusinessService.Add(noteAttachment);
                                    break;
                            }
                        }
                    }

                    List<ApiJsonCommentTR> apiJsonCommentTrList = new List<ApiJsonCommentTR>();

                    ApiJsonCommentDateTR apiJsonCommentDateTr = new ApiJsonCommentDateTR()
                    {
                        day = dateTimeNow.ToString("dddd, dd MMMM yyyy"),
                        time = dateTimeNow.ToString("hh:mm")
                    };

                    ApiJsonCommentSenderTR apiJsonCommentSenderTr = new ApiJsonCommentSenderTR()
                    {
                        name = _userBusinessService.GetDetail(int.Parse(HttpContext.Current.Request.Params.Get("UserId"))).Name,
                        is_verified = true
                    };

                    if (ticket.Responder == int.Parse(HttpContext.Current.Request.Params.Get("UserId")))
                    {
                        apiJsonCommentSenderTr.type = "Responder";
                        if (typeMsg == 1)
                        {
                            ticket.LastReply = dateTimeNow;
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                        else if (typeMsg == 2)
                        {
                            ticket.NextCommenter = ticket.Submiter;
                            ticket.LastStatusDate = dateTimeNow;
                            ticket.LastReply = dateTimeNow;
                            ticket.DueDateAnswer = dateTimeNow.AddDays(Common.NumberOfWorkDays(dateTimeNow, WebConfigure.GetRulesDay()));
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                    }
                    else
                    {
                        if (ticket.Submiter == int.Parse(HttpContext.Current.Request.Params.Get("UserId")))
                        {
                            apiJsonCommentSenderTr.type = "Submitter";
                            if (typeMsg == 1)
                            {
                                ticket.LastReply = dateTimeNow;
                                Ticket = _ticketBusinessService.Edit(ticket);
                            }
                            else if (typeMsg == 2)
                            {
                                ticket.LastStatusDate = dateTimeNow;
                                ticket.LastReply = dateTimeNow;
                                ticket.NextCommenter = ticket.Responder;
                                ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));
                                Ticket = _ticketBusinessService.Edit(ticket);
                            }

                        }
                        else
                        {
                            apiJsonCommentSenderTr.type = "Participant";
                            ticket.LastReply = dateTimeNow;
                            Ticket = _ticketBusinessService.Edit(ticket);
                        }
                    }

                    List<ApiJsonCommentImageTR> listAttachment = new List<ApiJsonCommentImageTR>();

                    var attachments = _discussionAttachmentBusinessService.GetByDiscussionId(ticketDiscussionId);
                    string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Discussion/");

                    if (typeMsg == 2)
                    {
                        attachments = _noteAttachmentBusinessService.GetByNoteId(ticketNoteId);
                        attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/Note/");
                    }

                    if (attachments != null)
                        foreach (var attachment in attachments)
                        {
                            listAttachment.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/" + attachment.Name,
                            Type = Path.GetExtension(attachment.Name),
                                nama = attachment.Name
                            });
                        }

                    ApiJsonCommentTR apiJsonCommentTr = new ApiJsonCommentTR()
                    {
                        date = apiJsonCommentDateTr,
                        sender = apiJsonCommentSenderTr,
                        text = msg,
                        image = listAttachment
                    };

                    switch (typeMsg)
                    {
                        case 1:
                            apiJsonCommentTr.type = "sent";
                            break;
                        default:
                            apiJsonCommentTr.type = "notes";
                            break;
                    }

                    apiJsonCommentTrList.Add(apiJsonCommentTr);

                    ApiJsonStatus apiJsonStatus = new ApiJsonStatus()
                    {
                        code = 200,
                        message = "ok"
                    };
                    if (typeMsg != 1)
                    {
                        if (ticket.TicketCategoryId != 9)
                        {
                            Email.GetEmailTagTsicsCommentTR(ticket);
                            Email.GetEmailTagTsicsCommentTR(ticket, true);
                        }
                        else
                        {
                            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId, false);
                            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId, true);
                        }
                    }
                    return Json(new { status = apiJsonStatus, data = apiJsonCommentTrList });
                }
                else
                {
                    _webStatus.code = 403;
                    _webStatus.description = "Invalid AccessToken";

                    return Ok(new { status = _webStatus, result = new object() });
                }
            }
            return BadRequest(ModelState);
        }
        private void SendNotif(Ticket ticket,String title, String Content)
        {
            if (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("Participants")))
            {
                string[] submitedParticipants = HttpContext.Current.Request.Params.Get("Participants").Split(',');
                var dataud = new List<String>();
                for (int i = 0, iMax = submitedParticipants.Length; i < iMax; i++)
                {
                    dataud.Add(_userBusinessService.GetByXupj(submitedParticipants[i]).PlayerId);
                }

                if (ticket.Status == 2)
                {
                    dataud.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                    dataud.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);
                    Onesignal.PushNotif(Content, dataud, title, ticket.TicketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                }
            }
        }
    }
}