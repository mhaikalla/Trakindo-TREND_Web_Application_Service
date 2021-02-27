using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: DashboardPPMUpdate1
        public ActionResult DashboardPPMUpdate1()
        {
            this.setViewBag();
            return View();
        }
    }
}