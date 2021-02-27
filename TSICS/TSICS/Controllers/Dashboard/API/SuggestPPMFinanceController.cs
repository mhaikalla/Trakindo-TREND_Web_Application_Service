using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Controllers.Dashboard
{
    public class SuggestPPMFinanceController : Controller
    {
        // GET: SuggestPPMFinance
        public ActionResult SuggestPPMFinancePartNumber()
        {
            return View();
        }

        public ActionResult SuggestPPMFinancePartDescription()
        {
            return View(); 
        }

        public ActionResult SuggestPPMFinanceModel()
        {
            return View();
        }

        public ActionResult SuggestPPMFinancePrefixSN()
        {
            return View();
        }
    }
}