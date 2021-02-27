using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service 
{
    public class TicketPreviewBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        public List<TicketPreview> MakeFrom(IQueryable<Ticket> tickets, int logedInUserId)
        {
            List<TicketPreview> ticketPreviewList = new List<TicketPreview>();

            foreach(var ticket in tickets)
            {
                var submitter = _ctx.User.Find(ticket.Submiter);
                var role = _ctx.UserRole.FirstOrDefault(q => q.UserRoleId == submitter.RoleId);
                TicketPreview ticketPreview = new TicketPreview
                {
                    TicketId = ticket.TicketId,
                    TicketNo = ticket.TicketNo,
                    Title = ticket.Title,
                    CreatedAt = ticket.CreatedAt,
                    Status = ticket.Status,
                    DPPMNumber= ticket.DPPMno,
                    TicketCategoryId = ticket.TicketCategoryId,
                    EmailCC = ticket.EmailCC
                };

                if (role != null) ticketPreview.RoleName = role.Name;
                ticketPreview.Submiter = ticket.Submiter;
                ticketPreview.Responder = ticket.Responder;
                ticketPreview.SubmiterFlag = ticket.SubmiterFlag;
                ticketPreview.ResponderFlag = ticket.ResponderFlag;
                ticketPreview.DueDateAnswer = ticket.DueDateAnswer;
                ticketPreview.Description = ticket.Description;
                ticketPreview.NextCommenter = ticket.NextCommenter;
                ticketPreview.ticketNoReference = _ticketBusinessService.CheckTicketReferences(ticket.TicketId) == null ? null : _ticketBusinessService.CheckTicketReferences(ticket.TicketId).TicketNo;
                if (logedInUserId != 0)
                {
                    ticketPreview.IsEscalated = _ticketBusinessService.IsEscalated(ticket.TicketId, logedInUserId);
                }

                ticketPreviewList.Add(ticketPreview);
            }

            return ticketPreviewList;
        }
        public List<TicketPreview> MakeFromSummary(IQueryable<Ticket> tickets)
        {
            List<TicketPreview> ticketPreviewList = new List<TicketPreview>();

            foreach (var ticket in tickets)
            {
                var submitter = _ctx.User.Find(ticket.Submiter);
                var role = _ctx.UserRole.FirstOrDefault(q => q.UserRoleId == submitter.RoleId);
                TicketPreview ticketPreview = new TicketPreview
                {
                    TicketId = ticket.TicketId,
                    TicketCategoryId = ticket.TicketCategoryId,
                    TicketNo = ticket.TicketNo,
                    Title = ticket.Title,
                    CreatedAt = ticket.CreatedAt,
                    Status = ticket.Status
                };

                if (role != null) ticketPreview.RoleName = role.Name;
                ticketPreview.Submiter = ticket.Submiter;  
                ticketPreview.Responder = ticket.Responder;
                ticketPreview.DueDateAnswer = ticket.DueDateAnswer;
                ticketPreview.Description = ticket.Description;

                ticketPreviewList.Add(ticketPreview);
            }

            return ticketPreviewList;
        }
    }
}
