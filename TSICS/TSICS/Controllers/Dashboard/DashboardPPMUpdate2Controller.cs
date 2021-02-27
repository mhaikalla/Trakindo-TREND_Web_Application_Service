using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: DashboardPPMUpdate2
        public ActionResult DashboardPPMUpdate2()
        {
            this.setViewBag();
            return View();
        }
    }
}