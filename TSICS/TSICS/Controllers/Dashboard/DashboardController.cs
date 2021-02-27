using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSICS.Helper;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController : Controller
    {
        private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly InventoryWeeklyBusinessService _InventoryWeeklyBs = Factory.Create<InventoryWeeklyBusinessService>("Inventory_Weekly", ClassType.clsTypeBusinessService);
        private readonly V_TR_MEPBusinessService V_TR_MEP_Bs = Factory.Create<V_TR_MEPBusinessService>("V_TR_MEP", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _TicketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _UserBs = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly RatingBusinessService _ratingBusinessService = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        private readonly DPPMSummaryBusinessService _DPPMSummaryBs = Factory.Create<DPPMSummaryBusinessService>("DPPMSummary", ClassType.clsTypeBusinessService);
        private readonly TicketCategoryBusinessService _TicketCategoryBs = Factory.Create<TicketCategoryBusinessService>("TicketCategory", ClassType.clsTypeBusinessService);
        private readonly PartResponsibleCostImpactAnalysisBusinessService _partResponsibleCostAnalysisBs = Factory.Create<PartResponsibleCostImpactAnalysisBusinessService>("PartResponsibleCostImpactAnalysis", ClassType.clsTypeBusinessService);
        // GET: Dashboard

        string[] SplitArea = new string[] { };
        string[] SplitCustomer = new string[] { };
        string[] SplitFamily = new string[] { };
        string[] SplitIndustry= new string[] { };
        string[] SplitSalesOffice = new string[] { };
        string[] SplitModel = new string[] { };
        string[] SplitSerialNumber = new string[] { };
        string[] SplitDelivery = new string[] { };
        string[] SplitRepairDate = new string[] { };
        string[] SplitPurchase = new string[] { };
        string[] SplitProduct = new string[] { };
        string[] SplitPlant = new string[] { };
        public string purchasedatefrom = "";
        public string purchasedateto = "";
        public string deliverydatefrom = "";
        public string deliverydateto = "";
        public string repairdatefrom = "";
        public string repairdateto = "";
        public ActionResult Index()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            DateTime timeNow = DateTime.Now; 
            ViewBag.GetTRMonthResponder = ticketBS.GetCountTrMonthResponder(Convert.ToInt32(Session["userid"]), timeNow.Month);
            ViewBag.GetTRMonthSubmitter = ticketBS.GetCountTrMonthSubmitter(Convert.ToInt32(Session["userid"]), timeNow.Month);
            ViewBag.GetTRYearResponder = ticketBS.GetCountTrYearResponder(Convert.ToInt32(Session["userid"]), timeNow.Year);
            ViewBag.GetTRYearSubmitter = ticketBS.GetCountTrYearSubmitter(Convert.ToInt32(Session["userid"]), timeNow.Year);
            ViewBag.GetCountDPPM = ticketBS.GetCountDppm(Convert.ToInt32(Session["userid"]));
            setViewBag();
            return View();
        }
        public void setViewBag()
        {
            ViewBag.Domain = WebConfigure.GetDomain();
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
        }
    }
}