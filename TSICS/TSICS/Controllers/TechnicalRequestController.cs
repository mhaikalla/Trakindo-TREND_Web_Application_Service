using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using PagedList;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace TSICS.Controllers
{
    public class TechnicalRequestController : Controller
    {
        #region Business Services
        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly TicketDiscussionBusinessService _ticketDiscussionBusinessService = Factory.Create<TicketDiscussionBusinessService>("TicketDiscussion", ClassType.clsTypeBusinessService);
        private readonly TicketNoteBusinessService _ticketNoteBusinessService = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        private readonly TicketCategoryBusinessService _ticketCategoryBusinessService = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        private readonly ArticleTagBusinessService _articleTagBusinessService = Factory.Create<ArticleTagBusinessService>("ArticleTag", ClassType.clsTypeBusinessService);
        private readonly MepBusinessService _mepBusinessService = Factory.Create<MepBusinessService>("Mep", ClassType.clsTypeBusinessService);
        private readonly TicketPreviewBusinessService _ticketPreviewBusinessService = Factory.Create<TicketPreviewBusinessService>("TicketPreview", ClassType.clsTypeBusinessService);
        private readonly TicketAttachmentBusinessService _ticketAttachmentBusinessService = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParticipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        private readonly DiscussionAttachmentBusinessService _discussionAttachmentBusinessService = Factory.Create<DiscussionAttachmentBusinessService>("DiscussionAttachment", ClassType.clsTypeBusinessService);
        private readonly NoteAttachmentBusinessService _noteAttachmentBusinessService = Factory.Create<NoteAttachmentBusinessService>("NoteAttachment", ClassType.clsTypeBusinessService);
        private readonly TicketResolutionBusinessService _ticketResolutionBusinessService = Factory.Create<TicketResolutionBusinessService>("TicketResolution", ClassType.clsTypeBusinessService);
        private readonly RatingBusinessService _ratingBusinessService = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBusinessService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly MasterAreaBusinessService _masterAreaBusinessService = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
        private readonly MasterBranchBusinessService _masterBranchBusinessService = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        private readonly ArticleTagBusinessService _articleTagBs = Factory.Create<ArticleTagBusinessService>("ArticleTag", ClassType.clsTypeBusinessService);
        private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly WarrantyTypeBusinessService _WarrantyTypeBS = Factory.Create<WarrantyTypeBusinessService>("WarrantyType", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);

        #endregion

        private String title = "", Description = "";
        // GET: TechnicalRequest
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
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

                var tickets = _ticketBusinessService.GetQueryableAllTr();
                //Search All TR
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
                         ticket.Description.Contains(searchString));
                    }
                    else if ("pra".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                         ticket.Title.Contains(searchString) ||
                         ticket.TicketNo.Contains(searchString) ||
                         ticket.SerialNumber.Contains(searchString) ||
                         (ticket.Status == 2 && !userlogin.Equals(ticket.NextCommenter) && ticket.NextCommenter.Equals(ticket.Responder)) ||
                         ticket.Description.Contains(searchString));
                    }
                    else if ("psa".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                         ticket.Title.Contains(searchString) ||
                         ticket.TicketNo.Contains(searchString) ||
                         ticket.SerialNumber.Contains(searchString) ||
                         (ticket.Status == 2 && !userlogin.Equals(ticket.NextCommenter) && ticket.NextCommenter.Equals(ticket.Submiter)) ||
                         ticket.Description.Contains(searchString));
                    }
                    else if ("draft".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(1) ||
                          ticket.Description.Contains(searchString));
                    }
                    else if ("solved".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(6) ||
                          ticket.Description.Contains(searchString));
                    }
                    else if ("closed".Contains(searchString.ToLower()))
                    {
                        tickets = tickets.Where(ticket =>
                          ticket.Title.Contains(searchString) ||
                          ticket.TicketNo.Contains(searchString) ||
                          ticket.SerialNumber.Contains(searchString) ||
                          ticket.Status.Equals(3) ||
                          ticket.Description.Contains(searchString));
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
                            ticket.DPPMno.Contains(searchString) ||
                            ticket.SerialNumber.Contains(searchString) ||
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

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                var ticketPreview = _ticketPreviewBusinessService.MakeFrom(tickets, Convert.ToInt32(Session["userid"]));

                List<InvolvedUser> submitters = new List<InvolvedUser>();
                List<InvolvedUser> responders = new List<InvolvedUser>();
                List<InvolvedUser> involvedUsers = new List<InvolvedUser>();
                List<TicketResolution> Resolutions = new List<TicketResolution>();
                if (ticketPreview != null)
                {
                    foreach (var ticket in ticketPreview)
                    {
                        User submitterData = _userBusinessService.GetDetail(ticket.Submiter);
                        ticket.IsEscalated = _ticketBusinessService.IsEscalated(ticket.TicketId, Convert.ToInt32(Session["userid"]));
                        TicketResolution resolution = _ticketResolutionBusinessService.GetByTicket(ticket.TicketId);
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
                            InvolvedUser submitter = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = _userBusinessService.GetDetail(ticket.Submiter).Name
                            };

                            submitters.Add(submitter);
                        }
                        else
                        {
                            InvolvedUser submitter = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = "-"
                            };

                            submitters.Add(submitter);
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
                                    EmployeName = "-"
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
                                    EmployeName = "-"
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
                ViewBag.Submiters = submitters;
                ViewBag.Responders = responders;
                ViewBag.InvolvedUsers = involvedUsers;
                ViewBag.User = _userBusinessService.GetDetail(Convert.ToInt32(Session["userid"]));
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(ticketPreview.ToPagedList(pageNumber, pageSize));
            }
        }
        public List<Ticket> GetRecentTR(int userid)
        {
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            return _ticketBusinessService.GetQueryableByUserConected(userid).OrderByDescending(t => t.CreatedAt).Take(5).ToList();
        }

        // GET: TechnicalRequest/ProductHealth
        [HttpGet]
        public ActionResult ProductHealth()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(1);
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                return View();
            }
        }

        /// <summary>
        /// Submit TR
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ProductHealth(FormCollection formCollection)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Product Health");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateProductHealth(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Product Health"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }
                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                    SendNotif(ticketUpdateResult, formCollection);
                    Email.SendMailCreateTr(ticketUpdateResult);
                    Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/PartsTechnical
        [HttpGet]
        public ActionResult PartsTechnical()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(2);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PartsTechnical(FormCollection formCollection)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Parts Technical");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }
                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdatePartsTechnical(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Parts Technical"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }
                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/Dimension
        [HttpGet]
        public ActionResult Dimension()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(3);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Dimension(FormCollection formCollection)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Dimension");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateDimension(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Dimension"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                //if (Request.Files.Count > 0)
                //{
                //    _ticketAttachmentBusinessService.DeleteAllAttachmentInTicket(ticketId);
                //    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                //}

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/References
        [HttpGet]
        public ActionResult Reference()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(4);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="mTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Reference(FormCollection formCollection, Ticket mTicket)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Reference");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateReference(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Reference"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/WarrantyAssistance
        [HttpGet]
        public ActionResult WarrantyAssistance()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(5);

                WarrantyTypeBusinessService warrantyTypeBusinessService = Factory.Create<WarrantyTypeBusinessService>("WarrantyType", ClassType.clsTypeBusinessService);
                ViewBag.warrantyType = warrantyTypeBusinessService.Get();
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="mTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult WarrantyAssistance(FormCollection formCollection, Ticket mTicket)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Warranty Assistance");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateWarrantyAssistance(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Warranty Assistance"));
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/GoodwillAssistance
        [HttpGet]
        public ActionResult GoodwillAssistance()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(6);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="mTicket"></param>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult GoodwillAssistance(FormCollection formCollection, Ticket mTicket)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Goodwill Assistance");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateGoodwillAssistance(FormCollection formCollection)
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

                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Goodwill Assistance"));
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                   
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                   
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/Password
        [HttpGet]
        public ActionResult Password()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(7);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="mTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Password(FormCollection formCollection, Ticket mTicket)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Password");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult);
                    Email.SendMailCreateTr(ticketAddResult, true);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdatePassword(FormCollection formCollection)
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
                Ticket TicketDetail = _ticketBusinessService.GetDetail(Convert.ToInt32(formCollection["TicketId"]));
                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Password"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                   
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }
        [HttpGet]
        public ActionResult GenerateCatElectronicTechniciann(int? id)
        {
            if (id == null)
                return RedirectToAction("Index", "TechnicalRequest");

            var ticket = _ticketBusinessService.GetDetail(Convert.ToInt32(id));
            ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
            if (ticket == null)
                return RedirectToAction("Index", "TechnicalRequest");

            return View("CatElectronicTechnician", ticket);

        }

        [HttpGet]
        public ActionResult GenerateCatElectronicTechnician(int? id)
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

                if (id == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                var ticket = _ticketBusinessService.GetDetail(Convert.ToInt32(id));
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                if (ticket == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                return View("CatElectronicTechnician", ticket);
            }
        }

        // ReSharper disable once CommentTypo
        // GET: TechnicalRequest/Telematics
        [HttpGet]
        // ReSharper disable once IdentifierTypo
        public ActionResult Telematics()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(8);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        // ReSharper disable once IdentifierTypo
        public ActionResult Telematics(FormCollection formCollection)
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

                // ReSharper disable once StringLiteralTypo
                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Telematics");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);

                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateTelematics(FormCollection formCollection)
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
                Ticket TicketDetail = _ticketBusinessService.GetDetail(Convert.ToInt32(formCollection["TicketId"]));
                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Telematics"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/HelpDesk
        [HttpGet]
        public ActionResult HelpDesk()
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
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(9);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HelpDesk(FormCollection formCollection)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Help Desk");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);
                }
                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateHelpDesk(ticketAddResult);
                }
                if (Session["username"].ToString().ToLower().Contains("Guest"))
                {
                    return RedirectToAction("Index", "Library");
                }
                else
                {
                    return RedirectToAction("Index", "MyTechnicalRequest");
                }
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateHelpDesk(FormCollection formCollection)
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
                Ticket TicketDetail = _ticketBusinessService.GetDetail(Convert.ToInt32(formCollection["TicketId"]));
                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Help Desk"));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                    Email.SendMailCreateHelpDesk(ticketUpdateResult);
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        // GET: TechnicalRequest/ProductHealth
        [HttpGet]
        public ActionResult ConditionMonitoring()
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
                if (Convert.ToInt32(Session["DelegateStatus"]) == 1)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Cant Create New Technical Request During Delegation Period'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(10);
                return View();
            }
        }

        /// <summary>
        /// Submit
        /// </summary>
        /// <param name="formCollection"></param>
        /// <param name="mTicket"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ConditionMonitoring(FormCollection formCollection, Ticket mTicket)
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

                Ticket ticket = PlottingWebFormCreateTechnicalRequest(formCollection, "Condition Monitoring");

                Ticket ticketAddResult = _ticketBusinessService.Add(ticket);

                int ticketId = ticketAddResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketAddResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    AddTechnicalRequestTags(formCollection, ticketId);
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    AddParticipant(formCollection, ticketId);
                }

                if (WebConfigure.GetSendEmail() && formCollection["type"] != "draft")
                {
                    SendNotif(ticketAddResult, formCollection);
                    Email.SendMailCreateTr(ticketAddResult, true);
                    Email.SendMailCreateTr(ticketAddResult);
                }

                return RedirectToAction("Index", "MyTechnicalRequest");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateConditionMonitoring(FormCollection formCollection)
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
                Ticket TicketDetail = _ticketBusinessService.GetDetail(Convert.ToInt32(formCollection["TicketId"]));
                Ticket ticketUpdateResult = _ticketBusinessService.Edit(PlottingWebFormUpdateTechnicalRequest(formCollection, "Condition Monitoring"));
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                int ticketId = ticketUpdateResult.TicketId;

                if (Request.Files.Count > 0)
                {
                    AddTechnicalRequestAttachments(ticketId, ticketUpdateResult.TicketNo, formCollection["file_level[]"].Split(','));
                }

                if (formCollection.AllKeys.Contains("input-tag[]"))
                {
                    UpdateTechnicalRequestTags(formCollection, ticketId);
                }
                else
                {
                    if (_articleTagBusinessService.GetTagsByTicket(ticketId).Count > 0)
                    {
                        _articleTagBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }

                if (formCollection.AllKeys.Contains("ParticipantList"))
                {
                    UpdateParticipant(formCollection, ticketId);
                }
                else
                {
                    if (_ticketParticipantBusinessService.GetByTicket(ticketId).Count > 0)
                    {
                        _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                    }
                }
                if (Convert.ToInt32(formCollection["Status"]) == 1 && formCollection["type"] == "submit")
                {
                    if (ticketUpdateResult.TicketCategoryId != 9)
                    {
                        SendNotif(ticketUpdateResult, formCollection);
                        Email.SendMailCreateTr(ticketUpdateResult);
                        Email.SendMailCreateTr(ticketUpdateResult, true);
                    }
                    else
                    {
                        Email.SendMailCreateHelpDesk(ticketUpdateResult);
                    }
                }
                return RedirectToAction("Details/" + ticketUpdateResult.TicketId, "TechnicalRequest");
            }
        }

        public ActionResult Details(int? id)
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

                if (String.IsNullOrWhiteSpace(id.ToString()))
                {
                    return RedirectToAction("Index", "TechnicalRequest");
                }
                Ticket ticket = _ticketBusinessService.GetDetail(int.Parse(id.ToString()));
                if (ticket == null)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('The Technical Request is Not Found'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                if (ticket.Status == 5)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('This Technical Request Has Been Deleted'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                if (ticket.Status == 1 && ticket.Submiter != Convert.ToInt32(Session["userid"]))
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('The Technical Request is Not Found'); location.href = '" + WebConfigure.GetDomain() + "/TechnicalRequest';</script>");
                }
                #region Involved Users

                List<int> involvedIdList = new List<int> { ticket.Submiter };
                if (ticket.Responder != 0)
                {
                    involvedIdList.Add(ticket.Responder);
                }

                var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);

                if (participants != null)
                {
                    foreach (var participantItem in participants)
                    {
                        involvedIdList.Add(participantItem.UserId);
                    }
                }

                ViewBag.Participants = _ticketParticipantBusinessService.GetParticipantWithName(participants);
                ViewBag.IsInvolved = false;

                if (involvedIdList.Contains(Convert.ToInt32(Session["userid"])))
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
                                Uri = attachment.Name
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
                                Uri = attachment.Name
                            });
                        }
                    }
                }
                ViewBag.Attachments = listAttachment;
                #endregion

                UserRole roleData = _userRoleBusinessService.GetDetail(Convert.ToInt32(Session["role"]));
                ViewBag.UserLevelName = String.IsNullOrWhiteSpace(roleData.Description) ? "Guest" : roleData.Description;
                ViewBag.CategoryName = _ticketCategoryBusinessService.GetDetail(ticket.TicketCategoryId).Name;
                ViewBag.Submiter = _userBusinessService.GetDetail(ticket.Submiter);
                ViewBag.SubmiterUserId = ticket.Submiter;
                ViewBag.Responder = ticket.Responder == 0 ? null : _userBusinessService.GetDetail(ticket.Responder);
                ViewBag.ResponderUserId = ticket.Responder;
                ViewBag.Tags = _articleTagBusinessService.GetTagsByTicket(ticket.TicketId);
                ViewBag.Ticket = ticket;
                ViewBag.WarrantyName = ticket.WarrantyTypeId == 0 ? "-" : _WarrantyTypeBS.Get(ticket.WarrantyTypeId).WarrantyTypeName;
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));

                ViewBag.IsEscalated = _ticketBusinessService.IsEscalated(ticket.TicketId, Convert.ToInt32(Session["userid"]));

                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.CurrentUser = _userBusinessService.GetDetail(Convert.ToInt32(Session["userid"]));


                ViewBag.SubmiterPhoto = ticket.Submiter == 0 ? null : _userBusinessService.GetDetail(ticket.Submiter).PhotoProfile;
                ViewBag.ResponderPhoto = ticket.Responder == 0 ? null : _userBusinessService.GetDetail(ticket.Responder) == null ? null : _userBusinessService.GetDetail(ticket.Responder).PhotoProfile;
                ViewBag.Stars = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, ticket.Responder);
                ViewBag.StarsResponder = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);
                return View(ticket);
            }
        }

        [HttpPost]
        public ActionResult Details(int? id, FormCollection formCollection)
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

                if (id == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                Ticket ticket = _ticketBusinessService.GetDetail(int.Parse(id.ToString()));

                AddParticipant(formCollection, ticket.TicketId);
                return RedirectToAction("Details", new { id });
            }
        }

        [HttpPost]
        public ActionResult DeleteParticipant(int id)
        {
            var deleteParticipant = _ticketParticipantBusinessService.Delete(_ticketParticipantBusinessService.GetDetail(id));
            //Response.Write("{\"status\":true}");
            return Json(new { data = deleteParticipant });
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDiscussion(FormCollection formCollection)
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

                int userId = 0;
                if (Session["userid"] != null)
                {
                    userId = Convert.ToInt32(Session["userid"]);
                }

                string name = "";
                if (Session["name"] != null)
                {
                    name = Session["name"].ToString();
                }

                string msg = formCollection["insert-comment"];
                int typeMsg = Convert.ToInt32(formCollection["chat-type"]);
                int ticketId = Convert.ToInt32(formCollection["TicketId"]);
                int ticketDiscussionId = 0;
                int ticketNoteId = 0;
                DateTime dateTimeNow = DateTime.Now;
                Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
                User user = _userBusinessService.GetDetail(userId);
                ApiJsonStatus apiJsonStatus = new ApiJsonStatus()
                {
                    code = 200,
                    message = "ok"
                };
                TimeSpan respond = ticket.LastReply == null ? (DateTime.Now.Subtract(ticket.CreatedAt.Value)) : (DateTime.Now.Subtract(ticket.LastReply.Value))/*TotalSeconds*/;
                if (typeMsg == 1)
                {
                    TicketDiscussion ticketDiscussion = new TicketDiscussion()
                    {
                        TicketId = ticketId,
                        CreatedAt = dateTimeNow,
                        UserId = userId,
                        Description = msg,
                        TicketNoteId = 0,// -> new comment don't have note
                        Status = 1,
                        //RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                        RespondTime = null
                    };
                    ticketDiscussionId = _ticketDiscussionBusinessService.Add(ticketDiscussion).TicketDiscussionId;
                    ticket.LastReply = dateTimeNow;
                    ticket = _ticketBusinessService.Edit(ticket);
                }
                else if (typeMsg == 2)
                {
                    TicketNote ticketNote = new TicketNote()
                    {
                        TicketId = ticketId,
                        CreatedAt = dateTimeNow,
                        UserId = userId,
                        Description = msg,
                        Status = 1,
                        RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    };
                    ticketNoteId = _ticketNoteBusinessService.Add(ticketNote).TicketNoteId;
                    _ticketDiscussionBusinessService.SetNote(ticketId, ticketNoteId);
                    ticket.LastReply = dateTimeNow;
                    ticket.LastStatusDate = dateTimeNow;
                    ticket = _ticketBusinessService.Edit(ticket);
                }
                else if (typeMsg == 3)
                {
                    WriteResolution(formCollection, ticket, dateTimeNow);

                    return Json(new { status = apiJsonStatus, data = new object() });
                }

                if (Request.Files.Count > 0)
                {
                    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    {
                        var file = Request.Files[i];
                        string response = Common.ValidateFileUpload(file);

                        if (response.Equals("true"))
                        {
                            switch (typeMsg)
                            {
                                case 1:// -> Comment
                                    DiscussionAttachment discussionAttachment = new DiscussionAttachment()
                                    {
                                        TicketDiscussionId = ticketDiscussionId,
                                        Name = Common.AddDiscussionAttachments(file, Path.GetFileNameWithoutExtension(file.FileName)),
                                        Status = 1
                                    };

                                    _discussionAttachmentBusinessService.Add(discussionAttachment);
                                    break;
                                default:// -> Note
                                    NoteAttachment noteAttachment = new NoteAttachment()
                                    {
                                        TicketNoteId = ticketNoteId,
                                        Name = Common.AddNoteAttachments(file, Path.GetFileNameWithoutExtension(file.FileName)),
                                        Status = 1
                                    };

                                    _noteAttachmentBusinessService.Add(noteAttachment);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, response);
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
                    name = name,
                    is_verified = true
                };
                if (ticket.TicketCategoryId != 9)
                {
                    if (ticket.Responder == userId || ticket.Responder == 0)
                    {
                        apiJsonCommentSenderTr.type = "Responder";
                        if (typeMsg == 2)
                        {
                            if (ticket.NextCommenter == userId) {
                                ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));

                            }
                            ticket.NextCommenter = ticket.Submiter;
                            _ticketBusinessService.Edit(ticket);
                        }
                    }
                    else
                    {
                        if (ticket.Submiter == userId)
                        {
                            apiJsonCommentSenderTr.type = "Submitter";
                            if (typeMsg == 2)
                            {
                                if (ticket.NextCommenter == userId)
                                {
                                    ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));
                                }
                                ticket.NextCommenter = ticket.Responder;
                                _ticketBusinessService.Edit(ticket);
                            }
                        }
                        apiJsonCommentSenderTr.type = "Participant";
                    }
                }
                else if (ticket.TicketCategoryId == 9)
                {
                    apiJsonCommentSenderTr.type = "Submitter";
                    if (typeMsg == 2)
                    {
                        if (ticket.NextCommenter == userId)
                        {
                            ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));
                        }
                        ticket.NextCommenter = ticket.Responder;
                        _ticketBusinessService.Edit(ticket);
                    }
                }

                List<ApiJsonCommentImageTR> listAttachment = new List<ApiJsonCommentImageTR>();

                var attachments = _discussionAttachmentBusinessService.GetByDiscussionId(ticketDiscussionId);
                string attachmentsPath = "/Upload/Discussion/";

                if (typeMsg == 2)
                {
                    attachments = _noteAttachmentBusinessService.GetByNoteId(ticketNoteId);
                    attachmentsPath = "/Upload/Note/";
                }

                if (attachments != null)
                    foreach (var attachment in attachments)
                    {
                        listAttachment.Add(new ApiJsonCommentImageTR { src = WebConfigure.GetDomain() + attachmentsPath + attachment.Name });
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
                var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);
                var datauid = new List<int>();
                var userD = new List<string>();
                foreach (var item in participants)
                {
                    datauid.Add(item.UserId);
                }
                var iduserdevice = _ticketParticipantBusinessService.Getuserdeviceforasnote(datauid);
                foreach (var item in iduserdevice)
                {
                    userD.Add(item.PlayerId);
                }


                if (typeMsg != 1)
                {
                    if (typeMsg == 2)
                    {
                        title = ticket.TicketNo + " - " + ticket.Title + " - Reply As Note";
                        Description = user.Name + (ticket.Submiter == user.UserId ? " (Submiter)" : " (Responder)") + " : " + msg;
                    }
                    else if (typeMsg == 3)
                    {
                        title = ticket.TicketNo + " - " + ticket.Title + " - Write Resolution";
                        Description = user.Name + " (Responder) : " + msg;
                    }
                    Onesignal.PushNotif(Description, userD, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                    if (ticket.TicketCategoryId != 9)
                    {
                        Email.GetEmailTagTsicsCommentTR(ticket);
                        Email.GetEmailTagTsicsCommentTR(ticket, true);
                        //Email.GetEmailTagTsicsCommentTR(ticketId, ticket.Submiter);
                    }
                    else
                    {
                        Email.GetEmailTagTsicsCommentHelpDesk(ticketId, Convert.ToInt32(Session["userid"]));
                    }
                }
                else
                {
                    title = ticket.TicketNo + " - " + ticket.Title + " - Reply";
                    Description = user.Name + (ticket.Submiter == user.UserId ? " (Submiter)" : (ticket.Responder == user.UserId ? " (Responder)" : " (Participant)")) + " : " + msg;

                    Onesignal.PushNotif(Description, userD, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                }
                return Json(new { status = apiJsonStatus, data = apiJsonCommentTrList });

            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddDiscussionBackend(FormCollection formCollection)
        {
            if (Session["useridbackend"] == null)
                return RedirectToAction("Login", "Account");

            int userId = 0;

            if (Session["useridbackend"] != null)
            {
                userId = Convert.ToInt32(Session["useridbackend"]);
            }
            string name = "";
            if (Session["namebackend"] != null)
            {
                name = Session["namebackend"].ToString();
            }

            string msg = formCollection["insert-comment"];
            int typeMsg = Convert.ToInt32(formCollection["chat-type"]);
            int ticketId = Convert.ToInt32(formCollection["TicketId"]);
            int ticketDiscussionId = 0;
            int ticketNoteId = 0;
            DateTime dateTimeNow = DateTime.Now;
            Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
            User user = _userBusinessService.GetDetail(userId);
            if (ticket.Responder == 0)
            {
                ticket.Responder = userId;
            }
            ticket = _ticketBusinessService.Edit(ticket);

            ApiJsonStatus apiJsonStatus = new ApiJsonStatus()
            {
                code = 200,
                message = "ok"
            };
            TimeSpan respond = ticket.LastReply == null ? (DateTime.Now.Subtract(ticket.CreatedAt.Value))/*TotalSeconds*/ : (DateTime.Now.Subtract(ticket.LastReply.Value))/*TotalSeconds*/;
            if (typeMsg == 1)
            {
                TicketDiscussion ticketDiscussion = new TicketDiscussion()
                {
                    TicketId = ticketId,
                    CreatedAt = dateTimeNow,
                    UserId = userId,
                    Description = msg,
                    TicketNoteId = 0,// -> new comment don't have note
                    Status = 1,
                    //RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    RespondTime = null
                };

                ticketDiscussionId = _ticketDiscussionBusinessService.Add(ticketDiscussion).TicketDiscussionId;

                ticket.LastReply = dateTimeNow;
                ticket = _ticketBusinessService.Edit(ticket);
            }
            else if (typeMsg == 2)
            {
                TicketNote ticketNote = new TicketNote()
                {
                    TicketId = ticketId,
                    CreatedAt = dateTimeNow,
                    UserId = userId,
                    Description = msg,
                    Status = 1,
                    RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                };
                ticketNoteId = _ticketNoteBusinessService.Add(ticketNote).TicketNoteId;
                _ticketDiscussionBusinessService.SetNote(ticketId, ticketNoteId);

                ticket.LastReply = dateTimeNow;
                ticket.LastStatusDate = dateTimeNow;
                ticket = _ticketBusinessService.Edit(ticket);
            }
            else if (typeMsg == 3)
            {
                WriteResolutionBackend(formCollection, ticket, dateTimeNow);
                return Json(new { status = apiJsonStatus, data = new object() });
            }

            if (Request.Files.Count > 0)
            {
                for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                {
                    var file = Request.Files[i];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        switch (typeMsg)
                        {
                            case 1:// -> Comment
                                DiscussionAttachment discussionAttachment = new DiscussionAttachment()
                                {
                                    TicketDiscussionId = ticketDiscussionId,
                                    Name = Common.AddDiscussionAttachments(file, Path.GetFileNameWithoutExtension(file.FileName)),
                                    Status = 1
                                };

                                _discussionAttachmentBusinessService.Add(discussionAttachment);
                                break;
                            default:// -> Note
                                NoteAttachment noteAttachment = new NoteAttachment()
                                {
                                    TicketNoteId = ticketNoteId,
                                    Name = Common.AddNoteAttachments(file, Path.GetFileNameWithoutExtension(file.FileName)),
                                    Status = 1
                                };

                                _noteAttachmentBusinessService.Add(noteAttachment);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, response);
                    }
                }
            }

            List<ApiJsonCommentTR> apiJsonCommentTrList = new List<ApiJsonCommentTR>();

            ApiJsonCommentDateTR apiJsonCommentDateTr = new ApiJsonCommentDateTR()
            {
                day = dateTimeNow.ToString("dddd, dd MMMM yyyy"),
                time = dateTimeNow.ToString("hh:mm")
            };

            ApiJsonCommentSenderTR apiJsonCommentSenderTr = new ApiJsonCommentSenderTR
            {
                name = name, is_verified = true, type = "Responder"
            };

            if (typeMsg == 2)
            {
                if (ticket.NextCommenter == userId)
                {
                    ticket.DueDateAnswer = DateTime.Now.AddDays(Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetRulesDay()));
                }
                ticket.NextCommenter = ticket.Submiter;
                _ticketBusinessService.Edit(ticket);
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
                    listAttachment.Add(new ApiJsonCommentImageTR { src = Common.ImageToBase64(attachmentsPath + attachment.Name) });
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
            var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);
            var datauid = new List<int>();
            var userD = new List<string>();
            foreach (var item in participants)
            {
                datauid.Add(item.UserId);
            }
            var iduserdevice = _ticketParticipantBusinessService.Getuserdeviceforasnote(datauid);
            foreach (var item in iduserdevice)
            {
                userD.Add(item.PlayerId);
            }
            String title = "", Description = "";
            if (typeMsg == 2)
            {
                title = ticket.TicketNo + " - " + ticket.Title + " - Reply As Note";
                Description = user.Name + (ticket.Submiter == user.UserId ? " (Submiter)" : " (Responder)") + " : " + msg;
            }
            else if (typeMsg == 3) {
                title = ticket.TicketNo + " - " + ticket.Title + " - Write Resolution";
                Description = user.Name + " (Responder) : " + msg;
            }

            Onesignal.PushNotif(Description, userD, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
            return Json(new { status = apiJsonStatus, data = apiJsonCommentTrList });
        }

        public ViewResult Summary(string sortOrder, string currentFilter, string searchString, int? page, Boolean library = false)
        {
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
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

            var tickets = _ticketBusinessService.GetQueryableSummary();

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(ticket =>
                    ticket.Title.Contains(searchString) ||
                    ticket.TicketNo.Contains(searchString)
                    );
            }
            switch (sortOrder)
            {
                case "DateDesc":
                    tickets = tickets.OrderByDescending(t => t.CreatedAt);
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
                    tickets = tickets.OrderBy(s => s.CreatedAt);
                    break;
            }

            ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
            var ticketPreview = (tickets);

            List<InvolvedUser> submiters = new List<InvolvedUser>();
            List<InvolvedUser> responders = new List<InvolvedUser>();
            List<TicketPreview> solvedDate = new List<TicketPreview>();
            List<Ticket> escalate = new List<Ticket>();
            if (ticketPreview != null)
            {
                foreach (var ticket in ticketPreview)
                {
                    User submiterData = _userBusinessService.GetDetail(ticket.Submiter);
                    ticket.EscalateId = _ticketBusinessService.IsEscalated(ticket.TicketId, Convert.ToInt32(Session["userid"])) == true ? 1 : 0;
                    if (submiterData != null)
                    {
                        InvolvedUser submiter = new InvolvedUser()
                        {
                            TicketNo = ticket.TicketNo,
                            EmployeName = _userBusinessService.GetDetail(ticket.Submiter).Name,
                            AreaName = _userBusinessService.GetDetail(ticket.Submiter).AreaName
                        };
                        submiters.Add(submiter);
                    }
                    else
                    {
                        InvolvedUser submiter = new InvolvedUser()
                        {
                            TicketNo = ticket.TicketNo,
                            EmployeName = "-",
                            AreaName = "-"
                        };
                        submiters.Add(submiter);
                    }

                    if (ticket.Responder != 0)
                    {
                        User responderData = _userBusinessService.GetDetail(ticket.Responder);
                        TicketResolution SolvedDateData = _ticketResolutionBusinessService.GetByTicket(ticket.TicketId);

                        if (responderData != null)
                        {

                            InvolvedUser responder = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = _userBusinessService.GetDetail(ticket.Responder).Name,
                                AreaName = _userBusinessService.GetDetail(ticket.Responder).AreaName
                            };

                            responders.Add(responder);
                        }
                        else
                        {
                            InvolvedUser responder = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = "-",
                                AreaName = "-"
                            };
                            responders.Add(responder);
                        }

                        if (SolvedDateData != null)
                        {
                            TicketPreview solveddate = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                SolvedDate = _ticketResolutionBusinessService.GetByTicket(ticket.TicketId).CreatedAt
                            };
                            solvedDate.Add(solveddate);
                        }
                        else
                        {
                            TicketPreview solveddate = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                SolvedDate = null
                            };
                            solvedDate.Add(solveddate);
                        }

                    }
                    else
                    {

                        if (ticket.Status != 1)
                        {
                            InvolvedUser responder = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = "TREND Admin",
                                AreaName = "-"
                            };
                            TicketPreview solveddate = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                SolvedDate = null
                            };
                            solvedDate.Add(solveddate);
                            responders.Add(responder);
                        }
                        else
                        {
                            InvolvedUser responder = new InvolvedUser()
                            {
                                TicketNo = ticket.TicketNo,
                                EmployeName = "-",
                                AreaName = "-"
                            };
                            TicketPreview solveddate = new TicketPreview()
                            {
                                TicketNo = ticket.TicketNo,
                                SolvedDate = null
                            };
                            solvedDate.Add(solveddate);
                            responders.Add(responder);
                        }
                    }

                }
            }
            ViewBag.SolvedDate = solvedDate;
            ViewBag.Submiters = submiters;
            ViewBag.Responders = responders;
            int pageSize = 10000;
            int pageNumber = (page ?? 1);
            return View(ticketPreview.ToPagedList(pageNumber, pageSize));
        }

        public string GetCategoryName(int id)
        {
            return _ticketCategoryBusinessService.GetDetail(id).Name;
        }

        private void SetListDropdown(int userId)
        {
            ViewBag.ListUser = _userBusinessService.GetUserListByHierarchy(userId);
            ViewBag.AllUserActive = _userBusinessService.GetListActive();
        }

        private void AddParticipant(FormCollection formCollection, int ticketId)
        {
            if (formCollection.AllKeys.Contains("ParticipantList"))
            {
                string[] part = formCollection["ParticipantList"].Split(',');
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (part != null)
                {
                    for (int i = 0, iLen = part.Length; i < iLen; i++)
                    {
                        TicketParcipant ticketParticipant = new TicketParcipant()
                        {
                            TicketId = ticketId,
                            UserId = Convert.ToInt32(part[i]),
                            CreatedAt = DateTime.Now,
                            Status = 1
                        };
                        _ticketParticipantBusinessService.Add(ticketParticipant);
                    }
                }
            }
        }

        private void UpdateParticipant(FormCollection formCollection, int ticketId)
        {
            string[] part = formCollection["ParticipantList"].Split(',');
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (part != null)
            {
                var currentParticipants = _ticketParticipantBusinessService.GetByTicket(ticketId);
                if (currentParticipants.Count > 0)
                {
                    _ticketParticipantBusinessService.BulkDeleteByTicket(ticketId);
                }
                for (int i = 0, iLen = part.Length; i < iLen; i++)
                {
                    TicketParcipant ticketParticipant = new TicketParcipant()
                    {
                        TicketId = ticketId,
                        UserId = Convert.ToInt32(part[i]),
                        CreatedAt = DateTime.Now,
                        Status = 1
                    };
                    _ticketParticipantBusinessService.Add(ticketParticipant);
                }
            }
        }

        private void AddTechnicalRequestTags(FormCollection formCollection, int ticketId)
        {
            ArticleTag articleTag = new ArticleTag();

            string[] tags = formCollection["input-tag[]"].Split(',');

            for (int i = 0, iLen = tags.Length; i < iLen; i++)
            {
                articleTag.Name = tags[i];
                articleTag.Freq = 1;
                articleTag.TicketId = ticketId;

                _articleTagBusinessService.Add(articleTag);
            }
        }

        private void UpdateTechnicalRequestTags(FormCollection formCollection, int ticketId)
        {
            string[] tags = formCollection["input-tag[]"].Split(',');
            var currentTags = _articleTagBusinessService.GetTagsByTicket(ticketId);
            if (currentTags != null)
            {
                List<TicketTagsAPI> listCurrentTagApi = new List<TicketTagsAPI>();
                foreach (var currentTag in currentTags)
                {
                    TicketTagsAPI tagApi = new TicketTagsAPI { Name = currentTag.Name };
                    listCurrentTagApi.Add(tagApi);
                }

                foreach (var currentTagApi in listCurrentTagApi)
                {
                    if (!tags.Any(currentTagApi.Name.Contains))
                    {
                        _articleTagBusinessService.Delete(ticketId, currentTagApi.Name);
                    }
                }
            }

            ArticleTag articleTag = new ArticleTag();

            for (int i = 0, iLen = tags.Length; i < iLen; i++)
            {
                if (!_articleTagBusinessService.IsThisTagExists(ticketId, tags[i]))
                {
                    articleTag.Name = tags[i];
                    articleTag.Freq = 1;
                    articleTag.TicketId = ticketId;

                    _articleTagBusinessService.Add(articleTag);
                }
            }
        }

        private void AddTechnicalRequestAttachments(int ticketId, string ticketNo, string[] fileLevel)
        {
            TicketAttachment ticketAttachment = new TicketAttachment();

            for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
            {
                var file = Request.Files[i];
                string response = Common.ValidateFileUpload(file);

                if (response.Equals("true"))
                {
                    string dateString = DateTime.Now.ToString("yyyyMMddHmmss");
                    ticketAttachment.TicketId = ticketId;
                    ticketAttachment.Name = Common.UploadFile(file, Path.GetFileNameWithoutExtension(file.FileName) + "-" + ticketNo + "-" + dateString + "-" + i);
                    ticketAttachment.LevelUser = fileLevel[i];
                    ticketAttachment.Status = 1;

                    _ticketAttachmentBusinessService.Add(ticketAttachment);
                }
            }
        }

        private void UpdateTechnicalRequestAttachments(FormCollection formCollection, int ticketId)
        {
            var currentAttachments = _ticketAttachmentBusinessService.GetByTicketId(ticketId);
            string[] submitedCurrentAttachments = formCollection["current-attachments[]"].Split(',');

            if (currentAttachments.Count == submitedCurrentAttachments.Length)
            {
                string[] submitedCurrentAttachmentsLevel = formCollection["current-file_level[]"].Split(',');
                for (int i = 0, iMax = submitedCurrentAttachments.Length; i < iMax; i++)
                {
                    TicketAttachment ticketAttachment = _ticketAttachmentBusinessService.GetDetail(Convert.ToInt32(submitedCurrentAttachments[i]));
                    ticketAttachment.LevelUser = submitedCurrentAttachmentsLevel[i];

                    _ticketAttachmentBusinessService.Edit(ticketAttachment);
                }
            }
            else
            {
                List<int> currentAttachmentsId = new List<int>();
                foreach (var item in currentAttachments)
                {
                    currentAttachmentsId.Add(item.Id);
                }

                int[] currentAttachmentsIdArray = currentAttachmentsId.ToArray();

                for (int i = 0, iMax = currentAttachmentsIdArray.Length; i < iMax; i++)
                {
                    if (!submitedCurrentAttachments.Contains(currentAttachmentsIdArray[i].ToString()))
                    {
                        TicketAttachment ticketAttachment = _ticketAttachmentBusinessService.GetDetail(currentAttachmentsIdArray[i]);
                        _ticketAttachmentBusinessService.Delete(ticketAttachment);
                    }
                }
            }
        }

        /// <summary>

        /// </summary>
        /// <param name="formCollection"></param>
        /// 

        private void WriteResolutionBackend(FormCollection formCollection, Ticket ticket, DateTime dateTimeNow)
        {
            int ticketId = int.Parse(formCollection["TicketId"]);
            int userIdBackend = 0;
            if (Session["useridbackend"] != null)
            {
                userIdBackend = Convert.ToInt32(Session["useridbackend"]);
            }
            string description = formCollection["insert-comment"];
            int rate = Convert.ToInt32(formCollection["Rate"]);
            string rateDescription = formCollection["review"];
            int userToRate = int.Parse(formCollection["UserToRate"]);

            TimeSpan respond = ticket.LastReply == null ? (dateTimeNow.Subtract(ticket.CreatedAt.Value)) : (dateTimeNow.Subtract(ticket.LastReply.Value));
            ticket.LastReply = dateTimeNow;
            ticket.LastStatusDate = dateTimeNow;
            ticket = _ticketBusinessService.Edit(ticket);

            TicketResolution ticketResolution = new TicketResolution()
            {
                TicketId = ticketId,
                UserId = userIdBackend,
                Description = description,
                CreatedAt = DateTime.Now,
                Status = 1,
                RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
            };

            _ticketResolutionBusinessService.Add(ticketResolution);
            _ticketBusinessService.RequestToClose(ticketId);

            Rating rating = new Rating()
            {
                UserId = userToRate,
                TicketId = ticketId,
                Rate = rate,
                Description = rateDescription,
                RatedFrom = userIdBackend,
                CreatedAt = dateTimeNow,
                RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
            };
            _ratingBusinessService.Add(rating);
            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, userIdBackend, false);
            Email.GetEmailTagTsicsCommentHelpDesk(ticketId, userIdBackend, true);

        }
        private void WriteResolution(FormCollection formCollection, Ticket ticket, DateTime dateTimeNow)
        {
            int ticketId = int.Parse(formCollection["TicketId"]);
            int userId = 0;
            if (Session["userid"] != null)
            {
                userId = Convert.ToInt32(Session["userid"]);
            }
            string description = formCollection["insert-comment"];

            int rate = String.IsNullOrWhiteSpace(formCollection["Rate"].ToString()) ? 0 : int.Parse(formCollection["Rate"]);
            string rateDescription = formCollection["review"];
            int userToRate = String.IsNullOrWhiteSpace(formCollection["UserToRate"].ToString()) ? 0 : int.Parse(formCollection["UserToRate"]);

            TimeSpan respond = ticket.LastReply == null ? (dateTimeNow.Subtract(ticket.CreatedAt.Value))/*TotalSeconds*/ : (dateTimeNow.Subtract(ticket.LastReply.Value))/*TotalSeconds*/;
            TicketResolution ticketResolution = new TicketResolution()
            {
                TicketId = ticketId,
                UserId = userId,
                Description = description,
                CreatedAt = dateTimeNow,
                Status = 1,
                RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
            };
            ticket.LastReply = dateTimeNow;
            ticket.LastStatusDate = dateTimeNow;
            _ticketBusinessService.Edit(ticket);
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
            Email.GetEmailTagTsicsCommentTR(ticket);
            Email.GetEmailTagTsicsCommentTR(ticket, true);
        }

        [HttpGet]
        public ActionResult Reopen(int? id)
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

                if (id == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                var closedTicket = _ticketBusinessService.GetDetail(int.Parse(id.ToString()));

                if (closedTicket == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                ViewBag.ReopenFrom = closedTicket;
                ViewBag.CategoryName = _ticketCategoryBusinessService.GetDetail(closedTicket.TicketCategoryId).Name;
                ViewBag.newTRNo = _ticketBusinessService.GetNewTicketNoByCategory(closedTicket.TicketCategoryId);
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));

                SetListDropdown(int.Parse(Session["userid"].ToString()));
                return View(Regex.Replace(ViewBag.CategoryName, " ", ""));
            }
        }

        [HttpPost]
        public ActionResult Escalate(Ticket mTicket)
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
                int ticketId = mTicket.TicketId;
                User currentResponder = _userBusinessService.GetDetail(Convert.ToInt32(Session["userid"]));
                User newResponder = _userBusinessService.GetDetail(mTicket.Responder);
                Ticket ticketData = _ticketBusinessService.Escalate(ticketId, currentResponder.UserId, newResponder.UserId);
                var playerid = new List<string>();
                var dataUid = new List<int>();
                var participants = _ticketParticipantBusinessService.GetByTicket(ticketId);
                foreach (var i in participants)
                {
                    dataUid.Add(i.UserId);
                }
                var iduserdevice = _ticketParticipantBusinessService.Getuserdeviceforasnote(dataUid);
                foreach (var p in iduserdevice)
                {
                    playerid.Add(p.PlayerId);
                }

                String title = ticketData.TicketNo + " - " + ticketData.Title + " - Escalated",
                    content = currentResponder.Name + " (Responder) Has Escalated To " + newResponder.Name;
                playerid.Add(_userBusinessService.GetDetail(ticketData.Responder).PlayerId);
                playerid.Add(_userBusinessService.GetDetail(ticketData.Submiter).PlayerId);
                Onesignal.PushNotif(content, playerid, title, ticketId, ticketData.TicketNo, ticketData.TicketCategoryId, ticketData.Description);

                return RedirectToAction("Details/" + ticketId, "TechnicalRequest");
            }
        }

        private Ticket PlottingWebFormCreateTechnicalRequest(FormCollection formCollection, string categoryName)
        {
            Ticket ticket = Factory.Create<Ticket>("Ticket", ClassType.clsTypeDataModel);
            DateTime Now = DateTime.Now;
            User user = _userBusinessService.GetDetail(Convert.ToInt32(Session["userid"]));
            MEP unit = new MEP();
            if (formCollection.AllKeys.Contains("SerialNumber") && formCollection["SerialNumber"] != null && formCollection["SerialNumber"] != "")
            {
                string serialNumber = formCollection["SerialNumber"];
                var mep = _mepBusinessService.GetBySn(serialNumber);
                unit = mep ?? new MEP();
            }
            var submitType = formCollection["type"];

            ticket.Status = submitType == "draft" ? 1 : 2;
            if (formCollection["Title"] != "")
            {
                ticket.Title = formCollection["Title"];
            }
            ticket.CreatedAt = Now;
            ticket.UpdatedAt = Now;

            if (submitType != "draft") {
                ticket.DueDateAnswer = Now.AddDays(Common.NumberOfWorkDays(Now, WebConfigure.GetRulesDay()));
                ticket.ResponderStatus = ticket.DueDateAnswer.Value.Subtract(Now).Ticks;
                ticket.SubmiterStatus = 0;
            }
            else
            {
                ticket.ResponderStatus = 0;
                ticket.SubmiterStatus = 0;
            }
            ticket.DelegateId = 0;
            ticket.EscalateId = 0;
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
            if (formCollection.AllKeys.Contains("ReferenceTicket"))
            {
                ticket.ReferenceTicket = Convert.ToInt32(formCollection["ReferenceTicket"]);
            }

            switch (categoryName)
            {
                case "Product Health":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Product Health").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + " - " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"] != "" ? formCollection["PartCausingFailure"] : null;
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.Description = formCollection["Description"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Parts Technical":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Parts Technical").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.Description = formCollection["Description"];
                    ticket.PartsNumber = formCollection["PartsNumber"] != "" ? formCollection["PartsNumber"] : null;
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Dimension":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Dimension").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.PartsNumber = formCollection["PartsNumber"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    ticket.Description = formCollection["Description"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    break;
                case "Reference":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Reference").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SMU = formCollection["SMU"];
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.Description = formCollection["Description"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Warranty Assistance":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Warranty Assistance").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.WarrantyTypeId = int.Parse(formCollection["WarrantyTypeId"]);
                    ticket.Description = formCollection["Description"];
                    ticket.ServiceOrderNumber = formCollection["ServiceOrderNumber"] != "" ? formCollection["ServiceOrderNumber"] : null;
                    ticket.ClaimNumber = formCollection["ClaimNumber"] != "" ? formCollection["ClaimNumber"] : null;
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Goodwill Assistance":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Goodwill Assistance").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.Description = formCollection["Description"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    ticket.ServiceOrderNumber = formCollection["ServiceOrderNumber"];
                    ticket.ClaimNumber = formCollection["ClaimNumber"];
                    break;
                case "Password":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Password").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.Description = formCollection["Description"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.ServiceToolSN = formCollection["ServiceToolSN"] != "" ? formCollection["ServiceToolSN"] : null;
                    ticket.EngineSN = formCollection["EngineSN"] != "" ? formCollection["EngineSN"] : null;
                    ticket.EcmSN = formCollection["EcmSN"] != "" ? formCollection["EcmSN"] : null;
                    ticket.TotalTattletale = formCollection["TotalTattletale"] != "" ? formCollection["TotalTattletale"] : null;
                    ticket.ReasonCode = formCollection["ReasonCode"] != "" ? formCollection["ReasonCode"] : null;
                    ticket.FromInterlock = formCollection["FromInterlock"] != "" ? formCollection["FromInterlock"] : null;
                    ticket.ToInterlock = formCollection["ToInterlock"] != "" ? formCollection["ToInterlock"] : null;
                    ticket.DiagnosticClock = formCollection["DiagnosticClock"] != "" ? formCollection["DiagnosticClock"] : null;
                    ticket.SoftwarePartNumber = formCollection["SoftwarePartNumber"] != "" ? formCollection["SoftwarePartNumber"] : null;
                    ticket.TestSpec = formCollection["TestSpec"] != "" ? formCollection["TestSpec"] : null;
                    ticket.TestSpecBrakeSaver = formCollection["TestSpecBrakeSaver"] != "" ? formCollection["TestSpecBrakeSaver"] : null;

                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Telematics":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Telematics").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"] != "" ? formCollection["PartCausingFailure"] : null;
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.Description = formCollection["Description"];
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;
                case "Help Desk":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Help Desk").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.Description = formCollection["Description"];
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    break;
                case "Condition Monitoring":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Condition Monitoring").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.Description = formCollection["Description"];
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"] != "" ? formCollection["PartCausingFailure"] : null;
                    ticket.Responder = formCollection["Responder"] != "" ? Convert.ToInt32(formCollection["Responder"]) : 0;
                    ticket.Submiter = Convert.ToInt32(Session["userid"]);
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.NextCommenter = ticket.Responder;
                    break;

            }

            return ticket;
        }

        private Ticket PlottingWebFormUpdateTechnicalRequest(FormCollection formCollection, string categoryName)
        {
            DateTime Now = DateTime.Now;
            Ticket ticket = _ticketBusinessService.GetDetail(Convert.ToInt32(formCollection["TicketId"]));

            MEP unit = new MEP();
            if (formCollection.AllKeys.Contains("SerialNumber") && formCollection["SerialNumber"] != null && formCollection["SerialNumber"] != "")
            {
                string serialNumber = formCollection["SerialNumber"];
                var mep = _mepBusinessService.GetBySn(serialNumber);
                unit = mep ?? new MEP();
            }
            var submitType = formCollection["type"];

            if (submitType == "draft")
            {
                ticket.Status = 1;
                if (categoryName != "Help Desk")
                {
                    int i = 0;
                    if (!int.TryParse(formCollection["Responder"].ToString(), out i)) {
                        ticket.Responder = 0;
                    }
                    else {
                        ticket.Responder = Convert.ToInt32(formCollection["Responder"]);
                    }
                }
            }
            else
            {
                if (categoryName != "Help Desk")
                {
                    if (ticket.Submiter == Convert.ToInt32(Session["userid"])) {
                        if (Convert.ToInt32(formCollection["Responder"]) == 0)
                        {
                            ticket.Responder = 0;
                        }
                        else
                        {
                            ticket.Responder = Convert.ToInt32(formCollection["Responder"]);
                        }
                    }
                    else if (ticket.Responder == Convert.ToInt32(Session["userid"]))
                    {
                        ticket.Responder = Convert.ToInt32(Session["userid"]);
                    }

                }
                ticket.LastStatusDate = Now;
                ticket.Status = 2;
                if (Convert.ToString(Session["userid"]).Equals(ticket.Submiter))
                {
                    ticket.NextCommenter = ticket.Responder;
                }

                if (ticket.Responder != Convert.ToInt32(Session["userid"]))
                {
                    ticket.DueDateAnswer = Now.AddDays(Common.NumberOfWorkDays(Now, WebConfigure.GetRulesDay()));
                    ticket.ResponderStatus = ticket.DueDateAnswer.Value.Subtract(Now).Ticks;
                    ticket.SubmiterStatus = 0;
                }
            }
            ticket.Title = formCollection["Title"];
            ticket.UpdatedAt = Now;
            ticket.DelegateId = 0;
            ticket.EscalateId = 0;
            switch (categoryName)
            {
                case "Product Health":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.EmailCC = formCollection["EmailCC"];
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.Description = formCollection["Description"];

                    break;
                case "Parts Technical":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMU = formCollection["SMU"];
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.EmailCC = formCollection["EmailCC"];
                    ticket.PartsNumber = formCollection["PartsNumber"];
                    ticket.Description = formCollection["Description"];
                    ticket.PartsDescription = formCollection["PartsDescription"];

                    break;
                case "Dimension":
                    ticket.PartsNumber = formCollection["PartsNumber"];
                    ticket.Description = formCollection["Description"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.EmailCC = formCollection["EmailCC"];

                    break;
                case "Reference":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.Description = formCollection["Description"];
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.EmailCC = formCollection["EmailCC"];


                    break;
                case "Warranty Assistance":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.WarrantyTypeId = int.Parse(formCollection["WarrantyTypeId"]);
                    ticket.ServiceOrderNumber = formCollection["ServiceOrderNumber"] != "" ? formCollection["ServiceOrderNumber"] : null;
                    ticket.ClaimNumber = formCollection["ClaimNumber"] != "" ? formCollection["ClaimNumber"] : null;
                    ticket.Description = formCollection["Description"];
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.EmailCC = formCollection["EmailCC"];

                    break;

                case "Help Desk":
                    ticket.Description = formCollection["Description"];
                    break;
                case "Goodwill Assistance":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Goodwill Assistance").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    ticket.Description = formCollection["Description"];
                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    ticket.ServiceOrderNumber = formCollection["ServiceOrderNumber"];
                    ticket.ClaimNumber = formCollection["ClaimNumber"];

                    break;
                case "Password":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"];
                    ticket.Description = formCollection["Description"];
                    ticket.EmailCC = formCollection["EmailCC"];
                    ticket.ServiceToolSN = formCollection["ServiceToolSN"];
                    ticket.EngineSN = formCollection["EngineSN"];
                    ticket.EcmSN = formCollection["EcmSN"];
                    ticket.TotalTattletale = formCollection["TotalTattletale"];
                    ticket.ReasonCode = formCollection["ReasonCode"];
                    ticket.SoftwarePartNumber = formCollection["SoftwarePartNumber"];
                    ticket.DiagnosticClock = formCollection["DiagnosticClock"];
                    ticket.EmailCC = formCollection["EmailCC"];
                    ticket.FromInterlock = formCollection["FromInterlock"];
                    ticket.ToInterlock = formCollection["ToInterlock"];
                    ticket.TestSpec = formCollection["TestSpec"];
                    ticket.TestSpecBrakeSaver = formCollection["TestSpecBrakeSaver"];

                    break;
                case "Telematics":
                    ticket.TicketCategoryId = _ticketCategoryBusinessService.GetByName("Telematics").TicketCategoryId;
                    ticket.TicketNo = _ticketBusinessService.GetNewTicketNoByCategory(ticket.TicketCategoryId);
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.Make = unit.Make;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.SMU = formCollection["SMU"] != "" ? formCollection["SMU"] : null;
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"] != "" ? formCollection["PartCausingFailure"] : null;
                    ticket.PartsDescription = formCollection["PartsDescription"] != "" ? formCollection["PartsDescription"] : null;
                    ticket.Description = formCollection["Description"];

                    ticket.EmailCC = formCollection["EmailCC"] != "" ? formCollection["EmailCC"] : null;
                    break;
                case "Condition Monitoring":
                    ticket.SerialNumber = formCollection["SerialNumber"];
                    ticket.Customer = unit.CustomerName;
                    ticket.Location = unit.ShipToAddress;
                    ticket.Make = unit.Make;
                    ticket.MepBranch = unit.SalesOffice + ", " + unit.SalesOfficeDescription;
                    ticket.DeliveryDate = unit.DeliveryDate;
                    ticket.ArrangementNo = unit.ArrNumber;
                    ticket.Family = unit.PT + " - " + unit.PTDescription;
                    ticket.Model = unit.Model;
                    ticket.SMU = formCollection["SMU"];
                    ticket.SMUDate = unit.SMUUpDate;
                    ticket.Description = formCollection["Description"];
                    ticket.PartCausingFailure = formCollection["PartCausingFailure"];
                    ticket.EmailCC = formCollection["EmailCC"];
                    ticket.PartsDescription = formCollection["PartsDescription"];
                    break;
            }


            return ticket;
        }
        private void SendNotif(Ticket ticket, FormCollection formCollection)
        {
            if (formCollection.AllKeys.Contains("ParticipantList"))
            {
                string[] submitedParticipants = formCollection["ParticipantList"].Split(',');
                var dataud = new List<String>();
                var dataresponder = new List<String>();
                for (int i = 0, iMax = submitedParticipants.Length; i < iMax; i++)
                {
                    if (_userBusinessService.GetDetail(Convert.ToInt32(submitedParticipants[i])).PlayerId != null)
                    {
                        dataud.Add(_userBusinessService.GetDetail(Convert.ToInt32(submitedParticipants[i])).PlayerId);
                    }
                }
                String title = "";
                if (ticket.Status == 2)
                {
                    if (_userBusinessService.GetDetail(ticket.Responder).PlayerId != null)
                    {
                        title = "(Waiting Your Feedback) " + ticket.TicketNo + " - " + ticket.Title;
                        dataresponder.Add(_userBusinessService.GetDetail(ticket.Responder).PlayerId);
                        Onesignal.PushNotif(ticket.Description, dataresponder, title, ticket.TicketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
                    }
                    if (_userBusinessService.GetDetail(ticket.Submiter).PlayerId != null)
                    {
                        dataud.Add(_userBusinessService.GetDetail(ticket.Submiter).PlayerId);
                    }
                }
                title = "(PRA) " + ticket.Title + " - " + ticket.TicketNo;
                Onesignal.PushNotif(ticket.Description, dataud, title, ticket.TicketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);
            }
        }

        public ActionResult DownloadExcel()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Upload/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "telematics.xlsx");
            string fileName = "telematics.xlsx";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult GeneratePdf(int? ticketId, int? tec = 1, int? res = 1, int? not = 1, int? com = 1)
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

                if (ticketId == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                if (tec == null && res == null && not == null && com == null)
                    return RedirectToAction("Index", "TechnicalRequest");

                Ticket ticket = _ticketBusinessService.GetDetail(Convert.ToInt32(ticketId));

                #region Involved Users

                List<int> involvedIdList = new List<int> { ticket.Submiter };
                if (ticket.Responder != 0)
                {
                    involvedIdList.Add(ticket.Responder);
                }

                var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);

                if (participants != null)
                {
                    foreach (var participantItem in participants)
                    {
                        involvedIdList.Add(participantItem.UserId);
                    }
                }

                ViewBag.IsInvolved = false;

                if (involvedIdList.Contains(Convert.ToInt32(Session["userid"])))
                    ViewBag.IsInvolved = true;
                #endregion

                if (ticket.Status.Equals(3))
                {
                    TicketResolution ticketResolution = _ticketResolutionBusinessService.GetByTicket(Convert.ToInt32(ticketId));

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
                                Type = Path.GetExtension(attachment.Name)
                            });
                        }
                        else
                        {
                            listAttachment.Add(new TicketAttachmentsAPI
                            {
                                Id = attachment.TicketAttachmentId,
                                Name = attachment.Name,
                                Level = attachment.LevelUser,
                                Type = Path.GetExtension(attachment.Name)
                            });
                        }
                    }
                }
                ViewBag.Attachments = listAttachment;
                #endregion

                ViewBag.UserLevelName = _userRoleBusinessService.GetDetail(Convert.ToInt32(Session["role"])).Description;
                ViewBag.CategoryName = _ticketCategoryBusinessService.GetDetail(ticket.TicketCategoryId).Name;
                ViewBag.Submiter = _userBusinessService.GetDetail(ticket.Submiter);
                ViewBag.SubmiterUserId = ticket.Submiter;
                ViewBag.Responder = _userBusinessService.GetDetail(ticket.Responder);
                ViewBag.ResponderUserId = ticket.Responder;
                ViewBag.Tags = _articleTagBusinessService.GetTagsByTicket(ticket.TicketId);
                ViewBag.Ticket = ticket;
                ViewBag.Recent = GetRecentTR(Convert.ToInt32(Session["userid"]));
                SetListDropdown(int.Parse(Session["userid"].ToString()));

                ViewBag.CurrentUser = _userBusinessService.GetDetail(Convert.ToInt32(Session["userid"]));

                return View("Pdf", ticket);
            }
        }

        /// <summary>

        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RateResponder(FormCollection formCollection)
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
                DateTime dateTimeNow = DateTime.Now;
                int ticketId = int.Parse(formCollection["TicketId"]);
                int userId = int.Parse(Session["userid"].ToString());
                int rate = int.Parse(formCollection["Rate"]);
                string rateDescription = formCollection["review"];
                int userToRate = int.Parse(formCollection["UserToRate"]);
                Ticket ticket = _ticketBusinessService.GetDetail(ticketId);
                User user = _userBusinessService.GetDetail(userId);
                TicketResolution resolution = _ticketResolutionBusinessService.GetByTicket(ticketId);
                _ticketBusinessService.Close(ticketId);
                TimeSpan respond = ticket.LastReply == null ? (dateTimeNow.Subtract(ticket.CreatedAt.Value))/*TotalSeconds*/ : (dateTimeNow.Subtract(ticket.LastReply.Value))/*TotalSeconds*/;
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

                var participants = _ticketParticipantBusinessService.GetByTicket(ticket.TicketId);
                var datauid = new List<int>();
                var userD = new List<string>();
                foreach (var item in participants)
                {
                    datauid.Add(item.UserId);
                }
                var iduserdevice = _ticketParticipantBusinessService.Getuserdeviceforasnote(datauid);
                foreach (var item in iduserdevice)
                {
                    userD.Add(item.PlayerId);
                }
                String title = ticket.TicketNo + " - " + ticket.Title + " - Rate To Responder",
                         content = user.Name + "(Submiter) Rated " + rate + " stars";

                Onesignal.PushNotif(Description, userD, title, ticketId, ticket.TicketNo, ticket.TicketCategoryId, ticket.Description);

                if (ticket.TicketCategoryId == 9)
                {
                    Email.SendMailCreateHelpDesk(ticket);
                }
                else
                {
                    Email.GetEmailTagTsicsCommentTR(ticket, true);
                    Email.GetEmailTagTsicsCommentTR(ticket, false);
                }
                return RedirectToAction("Details/" + ticketId, "TechnicalRequest");
            }
        }

        [HttpPost]
        public ActionResult AddDppmNumber(FormCollection formCollection, string submitButton)
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

                var ticketId = Convert.ToInt32(formCollection["TicketId"]);
                var ticketDppmNo = formCollection["DPPMno"];
                if (ticketDppmNo != null)
                {
                    if (ModelState.IsValid)
                    {
                        if (submitButton.Contains("Delete"))
                        {
                            _ticketBusinessService.AddDppm(ticketId, null);
                        }
                        else if (submitButton.Contains("Save")) {
                            _ticketBusinessService.AddDppm(ticketId, ticketDppmNo);
                        }
                    }
                }
                return RedirectToAction("Index", "TechnicalRequest");
            }
        }

        [HttpPost]
        public ActionResult DeleteFileAttachment(int id)
        {
            var deleteFileArticle = _ticketAttachmentBusinessService.Delete(_ticketAttachmentBusinessService.GetDetail(id));
            return Json(new { data = deleteFileArticle });
        }
        public string getNameUserinTicket(int id)
        {
            if (id != 0)
            {
                if (_userBusinessService.GetDetail(id) == null) {
                    return "-";
                }
                else
                {
                    return _userBusinessService.GetDetail(id).Name;
                }
            }
            else
            {
                return "-";
            }
        }

        public string CheckTicketStatus(int stat, int nextCommenter, int Responder, int Submiter)
        {
            if (stat == 1)
            {
                return "DRAFT";
            }
            else if (stat == 2)
            {
                if (nextCommenter == Responder)
                {
                    return "PRA";
                }
                else
                {
                    return "PSA";
                }
            }
            else if (stat == 3)
            {
                return "CLOSED";
            }
            else if (stat == 4)
            {
                return "RE-OPEN";
            }
            else if (stat == 6)
            {
                return "SOLVED";
            }
            else
            {
                return String.Empty;
            }
        }
        
    }
}
