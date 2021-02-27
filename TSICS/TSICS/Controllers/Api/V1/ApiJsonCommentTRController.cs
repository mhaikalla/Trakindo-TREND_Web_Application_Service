using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System.Web.Mvc;
using TSICS.Helper;
using System.Linq;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers.Api.V1
{
    public class ApiJsonCommentTrController : ApiController
    {
        private readonly TicketDiscussionBusinessService _ticketDiscussionBusinessService = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _ticketNoteBusinessService = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly DiscussionAttachmentBusinessService _discussionAttachmentBusinessService = Factory.Create<DiscussionAttachmentBusinessService>("DiscussionAttachment", ClassType.clsTypeBusinessService);
        private readonly NoteAttachmentBusinessService _noteAttachmentBusinessService = Factory.Create<NoteAttachmentBusinessService>("NoteAttachment", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);

        [ResponseType(typeof(ApiJsonCommentTR))]
        public IHttpActionResult GetApiJsonCommentTr(int id, int uid, Boolean isBackend = false)
        {
            var user = _userBusinessService.GetDetail(uid);

            List<TicketDiscussion> ticketDiscussions = _ticketDiscussionBusinessService.GetListByTicket(id);

            List<CustomTicketDiscussion> customTicketDiscussions = new List<CustomTicketDiscussion>();

            List<TicketNote> ticketNotes = _ticketNoteBusinessService.GetListByTicket(id);

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

            Ticket ticket = _ticketBs.GetDetail(id);

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
                            if (isBackend == false)
                            {
                                if (ticket.Responder == comment.UserId)
                                {
                                    apiJsonCommentSenderTr1.type = "Responder";
                                }
                                else
                                {
                                    apiJsonCommentSenderTr1.type = "Participant";
                                    if (ticket.Submiter == comment.UserId)
                                        apiJsonCommentSenderTr1.type = "Submitter";

                                }
                            }
                            else
                            {
                                apiJsonCommentSenderTr1.type = ticket.Submiter == comment.UserId ? "Submitter" : "Responder";
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
                    if (isBackend == false)
                    {
                        if (ticket.TicketCategoryId != 9)
                        {
                            if (ticket.Responder == ticketNote.UserId)
                            {
                                apiJsonCommentSenderTr.type = "Responder";
                            }
                            else
                            {
                                apiJsonCommentSenderTr.type = "Participant";

                                if (ticket.Submiter == ticketNote.UserId)
                                    apiJsonCommentSenderTr.type = "Submitter";
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
                    }
                    else if (isBackend)
                    {
                        apiJsonCommentSenderTr.type = ticket.Submiter == ticketNote.UserId ? "Submitter" : "Responder";
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
                        if (isBackend == false)
                        {
                            if (ticket.Responder == ticketDiscussion.UserId)
                            {
                                apiJsonCommentSenderTr1.type = "Responder";
                            }
                            else
                            {
                                apiJsonCommentSenderTr1.type = "Participant";
                                if (ticket.Submiter == ticketDiscussion.UserId)
                                    apiJsonCommentSenderTr1.type = "Submitter";
                            }
                        }
                        else if (isBackend)
                        {
                            apiJsonCommentSenderTr1.type =
                                ticket.Submiter == ticketDiscussion.UserId ? "Submitter" : "Responder";
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
        // POST: api/ApiJsonCommentTR
        [ResponseType(typeof(ApiJsonCommentTR))]
        public IHttpActionResult PostApiJsonCommentTr(FormCollection formCollection)
        {
            string msg = formCollection["comment-text"];
            int typeMsg = Convert.ToInt32(formCollection["chat-type"]);
            int ticketId = Convert.ToInt32(formCollection["TicketId"]);
            int uid = Convert.ToInt32(formCollection["userid"]);

            Ticket ticket = _ticketBs.GetDetail(ticketId);
            if (typeMsg == 1)
            {
                TicketDiscussion discussion = Factory.Create<TicketDiscussion>("TicketDiscussion", ClassType.clsTypeDataModel);
                discussion.TicketId = ticketId;
                discussion.CreatedAt = DateTime.Now;
                discussion.UserId = uid;
                discussion.Description = msg;
                discussion.Status = 1;

                _ticketDiscussionBusinessService.Add(discussion);
            }
            else if (typeMsg == 2)
            {
                TicketNote note = Factory.Create<TicketNote>("TicketNote", ClassType.clsTypeDataModel);
                note.TicketId = ticketId;
                note.CreatedAt = DateTime.Now;
                note.UserId = uid;
                note.Description = msg;
                note.Status = 1;

                _ticketNoteBusinessService.Add(note);
            }

            List<ApiJsonCommentTR> apiJsonCommentTrml = new List<ApiJsonCommentTR>();
            ApiJsonCommentTR apiJsonCommentTrm = Factory.Create<ApiJsonCommentTR>("ApiJsonCommentTR", ClassType.clsTypeDataModel);
            apiJsonCommentTrm.type = "sent";

            //ApiJsonCommentTRM.date.day = item.CreatedAt?.ToString("dd MMM yyyy");
            //ApiJsonCommentTRM.date.time = item.CreatedAt?.ToString("hh:mm");
            //ApiJsonCommentTRM.sender.name = item.UserId.ToString();
            apiJsonCommentTrm.text = msg;
            apiJsonCommentTrm.image = null;
            apiJsonCommentTrm.isRemovable = (ticket.Responder == uid);

            apiJsonCommentTrml.Add(apiJsonCommentTrm);
            ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);


            apiJsonStatusM.code = 200;
            apiJsonStatusM.message = "ok";

            return Ok(new { status = apiJsonStatusM, data = apiJsonCommentTrml });

            //if (Request.Files.Count > 0)
            //{
            //    TicketAttachmentBusinessService ticketAttachmentBS = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
            //    TicketAttachment ticketAttachment = Factory.Create<TicketAttachment>("TicketAttachment", ClassType.clsTypeDataModel);

            //    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
            //    {
            //        var file = Request.Files[i];
            //        string response = Common.ValidateFileUpload(file);

            //        if (response.Equals("true"))
            //        {
            //            ticketAttachment.TicketId = TicketId;
            //            ticketAttachment.Name = Common.UploadFile(file, ticketAddResult.TicketNo + "-" + i.ToString());
            //            ticketAttachment.Status = 1;

            //            ticketAttachmentBS.Add(ticketAttachment);
            //        }
            //        else
            //        {
            //            Console.WriteLine(GetType().FullName + "." + MethodInfo.GetCurrentMethod().Name, response);
            //        }
            //    }
            //}
            
        }

        [ResponseType(typeof(ApiJsonCommentTR))]
        public IHttpActionResult PostApiJsonCommentTr(int id, int uid, TicketDiscussionRemoveChat removeChat)
        {
            //int removeChat = Convert.ToInt32(formCollection["removeChat"]);
            
            if (_ticketDiscussionBusinessService.DeleteDiscussion(_ticketDiscussionBusinessService.GetDetail(removeChat.removeChat)).Status == 0 )
            {
                List<TicketDiscussion> ticketDiscussions = _ticketDiscussionBusinessService.GetListByTicket(id);

                List<TicketNote> ticketNotes = _ticketNoteBusinessService.GetListByTicket(id);

                ApiJsonStatus apiJsonStatusM = Factory.Create<ApiJsonStatus>("ApiJsonStatus", ClassType.clsTypeDataModel);

                List<ApiJsonCommentTR> apiJsonCommentTrml = new List<ApiJsonCommentTR>();

                if (ticketDiscussions == null && ticketNotes == null)
                {
                    return NotFound();
                }

                Ticket ticket = _ticketBs.GetDetail(id);
                List<TicketParcipant> ticketParticipants = _ticketParcipantBusinessService.GetByTicket(id);

                var ticketDiscussionGroupedByNote = _ticketDiscussionBusinessService.GetGroupedByNote(id)
                    .GroupBy(q => q.TicketNoteId)
                    .OrderBy(q => q.FirstOrDefault().CreatedAt)
                    .ToList();

                foreach (var items in ticketDiscussionGroupedByNote)
                {
                    List<ApiJsonCommentTR> noteChild = new List<ApiJsonCommentTR>();

                    foreach (TicketDiscussion ticketDiscussion in items)
                    {
                        ApiJsonCommentDateTR apiJsonCommentDateTr1 = new ApiJsonCommentDateTR()
                        {
                            day = ticketDiscussion.CreatedAt?.ToString("dd MMM yyyy"),
                            time = ticketDiscussion.CreatedAt?.ToString("hh:mm")
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
                        else
                        {
                            apiJsonCommentSenderTr1.type = "Admin";

                            if (isInParticipants(ticketParticipants, ticketDiscussion.UserId))
                                apiJsonCommentSenderTr1.type = "Participant";

                            if (ticket.Submiter == ticketDiscussion.UserId)
                                apiJsonCommentSenderTr1.type = "Submitter";
                        }

                        var attachments1 = _discussionAttachmentBusinessService.GetByDiscussionId(ticketDiscussion.TicketDiscussionId);
                        List<ApiJsonCommentImageTR> listAttachment1 = new List<ApiJsonCommentImageTR>();
                        string attachmentsPath1 = "/Upload/Discussion/";
                        if (attachments1 != null)
                        {
                            foreach (var attachment in attachments1)
                            {
                                listAttachment1.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + attachmentsPath1 + attachment.Name,
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
                            type = ticketDiscussion.UserId == uid ? "sent" : "received"
                        };


                        if (ticketDiscussion.TicketNoteId.Equals(0))
                        {
                            apiJsonCommentTrml.Add(apiJsonCommentTrm1);
                        }
                        else
                        {
                            noteChild.Add(apiJsonCommentTrm1);
                        }
                    }

                    var ticketNote = _ticketNoteBusinessService.GetDetail(items.Key);
                    if (ticketNote != null)
                    {
                        ApiJsonCommentDateTR apiJsonCommentDateTr = new ApiJsonCommentDateTR()
                        {
                            day = ticketNote.CreatedAt?.ToString("dd MMM yyyy"),
                            time = ticketNote.CreatedAt?.ToString("hh:mm")
                        };

                        ApiJsonCommentSenderTR apiJsonCommentSenderTr = new ApiJsonCommentSenderTR()
                        {
                            name = _userBusinessService.GetDetail(ticketNote.UserId).Name,
                            is_verified = true
                        };

                        if (ticket.Responder == ticketNote.UserId)
                        {
                            apiJsonCommentSenderTr.type = "Responder";
                        }
                        else
                        {
                            apiJsonCommentSenderTr.type = "Admin";


                            if (isInParticipants(ticketParticipants, ticketNote.UserId))
                                apiJsonCommentSenderTr.type = "Participant";

                            if (ticket.Submiter == ticketNote.UserId)
                                apiJsonCommentSenderTr.type = "Submitter";
                        }

                        var attachments = _noteAttachmentBusinessService.GetByNoteId(ticketNote.TicketNoteId);
                        List<ApiJsonCommentImageTR> listAttachment = new List<ApiJsonCommentImageTR>();
                        string attachmentsPath = "/Upload/Note/";
                        if (attachments != null)
                        {
                            foreach (var attachment in attachments)
                            {
                                listAttachment.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + attachmentsPath  +  attachment.Name,
                                    nama = attachment.Name
                                });
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

                apiJsonStatusM.code = 200;
                apiJsonStatusM.message = "Success Remove";
                return Ok(new { status = apiJsonStatusM, data = apiJsonCommentTrml });
            }

            return Json(new { status = new {code = 500, message = "Failed Remove"} });
        }

        private bool isInParticipants(List<TicketParcipant> participantList, int userIdToCheck)
        {
            if (participantList.Count > 0)
            {
                List<int> userIdList = new List<int>();
                foreach(TicketParcipant participant in participantList)
                {
                    userIdList.Add(participant.UserId);
                }
                int[] userIdArray = userIdList.ToArray();

                if (userIdArray.Contains(userIdToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}