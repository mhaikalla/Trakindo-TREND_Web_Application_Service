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
namespace TSICS.Areas.Admin.Controllers
{
    public class TicketController : Controller
    {
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
        private readonly EmployeeMasterBusinessService _mstEmployeeBService = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);

        private bool DebugMode = false;
        public ActionResult Index()
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            ViewBag.DebugMode = DebugMode;


            if (DebugMode == false)
            {
                var ticketPreview = _ticketBusinessService.GetQueryableSummary();
                List<User> submiters = new List<User>();
                List<TicketPreview> submitersRating = new List<TicketPreview>();
                List<TicketPreview> responderRating = new List<TicketPreview>();
                List<User> responders = new List<User>();
                List<TicketPreview> solvedDate = new List<TicketPreview>();
                List<User> EscalateData1 = new List<User>();
                List<User> EscalateData2 = new List<User>();
                List<User> EscalateData3 = new List<User>();
                List<TicketNote> SubmiterNote = new List<TicketNote>();
                List<TicketNote> ResponderNote = new List<TicketNote>();
                List<TicketNote> EscalatedFromNote = new List<TicketNote>();
                List<TicketNote> Escalated1Note = new List<TicketNote>();
                List<TicketNote> Escalated2Note = new List<TicketNote>();
                List<TicketNote> Escalated3Note = new List<TicketNote>();
                if (ticketPreview != null)
                {
                    foreach (var ticket in ticketPreview)
                    {
                        User submiterData = _userBusinessService.GetDetail(ticket.Submiter);
                        Rating ratingDataSubmiter = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);
                        TicketNote noteSubmiter = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, ticket.Submiter);

                        TicketNote notesubmiter = new TicketNote()
                        {
                            TicketNoteId = ticket.TicketId,
                            RespondTime = noteSubmiter == null ? null : /*noteSubmiter.RespondTime*/ new string[] { noteSubmiter == null ? null : noteSubmiter.RespondTime, ratingDataSubmiter == null ? null : ratingDataSubmiter.RespondTime, }.Max()
                        };
                        SubmiterNote.Add(notesubmiter);

                        TicketNote noteResponder = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, ticket.Responder);
                        Rating ratingDataResponder = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, ticket.Responder) ;
                        TicketNote noteresponder = new TicketNote()
                        {
                            TicketNoteId = ticket.TicketId,
                            RespondTime =  new string[] { noteResponder == null ? null : noteResponder.RespondTime, ratingDataResponder == null ? null : ratingDataResponder.RespondTime, }.Max()
                        };
                        ResponderNote.Add(noteresponder);

                        if (ratingDataSubmiter != null)
                        {
                            TicketPreview ratingdata = new TicketPreview()
                            {
                                TicketId = ticket.TicketId,
                                ClosedDate = ratingDataSubmiter.CreatedAt,
                                Rate = ratingDataSubmiter.Rate
                            };
                            submitersRating.Add(ratingdata);
                        }
                        else
                        {
                            TicketPreview ratingdata = new TicketPreview()
                            {
                                TicketId = ticket.TicketId,
                                ClosedDate = null,
                                Rate = 0
                            };
                            submitersRating.Add(ratingdata);
                        }
                        if (submiterData != null)
                        {
                            User submiter = new User()
                            {
                                UserId = ticket.TicketId,
                                Name = submiterData.Name,
                                AreaName = submiterData.AreaName,
                                POH_Name = _mstEmployeeBService.GetDetailbyUserName(submiterData.Username) == null ? null : _mstEmployeeBService.GetDetailbyUserName(submiterData.Username).Business_Area
                            };
                            submiters.Add(submiter);
                        }
                        else
                        {
                            User submiter = new User()
                            {
                                UserId = ticket.TicketId,
                                Name = "",
                                AreaName = "",
                                POH_Name = ""
                            };
                            submiters.Add(submiter);
                        }

                        if (ticket.Responder != 0)
                        {
                            User responderData = _userBusinessService.GetDetail(ticket.Responder);
                            TicketResolution SolvedDateData = _ticketResolutionBusinessService.GetByTicket(ticket.TicketId);
                            Rating ratingresponder = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);

                            int[] escalateId = new int[4] { 0, 0, 0, 0 };
                            int i = 0;
                            foreach (var item in _ticketBusinessService.getlistIdEscalatedbyTicket(ticket.TicketId))
                            {
                                escalateId[i] = item;
                            }
                            EscalateLog escalateData = escalateId[0] == 0 ? null : _ticketBusinessService.getEscalatedDetail(escalateId[0]);
                            EscalateLog escalateData2 = escalateId[1] == 0 ? null : _ticketBusinessService.getEscalatedDetail(escalateId[1]);
                            EscalateLog escalateData3 = escalateId[2] == 0 ? null : _ticketBusinessService.getEscalatedDetail(escalateId[2]);
                            if (responderData != null)
                            {

                                User responder = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = responderData.Name,
                                    AreaName = responderData.AreaName,
                                    POH_Name = responderData.EmployeeId == 0 ? null : _mstEmployeeBService.GetDetail(responderData.EmployeeId).Business_Area,

                                };

                                responders.Add(responder);

                            }
                            else
                            {
                                User responder = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = "",
                                    AreaName = "",
                                    POH_Name = ""
                                };

                                responders.Add(responder);

                            }

                            if (SolvedDateData != null)
                            {
                                TicketPreview solveddate = new TicketPreview()
                                {
                                    TicketNo = ticket.TicketNo,
                                    Description = SolvedDateData.Description,
                                    SolvedDate = SolvedDateData.CreatedAt
                                };
                                solvedDate.Add(solveddate);
                            }
                            else
                            {
                                TicketPreview solveddate = new TicketPreview()
                                {
                                    TicketNo = ticket.TicketNo,
                                    Description = null,
                                    SolvedDate = null
                                };
                                solvedDate.Add(solveddate);
                            }

                            if (ratingresponder != null)
                            {
                                TicketPreview rateResponder = new TicketPreview()
                                {
                                    TicketId = ticket.TicketId,
                                    Rate = ratingresponder.Rate,
                                    Description = ratingresponder.Description
                                };
                                responderRating.Add(rateResponder);
                            }
                            else
                            {
                                TicketPreview rateResponder = new TicketPreview()
                                {
                                    TicketId = ticket.TicketId,
                                    Rate = 0,
                                    Description = null
                                };
                                responderRating.Add(rateResponder);
                            }

                            if (escalateData != null)
                            {
                                User userEscalate1 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = _userBusinessService.GetDetail(escalateData.EscalateTo).Name,
                                    AreaName = _userBusinessService.GetDetail(escalateData.EscalateTo).AreaName == null ? "HEAD OFFICE" : _userBusinessService.GetDetail(escalateData.EscalateTo).AreaName,

                                    Position = _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData.EscalateTo).EmployeeId) == null ? null : _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData.EscalateTo).EmployeeId).Business_Area,

                                    Username = _userBusinessService.GetDetail(escalateData.EscalateFrom).Name,
                                    BranchName = _userBusinessService.GetDetail(escalateData.EscalateFrom).AreaName == null ? "HEAD OFFICE" : _userBusinessService.GetDetail(escalateData.EscalateFrom).AreaName,
                                    POH_Name = _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData.EscalateFrom).EmployeeId) == null ? null : _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData.EscalateFrom).EmployeeId).Business_Area,
                                };
                                EscalateData1.Add(userEscalate1);

                                TicketDiscussion replyEsFrom = _ticketDiscussionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateFrom);
                                TicketResolution ResolutionEsFrom = _ticketResolutionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateFrom);
                                TicketNote noteEsFrom = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateFrom);
                                TicketNote noteesFrom = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = noteEsFrom == null ? null : noteEsFrom.RespondTime /* new double[] { noteResponder == null ? 0 : noteResponder.RespondTime, replySubmiter == null ? 0 : replySubmiter.RespondTime, ResolutionResponder == null ? 0 : ResolutionResponder.RespondTime }.Max()*/
                                };
                                EscalatedFromNote.Add(noteesFrom);

                                TicketDiscussion replyEs1 = _ticketDiscussionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateTo);
                                TicketResolution ResolutionEs1 = _ticketResolutionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateTo);
                                TicketNote noteEs1 = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData.EscalateTo);
                                TicketNote notees1 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = noteEs1 == null ? null : noteEs1.RespondTime /*new double[] { noteResponder == null ? 0 : noteResponder.RespondTime, replySubmiter == null ? 0 : replySubmiter.RespondTime, ResolutionResponder == null ? 0 : ResolutionResponder.RespondTime }.Max()*/
                                };
                                Escalated1Note.Add(notees1);
                            }
                            else
                            {
                                User userEscalate1 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    Username = null,
                                    POH_Name = null,
                                    BranchName = null,
                                    Position = null
                                };
                                EscalateData1.Add(userEscalate1);
                                TicketNote noteesFrom = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = null
                                };
                                EscalatedFromNote.Add(noteesFrom);
                                TicketNote notees1 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = null
                                };
                                Escalated1Note.Add(notees1);
                            }
                            if (escalateData2 != null)
                            {
                                User userEscalate2 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = _userBusinessService.GetDetail(escalateData2.EscalateTo).Name,
                                    AreaName = _userBusinessService.GetDetail(escalateData2.EscalateTo).AreaName == null ? "HEAD OFFICE" : _userBusinessService.GetDetail(escalateData2.EscalateTo).AreaName,
                                    POH_Name = _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData2.EscalateTo).EmployeeId) == null ? null : _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData2.EscalateTo).EmployeeId).Business_Area,
                                };
                                EscalateData2.Add(userEscalate2);
                                TicketDiscussion replyEs2 = _ticketDiscussionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData2.EscalateTo);
                                TicketResolution ResolutionEs2 = _ticketResolutionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData2.EscalateTo);
                                TicketNote noteEs2 = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData2.EscalateTo);
                                TicketNote notees2 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = noteEs2 == null ? null : noteEs2.RespondTime/* new double[] { noteResponder == null ? 0 : noteResponder.RespondTime, replySubmiter == null ? 0 : replySubmiter.RespondTime, ResolutionResponder == null ? 0 : ResolutionResponder.RespondTime }.Max()*/
                                };
                                Escalated2Note.Add(notees2);

                            }
                            else
                            {
                                User userEscalate2 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    POH_Name = null
                                };
                                EscalateData2.Add(userEscalate2);
                                TicketNote notees2 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = null
                                };
                                Escalated2Note.Add(notees2);
                            }
                            if (escalateData3 != null)
                            {
                                User userEscalate3 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = _userBusinessService.GetDetail(escalateData3.EscalateTo).Name,
                                    AreaName = _userBusinessService.GetDetail(escalateData3.EscalateTo).AreaName == null ? "HEAD OFFICE" : _userBusinessService.GetDetail(escalateData3.EscalateTo).AreaName,
                                    POH_Name = _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData3.EscalateTo).EmployeeId) == null ? null : _mstEmployeeBService.GetDetail(_userBusinessService.GetDetail(escalateData3.EscalateTo).EmployeeId).Business_Area,
                                };
                                EscalateData3.Add(userEscalate3);
                                TicketDiscussion replyEs3 = _ticketDiscussionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData3.EscalateTo);
                                TicketResolution ResolutionEs3 = _ticketResolutionBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData3.EscalateTo);
                                TicketNote noteEs3 = _ticketNoteBusinessService.GetDatebyTicketandUserId(ticket.TicketId, escalateData3.EscalateTo);
                                TicketNote notees3 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = noteEs3 == null ? null : noteEs3.RespondTime/*new double[] { noteResponder == null ? 0 : noteResponder.RespondTime, replySubmiter == null ? 0 : replySubmiter.RespondTime, ResolutionResponder == null ? 0 : ResolutionResponder.RespondTime }.Max()*/
                                };
                                Escalated3Note.Add(notees3);
                            }
                            else
                            {
                                User userEscalate3 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    POH_Name = null
                                };
                                EscalateData3.Add(userEscalate3);
                                TicketNote notees3 = new TicketNote()
                                {
                                    TicketNoteId = ticket.TicketId,
                                    RespondTime = null
                                };
                                Escalated3Note.Add(notees3);
                            }
                        }
                        else
                        {
                            TicketNote noteres = new TicketNote()
                            {
                                TicketNoteId = ticket.TicketId,
                                RespondTime = null
                            };
                            ResponderNote.Add(noteres);
                            TicketNote noteesFrom = new TicketNote()
                            {
                                TicketNoteId = ticket.TicketId,
                                RespondTime = null
                            };
                            EscalatedFromNote.Add(noteesFrom);
                            TicketNote notees1 = new TicketNote()
                            {
                                TicketNoteId = ticket.TicketId,
                                RespondTime = null
                            };
                            Escalated1Note.Add(notees1);
                            TicketNote notees2 = new TicketNote()
                            {
                                TicketNoteId = ticket.TicketId,
                                RespondTime = null
                            };
                            Escalated2Note.Add(notees2);
                            TicketNote notees3 = new TicketNote()
                            {
                                TicketNoteId = ticket.TicketId,
                                RespondTime = null
                            };
                            Escalated3Note.Add(notees3);

                            EscalatedFromNote.Add(noteesFrom);
                            if (ticket.Status != 1)
                            {
                                User responder = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = "TREND Admin",
                                    AreaName = "-",
                                    POH_Name = "-"
                                };
                                TicketPreview solveddate = new TicketPreview()
                                {
                                    TicketNo = ticket.TicketNo,
                                    Description = null,
                                    SolvedDate = null
                                };
                                TicketPreview rateResponder = new TicketPreview()
                                {
                                    TicketId = ticket.TicketId,
                                    Rate = 0,
                                    Description = null
                                };
                                User userEscalate1 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    Username = null,
                                    BranchName = null,
                                    POH_Name = null,
                                    Position = null
                                };
                                User userEscalate2 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null
                                };
                                User userEscalate3 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null
                                };
                                EscalateData1.Add(userEscalate1);
                                EscalateData2.Add(userEscalate2);
                                EscalateData3.Add(userEscalate3);
                                responderRating.Add(rateResponder);
                                solvedDate.Add(solveddate);
                                responders.Add(responder);
                            }
                            else
                            {
                                User responder = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = "",
                                    AreaName = "",
                                    POH_Name = ""
                                };
                                TicketPreview solveddate = new TicketPreview()
                                {
                                    TicketNo = ticket.TicketNo,
                                    Description = null,
                                    SolvedDate = null
                                };
                                TicketPreview rateResponder = new TicketPreview()
                                {
                                    TicketId = ticket.TicketId,
                                    Rate = 0,
                                    Description = null
                                };
                                User userEscalate1 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    Username = null,
                                    POH_Name = null,
                                    BranchName = null,
                                    Position = null
                                };
                                User userEscalate2 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    POH_Name = null
                                };
                                User userEscalate3 = new User()
                                {
                                    UserId = ticket.TicketId,
                                    Name = null,
                                    AreaName = null,
                                    POH_Name = null
                                };
                                EscalateData1.Add(userEscalate1);
                                EscalateData2.Add(userEscalate2);
                                EscalateData3.Add(userEscalate3);
                                responderRating.Add(rateResponder);
                                solvedDate.Add(solveddate);
                                responders.Add(responder);
                            }
                        }
                    }
                }
                ViewBag.RespondEsFrom = EscalatedFromNote;
                ViewBag.RespondEs1 = Escalated1Note;
                ViewBag.RespondEs2 = Escalated2Note;
                ViewBag.RespondEs3 = Escalated3Note;
                ViewBag.NoteSubmiter = SubmiterNote;
                ViewBag.NoteResponder = ResponderNote;
                ViewBag.EscalatedLog1 = EscalateData1;
                ViewBag.EscalatedLog2 = EscalateData2;
                ViewBag.EscalatedLog3 = EscalateData3;
                ViewBag.RatingSubmiters = submitersRating;
                ViewBag.RatingResponders = responderRating;
                ViewBag.SolvedDate = solvedDate;
                ViewBag.Submiters = submiters;
                ViewBag.Responders = responders;

                return View(ticketPreview.ToList());
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            if (collection["TicketId"] != null)
            {
                Ticket tickets = _ticketBusinessService.GetDetail(Convert.ToInt32(collection["TicketId"]));
                _ticketBusinessService.Delete(tickets);
                return RedirectToAction("Index", "Ticket");
            }
            return View();
        }
        public string GetCategoryName(int id)
        {
            return _ticketCategoryBusinessService.GetDetail(id).Name;
        }
        [HttpPost]
        public ActionResult DeleteSelected(FormCollection collection)
        {
            _ticketBusinessService.DeleteSelectedTicket(collection["ListTicket"]);
            return RedirectToAction("index", "Ticket");
        }
    }
}