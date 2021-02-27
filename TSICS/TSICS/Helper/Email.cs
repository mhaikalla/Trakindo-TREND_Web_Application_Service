using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace TSICS.Helper
{
    public class Email
    {
        private static readonly EmployeeMasterBusinessService EmployeeBs = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);
        static readonly LogReportBusinessService LogReportBService = Factory.Create<LogReportBusinessService>("LogReport", ClassType.clsTypeBusinessService);
        static readonly UserBusinessService UserBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        static readonly TicketCategoryBusinessService TicketCategoryBs = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        static readonly TicketBusinessService TicketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        static readonly TicketNoteBusinessService TicketNoteBs = Factory.Create<TicketNoteBusinessService>("TicketNote", ClassType.clsTypeBusinessService);
        static readonly TicketResolutionBusinessService TicketResolutionBs = Factory.Create<TicketResolutionBusinessService>("TicketResolution", ClassType.clsTypeBusinessService);
        static readonly TicketParcipantBusinessService TicketParcipantBs = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        static readonly MasterAreaBusinessService MasterAreaBs = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
        static readonly MasterBranchBusinessService MasterBranchBs = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        static readonly UserRoleBusinessService UserRoleBs = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
      
        static readonly LogErrorBusinessService LogErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        static readonly UserTsManagerBusinessService UserTsManagerBusinessService = Factory.Create<UserTsManagerBusinessService>("UserTsManager", ClassType.clsTypeBusinessService);
        static readonly ArticleBusinessService articleBusinessService = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private static string SendMail(string mailJson)
        {
            string data;
            using (WebClient wb = new WebClient())
            {
                wb.Headers.Add("content-type", "application/json");

                data = wb.UploadString(WebConfigure.GetUrlSendEmail(), "post", mailJson);
            }

            var result = data;

            return result;
        }

        //Registering =========================================================
        public static void SendMailRegister(User model)
        {
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDatanew.To = UserBService.GetDetailByEmployeeId(model.EmployeeId).Email;/*DefaultEmail;*/
            //EmailDatanew.Cc = Email;
            emailDatanew.Tag = WebConfigure.GetEmailTagRegister(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "Name", Value = model.Name},
                new EmailJsonkeyValues() {Key = "UserId", Value = model.Username},
                new EmailJsonkeyValues()
                {
                    Key = "Location", Value = model.MasterBranchId == 0? "-" : MasterBranchBs.GetDetail(model.MasterBranchId).Name
                },
                new EmailJsonkeyValues() {Key = "Area", Value =  model.MasterAreaId == 0? "-" : MasterAreaBs.GetDetail(model.MasterAreaId).Name },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace(model.RoleDescription)? "-" : model.RoleDescription},
                new EmailJsonkeyValues() {Key = "Level", Value = UserRoleBs.GetDetail(model.RoleId).Name},
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value =  String.IsNullOrWhiteSpace(model.TechnicalJobExperienceDuration)? "-" : model.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value =  String.IsNullOrWhiteSpace(model.TechnicalJobTitle)? "-" : model.TechnicalJobTitle}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            SendMail(result);
            LogReportBService.WriteLog("RegisterEmail", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //To User, Tag :
        public static void SendMailNeedApp1(User model)
        {
            var employeeM = EmployeeBs.GetDetail(model.Approval1);

            var link = "<a href='" + WebConfigure.GetDomain() + "/Account/Approve1Mail/" + model.MobileToken + "'>Link</a>";

            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            
            emailDataNew.To = EmployeeBs.GetDetail(model.Approval1).Email;
            //EmailDatanew.Cc = Email;
            emailDataNew.Tag = WebConfigure.GetEmailTagNeedApprove1(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval", Value = employeeM.Employee_Name},
                new EmailJsonkeyValues() {Key = "Name", Value = model.Name},
                new EmailJsonkeyValues() {Key = "UserId", Value = model.Username},
                new EmailJsonkeyValues()
                {
                    Key = "Location", Value = model.MasterBranchId ==0 ? "-" : MasterBranchBs.GetDetail(model.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value = model.MasterAreaId == 0 ? "HEAD OFFICE" : MasterAreaBs.GetDetail(model.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace(model.RoleDescription) ? "-" : model.RoleDescription },
                new EmailJsonkeyValues() {Key = "Level", Value = UserRoleBs.GetDetail(model.RoleId).Name},
                new EmailJsonkeyValues() {Key = "Link", Value = link},
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace(model.TechnicalJobExperienceDuration) ? "-" : model.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace(model.TechnicalJobTitle) ? "-" : model.TechnicalJobTitle}
            };


            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }  //To Approval Level 1, Tag: 
        public static void SendMailNeedApp2(User model, string message)
        {
            var employeeM1 = EmployeeBs.GetDetail(model.Approval1);

            var employeeM = EmployeeBs.GetDetail(model.Approval2);

            var link = "<a href='" + WebConfigure.GetDomain() + "/Account/Approve2Mail/" + model.MobileToken + "'>Link</a>";

            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDatanew.To = employeeM.Email/*DefaultEmail*/;
            //EmailDatanew.Cc = Email;
            emailDatanew.Tag = WebConfigure.GetEmailTagNeedApprove2(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval2", Value = employeeM.Employee_Name},
                new EmailJsonkeyValues() {Key = "NameApproval1", Value = employeeM1.Employee_Name},
                new EmailJsonkeyValues() {Key = "Message", Value = message},
                new EmailJsonkeyValues() {Key = "User", Value = model.Name},
                new EmailJsonkeyValues() {Key = "UserId", Value = model.Username},
                new EmailJsonkeyValues()
                {
                    Key = "Location", Value = model.MasterBranchId == 0? "-" : MasterBranchBs.GetDetail(model.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value = model.MasterAreaId == 0? "HEAD OFFICE" : MasterAreaBs.GetDetail(model.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace(model.RoleDescription) ? "-" : model.RoleDescription},
                new EmailJsonkeyValues() {Key = "Level", Value = UserRoleBs.GetDetail(model.RoleId).Name},
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace(model.TechnicalJobExperienceDuration) ? "-" : model.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace(model.TechnicalJobTitle) ? "-": model.TechnicalJobTitle},
                new EmailJsonkeyValues() {Key = "Link", Value = link}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }  //To Approval Level 1, Tag:
        public static void SendMailNeedAppTs1(User model, int tsId, string msg = "")
        {
            var employeeM1 = EmployeeBs.GetDetail(model.Approval1);

            var employeeM2 = EmployeeBs.GetDetail(model.Approval2);

            var userTsManagerModel = UserTsManagerBusinessService.GetDetail(tsId);

            List<int> idLevelTs = new List<int>();

            if (tsId == 1)
            {
                idLevelTs.Add(2);
                idLevelTs.Add(3);
            }
            else if (tsId == 2)
            {
                idLevelTs.Add(1);
                idLevelTs.Add(3);
            }
            else if (tsId == 3)
            {
                idLevelTs.Add(2);
                idLevelTs.Add(1);
            }

            var link = "<a href='" + WebConfigure.GetDomain() + "/Account/ApproveTsMail/" + model.MobileToken + "'>Link</a>";

            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDatanew.To = userTsManagerModel.Email/*DefaultEmail*/;
            emailDatanew.Cc += UserTsManagerBusinessService.GetListEmail(idLevelTs);

            emailDatanew.Tag = WebConfigure.GetEmailTagNeedApproveTs(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApprovalTs", Value = userTsManagerModel.Name},
                new EmailJsonkeyValues() {Key = "NameApproval1", Value = employeeM1.Employee_Name},
                new EmailJsonkeyValues() {Key = "NameApproval2", Value = employeeM2.Employee_Name},
                new EmailJsonkeyValues() {Key = "Message", Value = msg},
                new EmailJsonkeyValues() {Key = "User", Value = model.Name},
                new EmailJsonkeyValues() {Key = "UserId", Value = model.Username},
                new EmailJsonkeyValues()
                {
                    Key = "Location", Value = model.MasterBranchId ==0? "-" : MasterBranchBs.GetDetail(model.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value = model.MasterAreaId == 0? "HEAD OFFICE" :MasterAreaBs.GetDetail(model.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace(model.RoleDescription) ? "-" : model.RoleDescription},
                new EmailJsonkeyValues() {Key = "Level", Value = UserRoleBs.GetDetail(model.RoleId).Name},
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace(model.TechnicalJobExperienceDuration)? "-" : model.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace(model.TechnicalJobTitle) ? "-": model.TechnicalJobTitle},
                new EmailJsonkeyValues() {Key = "Link", Value = link}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApprovalTS", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //To TS Manager, Tag:
        public static void SendMailNeedActivedAdmin(User user, Dictionary<String, EmployeeMaster> ApprovalData, Dictionary<String, String> message, Boolean isApproved = true)
        {
            //find all admin
            var userL = UserBService.GetListAdmin();
            foreach (User item in userL)
            {
                EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
                 String Link = "<a href='" + WebConfigure.GetDomain() + "/Admin/User/Approval/" + user.UserId + "'>" + WebConfigure.GetDomain() + "/Admin/User/Edit/" + user.UserId + "</a>";
                emailDatanew.To = item.Email/*DefaultEmail*/;
                //EmailDatanew.Cc = item.Email;
                emailDatanew.Tag = WebConfigure.GetEmailTagResApproveAdmin(); //tag ==> key for template
                emailDatanew.KeyValues = new List<EmailJsonkeyValues>
                {
                    new EmailJsonkeyValues() {Key = "Link", Value = Link},
                    new EmailJsonkeyValues() {Key = "Name", Value = user.Name},
                    new EmailJsonkeyValues() {Key = "Approval1Name", Value = ApprovalData["Approval1"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "Approval2Name", Value = ApprovalData["Approval2"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "ApprovalTsName", Value = ApprovalData["ApprovalTS"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "MessageApp1", Value = message["Approval1"]},
                    new EmailJsonkeyValues() {Key = "MessageApp2", Value = message["Approval2"]},
                    new EmailJsonkeyValues() {Key = "MessageTs", Value = message["ApprovalTS"]},
                    new EmailJsonkeyValues() {Key = "Decision", Value = isApproved == true ? "Approved" : "Declined" }
                   
                };

                var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);

                LogReportBService.WriteLog("Need Approval Admin Email", MethodBase.GetCurrentMethod().Name, SendMail(result));

            }
        } 
        //to Admin
        //=====================================================================
        //comment
        //Respawn Approval To User==============================================
        public static void SendMailResApp1(User model, string message, int levelApproval =1, int isApprove =0)
        {            
            var status = isApprove == 0 ? "Declined" : "Approved";
            var employeeM = EmployeeBs.GetDetail(model.Approval1);
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            String Level = ""; 
            if (levelApproval == 1)
            {
                Level = "1st Superior";
            }
            else if(levelApproval ==2)
            {
                Level = "2nd Superior";
                employeeM = EmployeeBs.GetDetail(model.Approval2);
            }
            else if (levelApproval ==3) 
            {
                Level = "TS Manager";
                employeeM = EmployeeBs.GetDetail(UserTsManagerBusinessService.GetDetail(model.UserTsManager1Id).EmployeeId);
            }

            emailDatanew.To = employeeM.Email;
            //EmailDatanew.Cc = Email;
            emailDatanew.Tag = WebConfigure.GetEmailTagResApprove1(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "Level", Value = Level},
                new EmailJsonkeyValues() {Key = "NameApproval", Value = employeeM.Employee_Name},
                new EmailJsonkeyValues() {Key = "Message", Value = message},
                new EmailJsonkeyValues() {Key = "User", Value = model.Name},
                new EmailJsonkeyValues() {Key = "Status", Value = status}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //Kalau di Approve sama Approval
        public static void SendMailReject(User user, UserMessage userM, EmployeeMaster Superior)
        {
            //EmployeeMaster EmployeeM = Factory.Create<EmployeeMaster>("EmployeeMaster", ClassType.clsTypeDataModel);
            //EmployeeM = EmployeeBS.GetDetail(Ids);

            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            emailDataNew.To = user.Email/*DefaultEmail*/;
            //EmailDatanew.Cc = Email;
            emailDataNew.Tag = WebConfigure.GetEmailTagReject(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "Name", Value = user.Name},
                new EmailJsonkeyValues() {Key = "Msg", Value = userM.Message},
                new EmailJsonkeyValues() {Key = "NameSuperior", Value = Superior.Employee_Name}
            };
            //EmailDatanew.KeyValues.Add(new EmailJsonkeyValues() { Key = "Level", Value = UserM.MessageType });

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("RejectEmail", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } // Kalau di Reject sama Approval
        public static void SendMailActived(User user)
        {
            var link = "<a href='" + WebConfigure.GetDomain() + "'>Click here</a>";

            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            //
            emailDataNew.To = user.Email/*DefaultEmail*/;
            //EmailDatanew.Cc = Email;
            emailDataNew.Tag = WebConfigure.GetEmailTagActived(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {  
                new EmailJsonkeyValues() {Key = "Name", Value = user.Name},
                new EmailJsonkeyValues() {Key = "Link", Value = link},
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("ActivedEmail", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //Kalau di Active sama admin


        //Create Technical Request==================================
        public static void SendMailCreateTr(Ticket ticketM, bool Submiter = false)
        {
                var emailRes = UserBService.GetDetail(ticketM.Responder);
                var emailSub = UserBService.GetDetail(ticketM.Submiter);
                String Link = "<a href='" + WebConfigure.GetDomain() + "/TechnicalRequest/Details/" + ticketM.TicketId + "'>Click Here</a>";
                String mobileLink = "<a href='http://www.trendapp.com/" + ticketM.TicketId + "'>Open TREND Mobile Apps</a>";
                String status = "";
                var lUser = TicketParcipantBs.GetByTicket(ticketM.TicketId);
                
                List<int> idp = new List<int>();
               
                if (lUser != null)
                {
                    foreach (var item in lUser)
                    {
                        idp.Add(item.UserId);
                    }
                }
         
                #region Status Ticket
             if (ticketM.Status == 2)
             {
                    if (Submiter == true) {
                        status = "PRA";
                    }
                    else
                    {
                        status = "Waiting Your Feedback";
                    }
             }
               #endregion

                EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
                if (Submiter == false) {
                    emailDatanew.To = emailRes.Email;
                    emailDatanew.Tag = WebConfigure.GetEmailCreateTr(); //tag ==> key for template
                    emailDatanew.KeyValues = new List<EmailJsonkeyValues>
                {
                    new EmailJsonkeyValues() {Key = "ResponderName", Value = emailRes.Name},
                    new EmailJsonkeyValues() {Key = "TicketNo", Value = ticketM.TicketNo},
                    new EmailJsonkeyValues()
                    {
                        Key = "TicketCategory",
                        Value = TicketCategoryBs.GetDetail(ticketM.TicketCategoryId).Name
                    },
                    new EmailJsonkeyValues() {Key = "TicketTitle", Value = ticketM.Title},
                    new EmailJsonkeyValues() {Key = "TicketType", Value = "Technical Request"},
                    new EmailJsonkeyValues() {Key = "TicketDesc", Value = String.IsNullOrWhiteSpace(ticketM.Description) ? "-" : ticketM.Description},
                    new EmailJsonkeyValues() {Key = "Link", Value = Link},
                     new EmailJsonkeyValues() {Key = "MobileLink", Value = mobileLink},
                    new EmailJsonkeyValues() {Key = "Status", Value = status}
                };
                }
                else
                {
                    emailDatanew.To = emailSub.Email;
                    emailDatanew.Cc = ticketM.EmailCC + ";";
                    emailDatanew.Cc += UserBService.GetListEmailParticipant(idp);
              
                emailDatanew.Tag = WebConfigure.GetEmailCreateTr(); //tag ==> key for template
                    emailDatanew.KeyValues = new List<EmailJsonkeyValues>
                {
                    new EmailJsonkeyValues() {Key = "ResponderName", Value = emailSub.Name},
                    new EmailJsonkeyValues() {Key = "TicketNo", Value = ticketM.TicketNo},
                    new EmailJsonkeyValues()
                    {
                        Key = "TicketCategory",
                        Value = TicketCategoryBs.GetDetail(ticketM.TicketCategoryId).Name
                    },
                    new EmailJsonkeyValues() {Key = "TicketTitle", Value = ticketM.Title},
                    new EmailJsonkeyValues() {Key = "TicketType", Value = "Technical Request"},
                    new EmailJsonkeyValues() {Key = "TicketDesc", Value = String.IsNullOrWhiteSpace(ticketM.Description) ? "-" : ticketM.Description},
                    new EmailJsonkeyValues() {Key = "Link", Value = Link},
                    new EmailJsonkeyValues() {Key = "MobileLink", Value = mobileLink},
                    new EmailJsonkeyValues() {Key = "Status", Value = status}
                };
                }
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
                //SendMail(Result);
                LogReportBService.WriteLog("EmailCreateTR", MethodBase.GetCurrentMethod().Name, SendMail(result));

           
        }

        public static void SendMailCreateHelpDesk(Ticket ticketM)
        {
            if (ticketM.TicketCategoryId != 9)
            {
                try
                {
                    String status = String.Empty;
                    String Link = "<a href='" + WebConfigure.GetDomain() + "/TechnicalRequest/Details/" + ticketM.TicketId + "'>Click Here</a>";
                    String mobileLink = "<a href='http://www.trendapp.com/" + ticketM.TicketId + "'>Open TREND Mobile Apps</a>";
                    var emailRes = UserBService.GetDetailbyUsername("Technical.c.admin1".ToUpper());

                    var userAdmin = UserBService.GetListOtherAdmin();

                    var SubmiterEmail = UserBService.GetDetail(ticketM.Submiter);
                    List<int> idAdmin = new List<int>();

                    if (userAdmin != null)
                    {
                        foreach (var item in userAdmin)
                        {
                            idAdmin.Add(item.UserId);
                        }
                    }

                    #region Status Ticket
                    if (ticketM.Status == 1)
                    {
                        status = "<p class='background-color : black;'>DRAFT</p>";
                    }
                    else if (ticketM.Status == 2)
                    {
                        status = "<p class='background-color : black;'>PRA</p>";
                    }

                    #endregion
                    EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

                    emailDatanew.To = emailRes.Email;
                    emailDatanew.Cc = SubmiterEmail.Email;
                    emailDatanew.Cc += UserBService.GetListEmailParticipant(idAdmin, true);
                    emailDatanew.Tag = WebConfigure.GetEmailCreateTr(); //tag ==> key for template
                    emailDatanew.KeyValues = new List<EmailJsonkeyValues>
                    {
                        new EmailJsonkeyValues() {Key = "ResponderName", Value = emailRes.Name},
                        new EmailJsonkeyValues() {Key = "TicketNo", Value = ticketM.TicketNo},
                        new EmailJsonkeyValues()
                        {
                            Key = "TicketCategory",
                            Value = TicketCategoryBs.GetDetail(ticketM.TicketCategoryId).Name
                        },
                        new EmailJsonkeyValues() {Key = "TicketType", Value = "TREND Help Desk"},
                        new EmailJsonkeyValues() {Key = "TicketTitle", Value = ticketM.Title},
                        new EmailJsonkeyValues() {Key = "TicketDesc", Value = String.IsNullOrWhiteSpace(ticketM.Description) ? "-" : ticketM.Description},
                          new EmailJsonkeyValues() {Key = "Link", Value = Link},
                        new EmailJsonkeyValues() {Key = "MobileLink", Value = mobileLink},
                    };

                    var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
                    //SendMail(Result);
                    LogReportBService.WriteLog("EmailCreateTR", MethodBase.GetCurrentMethod().Name, SendMail(result));

                }
                catch (Exception er)
                {
                    LogErrorBService.WriteLog("Email TR", MethodBase.GetCurrentMethod().Name, er.ToString());

                    throw;
                }
            }
            else
            {
                LogReportBService.WriteLog("Email Create TR for Help Desk", MethodBase.GetCurrentMethod().Name, ticketM.TicketNo + " Is Not Help Desk Ticket, Using Method Invalid");
            }
        }

        //Comment TR==================================================
        public static void GetEmailTagTsicsCommentTR(Ticket ticket, bool Submiter = false)
        {
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            String Note = "", Resolution = "", status = "";
            User receiverData = null;
            var ticketNote = TicketNoteBs.GetListforEmail(ticket.TicketId);
            TicketResolution ticketResolution = TicketResolutionBs.GetListforEmail(ticket.TicketId);
            String CategoryName = ticket.TicketCategoryId == 9 ? "Help Desk" : TicketCategoryBs.GetDetail(ticket.TicketCategoryId).Name;
            String Link = "<a href='" + WebConfigure.GetDomain() + "/TechnicalRequest/Details/" + ticket.TicketId + "#comment'>Click Here</a>";
            String mobileLink = "<a href='http://www.trendapp.com/" + ticket.TicketId + "'>Open TREND Mobile Apps</a>";

            if (Submiter)
            {
                var lUser = TicketParcipantBs.GetByTicket(ticket.TicketId);
                receiverData = UserBService.GetDetail(ticket.Submiter);
                #region Ticket Status

                if (ticket.Status == 2)
                {
                    if (receiverData.UserId == ticket.NextCommenter)
                    {
                          status = "Waiting Your Feedback";
                    }
                    else
                    {
                          status = "PRA";
                    }
                }
                else if (ticket.Status == 6)
                {
                    status = "Solved";
                }
                else if (ticket.Status == 3)
                {
                    status = "Closed";
                }
                #endregion

                #region Participant
                List<int> idp = new List<int>();

                if (lUser != null)
                {
                    foreach (var item in lUser)
                    {
                        idp.Add(item.UserId);
                    }
                }
                #endregion

                emailDatanew.To = receiverData.Email;
                emailDatanew.Cc = ticket.EmailCC + ";";
                emailDatanew.Cc += UserBService.GetListEmailParticipant(idp);
            }
            else
            {
                receiverData = UserBService.GetDetail(ticket.Responder);
                bool IsEscalated = TicketBs.IsEscalated(ticket.TicketId, ticket.Responder);

                #region Ticket Status

                if (ticket.Status == 2)
                {
                    if (IsEscalated)
                    {
                        if (receiverData.UserId == ticket.NextCommenter)
                        {
                            status = "ESCALATED - Waiting Your Feedback";
                        }
                        else
                        {
                            status = "ESCALATED - PSA";
                        }
                    }
                    else { 
                        if (receiverData.UserId == ticket.NextCommenter)
                        {
                            status = "Waiting Your Feedback";
                        }
                        else
                        {
                            status = "PSA";
                        }
                    }
                }
                else if (ticket.Status == 6)
                {
                    status = "Solved";
                }
                else if (ticket.Status == 3)
                {
                    status = "Closed";
                }
                #endregion
                emailDatanew.To = receiverData.Email;
            }
            #region Multi Comment

            Note = "Reply as Notes</br>";
            foreach (TicketNote note in ticketNote)
            {
                string commentas = ticket.Responder.Equals(note.UserId) ? "(Responder)" : ticket.Submiter.Equals(note.UserId) ? "(Submiter)" : "Participant (Escalated)";
                Note = Note + "<ul><li>"+ UserBService.GetDetail(note.UserId).Name + " " + commentas + ", "+ note.CreatedAt + "</li></ul><blockquote>"+ note.Description+"</blockquote></br></br>";
            }
            

            if (ticketResolution != null)
            {
                Resolution = "Write Resolution from Responder : ";
                Resolution = Resolution + "<ul><li>" + UserBService.GetDetail(ticketResolution.UserId).Name +"(Responder), " + ticketResolution.CreatedAt + "</li></ul><blockquote>" + ticketResolution.Description + "</blockquote></br></br>";
            }
            #endregion
            emailDatanew.Tag = WebConfigure.GetEmailTagTsicsCommentTR(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                    new EmailJsonkeyValues() {Key = "Title", Value = ticket.Title},
                    new EmailJsonkeyValues() {Key = "Status", Value = status},
                    new EmailJsonkeyValues() {Key = "TicketNo", Value = ticket.TicketNo},
                    new EmailJsonkeyValues() {Key = "CategoryName", Value = CategoryName},
                    new EmailJsonkeyValues() {Key = "ReceiverName", Value = receiverData.Name},
                    new EmailJsonkeyValues() {Key = "TicketDesc", Value = ticket.Description},
                    new EmailJsonkeyValues() {Key = "Link", Value = Link},
                     new EmailJsonkeyValues() {Key = "MobileLink", Value = mobileLink },
                    new EmailJsonkeyValues() {Key = "Note", Value = Note },
                    new EmailJsonkeyValues() {Key = "Resolution", Value = Resolution},
            };
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            LogReportBService.WriteLog("Comment Technical Request", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }


        public static void GetEmailTagTsicsCommentHelpDesk(int ticketid, int UserLogedIn, bool forSubmiter =false )
        {
            String Note = "", Resolution = "", status = "";
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            Ticket ticketData = TicketBs.GetDetail(ticketid);
            var ticketNote = TicketNoteBs.GetListforEmail(ticketData.TicketId);
            var ticketResolution = TicketResolutionBs.GetListforEmail(ticketData.TicketId);
            var lUser =  UserBService.GetListAdmin();

            String CategoryName = "Help Desk";
            String mobileLink = "<a href='http://www.trendapp.com/" + ticketData.TicketId + "'>Open TREND Mobile Apps</a>";
            String Link = "<a href='" + WebConfigure.GetDomain() + "/TechnicalRequest/Details/" + ticketData.TicketId + "#comment'>Here</a>";
            #region sender & receive
            int senderID = ticketData.NextCommenter.Equals(ticketData.Responder) ? ticketData.Submiter : ticketData.Responder;
            
            User senderData = UserBService.GetDetail(senderID);
           
            User receiverData = UserBService.GetListFirstAdmin(WebConfigure.GetLoginManualXupj());
           
            if (forSubmiter)
            {
                #region Participant
                List<int> idp = new List<int>();

                if (lUser != null)
                {
                    foreach (var item in lUser)
                    {
                        idp.Add(item.UserId);
                    }
                }
                #endregion
                receiverData = UserBService.GetDetail(ticketData.Submiter);
                emailDatanew.Cc = ticketData.EmailCC + ";";
                emailDatanew.Cc += UserBService.GetListEmailParticipant(idp);
            }
                #endregion

                #region Status Ticket
                if (ticketData.Status == 1)
                {
                    status = "<p class='background-color : black;'>DRAFT</p>";
                }
                else if (ticketData.Status == 2)
                {
                    if (TicketBs.IsEscalated(ticketData.TicketId, UserLogedIn))
                    {
                        status = "<p class='background-color : black;'>ESCALATED</p>";
                    }
                    else
                    {
                        if (ticketData.NextCommenter.Equals(0))
                        {
                            if (ticketData.Responder.Equals(UserLogedIn) && ticketData.Submiter.Equals(UserLogedIn))
                            {
                                status = "<p class='background-color : black;'>waiting your feedback</p>";
                            }
                            else
                            {
                                status = "<p class='background-color : black;'>PRA</p>";
                            }
                        }
                        else
                        {
                            if (UserLogedIn == ticketData.NextCommenter)
                            {
                                status = "<p class='background-color : red;'>waiting your feedback</p>";
                            }
                            else
                            {
                                if (ticketData.NextCommenter == ticketData.Responder)
                                {
                                    status = "<p class='background-color : black;'>PRA</p>";
                                }
                                else if (ticketData.NextCommenter == ticketData.Submiter)
                                {
                                    status = "<p class='background-color : black;'>PSA</p>";
                                }
                            }
                        }
                    }
                }
                else if (ticketData.Status == 3)
                {
                    status = "<p class='background-color : red;'>CLOSED</p>";
                }
                else if (ticketData.Status == 6)
                {
                    status = "<p class='background-color : red;'>SOLVED</p>";
                }
                #endregion

                #region Multi Comment
                Note = "Comment as Notes : ";
                foreach (TicketNote note in ticketNote)
                {
                    string commentas = ticketData.Responder.Equals(note.UserId) ? "Responder" : "Submiter";
                    Note = Note + "<p class='MsoNormal'>Date " + note.CreatedAt + " By " + UserBService.GetDetail(note.UserId).Name + " (" + commentas + ") </p><p class='MsoNormal'><u>" + note.Description + "</u></p></br>";
                }
                Resolution = "Write Resolution from Responder : ";
                if (ticketResolution != null)
                {
                    Resolution = Resolution + "<p>Date " + ticketResolution.CreatedAt + " By  " + UserBService.GetDetail(ticketResolution.UserId).Name + " (Responder)</p><p><u>" + ticketResolution.Description + "</u></p></br>";
                }
                #endregion
                emailDatanew.To = receiverData.Email;
            emailDatanew.Tag = WebConfigure.GetEmailTagTsicsCommentTR(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                    new EmailJsonkeyValues() {Key = "Title", Value = ticketData.Title},
                    new EmailJsonkeyValues() {Key = "Status", Value = status},
                    new EmailJsonkeyValues() {Key = "TicketNo", Value = ticketData.TicketNo},
                    new EmailJsonkeyValues() {Key = "CategoryName", Value = CategoryName},
                    new EmailJsonkeyValues() {Key = "ReceiverName", Value = receiverData.Name},
                    new EmailJsonkeyValues() {Key = "TicketDesc", Value = ticketData.Description},
                    new EmailJsonkeyValues() {Key = "Link", Value = Link},
                     new EmailJsonkeyValues() {Key = "MobileLink", Value = mobileLink},
                    new EmailJsonkeyValues() {Key = "Note", Value = Note },
                    new EmailJsonkeyValues() {Key = "Resolution", Value = Resolution},
            };
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            LogReportBService.WriteLog("Comment Technical Request", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }
        //=============================================================


        //Other==========================================
        public static void SendMailFlagRed(int idu)
        {
            //find 
            User usr = Factory.Create<User>("User", ClassType.clsTypeDataModel);

            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDatanew.To = usr.Email;
            //EmailDatanew.Cc = item.Email;
            emailDatanew.Tag = WebConfigure.GetEmailTagTsicstrFlagRed(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "User", Value = usr.Name}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);

            LogReportBService.WriteLog("Email Red TR", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }
        public static void SendMailRejectAuto(User user)
        {
            var ids = user.Approval2 != 0 ? user.Approval2 : user.Approval1;

            var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalSpvDay());

            var employeeM = EmployeeBs.GetDetail(ids);

            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDatanew.To = user.Email;
            //EmailDatanew.Cc = Email;
            emailDatanew.Tag = WebConfigure.GetEmailAutoRejectRegister(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "User", Value = user.Name},
                new EmailJsonkeyValues() {Key = "Days", Value = addDay.ToString()},
                new EmailJsonkeyValues() {Key = "ApprovalName", Value = employeeM.Employee_Name}
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("RejectEmailAuto", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

        //=========================================================
        //Article
        //=========================================================
       public static void SendMailPublishArticle(Article Artikel)
        {
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            var Userdata = UserBService.GetDetail(Artikel.CreatedBy);
           
            emailDatanew.To = Userdata.Email;
          
            emailDatanew.Tag = WebConfigure.GetEmailPublishedArticle(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "Title", Value = Artikel.Title},
                new EmailJsonkeyValues() {Key = "Description", Value = Artikel.Description},
                new EmailJsonkeyValues()
                {
                    Key = "NameUser", Value =  Userdata.Name
                },
                new EmailJsonkeyValues() {Key = "LinkDetail", Value = "<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.ArticleId + "#comment'>Want to see your Article ? Click Here</a>" },
                new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm") }
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("Publish Article Email", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //To User
       public static void SendMailNeedApp1Article(Article Artikel)
        {
            var UserData = UserBService.GetDetail(Artikel.CreatedBy);
            
            var employeeM = EmployeeBs.GetDetail(EmployeeBs.GetDetail(UserData.EmployeeId).Superior_ID);
           
            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

           emailDataNew.To = employeeM.Email;
           emailDataNew.Tag = WebConfigure.GetEmailneedApprove1Article(); //tag ==> key for template
           emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval", Value = employeeM.Employee_Name},
                new EmailJsonkeyValues() {Key = "Name", Value = UserData.Name},
                new EmailJsonkeyValues() {Key = "UserName", Value = UserData.Username},
                new EmailJsonkeyValues()
                {
                    Key = "Title", Value = Artikel.Title
                },
               
                new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm")  },
                                new EmailJsonkeyValues()
                {
                    Key = "Location", Value = UserData.MasterBranchId ==0 ? "-" : MasterBranchBs.GetDetail( UserData.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value =  UserData.MasterAreaId == 0 ? "HEAD OFFICE" : MasterAreaBs.GetDetail( UserData.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace( UserData.RoleDescription) ? "-" :  UserData.RoleDescription },
                new EmailJsonkeyValues() {Key = "Level", Value = UserRoleBs.GetDetail( UserData.RoleId).Name},
                new EmailJsonkeyValues() {Key = "LinkApproval", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/Approve1Mail/" + Artikel.Token + "'>Click Here in this Link</a>" },
                 new EmailJsonkeyValues() {Key = "Link", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.ArticleId + "'>Click Here to See More Detail Information</a>" },
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace( UserData.TechnicalJobExperienceDuration) ? "-" :  UserData.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace( UserData.TechnicalJobTitle) ? "-" :  UserData.TechnicalJobTitle}
            };


            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

       public static void SendMailResAppArticle(Article Artikel, EmployeeMaster Emp, User user, string msg, Boolean isApprove = true)
        {
            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
            
            emailDataNew.To = Emp.Email ;
            emailDataNew.Tag = WebConfigure.GetEmailRespondApprovalArticle();
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                 new EmailJsonkeyValues() {Key = "Decision", Value = isApprove == true ? "Approved": "Rejected"},
                 new EmailJsonkeyValues() {Key = "NameApproval", Value = Emp.Employee_Name},
                 new EmailJsonkeyValues() {Key = "Message", Value = msg },
                 new EmailJsonkeyValues() {Key = "Name", Value = user.Name},
                 new EmailJsonkeyValues() {Key = "Title", Value = Artikel.Title},
                 new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm")},
                  new EmailJsonkeyValues() {Key = "LinkDetail", Value = "<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.Token + "'>Click here to see more detail</a>"},
            };
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

        public static void SendMailRejectArticle(Article Artikel, EmployeeMaster Emp, User user, string msg)
        {
            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDataNew.To = user.Email;
            emailDataNew.Tag = WebConfigure.GetEmailRejectedArticle(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval", Value = Emp.Employee_Name},
                new EmailJsonkeyValues() {Key = "Name", Value = user.Name},
                new EmailJsonkeyValues() {Key = "Message", Value = msg},
                new EmailJsonkeyValues()
                {
                    Key = "Title", Value = Artikel.Title
                },
               new EmailJsonkeyValues()
                {
                    Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy, HH:mm")
                },
                new EmailJsonkeyValues()
                {
                    Key = "LinkDetail", Value = "<a href='" + WebConfigure.GetDomain() + "/Library/Detail/"+ Artikel.ArticleId + "'>Click Here to see your Article</a>"
                }
            };
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

        public static void SendMailNeedApp2Article(Article Artikel, Dictionary<String, EmployeeMaster> Emp, User User, String msg)
        {
            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDataNew.To = Emp["Approval2"].Email;
            emailDataNew.Tag = WebConfigure.GetEmailneedApprove2Article(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval1", Value = Emp["Approval1"].Employee_Name},
                new EmailJsonkeyValues() {Key = "NameApproval2", Value = Emp["Approval2"].Employee_Name},
                new EmailJsonkeyValues() {Key = "Name", Value = User.Name},
                new EmailJsonkeyValues() {Key = "Message", Value = msg},
                new EmailJsonkeyValues()
                {
                    Key = "Title", Value = Artikel.Title
                },

                new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm")  },
                new EmailJsonkeyValues()
                {
                    Key = "Branch", Value = User.MasterBranchId ==0 ? "-" : MasterBranchBs.GetDetail( User.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value = User.MasterAreaId == 0 ? "HEAD OFFICE" : MasterAreaBs.GetDetail( User.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Spesification", Value = String.IsNullOrWhiteSpace(User.RoleDescription) ? "-" :  User.RoleDescription },
                new EmailJsonkeyValues() {Key = "Role", Value = UserRoleBs.GetDetail( User.RoleId).Name},
                new EmailJsonkeyValues() {Key = "LinkApproval", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/Approve2Mail/" + Artikel.Token + "'>Click Here in this Link</a>" },
                 new EmailJsonkeyValues() {Key = "LinkDetail", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.ArticleId + "'>Click Here to See More Detail Information</a>" },
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace( User.TechnicalJobExperienceDuration) ? "-" :  User.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace(User.TechnicalJobTitle) ? "-" :  User.TechnicalJobTitle}
            };


            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

       public static void SendMailNeedAppTsArticle(Article Artikel, Dictionary<String, EmployeeMaster> Emp, UserTsManager TsManager, User User, Dictionary<String, String>msg)
        {
            EmailJson emailDataNew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            emailDataNew.To = TsManager.Email;
            emailDataNew.Tag = WebConfigure.GetEmailneedApproveTsArticle(); //tag ==> key for template
            emailDataNew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "NameApproval1", Value = Emp["Approval1"].Employee_Name},
                 new EmailJsonkeyValues() {Key = "NameTsManager", Value = TsManager.Name},
                 new EmailJsonkeyValues() {Key = "NameApproval2", Value = Emp["Approval2"].Employee_Name},
                new EmailJsonkeyValues() {Key = "Name", Value = User.Name},
                 new EmailJsonkeyValues() {Key = "MessageApp2", Value = msg["Approval2"]},
                  new EmailJsonkeyValues() {Key = "MessageApp1", Value = msg["Approval1"]},
                new EmailJsonkeyValues()
                {
                    Key = "Title", Value = Artikel.Title
                },

                new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm")  },
                new EmailJsonkeyValues()
                {
                    Key = "Branch", Value = User.MasterBranchId ==0 ? "-" : MasterBranchBs.GetDetail( User.MasterBranchId).Name
                },
                new EmailJsonkeyValues()
                {
                    Key = "Area", Value = User.MasterAreaId == 0 ? "HEAD OFFICE" : MasterAreaBs.GetDetail( User.MasterAreaId).Name
                },
                new EmailJsonkeyValues() {Key = "Specification", Value = String.IsNullOrWhiteSpace(User.RoleDescription) ? "-" :  User.RoleDescription },
                new EmailJsonkeyValues() {Key = "Role", Value = UserRoleBs.GetDetail( User.RoleId).Name},
                new EmailJsonkeyValues() {Key = "LinkApproval", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/ApproveTsMail/" + Artikel.Token + "'>Click Here in this Link</a>" },
                 new EmailJsonkeyValues() {Key = "LinkDetail", Value ="<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.ArticleId + "'>Click Here to See More Detail Information</a>" },
                new EmailJsonkeyValues()
                {
                    Key = "TechnicalJobExperienceDuration", Value = String.IsNullOrWhiteSpace( User.TechnicalJobExperienceDuration) ? "-" :  User.TechnicalJobExperienceDuration
                },
                new EmailJsonkeyValues() {Key = "TechnicalJobTitle", Value = String.IsNullOrWhiteSpace(User.TechnicalJobTitle) ? "-" :  User.TechnicalJobTitle}
            };
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDataNew);
            //SendMail(Result);
            LogReportBService.WriteLog("EmailNeedApproval1", MethodBase.GetCurrentMethod().Name, SendMail(result));
        }

       public static void SendMailNeedSubmitedAdminArticle(Article artikel, Dictionary<String, EmployeeMaster> ApprovalData, User UserData, Dictionary<String, String> message, Boolean isApproved = true)
        {
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);
           
            String EmailCC = "";
            var userAdmin = UserBService.GetListOtherAdmin();
            if (userAdmin != null) {
                foreach (User item in userAdmin)
                {
                    EmailCC += item.Email + ";";
                }
            }
                emailDatanew.To = UserBService.GetDetailbyUsername("Technical.c.admin1").Email; 
                emailDatanew.Cc = EmailCC;
                emailDatanew.Tag = WebConfigure.GetEmailNeedSubmitArticle(); //tag ==> key for template
                emailDatanew.KeyValues = new List<EmailJsonkeyValues>
                {
                    new EmailJsonkeyValues() {Key = "Name", Value = UserData.Name},
                    new EmailJsonkeyValues() {Key = "Approval1Name", Value = ApprovalData["Approval1"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "Approval2Name", Value = ApprovalData["Approval2"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "ApprovalTsName", Value = ApprovalData["ApprovalTS"].Employee_Name},
                    new EmailJsonkeyValues() {Key = "MessageApp1", Value = message["Approval1"]},
                    new EmailJsonkeyValues() {Key = "MessageApp2", Value = message["Approval2"]},
                    new EmailJsonkeyValues() {Key = "MessageTs", Value = message["ApprovalTS"]},
                    new EmailJsonkeyValues() {Key = "Decision", Value = isApproved == true ? "Approved" : "Declined" },
                    new EmailJsonkeyValues() {Key = "Title", Value = artikel.Title},
                    new EmailJsonkeyValues() {Key = "CreatedDate", Value = artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm")},
                    
                    new EmailJsonkeyValues() {Key = "LinkDetail", Value =  "<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + artikel.ArticleId +"'>Click Here To See Detail Article</a>"}
                };

                var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);

                LogReportBService.WriteLog("Need Approval Admin Email", MethodBase.GetCurrentMethod().Name, SendMail(result));

            
        } //to Admin

        public static void SendMailSubmitedArticle(Article Artikel, User user)
        {
            EmailJson emailDatanew = Factory.Create<EmailJson>("EmailJson", ClassType.clsTypeDataModel);

            var Userdata = UserBService.GetDetail(Artikel.CreatedBy);

            emailDatanew.To = Userdata.Email;
            //EmailDatanew.Cc = Email;
            emailDatanew.Tag = WebConfigure.GetEmailSubmitedArticle(); //tag ==> key for template
            emailDatanew.KeyValues = new List<EmailJsonkeyValues>
            {
                new EmailJsonkeyValues() {Key = "Title", Value = Artikel.Title},
                new EmailJsonkeyValues() {Key = "Description", Value = Artikel.Description},
                new EmailJsonkeyValues()
                {
                    Key = "Name", Value =  Userdata.Name
                },
                new EmailJsonkeyValues() {Key = "LinkDetail", Value = "<a href='" + WebConfigure.GetDomain() + "/Library/Detail/" + Artikel.ArticleId + "#comment'>Wan to see your Article ? Click Here</a>" },
                new EmailJsonkeyValues() {Key = "CreatedDate", Value = Artikel.CreatedAt.Value.ToString("dddd, dd MMMM yyyy - HH:mm") }
            };

            var result = Newtonsoft.Json.JsonConvert.SerializeObject(emailDatanew);
            //SendMail(Result);
            LogReportBService.WriteLog("Publish Article Email", MethodBase.GetCurrentMethod().Name, SendMail(result));
        } //To User
    }
}