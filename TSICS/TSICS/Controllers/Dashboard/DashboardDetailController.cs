using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: DashboardDetail
        public ActionResult Details()
        {
            this.setViewBag();
            return View();
        }
    }
}