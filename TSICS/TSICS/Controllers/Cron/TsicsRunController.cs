using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TSICS.Helper;
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo

namespace TSICS.Controllers.Cron
{
    public class TsicsRunController : Controller
    {

        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly UserTsManagerBusinessService _userTsManagerService = Factory.Create<UserTsManagerBusinessService>("UserTsManager", ClassType.clsTypeBusinessService);
        private readonly UserMessageBusinessService _userMessageBService = Factory.Create<UserMessageBusinessService>("UserMessage", ClassType.clsTypeBusinessService);
        public string SetStatusTrRed()
        {
            var cTicket = _ticketBs.GetCronTicketSetRed();

            if (cTicket.Count != 0)
            {
                foreach (Ticket item in cTicket)
                {
                    if(item.NextCommenter == item.Responder)
                    {
                        //set red flag responder
                        item.ResponderFlag = 1;
                    } else
                    {
                        //set red flag submiter
                        item.SubmiterFlag = 1;
                    }
                    _ticketBs.Edit(item);

                    //send email
                    if(WebConfigure.GetSendEmail())
                        Email.SendMailFlagRed(item.NextCommenter);
                }
                return "Flag Updated";
            } 
            return "Already Updated";
        }

        public string RegisterSendMailTsManager()
        {
            var listuser = _userService.GetListNeedApproveTsManager();

            var result = listuser.Count != 0 ? SetFlagTsManager(listuser) : "Already Updated";
            return result;
        }

        private string SetFlagTsManager(List<User> userList)
        {
            string Result = "Update";

            foreach (User itemUser in userList)
            {

                if (itemUser.UserTsManagerDueFlag == 1)
                {
                    //send mail ts manager 1
                    Email.SendMailNeedAppTs1(itemUser,1);

                    //set DueDate ts manager2
                    UpdateDueDateTsManager(itemUser.UserId, 2);
                }
                else if (itemUser.UserTsManagerDueFlag == 2)
                {
                    //send mail ts manager 2
                    Email.SendMailNeedAppTs1(itemUser, 2);

                    //set DueDate ts manager3
                    UpdateDueDateTsManager(itemUser.UserId, 3);
                }
                else if (itemUser.UserTsManagerDueFlag == 3)
                {
                    //send mail ts manager 3
                    Email.SendMailNeedAppTs1(itemUser, 3);

                    //set DueDate ts manager1
                    UpdateDueDateTsManager(itemUser.UserId, 1);
                }
            }

            return Result;
        }
        private void UpdateDueDateTsManager(int id, int tsId)
        {
            var userUpdate = _userService.GetDetail(id);

            var userTsManagerModel = _userTsManagerService.GetDetail(tsId);

            var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalTsManagerDay());

            if (tsId == 1)
            {
                userUpdate.UserTsManager1Id = userTsManagerModel.UserTsManagerId;
                userUpdate.UserTsManager1Name = userTsManagerModel.Name;
                userUpdate.UserTsManager1DueDate = DateTime.Now.AddDays(addDay);
            }
            else if (tsId == 2)
            {
                userUpdate.UserTsManager2Id = userTsManagerModel.UserTsManagerId;
                userUpdate.UserTsManager2Name = userTsManagerModel.Name;
                userUpdate.UserTsManager2DueDate = DateTime.Now.AddDays(addDay);
            }
            else if (tsId == 3)
            {
                userUpdate.UserTsManager3Id = userTsManagerModel.UserTsManagerId;
                userUpdate.UserTsManager3Name = userTsManagerModel.Name;
                userUpdate.UserTsManager3DueDate = DateTime.Now.AddDays(addDay);
            }

            userUpdate.UserTsManagerDueFlag = tsId;

            _userService.Edit(userUpdate);
        }

        public string RegisterAutoRejectSpv()
        {
            var listuser = _userService.GetListNeedRejectAuto();

            var result = listuser.Count != 0 ? SetRejectAuto(listuser) : "Already Updated";

            return result;
        }

        private string SetRejectAuto(List<User> listuser)
        {
            string Result = "update Reject";

            foreach (User itemUser in listuser)
            {
                //Reject User
                UpdateUserReject(itemUser.UserId);
                UpdateUserRejectMessage(itemUser);
                
                //send mail to??
                Email.SendMailRejectAuto(itemUser);
            }

            return Result;
        }
        private void UpdateUserReject(int idUser)
        {
            var userUpdate = _userService.GetDetail(idUser);

            userUpdate.Status = 8;
            userUpdate.UpdatedAt = DateTime.Now;

            _userService.Edit(userUpdate);
        }
        private void UpdateUserRejectMessage(User userT)
        {
            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = "Reject by System after " + WebConfigure.GetApprovalSpvDay() + " days";
            userM.MessageType = "Auto Reject";
            userM.ToUserId = userT.UserId;
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = userT.Status == 2 ? userT.Approval1 : userT.Approval2;            

            _userMessageBService.Add(userM);
        }
    }
}