using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: DashboardSimsSummary
        public ActionResult DashboardSimsSummary()
        {
            this.setViewBag();
            return View();
        }
    }
}