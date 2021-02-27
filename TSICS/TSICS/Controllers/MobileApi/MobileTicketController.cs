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
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using TSICS.Helper;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TSICS.Controllers.MobileApi
{
    public class MobileTicketController : ApiController
    {
        #region Business Services
        private readonly WebStatus _webStatus = Factory.Create<WebStatus>("WebStatus", ClassType.clsTypeDataModel);

        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly TicketCategoryBusinessService _ticketCategoryBusinessService = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        private readonly ArticleTagBusinessService _articleTagBusinessService = Factory.Create<ArticleTagBusinessService>("ArticleTag", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly MepBusinessService _mepBusinessService = Factory.Create<MepBusinessService>("Mep", ClassType.clsTypeBusinessService);
        private readonly TicketAttachmentBusinessService _ticketAttachmentBusinessService = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        private readonly TicketResolutionBusinessService _ticketResolutionBusinessService = Factory.Create<TicketResolutionBusinessService>("TicketResolution", ClassType.clsTypeBusinessService);
        private readonly RatingBusinessService _ratingBusinessService = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        private readonly TicketDiscussionBusinessService _ticketDiscussionBusinessService = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _ticketNoteBusinessService = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly DiscussionAttachmentBusinessService _discussionAttachmentBusinessService = Factory.Create<DiscussionAttachmentBusinessService>("DiscussionAttachment", ClassType.clsTypeBusinessService);
        private readonly NoteAttachmentBusinessService _noteAttachmentBusinessService = Factory.Create<NoteAttachmentBusinessService>("NoteAttachment", ClassType.clsTypeBusinessService);
        private readonly MasterAreaBusinessService _masterAreaBusinessService = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
       private readonly MasterBranchBusinessService _masterBranchBusinessService = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);

      

        #endregion

        #region Get Request

        [HttpGet]
        [Route("api/mobile/ticketproperties")]
        public IHttpActionResult GetTicketProperties()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                TicketAPI ticketApi = new TicketAPI {InvoiceDate = DateTime.Now, CreatedAt = DateTime.Now};
                
                List<TicketAttachmentsAPI> attachments = new List<TicketAttachmentsAPI>();
                TicketAttachmentsAPI attachment = new TicketAttachmentsAPI
                {
                    Name = ""
                };
                attachments.Add(attachment);
                ticketApi.Attachments = attachments;

                List<TicketTagsAPI> tags = new List<TicketTagsAPI>();
                TicketTagsAPI tag = new TicketTagsAPI
                {
                    Name = ""
                };
                tags.Add(tag);
                ticketApi.Tags = tags;

                return Ok(ticketApi);
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpGet]
        [Route("api/mobile/ticketnumber")]
        public IHttpActionResult GetTicketNumber(int categoryId)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                return Ok(_ticketBusinessService.GetNewTicketNoByCategory(categoryId));
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpPost]
        [Route("api/mobile/mytickets")]
        public IHttpActionResult GetMyTickets(MobilePagination pageData)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from UserDevices where PlayerId = '' or PlayerId = null");
            }
            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                var listTicket = _ticketBusinessService.GetBySubmitter(user.UserId, pageData.PerPage, pageData.PageNum);
                List<TicketAPI> result = new List<TicketAPI>();
                foreach (var ticket in listTicket)
                {
                    result.Add(this.GenerateDetail(ticket.TicketId, user.UserId, false));
                }

                _webStatus.code = 200;
                _webStatus.description = "Success - " + listTicket.Count() + " Entries";
                return Ok(new { message = _webStatus, result = result.OrderByDescending(ticket => ticket.RecentDate) });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { message = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/tickets")]
        public IHttpActionResult GetTickets(MobilePagination pageData)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from UserDevices where PlayerId = '' or PlayerId = null");
            }
            if (ModelState.IsValid)
            {
                if (pageData.PerPage <= 0 && pageData.PageNum <= 0)
                    return BadRequest();

                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    var listTicket = _ticketBusinessService.GetList(pageData.PerPage, pageData.PageNum);
                    
                    List<TicketAPI> result = new List<TicketAPI>();
                    foreach (var ticket in listTicket)
                    {

                        result.Add(this.GenerateDetail(ticket.TicketId, user.UserId, false));
                    }

                    return Ok(new { message = _webStatus, result = result.OrderByDescending(ticket => ticket.RecentDate) });
                }
                else
                {
                    _webStatus.code = 403;
                    _webStatus.description = "Invalid AccessToken";

                    return Ok(_webStatus);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        [Route("api/mobile/ticketdetail")]
        public IHttpActionResult GetTicketDetail(int ticketId)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                var ticket = _ticketBusinessService.GetDetail(ticketId);

                if (ticket != null)
                {
                    TicketAPI ticketDetail = this.GenerateDetail(ticketId, user.UserId, true);

                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    return Ok(new { message = _webStatus, result = ticketDetail });
                }

                _webStatus.code = 404;
                _webStatus.description = "Ticket Not Found";

                return Ok(new { message = _webStatus, result = new Object() });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        public TicketAPI GenerateDetail(int ticketId, int userId, bool isCompleteInfo)
        {
            TicketAttachmentBusinessService ticketAttachmentBusinessService = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
            TicketAPI ticketDetail = new TicketAPI();
            var ticket = _ticketBusinessService.GetDetail(ticketId);
           
            if (!String.IsNullOrWhiteSpace(ticket.PartsDescription))
            {
                string step_1 = Regex.Replace(ticket.PartsDescription, @"<[^>]+>|&nbsp;", "").Trim();
                ticket.PartsDescription = Regex.Replace(step_1, @"\s{2,}", " ");
            }
            if (!String.IsNullOrWhiteSpace(ticket.Description))
            {
                string step1 = Regex.Replace(ticket.Description, @"<[^>]+>|&nbsp;", "").Trim();
                ticket.Description = Regex.Replace(step1, @"\s{2,}", " ");
            }

            if (isCompleteInfo)
            {
                var attachments = ticketAttachmentBusinessService.GetFullByTicketId(ticket.TicketId);
                List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
                List<TicketAttachmentsAPI> wholeAttachments = new List<TicketAttachmentsAPI>();
               
                string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };
                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                        {
                            listAttachment.Add(new TicketAttachmentsAPI
                            {
                                Id = attachment.TicketAttachmentId,
                                Name = attachment.Name,
                                Uri = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/" + attachment.Name,
                                Level = attachment.LevelUser,
                                Type = Path.GetExtension(attachment.Name)
                            });
                            wholeAttachments.Add(new TicketAttachmentsAPI
                            {
                                Name = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/" + attachment.Name,
                                Type = Path.GetExtension(attachment.Name)
                            });
                        }
                        else
                        {
                            listAttachment.Add(new TicketAttachmentsAPI
                            {
                                Id = attachment.TicketAttachmentId,
                                Name = attachment.Name,
                                Uri = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/" + attachment.Name,
                                Level = attachment.LevelUser,
                                Type = Path.GetExtension(attachment.Name)
                            });
                            wholeAttachments.Add(new TicketAttachmentsAPI
                            {
                                Name = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/" + attachment.Name,
                                Type = Path.GetExtension(attachment.Name)
                            });
                        }
                    }
                    ticketDetail.Attachments = listAttachment;
                }

                List<TicketTagsAPI> tagList = new List<TicketTagsAPI>();
                var tags = _articleTagBusinessService.GetTagsByTicket(ticketId);
                if (tags.Count != 0)
                {
                    foreach (ArticleTag tag in tags)
                    {
                        TicketTagsAPI tagObj = new TicketTagsAPI()
                        {
                            Name = tag.Name
                        };
                        tagList.Add(tagObj);
                    }
                    ticketDetail.Tags = tagList;
                }

                List<MobileUser> participantList = new List<MobileUser>();
                var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                if (participants.Count != 0)
                {
                    foreach (TicketParcipant participant in participants)
                    {
                        User userDetail = _userBusinessService.GetDetail(participant.UserId);
                        MobileUser mobileUser = new MobileUser()
                        {
                            UserId = participant.UserId,
                            UserName = userDetail.Username,
                            Name = userDetail.Name
                        };
                        participantList.Add(mobileUser);
                    }
                    ticketDetail.Participants = participantList;
                }
                if (ticket.Responder == 0)
                {
                    if(ticket.Status != 1)
                    {
                        if(ticket.TicketCategoryId == 9) {
                            User userData = _userBusinessService.GetDetail(ticket.Responder);
                            MobileUser mobileUserData = new MobileUser()
                            {
                                UserId = 0,
                                UserName = null,
                                Name = "TREND Admin"
                            };
                            ticketDetail.ResponderDetails = mobileUserData;
                        }
                        else
                        {
                            User userData = _userBusinessService.GetDetail(ticket.Responder);
                            MobileUser mobileUserData = new MobileUser()
                            {
                                UserId = 0,
                                UserName = null,
                                Name = null
                            };
                            ticketDetail.ResponderDetails = mobileUserData;
                        }
                       
                    }
                    else
                    {
                        MobileUser mobileUserData = new MobileUser()
                        {
                            UserId = 0,
                            UserName = null,
                            Name = null
                        };
                        ticketDetail.ResponderDetails = mobileUserData;
                    }
                }
                else
                {
                    User userData = _userBusinessService.GetDetail(ticket.Responder);
                    MobileUser mobileUserData = new MobileUser()
                    {
                        UserId = userData.UserId,
                        UserName = userData.Username,
                        Name = userData.Name
                    };
                    ticketDetail.ResponderDetails = mobileUserData;
                }
                
                 var ticketDiscussions = _ticketDiscussionBusinessService.GetListByTicket(ticketId);
                if (ticketDiscussions != null)
                {
                    foreach (TicketDiscussion comment in ticketDiscussions)
                    {
                        var commentAttachments = _discussionAttachmentBusinessService.GetByDiscussionId(comment.TicketDiscussionId);
                        if (commentAttachments != null)
                        {
                            HttpContext.Current.Server.MapPath("~/Upload/Discussion/");
                            foreach (TicketAttachmentsAPI commentAttachment in commentAttachments)
                            {
                                wholeAttachments.Add(new TicketAttachmentsAPI
                                {
                                    Name = WebConfigure.GetDomain() + "/Upload/Discussion/" + commentAttachment.Name,
                                    Type = Path.GetExtension(commentAttachment.Name)
                                });
                            }
                        }
                    }
                }

                var ticketNotes = _ticketNoteBusinessService.GetListByTicket(ticketId);
                if (ticketNotes != null)
                {
                    foreach (TicketNote note in ticketNotes)
                    {
                        var noteAttachments = _noteAttachmentBusinessService.GetByNoteId(note.TicketNoteId);
                        if (noteAttachments != null)
                        {
                            string noteAttachmentsPath = WebConfigure.GetDomain() + "Upload/Note/";
                            foreach (TicketAttachmentsAPI noteAttachment in noteAttachments)
                            {
                                TicketAttachmentsAPI attachmentEncoded = new TicketAttachmentsAPI()
                                {
                                    Name = noteAttachmentsPath + noteAttachment.Name
                                };
                                wholeAttachments.Add(attachmentEncoded);
                            }
                        }
                    }
                }

                ticketDetail.WholeAttachments = wholeAttachments;

                if (ticket.Status.Equals(6) || ticket.Status.Equals(3))
                {
                    TicketResolution ticketResolution = _ticketResolutionBusinessService.GetByTicket(ticketId);

                    DateTime createdAt = DateTime.Parse(ticketResolution.CreatedAt.ToString());
                    string commenterAs = null;

                    if (ticketResolution.UserId.Equals(ticket.Submiter))
                    {
                        commenterAs = "Submiter";
                    }
                    else if (ticketResolution.UserId.Equals(ticket.Responder))
                    {
                        commenterAs = "Responder";
                    }

                    var commenterName = _userBusinessService.GetDetail(ticketResolution.UserId).Name;

                    JsonResolution resolution = new JsonResolution()
                    {
                        Day = createdAt.ToString("dddd, dd MMMM yy"),
                        Time = createdAt.ToString("hh:mm tt"),
                        CommenterName = commenterName,
                        CommenterAs = commenterAs,
                        Description = ticketResolution.Description
                    };

                    ticketDetail.Resolution = resolution;
                }
            }

            ticketDetail.TicketId = ticket.TicketId;
            if (ticket.WarrantyTypeId == 1)
            {
                ticketDetail.WarrantyCategoryName = "Warranty CAT";

            }
            else if (ticket.WarrantyTypeId == 2)
            {
                ticketDetail.WarrantyCategoryName = "Warranty Non-CAT";
            }
            else if (ticket.WarrantyTypeId == 3)
            {
                ticketDetail.WarrantyCategoryName = "Service Letter";

            }
            else if (ticket.WarrantyTypeId == 4)
            {
                ticketDetail.WarrantyCategoryName = "Extended Warranty";
            }
            else
            {
                ticketDetail.WarrantyCategoryName = "-";
            }
            ticketDetail.TicketCategoryId = ticket.TicketCategoryId;
            ticketDetail.TicketNo = ticket.TicketNo;
            ticketDetail.Title = ticket.Title;
            ticketDetail.Description = ticket.Description;
            ticketDetail.Submiter = ticket.Submiter;
            ticketDetail.SubmiterName = _userBusinessService.GetDetail(ticket.Submiter).Name;
            ticketDetail.Responder = ticket.Responder;
           
            if (ticket.TicketCategoryId.Equals(9))
            {
                ticketDetail.ResponderName = "TREND Admin";
            }
            else
            {
                if (ticket.Responder == 0)
                {
                   ticketDetail.ResponderName = "Submiter has not chosen yet";
                }
                else
                {
                    ticketDetail.ResponderName = _userBusinessService.GetDetail(ticket.Responder) == null ? "-" : _userBusinessService.GetDetail(ticket.Responder).Name;
                }
            }

            ticketDetail.SubmiterFlag = ticket.SubmiterFlag;
            ticketDetail.ResponderFlag =ticket.ResponderFlag;
            ticketDetail.SerialNumber = String.IsNullOrWhiteSpace(ticket.SerialNumber) ? "-" : ticket.SerialNumber;
            ticketDetail.Customer = String.IsNullOrWhiteSpace(ticket.Customer) ? "-" : ticket.Customer;
            ticketDetail.Location = String.IsNullOrWhiteSpace(ticket.Location) ? "-" : ticket.Location ;
            ticketDetail.MepBranch = String.IsNullOrWhiteSpace(ticket.MepBranch) || ticket.MepBranch == "," || ticket.MepBranch== "-"? "-" : ticket.MepBranch;
            ticketDetail.Make = String.IsNullOrWhiteSpace(ticket.Make) ? "-" : ticket.Make;
            ticketDetail.DeliveryDate = ticket.DeliveryDate == null ? null : ticket.DeliveryDate ;
            ticketDetail.ArrangementNo = String.IsNullOrWhiteSpace(ticket.ArrangementNo) ? "-" : ticket.ArrangementNo;
            ticketDetail.Family = String.IsNullOrWhiteSpace(ticket.Family) ? "-" : ticket.Family;
            ticketDetail.Model = String.IsNullOrWhiteSpace(ticket.Model) ? "-" : ticket.Model;
            ticketDetail.SMU = String.IsNullOrWhiteSpace(ticket.SMU) ? "-" : ticket.SMU;
            ticketDetail.SMUDate = ticket.SMUDate == null ? null : ticket.SMUDate ;
           
            ticketDetail.PartCausingFailure = ticket.PartCausingFailure;
            ticketDetail.PartsDescription = ticket.PartsDescription;
            ticketDetail.EmailCC = ticket.EmailCC;
            ticketDetail.Manufacture = ticket.Manufacture;
            ticketDetail.PartsNumber = ticket.PartsNumber;
            ticketDetail.ServiceToolSN = ticket.ServiceToolSN;
            ticketDetail.EngineSN = ticket.EngineSN;
            ticketDetail.EcmSN = ticket.EcmSN;
            ticketDetail.TotalTattletale = ticket.TotalTattletale;
            ticketDetail.ReasonCode = ticket.ReasonCode;
            ticketDetail.DiagnosticClock = ticket.DiagnosticClock;
            ticketDetail.Password = ticket.Password;
            ticketDetail.ServiceOrderNumber = ticket.ServiceOrderNumber;
            ticketDetail.ClaimNumber = ticket.ClaimNumber;
            ticketDetail.InvoiceDate = ticket.InvoiceDate;
            ticketDetail.UpdatedAt = ticket.UpdatedAt;
            ticketDetail.Status = ticket.Status;
            ticketDetail.WarrantyTypeId = ticket.WarrantyTypeId;
            ticketDetail.MasterAreaId = ticket.MasterAreaId;
            ticketDetail.MasterAreaName = ticket.MasterAreaName;
            ticketDetail.MasterBranchId = ticket.MasterBranchId;
            ticketDetail.MasterBranchName = ticket.MasterBranchName;
            ticketDetail.NextCommenter = ticket.NextCommenter;
            ticketDetail.DueDateAnswer = ticket.DueDateAnswer;
            ticketDetail.ReferenceTicket = ticket.ReferenceTicket;
            var ticketref = _ticketBusinessService.getTicketByTicketReferences(ticket.ReferenceTicket);
            ticketDetail.ReferenceTicketNo = ticketref == null ? null : ticketref.TicketNo;
            ticketDetail.DPPMno = ticket.DPPMno;
            ticketDetail.SoftwarePartNumber = ticket.SoftwarePartNumber;
            ticketDetail.IsEscalated = _ticketBusinessService.IsEscalated(ticket.TicketId, userId);
            ticketDetail.FromInterlock = ticket.FromInterlock;
            ticketDetail.ToInterlock = ticket.ToInterlock;
            ticketDetail.TestSpec = ticket.TestSpec;
            ticketDetail.TestSpecBrakeSaver= ticket.TestSpecBrakeSaver;
            var submiterRating = _ratingBusinessService.GetRatingFromResponder(ticketId, ticket.Responder);
            ticketDetail.SubmiterRating = submiterRating != null ? submiterRating.Rate : 0;
            ticketDetail.RatingDescSubmiter = submiterRating != null ? submiterRating.Description : null;
            var responderRating = _ratingBusinessService.GetRatingFromSubmiter(ticketId, ticket.Submiter);
            ticketDetail.ResponderRating = responderRating != null ? responderRating.Rate : 0;
            ticketDetail.RatingDescResponder = responderRating != null ? responderRating.Description : null;
            ticketDetail.CreatedAt = ticket.CreatedAt;
            ticketDetail.RecentDate = new[] { ticketDetail.CreatedAt, ticketDetail.UpdatedAt, ticket.LastStatusDate, ticket.LastReply }.Max();
            return ticketDetail;
        }

        #endregion

        [HttpPut]
        [Route("api/mobile/deleteticket")]
        public IHttpActionResult DeleteTicket(int ticketId)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                if (_ticketBusinessService.IsTicketExists(ticketId))
                {
                    var ticket = _ticketBusinessService.GetByIdAsNoTracking(ticketId);
                    ticket.Status = 5;
                    _ticketBusinessService.Edit(ticket);

                    _webStatus.code = 204;
                    _webStatus.description = "Success";

                    return Ok(_webStatus);
                }
                else
                {
                    _webStatus.code = 404;
                    _webStatus.description = "Ticket Doesn't Exists";

                    return Ok(_webStatus);

                }
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(_webStatus);
            }
        }

        [HttpPost]
        [Route("api/mobile/producthealth")]
        public IHttpActionResult PostProductHealth()
        {
            
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };
            string missingFields = MissingFields(requiredFields.ToArray());
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Product Health");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                   
                }
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
             
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/producthealth")]
        public IHttpActionResult PutProductHealth()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Product Health", user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;
                
                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
               
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/partstechnical")]
        public IHttpActionResult PostPartsTechnical()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "SMU",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Parts Technical");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }
              
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                  
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/partstechnical")]
        public IHttpActionResult PutPartsTechnical()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "SMU",
                "Description",
                "Responder",
                "IsDraft"
            };
          
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Parts Technical",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);
                
                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
             
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/dimension")]
        public IHttpActionResult PostDimension()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "PartsNumber",
                "PartsDescription",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Dimension");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }
                
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/dimension")]
        public IHttpActionResult PutDimension()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "PartsNumber",
                "PartsDescription",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Dimension",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);
                
                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/reference")]
        public IHttpActionResult PostReference()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Reference");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }

                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }

                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/reference")]
        public IHttpActionResult PutReference()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Reference",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }
        public void UpdateAttachment(int ticketId, string ticketNo)
        {
            _ticketAttachmentBusinessService.DeleteAllAttachmentInTicket(ticketId);
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                AddTechnicalRequestAttachments(ticketId, ticketNo);
            }
        }
        [HttpPost]
        [Route("api/mobile/telematics")]
        public IHttpActionResult PostTelematics()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Telematics");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }
               
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/telematics")]
        public IHttpActionResult PutTelematics()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Telematics",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/password")]
        public IHttpActionResult PostPassword()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Password");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }

                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/password")]
        public IHttpActionResult PutPassword()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Password",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/help")]
        public IHttpActionResult PostHelp()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string> {"Title", "Description", "IsDraft"};

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Help Desk");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }
                
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateHelpDesk(ticketAddResult);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/help")]
        public IHttpActionResult PutHelp()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "Description",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Help Desk",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }

                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/warranty")]
        public IHttpActionResult PostWarranty()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft",
                "WarrantyTypeId",
                "PartCausingFailure"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Warranty Assistance");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Params.AllKeys.Contains("PartsDescription"))
                {
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                }
                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    _ticketAttachmentBusinessService.DeleteAllTagsInTicket(ticketId);
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/warranty")]
        public IHttpActionResult PutWarranty()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft",
                "WarrantyTypeId",
                "PartCausingFailure"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Warranty Assistance",user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/goodwill")]
        public IHttpActionResult PostGoodwill()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "ServiceOrderNumber",
                "ClaimNumber",
                "SerialNumber",
                "SMU",
                "PartCausingFailure",
                "PartsDescription",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (HttpContext.Current.Request.Params.Get("IsDraft") == "false")
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Goodwill Assistance");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                }
               
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                        
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/goodwill")]
        public IHttpActionResult PutGoodwill()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "ServiceOrderNumber",
                "ClaimNumber",
                "SerialNumber",
                "SMU",
                "PartCausingFailure",
                "PartsDescription",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Goodwill Assistance", user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);
                               
                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                      
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPost]
        [Route("api/mobile/resolution")]
        public IHttpActionResult PostResolution(MobileResolutionPostAPI data)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    DateTime dateTimeNow = DateTime.Now;
                    int ticketId = data.TicketId;
                    int userId = data.UserId;
                    string description = data.Description;
                    int rate = data.Rate;
                    string rateDescription = data.RateDescription;
                    int userToRate = data.UserToRate;
                    Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
                    TimeSpan respond = ticket.LastReply == null ? (DateTime.Now.Subtract(ticket.CreatedAt.Value)) : (DateTime.Now.Subtract(ticket.LastReply.Value));
                    ticket.LastReply = dateTimeNow;
                    ticket.LastStatusDate = dateTimeNow;
                    _ticketBusinessService.Edit(ticket);
                    TicketResolution ticketResolution = new TicketResolution()
                    {
                        TicketId = ticketId,
                        UserId = userId,
                        Description = description,
                        CreatedAt = DateTime.Now,
                        Status = 1,
                        RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    };
                    string msg = data.Description;
                  
                    User userData = _userBusinessService.GetDetail(userId);
                    var playerid = new List<string>();
                    var dataUid = new List<int>();
                    var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                    foreach (var i in participants)
                    {
                        dataUid.Add(i.UserId);
                    }
                    var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                    foreach (var p in iduserdevice)
                    {
                        playerid.Add(p.PlayerId);
                    }
                    String title = ticket.TicketNo + " - "+ ticket.Title + " - Write Resolution", 
                        content = userData.Name + " (Responder) : "+ msg;
                    playerid.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                    playerid.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);
                    Onesignal.PushNotif(content, playerid, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                    _ticketResolutionBusinessService.Add(ticketResolution);
                    _ticketBusinessService.RequestToClose(ticketId);

                    Rating rating = new Rating()
                    {
                        UserId = userToRate,
                        TicketId = ticketId,
                        Rate = rate,
                        Description = rateDescription,
                        RatedFrom = userId,
                        CreatedAt = DateTime.Now,
                        RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    };

                    _ratingBusinessService.Add(rating);

                    if (ticket.TicketCategoryId != 9)
                    {
                        Email.GetEmailTagTsicsCommentTR(ticket,false);
                        Email.GetEmailTagTsicsCommentTR(ticket, true);
                    }
                    else
                    {
                        Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId,false);
                        Email.GetEmailTagTsicsCommentHelpDesk(ticketId, user.UserId,true);
                    }
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    return Ok(new { status = _webStatus, result = data });
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
        [Route("api/mobile/dppm")]
        public IHttpActionResult PostDppm(MobileDPPM data)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    int ticketId = data.TicketId;
                    string dppmNo = data.DPPMno;

                    _ticketBusinessService.AddDppm(ticketId, dppmNo);

                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    return Ok(_webStatus);
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
        [Route("api/mobile/conditionmonitoring")]
        public IHttpActionResult PostCondition()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingCreateTechnicalRequest(user, "Condition Monitoring");
                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;
                string ticketNo = ticketAddResult.TicketNo;

                if (HttpContext.Current.Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketNo);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    AddTechnicalRequestTags(ticketId);
                }

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
                {
                    AddParticipant(ticketId);
                   
                }
                
                if (ticketAddResult.Status == 2 || ticketAddResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    SendNotif(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult, false);
                }
                  
                _webStatus.code = 201;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketAddResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        [HttpPut]
        [Route("api/mobile/conditionmonitoring")]
        public IHttpActionResult PutCondition()
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            List<string> requiredFields = new List<string>
            {
                "TicketId",
                "Title",
                "SerialNumber",
                "Description",
                "Responder",
                "IsDraft"
            };

            if (HttpContext.Current.Request.Files.Count > 0)
            {
                requiredFields.Add("AttachmentsLevel");
            }

            string missingFields = MissingFields(requiredFields.ToArray());
            if (missingFields != null)
            {
                if (String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("IsDraft")))
                {
                    return BadRequest("Missing arguments : " + missingFields);
                }
            }

            var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

            if (user != null)
            {
                Ticket ticket = PlottingUpdateTechnicalRequest("Condition Monitoring", user);
                Ticket ticketEditResult = _ticketBusinessService.Edit(ticket);

                int ticketId = ticketEditResult.TicketId;
                string ticketNo = ticketEditResult.TicketNo;

                UpdateAttachment(ticketId, ticketNo);
                UpdateParticipant(ticketId);
                UpdateTechnicalRequestTags(ticketId);

                if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
                {
                    _ticketAttachmentBusinessService.DeleteAllTagsInTicket(ticketId);
                    AddTechnicalRequestTags(ticketId);
                }
                if (ticketEditResult.Status == 2 || ticketEditResult.Status == 4)
                {
                    var playerid = new List<string>();
                    List<UserDevices> playerId = _ticketParcipantBusinessService.getuserdevicesresponder(ticket.Responder);
                    string playeridLog = "";
                    foreach (var item in playerId)
                    {
                        playeridLog += item.PlayerId + ",";
                        playerid.Add(item.PlayerId);
                    }
                    //_logErrorBService.WriteLog("Send Notif Player Id", MethodBase.GetCurrentMethod().Name, playeridLog);
                    SendNotif(ticketEditResult);
                    Email.SendMailCreateTr(ticketEditResult, true);
                    Email.SendMailCreateTr(ticketEditResult, false);
                }
                _webStatus.code = 200;
                _webStatus.description = "Success";
                return Ok(new { status = _webStatus, result = ticketEditResult });
            }
            else
            {
                _webStatus.code = 403;
                _webStatus.description = "Invalid AccessToken";

                return Ok(new { status = _webStatus, result = new object() });
            }
        }

        private string MissingFields(string[] requiredFields)
        {
            List<string> missingFields = new List<string>();
            for (int i = 0, iMax = requiredFields.Length; i < iMax; i++)
            {
                if (!HttpContext.Current.Request.Params.AllKeys.Contains(requiredFields[i]))
                {
                    missingFields.Add(requiredFields[i]);
                }
            }

            if (missingFields.Count > 0)
            {
                return String.Join(", ", missingFields.ToArray());
            }

            return null;
        }

        private void AddTechnicalRequestAttachments(int ticketId, string ticketNo)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                for (int i = 0, iLen = HttpContext.Current.Request.Files.Count; i < iLen; i++)
                {
                    string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                    var postedFile = HttpContext.Current.Request.Files[i];
                    string[] fileLevel = HttpContext.Current.Request.Params.Get("AttachmentsLevel").Split(',');
                    var fileName = Path.GetFileNameWithoutExtension(postedFile.FileName) + "-" + ticketNo + "-" + dateString + "-" + i + Path.GetExtension(postedFile.FileName);
                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/"), fileName);
                    postedFile.SaveAs(path);
                    TicketAttachment ticketAttachment = new TicketAttachment()
                    {
                        TicketId = ticketId,
                        Name = fileName,
                        LevelUser = fileLevel[i],
                        Status = 1
                    };

                    _ticketAttachmentBusinessService.Add(ticketAttachment);
                }
            }
        }
        private void AddParticipant(int ticketId)
        {
            if (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("Participants"))) {
                string[] participants = HttpContext.Current.Request.Params.Get("Participants").Split(',');
                for (int i = 0, iMax = participants.Length; i < iMax; i++)
                {
                    var participant = _userBusinessService.GetByXupj(participants[i]) == null ? null : _userBusinessService.GetByXupj(participants[i]);
                    if (participant != null)
                    {
                        TicketParcipant ticketParticipant = new TicketParcipant()
                        {
                            TicketId = ticketId,
                            UserId = participant.UserId,
                            CreatedAt = DateTime.Now,
                            Status = 1
                        };
                        _ticketParcipantBusinessService.Add(ticketParticipant);
                    }
                }
            }
        }

        private void UpdateParticipant(int ticketId)
        {
            _ticketParcipantBusinessService.DeleteAllParticipant(ticketId);
            if (HttpContext.Current.Request.Params.AllKeys.Contains("Participants"))
            {
                var currentParticipants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                if (currentParticipants.Count > 0)
                {
                    _ticketParcipantBusinessService.BulkDeleteByTicket(ticketId);
                }
                AddParticipant(ticketId);
            }
        }
        private void AddTechnicalRequestTags(int ticketId)
        {
            string[] tags = HttpContext.Current.Request.Params.Get("Tags").Split(',');
            for (int i = 0, iMax = tags.Length; i < iMax; i++)
            {
                ArticleTag articleTag = new ArticleTag()
                {
                    Name = tags[i],
                    Freq = 1,
                    TicketId = ticketId
                };

                _articleTagBusinessService.Add(articleTag);
            }
        }

        private void UpdateTechnicalRequestTags(int ticketId)
        {
            _articleTagBusinessService.DeleteAllTagsInTicket(ticketId);
            if (HttpContext.Current.Request.Params.AllKeys.Contains("Tags"))
            {
                AddTechnicalRequestTags(ticketId);
            }
        }

        private Ticket PlottingCreateTechnicalRequest(User user, string categoryName)
        {
            Ticket ticket = Factory.Create<Ticket>("Ticket", ClassType.clsTypeDataModel);
            MEP unit = new MEP();
            if (HttpContext.Current.Request.Params.AllKeys.Contains("SerialNumber") && HttpContext.Current.Request.Params.Get("SerialNumber") != null && HttpContext.Current.Request.Params.Get("SerialNumber") != "")
            {
                string serialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                var mep = _mepBusinessService.GetBySn(serialNumber);
                unit = mep ?? new MEP();
            }
            string isDraft = HttpContext.Current.Request.Params.Get("IsDraft");

            ticket.Status = isDraft == "true" ? 1 : 2;

            if (HttpContext.Current.Request.Params.AllKeys.Contains("Title"))
            {
                ticket.Title =String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("Title")) ? null : HttpContext.Current.Request.Params.Get("Title");
            }
            ticket.CreatedAt = DateTime.Now;
            ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));
            ticket.DelegateId = 0;
            ticket.MasterAreaId = user.MasterAreaId;
            var masterArea = _masterAreaBusinessService.GetDetail(user.MasterAreaId);
            if (masterArea != null)
            {
                ticket.MasterAreaName = masterArea.Name;
            }
            ticket.MasterBranchId = user.MasterBranchId;
            var masterBranch = _masterBranchBusinessService.GetDetail(user.MasterBranchId);
            if (masterBranch != null)
            {
                ticket.MasterBranchName = masterBranch.Name;
            }
            if (HttpContext.Current.Request.Params.AllKeys.Contains("ticketRefence"))
            {
                ticket.ReferenceTicket = _ticketBusinessService.GetDetailbyTicketNo(HttpContext.Current.Request.Params.Get("ticketRefence")).TicketId;
            }
            ticket.Submiter = user.UserId;

            if (HttpContext.Current.Request.Params.AllKeys.Contains("Responder"))
            {
                var responder = HttpContext.Current.Request.Params.Get("Responder");
                if (categoryName != "Help Desk")
                {
                    if (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("Responder")))
                    {

                        ticket.Responder = _userBusinessService.GetByXupj(HttpContext.Current.Request.Params.Get("Responder")).UserId;
                        ticket.NextCommenter = ticket.Responder;
                    }
                    else
                    {
                        ticket.Responder = 0;
                        ticket.NextCommenter = ticket.Responder;
                    }
                }
                else
                {
                    ticket.Responder = _userBusinessService.GetDetailbyUsername("TECHNICAL.C.ADMIN1").UserId;
                }
            }            
            switch (categoryName)
            {
                case "Product Health":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Product Health").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartCausingFailure"))
                    {
                        ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartsDescription"))
                    {
                        ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    }
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                   
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Parts Technical":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Parts Technical").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartsNumber"))
                    {
                        ticket.PartsNumber = HttpContext.Current.Request.Params.Get("PartsNumber");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartsDescription"))
                    {
                        ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    }
                  
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Dimension":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Dimension").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    ticket.PartsNumber = HttpContext.Current.Request.Params.Get("PartsNumber");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    ticket.NextCommenter = ticket.Responder;
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    break;
                case "Reference":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Reference").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = unit.SMU;
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Warranty Assistance":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Warranty Assistance").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    ticket.WarrantyTypeId = int.Parse(HttpContext.Current.Request.Params.Get("WarrantyTypeId"));
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("ServiceOrderNumber"))
                    {
                        ticket.ServiceOrderNumber = HttpContext.Current.Request.Params.Get("ServiceOrderNumber");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("ClaimNumber"))
                    {
                        ticket.ClaimNumber = HttpContext.Current.Request.Params.Get("ClaimNumber");
                    }
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                   
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Goodwill Assistance":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Goodwill Assistance").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.ServiceOrderNumber = HttpContext.Current.Request.Params.Get("ServiceOrderNumber");
                    ticket.ClaimNumber = HttpContext.Current.Request.Params.Get("ClaimNumber");
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                 
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Password":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Password").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = unit.SMU;
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                   
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("ServiceToolSN"))
                    {
                        ticket.ServiceToolSN = HttpContext.Current.Request.Params.Get("ServiceToolSN");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EngineSN"))
                    {
                        ticket.EngineSN = HttpContext.Current.Request.Params.Get("EngineSN");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EcmSN"))
                    {
                        ticket.EcmSN = HttpContext.Current.Request.Params.Get("EcmSN");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("TotalTattletale"))
                    {
                        ticket.TotalTattletale = HttpContext.Current.Request.Params.Get("TotalTattletale");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("ReasonCode"))
                    {
                        ticket.ReasonCode = HttpContext.Current.Request.Params.Get("ReasonCode");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("FromInterlock"))
                    {
                        ticket.FromInterlock = HttpContext.Current.Request.Params.Get("FromInterlock");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("ToInterlock"))
                    {
                        ticket.ToInterlock = HttpContext.Current.Request.Params.Get("ToInterlock");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SerialNuDiagnosticClockmber"))
                    {
                        ticket.DiagnosticClock = HttpContext.Current.Request.Params.Get("SerialNuDiagnosticClockmber");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SoftwarePartNumber"))
                    {
                        ticket.SoftwarePartNumber = HttpContext.Current.Request.Params.Get("SoftwarePartNumber");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("TestSpec"))
                    {
                        ticket.TestSpec = HttpContext.Current.Request.Params.Get("TestSpec");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("TestSpecBrakeSaver")) 
                    {
                        ticket.TestSpecBrakeSaver = HttpContext.Current.Request.Params.Get("TestSpecBrakeSaver");
                    }
                    break;
                case "Telematics":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Telematics").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + "-" + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartCausingFailure"))
                    {
                        ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartsDescription"))
                    {
                        ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    }
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                  
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Condition Monitoring":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Condition Monitoring").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("SerialNumber")) ?  null : HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + "-" + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("PartCausingFailure"))
                    {
                        ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    }
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                   
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Help Desk":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Help Desk").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    break;

            }

            return ticket;
        }

        private Ticket PlottingUpdateTechnicalRequest(string categoryName, User user)
        {
            DateTime Now = DateTime.Now;
            Ticket ticket = _ticketBusinessService.GetDetail(Convert.ToInt32(HttpContext.Current.Request.Params.Get("TicketId")));
            MEP unit = new MEP();
            if (HttpContext.Current.Request.Params.AllKeys.Contains("SerialNumber") && HttpContext.Current.Request.Params.Get("SerialNumber") != null && HttpContext.Current.Request.Params.Get("SerialNumber") != "")
            {
                string serialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                var mep = _mepBusinessService.GetBySn(serialNumber);
                unit = mep ?? new MEP();
            }

            string isDraft = HttpContext.Current.Request.Params.Get("IsDraft");
            ticket.Status = isDraft == "true" ? 1 : 2;
            if (HttpContext.Current.Request.Params.AllKeys.Contains("Responder"))
            {
                var responder = HttpContext.Current.Request.Params.Get("Responder");
                if (isDraft == "true")
                {
                    if (String.IsNullOrWhiteSpace(responder))
                    {
                        ticket.Responder = 0;
                    }
                    else
                    {
                        ticket.Responder = _userBusinessService.GetDetailbyUsername(responder).UserId;
                    }
                    
                }
                else
                {
                        if (ticket.Submiter == user.UserId)
                        {
                            if (String.IsNullOrWhiteSpace(responder))
                            {
                                ticket.Responder = 0;
                            }
                            else
                            {
                                ticket.Responder = _userBusinessService.GetDetailbyUsername(responder).UserId;
                            }
                        }
                        else if (ticket.Responder == user.UserId)
                        {
                            ticket.Responder = user.UserId;
                        }
                   
                    ticket.LastStatusDate = Now;
                    if (Convert.ToString(user.UserId).Equals(ticket.Submiter))
                    {
                        ticket.NextCommenter = ticket.Responder;
                    }

                    if (ticket.Responder != Convert.ToInt32(user.UserId))
                    {
                        ticket.DueDateAnswer = Now.AddDays(Common.NumberOfWorkDays(Now, WebConfigure.GetRulesDay()));
                        ticket.ResponderStatus = ticket.DueDateAnswer.Value.Subtract(Now).Ticks;
                        ticket.SubmiterStatus = 0;
                    }
                }
            }

            ticket.Title = HttpContext.Current.Request.Params.Get("Title");
            ticket.UpdatedAt = Now;
            ticket.DelegateId = 0;
            ticket.EscalateId = 0;
            switch (categoryName)
            {
                case "Product Health":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }

                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    break;
                case "Parts Technical":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                  
                    ticket.PartsNumber = HttpContext.Current.Request.Params.Get("PartsNumber");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    break;
                case "Dimension":
                    ticket.PartsNumber = HttpContext.Current.Request.Params.Get("PartsNumber");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }

                    break;
                case "Reference":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
            
                    break;
                case "Warranty Assistance":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    ticket.WarrantyTypeId = int.Parse(HttpContext.Current.Request.Params.Get("WarrantyTypeId"));
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                 
                    break;

                case "Help Desk":
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");

                    break;
                case "Goodwill Assistance":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                   
                    ticket.ServiceOrderNumber = HttpContext.Current.Request.Params.Get("ServiceOrderNumber");
                    ticket.ClaimNumber = HttpContext.Current.Request.Params.Get("ClaimNumber");
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");

                    break;
                case "Password":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    ticket.ServiceToolSN = HttpContext.Current.Request.Params.Get("ServiceToolSN");
                    ticket.EngineSN = HttpContext.Current.Request.Params.Get("EngineSN");
                    ticket.EcmSN = HttpContext.Current.Request.Params.Get("EcmSN");
                    ticket.TotalTattletale = HttpContext.Current.Request.Params.Get("TotalTattletale");
                    ticket.ReasonCode = HttpContext.Current.Request.Params.Get("ReasonCode");
                    ticket.SoftwarePartNumber = HttpContext.Current.Request.Params.Get("SoftwarePartNumber");
                    ticket.DiagnosticClock = HttpContext.Current.Request.Params.Get("SerialNuDiagnosticClockmber");
                    ticket.FromInterlock = HttpContext.Current.Request.Params.Get("FromInterlock");
                    ticket.ToInterlock = HttpContext.Current.Request.Params.Get("ToInterlock");
                    ticket.TestSpec = HttpContext.Current.Request.Params.Get("TestSpec");
                    ticket.TestSpecBrakeSaver = HttpContext.Current.Request.Params.Get("TestSpecBrakeSaver");
                    ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                  
                    break;
                case "Telematics":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
            
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    ticket.PartsDescription = HttpContext.Current.Request.Params.Get("PartsDescription");
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.SMUDate = unit.SMUUpDate;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
                case "Condition Monitoring":
                    ticket.SerialNumber = HttpContext.Current.Request.Params.Get("SerialNumber");
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("SMU"))
                    {
                        ticket.SMU = HttpContext.Current.Request.Params.Get("SMU");
                    }
                    
                    ticket.Description = HttpContext.Current.Request.Params.Get("Description");
                    ticket.PartCausingFailure = HttpContext.Current.Request.Params.Get("PartCausingFailure");
                    if (HttpContext.Current.Request.Params.AllKeys.Contains("EmailCC"))
                    {
                        ticket.EmailCC = HttpContext.Current.Request.Params.Get("EmailCC");
                    }
                    break;
            }

            return ticket;
        }

        private void SendNotif(Ticket ticket)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("delete from UserDevices where PlayerId = '' or PlayerId = null");
            }
                if (!String.IsNullOrWhiteSpace(HttpContext.Current.Request.Params.Get("Participants")))
                {
                    string[] submitedParticipants = HttpContext.Current.Request.Params.Get("Participants").Split(',');
                    var dataud = new List<String>();
                    var dataresponder = new List<String>();
                    for (int i = 0, iMax = submitedParticipants.Length; i < iMax; i++)
                    {
                        var userData = _userBusinessService.GetByXupj(submitedParticipants[i]);
                        if (userData != null)
                        {
                            dataud.Add(_userBusinessService.GetByXupj(submitedParticipants[i]).PlayerId);
                        }
                    }

                    if (ticket.Status == 2)
                    {

                        if (_userBusinessService.GetDetail(ticket.Responder).PlayerId != null)
                        {
                            dataresponder.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                            Onesignal.PushNotif(ticket.Description, dataresponder, ticket.Title + "-" + ticket.TicketNo, ticket.TicketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                        }
                        if (_userBusinessService.GetDetail(ticket.Submiter).PlayerId != null)
                        {
                            dataud.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);
                        }
                    }

                    Onesignal.PushNotif(ticket.Description, dataud, ticket.Title + " - " + ticket.TicketNo, ticket.TicketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                }
        }

        [HttpPost]
        [Route("api/mobile/rateresponder")]
        public IHttpActionResult PostRateResponder(MobileRateResponderAPI data)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    int ticketId = data.TicketId;
                    int userId = data.UserId;
                    int rate = data.Rate;
                    string rateDescription = data.RateDescription;
                    int userToRate = data.UserToRate;
                    User userData = _userBusinessService.GetDetail(userId);
                    User userResponder = _userBusinessService.GetDetail(userToRate);
                    TicketResolution resolution = _ticketResolutionBusinessService.GetByTicket(ticketId);
                    Ticket ticketData = _ticketBusinessService.GetDetail(ticketId);
                    _ticketBusinessService.Close(ticketId);

                    DateTime dateTimeNow = new DateTime();
                    TimeSpan respond = ticketData.LastReply == null ? (dateTimeNow.Subtract(ticketData.CreatedAt.Value))/*TotalSeconds*/ : (dateTimeNow.Subtract(ticketData.LastReply.Value))/*TotalSeconds*/;
                    Rating rating = new Rating()
                    {
                        UserId = userToRate,
                        TicketId = ticketId,
                        Rate = rate,
                        Description = rateDescription,
                        RatedFrom = userId,
                        CreatedAt = DateTime.Now,
                        RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    };

                    
                    ticketData.LastStatusDate = DateTime.Now;
                    ticketData.LastReply = ticketData.LastStatusDate;
                    ticketData = _ticketBusinessService.Edit(ticketData);
                    _ratingBusinessService.Add(rating);


                    var playerid = new List<string>();
                    var dataUid = new List<int>();
                    var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                    foreach (var i in participants)
                    {
                        dataUid.Add(i.UserId);
                    }
                    var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                    foreach (var p in iduserdevice)
                    {
                        playerid.Add(p.PlayerId);
                    }

                    string msg = resolution.Description;
                    String title = ticketData.TicketNo + " - " + ticketData.Title + " - Rate To Responder",
                        //content = userResponder.Name + " (Responder) : " + msg;
                        content = userData.Name + "(Submiter) Rated "+ rate +" stars";
                    playerid.Add(_userBusinessService.GetDetail(ticketData.Responder).PlayerId);
                    playerid.Add(_userBusinessService.GetDetail(ticketData.Submiter).PlayerId);
                    Onesignal.PushNotif(content, playerid, title, ticketId, ticketData.TicketNo, ticketData.TicketCategoryId, ticketData.Description);
                    
                    if (ticketData.TicketCategoryId == 9)
                    {
                        Email.SendMailCreateHelpDesk(ticketData);
                    }
                    else
                    {
                        Email.GetEmailTagTsicsCommentTR(ticketData, true);
                        Email.GetEmailTagTsicsCommentTR(ticketData, false);
                    }
                 
                    _webStatus.code = 200;
                    _webStatus.description = "Success";

                    return Ok(new { status = _webStatus, result = data });
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
        [Route("api/mobile/escalate")]
        public IHttpActionResult Escalate(MobileEscalate data)
        {
            if (!Common.GetAuthorization(Request))
                return Ok(new { Code = 401, Message = "Unauthorization" });

            if (ModelState.IsValid)
            {
                var user = _userBusinessService.GetByToken(Request.Headers.GetValues("AccessToken").First());

                if (user != null)
                {
                    int ticketId = data.TicketId;
                    User newResponder = _userBusinessService.GetByXupj(data.Responder);
                    int currentResponder = user.UserId;

                    Ticket ticketData = _ticketBusinessService.Escalate(ticketId, currentResponder, newResponder.UserId);

                    var playerid = new List<string>();
                    var dataUid = new List<int>();
                    var participants = _ticketParcipantBusinessService.GetByTicket(ticketId);
                    foreach (var i in participants)
                    {
                        dataUid.Add(i.UserId);
                    }
                    var iduserdevice = _ticketParcipantBusinessService.Getuserdeviceforasnote(dataUid);
                    foreach (var p in iduserdevice)
                    {
                        playerid.Add(p.PlayerId);
                    }
                    
                    String title = ticketData.TicketNo + " - " + ticketData.Title + " - Escalated",
                       
                        content = user.Name + " (Responder) Has Escalated To " + newResponder.Name;
                    playerid.Add(_userBusinessService.GetDetail(ticketData.Responder).PlayerId);
                    playerid.Add(_userBusinessService.GetDetail(ticketData.Submiter).PlayerId);
                    Onesignal.PushNotif(content, playerid, title, ticketId, ticketData.TicketNo, ticketData.TicketCategoryId, ticketData.Description);


                    _webStatus.code = 201;
                    _webStatus.description = "Success";

                    return Ok(new { status = _webStatus, result = this.GenerateDetail(ticketId, user.UserId, true) });
                }
                else
                {
                    _webStatus.code = 403;
                    _webStatus.description = "Invalid AccessToken";

                    return Ok(new { status = _webStatus, result = new object() });
                }
            }
            else
            {
                return (BadRequest(ModelState));
            }
        }

       
    }
}