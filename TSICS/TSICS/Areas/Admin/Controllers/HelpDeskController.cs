using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using PagedList;
using TSICS.Helper;
using System.IO;
namespace TSICS.Areas.Admin.Controllers
{
    public class HelpDeskController : Controller
    {
        
        private readonly  ArticleTagBusinessService _articleTagBusinessService = Factory.Create<ArticleTagBusinessService>("ArticleTag", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly TicketAttachmentBusinessService _ticketAttachmentBusinessService = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketCategoryBusinessService _ticketCategoryBusinessService = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        private readonly TicketPreviewBusinessService _ticketPreviewBusinessService = Factory.Create<TicketPreviewBusinessService>("TicketPreview", ClassType.clsTypeBusinessService);
        private readonly TicketResolutionBusinessService _ticketResolutionBusinessService = Factory.Create<TicketResolutionBusinessService>("TicketResolution", ClassType.clsTypeBusinessService);
        private readonly RatingBusinessService _ratingBusinessService = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _TicketNote = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly TicketDiscussionBusinessService _ticketDiscussionBs = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBusinessService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly EmployeeMasterBusinessService _EmployeeMasterBusinessService = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);

        // GET: Admin/HelpDesk
        public ActionResult Index(int? page, String currentFilter, string sortOrder = "" , String searchString = "")
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "DateDesc" : "";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "TitleDesc" : "Title";
            ViewBag.StatusSortParm = sortOrder == "Status" ? "StatusDesc" : "Status";

            var tickets = _ticketBusinessService.GetHelpDeskList();

            switch (sortOrder)
            {
                case "DateDesc":
                    tickets = tickets.OrderBy(s => s.CreatedAt);
                    break;
                case "Title":
                    tickets = tickets.OrderBy(t => t.Title);
                    break;
                case "TitleDesc":
                    tickets = tickets.OrderByDescending(t => t.Title);
                    break;
                case "Status":
                    tickets = tickets.OrderBy(t => t.Status);
                    break;
                case "StatusDesc":
                    tickets = tickets.OrderByDescending(t => t.Status);
                    break;
                default:
                    tickets = tickets.OrderByDescending(t => t.CreatedAt);
                    break;
            }

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            tickets = tickets.Where(ticket =>
                    ticket.Title.Contains(searchString) ||
                    ticket.TicketNo.Contains(searchString)
                    );

            var ticketPreview = _ticketPreviewBusinessService.MakeFrom(tickets, 0);

            List<InvolvedUser> submiters = new List<InvolvedUser>();
            List<User> responders = new List<User>();
            List<TicketResolution> ticketresolutions = new List<TicketResolution>();
            List<TicketPreview> ratings = new List<TicketPreview>();
            List<TicketPreview> ratingresponders = new List<TicketPreview>();
            List<User> userData = new List<User>();
            List<TicketPreview> userRoles = new List<TicketPreview>();
            List<TicketNote> SubmiterNote  = new List<TicketNote>();
            List<TicketNote> ResponderNote = new List<TicketNote>();
            if (ticketPreview != null)
            {
                foreach (var ticket in ticketPreview)
                {
                    User submiterData = _userBusinessService.GetDetail(ticket.Submiter);
                    TicketResolution resolutionData = _ticketResolutionBusinessService.GetByTicket(ticket.TicketId);
                    Rating ratingData = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);
                    TicketNote noteSubmiter = _TicketNote.GetDatebyTicketandUserId(ticket.TicketId, ticket.Submiter);
                    TicketDiscussion replySubmiter = _ticketDiscussionBs.GetDatebyTicketandUserId(ticket.TicketId, ticket.Submiter) == null ? null : _ticketDiscussionBs.GetDatebyTicketandUserId(ticket.TicketId, ticket.Submiter);
                   
                    TicketNote notesubmiter = new TicketNote()
                    {
                        TicketNoteId = ticket.TicketId,
                        RespondTime = noteSubmiter == null ? null: noteSubmiter.RespondTime
                    };
                    SubmiterNote.Add(notesubmiter);
                   
                    if (ratingData != null)
                    {
                        TicketPreview ratingdata = new TicketPreview()
                        {
                            TicketId = ticket.TicketId,
                            ClosedDate = ratingData.CreatedAt,
                            Rate = ratingData.Rate
                        };
                        ratings.Add(ratingdata);
                    }
                    else
                    {
                        TicketPreview ratingdata = new TicketPreview()
                        {
                            TicketId = ticket.TicketId,
                            ClosedDate = null,
                            Rate = 0
                        };
                        ratings.Add(ratingdata);
                    }
                    if (resolutionData != null)
                    {
                        TicketResolution resolutiondata = new TicketResolution()
                        {
                            TicketId = ticket.TicketId,
                            CreatedAt = resolutionData.CreatedAt
                        };
                        ticketresolutions.Add(resolutiondata);
                    }
                    else
                    {
                        TicketResolution resolutiondata = new TicketResolution()
                        {
                            TicketId = ticket.TicketId,
                            CreatedAt = null
                        };
                        ticketresolutions.Add(resolutiondata);
                    }
                    if (submiterData != null)
                    {
                        if (submiterData.RoleId == 0) {
                            TicketPreview userrole = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                RoleName = "-"
                            };
                            userRoles.Add(userrole);
                        }
                        else
                        {
                            TicketPreview userrole = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                RoleName = _userRoleBusinessService.GetDetail(submiterData.RoleId).Name
                            };
                            userRoles.Add(userrole);
                        }
                        InvolvedUser submiter = new InvolvedUser()
                        {
                            TicketNo = ticket.TicketNo,
                            EmployeName = _userBusinessService.GetDetail(ticket.Submiter).Name,
                            
                        };
                        User userdata = new User()
                        {
                            Username = ticket.TicketNo,
                            BranchName = submiterData.BranchName,
                            AreaName = submiterData.AreaName,
                            POH_Name = _EmployeeMasterBusinessService.GetDetail(submiterData.EmployeeId) == null ?"N/A" : _EmployeeMasterBusinessService.GetDetail(submiterData.EmployeeId).Business_Area 
                        };
                        
                        
                        userData.Add(userdata);
                        submiters.Add(submiter);
                    }
                    else
                    {
                        TicketPreview userrole = new TicketPreview()
                        {
                            TicketNo = ticket.TicketNo,
                            RoleName = ""
                        };
                        InvolvedUser submiter = new InvolvedUser()
                        {
                            TicketNo = ticket.TicketNo,
                            EmployeName = ""
                        };
                        User userdata = new User()
                        {
                            Username = ticket.TicketNo,
                            BranchName = "",
                            AreaName = ""
                        };
                        userData.Add(userdata);
                        userRoles.Add(userrole);
                        submiters.Add(submiter);
                    }
                    if (ticket.Responder != 0)
                    {
                        User responderData = _userBusinessService.GetDetail(ticket.Responder);
                        Rating ratingDataResponder = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, ticket.Responder);
                        TicketNote noteResponder = _TicketNote.GetDatebyTicketandUserId(ticket.TicketId, ticket.Responder);
                        TicketDiscussion replyResponder = _ticketDiscussionBs.GetDatebyTicketandUserId(ticket.TicketId, ticket.Responder);
                        TicketResolution ResolutionResponder = _ticketResolutionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, ticket.Submiter);

                        TicketNote noteresponder = new TicketNote()
                        {
                            TicketNoteId = ticket.TicketId,
                            RespondTime = noteResponder == null ? null : noteResponder.RespondTime /* new double[] { noteSubmiter == null ? 0 : noteSubmiter.RespondTime, replySubmiter == null ? 0 : replySubmiter.RespondTime, ResolutionResponder == null ? 0 : ResolutionResponder.RespondTime }.Max()*/
                        };
                        ResponderNote.Add(noteresponder);


                        if (responderData != null)
                        {
                            User responderdata = new User()
                            {
                                Username = ticket.TicketNo,
                                Name = responderData.Name
                            };

                            responders.Add(responderdata);
                        }
                        else
                        {
                            User responderdata = new User()
                            {
                                Username = ticket.TicketNo,
                                Name = null
                            };

                            responders.Add(responderdata);
                        }
                        if (ratingDataResponder != null)
                        {
                            TicketPreview ratingdataresponder = new TicketPreview()
                            {
                                TicketId = ticket.TicketId,
                                ClosedDate = ratingDataResponder.CreatedAt,
                                Rate = ratingDataResponder.Rate
                            };
                            ratingresponders.Add(ratingdataresponder);
                        }
                        else
                        {
                            TicketPreview ratingdataresponder = new TicketPreview()
                            {
                                TicketId = ticket.TicketId,
                                ClosedDate = null,
                                Rate = 0
                            };
                            ratingresponders.Add(ratingdataresponder);
                        }
                    }
                    else
                    {
                        TicketNote noteresponder = new TicketNote()
                        {
                            TicketNoteId = ticket.TicketId,
                            RespondTime = null
                        };
                        ResponderNote.Add(noteresponder);
                        TicketPreview ratingdataresponder = new TicketPreview()
                            {
                                TicketId = ticket.TicketId,
                                ClosedDate = null,
                                Rate = 0
                            };
                            User responderdata = new User()
                            {
                                Username = ticket.TicketNo,
                                Name = null
                            };

                            responders.Add(responderdata);
                            ratingresponders.Add(ratingdataresponder);
                    }
                }
            }
            ViewBag.NoteSubmiter = SubmiterNote;
            ViewBag.NoteResponder = ResponderNote;
            ViewBag.Responders = responders;
            ViewBag.RatingResponders = ratingresponders;
            ViewBag.Rating = ratings;
            ViewBag.Resolution = ticketresolutions;
            ViewBag.UserData = userData;
            ViewBag.User = userRoles;
            ViewBag.Submiters = submiters;
            int pageSize = 999999999;
            int pageNumber = (page ?? 1);

            return View(tickets.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/HelpDesk/Detail/5
        public ActionResult Detail(int? id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            if (id == null)
                return RedirectToAction("Index", "HelpDesk");

            Ticket ticket = _ticketBusinessService.GetDetail(int.Parse(id.ToString()));
            if (ticket == null)
            {
                return HttpNotFound();
            }

            #region Involved Users

            List<int> involvedIdList = new List<int> {ticket.Submiter};
            if (ticket.Responder != 0)
            {
                involvedIdList.Add(ticket.Responder);
            }

            var participants = _ticketParcipantBusinessService.GetByTicket(ticket.TicketId);
            if (participants != null)
            {
                foreach (var participantItem in participants)
                {
                    involvedIdList.Add(participantItem.UserId);
                }
            }

            ViewBag.Participants = _ticketParcipantBusinessService.GetParticipantWithName(participants);
            ViewBag.IsInvolved = false;

            if (involvedIdList.Contains(Convert.ToInt32(Session["useridbackend"])))
                ViewBag.IsInvolved = true;
            #endregion



            if (ticket.Status.Equals(6) || ticket.Status.Equals(3))
            {
                TicketResolution ticketResolution = _ticketResolutionBusinessService.GetByTicket(int.Parse(id.ToString()));

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

                ViewBag.Resolution = resolution;
                ViewBag.ResolutionData = ticketResolution;
                var submiterRatingBack = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);

                ViewBag.IsRatedBySubmiter = false;

                if (submiterRatingBack != null)
                    ViewBag.IsRatedBySubmiter = true;
            }

            #region Attachments
            

            var attachments = _ticketAttachmentBusinessService.GetFullByTicketId(ticket.TicketId);
            List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
            string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/");
            if (attachments != null)
            {
                string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };
                foreach (var attachment in attachments)
                {
                    if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.TicketAttachmentId,
                            Name = Common.ImageToBase64(attachmentsPath + attachment.Name),
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name),
                           NameWithOutBase64 = attachment.Name,
                            Uri = Common.GetDomainSecure() + "/Upload/TechnicalRequestAttachments/" + attachment.Name
                        });
                    }
                    else
                    {
                        listAttachment.Add(new TicketAttachmentsAPI
                        {
                            Id = attachment.TicketAttachmentId,
                            Name = attachment.Name,
                            Level = attachment.LevelUser,
                            Type = Path.GetExtension(attachment.Name),
                            NameWithOutBase64 = attachment.Name,
                            Uri = Common.GetDomainSecure() + "/Upload/TechnicalRequestAttachments/" + attachment.Name
                        });
                    }
                }
            }
            ViewBag.Attachments = listAttachment;
            #endregion

            ViewBag.CategoryName = _ticketCategoryBusinessService.GetDetail(ticket.TicketCategoryId).Name;
            
            var userData = _userBusinessService.GetDetail(ticket.Submiter);
            ViewBag.Submiter = userData;
            ViewBag.SubmiterUserId = ticket.Submiter;
            ViewBag.Responder = ticket.Responder == 0 ? null : _userBusinessService.GetDetail(ticket.Responder);
            ViewBag.Tags = _articleTagBusinessService.GetTagsByTicket(ticket.TicketId);
            ViewBag.Ticket = ticket;
            ViewBag.Area = userData.AreaName;
            ViewBag.Branch = userData.BranchName;
            ViewBag.RoleId = userData.RoleId;
            ViewBag.RoleName = userData.RoleId == 0 ? "-" : _userRoleBusinessService.GetDetail(userData.RoleId).Name;
            ViewBag.UserLevelName = userData.RoleId == 0 ? "-" : _userRoleBusinessService.GetDetail(userData.RoleId).Description;
            ViewBag.Stars = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, ticket.Responder);
            ViewBag.StarsResponder = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);
            return View(ticket);
        }

        // GET: Admin/HelpDesk/Delete/5
        public ActionResult Delete(int id)
        {
            Ticket ticket = _ticketBusinessService.GetDetail(id);
            _ticketBusinessService.Delete(ticket);
            
            return RedirectToAction("Index", "HelpDesk");
        }
        public string GetCategoryName(int id)
        {
            return _ticketCategoryBusinessService.GetDetail(id).Name;
        }
    }
}
