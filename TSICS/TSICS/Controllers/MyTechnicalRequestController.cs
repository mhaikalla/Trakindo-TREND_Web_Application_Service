using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using PagedList;
using TSICS.Helper;

namespace TSICS.Controllers
{
    public class MyTechnicalRequestController : Controller
    {
        #region Business Services

        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly TicketResolutionBusinessService _ticketResolutionBs = Factory.Create<TicketResolutionBusinessService>("TicketResolution", ClassType.clsTypeBusinessService);
        private readonly ArticleTagBusinessService _articleTagBs = Factory.Create<ArticleTagBusinessService>("ArticleTag", ClassType.clsTypeBusinessService);
        private readonly TicketPreviewBusinessService _ticketPreviewBusinessService = Factory.Create<TicketPreviewBusinessService>("TicketPreview", ClassType.clsTypeBusinessService);
        private readonly TicketAttachmentBusinessService _ticketAttachmentBs = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        // ReSharper disable once StringLiteralTypo
        private readonly TicketParcipantBusinessService _ticketParticipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        #endregion

        TechnicalRequestController TechnicalRequest = new TechnicalRequestController();
        // GET: MyTechnicalRequest
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (Session["userid"] == null)
            {
                    return View("LoginRedirect");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return View("LoginRedirect");
                }

                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "DateDesc" : "";
                ViewBag.TitleSortParm = sortOrder == "Title" ? "TitleDesc" : "Title";
                ViewBag.StatusSortParm = sortOrder == "Status" ? "StatusDesc" : "Status";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;
                
                var tickets = _ticketBs.GetQueryableByUserConected(int.Parse(Session["userid"].ToString()));
                //Search MyTR
                if (!String.IsNullOrEmpty(searchString))
                {
                    int userlogin = int.Parse(Session["userid"].ToString());
                    if ("waiting your feedback".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                         ticket.Title.Contains(searchString) ||
                         ticket.TicketNo.Contains(searchString) ||
                         ticket.SerialNumber.Contains(searchString) ||
                         (ticket.Status == 2 && userlogin.Equals(ticket.NextCommenter) && (userlogin.Equals(ticket.Responder) || userlogin.Equals(ticket.Submiter))) ||
                         ticket.Description.Contains(searchString)||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else if ("pra".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                         ticket.Title.Contains(searchString) ||
                         ticket.TicketNo.Contains(searchString) ||
                         ticket.SerialNumber.Contains(searchString) ||
                         (ticket.Status == 2 && !userlogin.Equals(ticket.NextCommenter) && ticket.NextCommenter.Equals(ticket.Responder)) ||
                         ticket.Description.Contains(searchString) ||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else if ("psa".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                         ticket.Title.Contains(searchString) ||
                         ticket.TicketNo.Contains(searchString) ||
                         ticket.SerialNumber.Contains(searchString) ||
                         (ticket.Status == 2 && !userlogin.Equals(ticket.NextCommenter) && ticket.NextCommenter.Equals(ticket.Submiter)) ||
                         ticket.Description.Contains(searchString) ||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else if ("draft".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(1) ||
                          ticket.Description.Contains(searchString) ||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else if ("solved".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(6) ||
                          ticket.Description.Contains(searchString) ||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else if ("closed".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(3) ||
                          ticket.Description.Contains(searchString) ||
                         ticket.EmailCC.Contains(searchString));
                    }
                    else
                    {
                        int[] UserParticipant = _ticketParticipantBusinessService.GetTicketIdBySearchUserId(searchString).ToArray();
                        int[] User = _userBusinessService.GetListSearchUser(searchString).ToArray();
                        int[] tagTicket = _articleTagBs.searchTagTicket(searchString).ToArray();
                        if (tagTicket.Length > 0)
                        {
                            tickets = tickets.Where(ticket =>
                            ticket.Title.Contains(searchString) ||
                            ticket.TicketNo.Contains(searchString) ||
                            ticket.SerialNumber.Contains(searchString) ||
                            ticket.DPPMno.Contains(searchString) ||
                            ticket.Description.Contains(searchString) ||
                            ticket.EmailCC.Contains(searchString) || (tagTicket.Contains(ticket.TicketId) && ticket.Status != 1) || ((User.Contains(ticket.Responder) || User.Contains(ticket.Submiter) || UserParticipant.Contains(ticket.TicketId)) && ticket.Status != 1));
                        }
                        else
                        {
                            tickets = tickets.Where(ticket =>
                            ticket.Title.Contains(searchString) ||
                            ticket.TicketNo.Contains(searchString) ||
                            ticket.SerialNumber.Contains(searchString) ||
                            ticket.DPPMno.Contains(searchString) ||
                            ticket.Description.Contains(searchString) ||
                            ticket.EmailCC.Contains(searchString) || ((User.Contains(ticket.Responder) || User.Contains(ticket.Submiter) || UserParticipant.Contains(ticket.TicketId)) && ticket.Status != 1));
                        }

                    }
                }
                List<Ticket> recent = new List<Ticket>();
                if (tickets != null)
                {
                    foreach (var item in tickets)
                    {
                            Ticket recentDate = new Ticket()
                            {
                                TicketId = item.TicketId,
                                CreatedAt = new[] { item.CreatedAt, item.LastReply, item.UpdatedAt, item.LastStatusDate }.Max()
                            };
                            recent.Add(recentDate);
                    }
                    ViewBag.RecentDate = recent;
                }
                else
                {
                    ViewBag.RecentDate = null;
                }
                switch (sortOrder)
                {
                    case "DateDesc":
                        tickets = tickets.OrderBy(t => new[] { t.CreatedAt, t.LastReply, t.UpdatedAt, t.LastStatusDate }.Max());
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
                        tickets = tickets.OrderByDescending(t => new[] { t.CreatedAt, t.LastReply, t.UpdatedAt, t.LastStatusDate }.Max());
                        break;
                }

                ViewBag.Recent = TechnicalRequest.GetRecentTR(Convert.ToInt32(Session["userid"]));
                var ticketPreview = _ticketPreviewBusinessService.MakeFrom(tickets, Convert.ToInt32(Session["userid"]));

                List<InvolvedUser> submitter = new List<InvolvedUser>();
                List<InvolvedUser> responders = new List<InvolvedUser>();
                List<InvolvedUser> involvedUsers = new List<InvolvedUser>();
                List<TicketResolution> Resolutions = new List<TicketResolution>();
                if (ticketPreview != null)
                {
                    foreach (var ticket in ticketPreview)
                    {
                        User submitterData = _userBusinessService.GetDetail(ticket.Submiter);
                        ticket.IsEscalated = _ticketBs.IsEscalated(ticket.TicketId, Convert.ToInt32(Session["userid"]));
                        TicketResolution resolution =  _ticketResolutionBs.GetByTicket(ticket.TicketId);
                        if (resolution != null)
                        {
                            TicketResolution resolutionObj = new TicketResolution()
                            {
                                TicketId = ticket.TicketId,
                                Description = resolution.Description,
                                CreatedAt = resolution.CreatedAt
                            };
                            Resolutions.Add(resolutionObj);
                        }
                        else
                        {
                            TicketResolution resolutionObj = new TicketResolution()
                            {
                                TicketId = ticket.TicketId,
                                Description = null,
                                CreatedAt = null
                            };
                            Resolutions.Add(resolutionObj);
                        }
                        if (submitterData != null)
                        {
                            InvolvedUser submitterUser = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = _userBusinessService.GetDetail(ticket.Submiter).Name
                            };

                            submitter.Add(submitterUser);
                        }
                        else
                        {
                            InvolvedUser submitterUser = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = ""
                            };

                            submitter.Add(submitterUser);
                        }

                        if (ticket.Responder != 0)
                        {
                            User responderData = _userBusinessService.GetDetail(ticket.Responder);
                            if (responderData != null)
                            {
                                InvolvedUser responder = new InvolvedUser()
                                {
                                    TicketNo = ticket.TicketNo,
                                    EmployeName = _userBusinessService.GetDetail(ticket.Responder).Name
                                };
                                responders.Add(responder);
                            }
                            else
                            {
                                InvolvedUser responder = new InvolvedUser()
                                {
                                    TicketNo = ticket.TicketNo,
                                    EmployeName = ""
                                };
                                responders.Add(responder);
                            }
                        }
                        else
                        {
                            if (ticket.Status != 1)
                            {
                                InvolvedUser responder = new InvolvedUser()
                                {
                                    TicketNo = ticket.TicketNo,
                                    EmployeName = "TREND Admin"
                                };
                                responders.Add(responder);
                            }
                            else
                            {
                                InvolvedUser responder = new InvolvedUser()
                                {
                                    TicketNo = ticket.TicketNo,
                                    EmployeName = ""
                                };
                                responders.Add(responder);
                            }
                        }

                        var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);
                        if (participants != null)
                        {
                            foreach (var participantItem in participants)
                            {
                                var useCek = _userBusinessService.GetDetail(participantItem.UserId);

                                if (useCek != null)
                                {
                                    InvolvedUser participant = new InvolvedUser()
                                    {
                                        TicketNo = ticket.TicketNo,
                                        EmployeName = useCek.Name,
                                        UserId = useCek.UserId
                                    };
                                    involvedUsers.Add(participant);
                                }
                            }
                        }
                    }
                }
                ViewBag.Resolutions = Resolutions;
                ViewBag.Submiters = submitter;
                ViewBag.Responders = responders;
                ViewBag.InvolvedUsers = involvedUsers;
                
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View("Index", ticketPreview.ToPagedList(pageNumber, pageSize));
            }
        }
       
        /// <summary>
        /// MyTechnicalRequest/Update/DM20191230001
        /// </summary>
        /// <param name="id">Ticket Number</param>
        /// <returns>
        /// The view if param not null
        /// Redirect to all TR if param null
        /// </returns>
        [HttpGet]
        public ActionResult Update(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.Domain = WebConfigure.GetDomain() + "/Upload/TechnicalRequestAttachments/";
                if (id != null)
                {
                    string ticketCategory = _ticketBs.GetCategoryName(id);

                    var ticket = _ticketBs.GetByTicketNumber(id);
                    var ticketParticipants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);
                    if (!((ticket.Status.Equals(2) && Convert.ToInt32(Session["userid"]) == ticket.Responder) || (ticket.Status.Equals(1) && Convert.ToInt32(Session["userid"]) == ticket.Submiter)))
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('You are Not Included with This Technical Request'); location.href = '" + WebConfigure.GetDomain() + "/MyTechnicalRequest';</script>");
                    }
                    if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Cant Update Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/MyTechnicalRequest';</script>");
                    }
                    if (ticket.Status == 1 && ticket.Submiter != Convert.ToInt32(Session["userid"]))
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('The Technical Request is Not Found'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                    }
                    var responder = _userBusinessService.GetDetail(ticket.Responder);
                    var attachments = _ticketAttachmentBs.GetFullByTicketId(ticket.TicketId);
                    List<TicketAttachmentsAPI> listAttachment = new List<TicketAttachmentsAPI>();
                    string attachmentsPath = System.Web.HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/");
                    if (attachments != null)
                    {
                        foreach (var attachment in attachments)
                        {
                            string[] imgExt = { ".jpg", ".png", ".jpeg", ".gif", ".bmp", ".tif", ".tiff" };

                            if (imgExt.Contains(Path.GetExtension(attachment.Name)))
                            {
                                listAttachment.Add(new TicketAttachmentsAPI
                                {
                                    Id = attachment.TicketAttachmentId,
                                    Name = Common.ImageToBase64(attachmentsPath + attachment.Name),
                                    Level = attachment.LevelUser,
                                    Type = Path.GetExtension(attachment.Name),
                                    NameWithOutBase64 = attachment.Name
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
                                    NameWithOutBase64 = attachment.Name
                                });
                            }
                        }
                    }

                    var tags = _articleTagBs.GetTagsByTicket(ticket.TicketId);
                    WarrantyTypeBusinessService warrantyTypeBusinessService = Factory.Create<WarrantyTypeBusinessService>("WarrantyType", ClassType.clsTypeBusinessService);
                    ViewBag.warrantyType = warrantyTypeBusinessService.Get();
                    ViewBag.Ticket = ticket;
                    ViewBag.Attachments = listAttachment;
                    ViewBag.Tags = tags;
                    ViewBag.Responder = responder;

                    if (ticketParticipants != null)
                    {

                        ViewBag.Participants = _ticketParticipantBusinessService.GetParticipantWithName(ticketParticipants);
                    }
                    ViewBag.Recent = TechnicalRequest.GetRecentTR(Convert.ToInt32(Session["userid"]));

                    SetListDropdown(int.Parse(Session["userid"].ToString()));

                    return View("../TechnicalRequest/Update" + ticketCategory);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                if (WebConfigure.GetLoginPortal() == "true" && Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                {
                    return RedirectToAction("Login", "Account");
                }

                if (id != null)
                {
                    Ticket tickets = _ticketBs.GetDetail(int.Parse(id));
                    if (Convert.ToInt32(Session["DelegateStatus"]) != 1)
                    {
                        if (Convert.ToInt32(Session["userid"]).Equals(tickets.Submiter) && tickets.Status == 1)
                        {
                            _ticketBs.Delete(tickets);
                            return RedirectToAction("Index", "MyTechnicalRequest");
                        }
                        else
                        {
                            return Content("<script language='javascript' type='text/javascript'>alert('You are Not Included with This Technical Request'); location.href = '" + WebConfigure.GetDomain() + "/MyTechnicalRequest';</script>");
                        }
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Cant Delete Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/MyTechnicalRequest';</script>");

                    }
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        private void SetListDropdown(int userId)
        {
            ViewBag.ListUser = _userBusinessService.GetUserListByHierarchy(userId);
            ViewBag.AllUserActive = _userBusinessService.GetListActive();
        }
    }
}
