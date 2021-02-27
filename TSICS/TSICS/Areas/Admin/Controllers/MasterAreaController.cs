using System.Web.Mvc;
using Com.Trakindo.TSICS.Data.Model;
using PagedList;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using TSICS.Helper;
// ReSharper disable IdentifierTypo

namespace TSICS.Areas.Admin.Controllers
{
    public class MasterAreaController : Controller 
    {
        private readonly MasterAreaBusinessService _masterAreaService = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);

        // GET: Admin/MasterArea
        public ActionResult Index(int? page)
        {
            ViewBag.alertMessage = null;
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            int pageSize = 999999999;
            int pageNumber = (page ?? 1);

            var listMasterArea = _masterAreaService.GetList();

            return View(listMasterArea.ToPagedList(pageNumber, pageSize));

        }

        // GET: Admin/MasterArea/Details/5
        public ActionResult Details(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            MasterArea masterArea = _masterAreaService.GetDetail(id);
            if (masterArea == null)
            {
                return HttpNotFound();
            }
            return View(masterArea);
        }

        // GET: Admin/MasterArea/Create
        public ActionResult Create()
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MasterArea masterArea)
        {
            ViewBag.alertMessage = "Master Area Created";
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            if (ModelState.IsValid)
            {
                _masterAreaService.Add(masterArea);
                return RedirectToAction("Index");
            }
            return View();
            //return View(masterArea);
        }

        // GET: Admin/MasterArea/Edit/5
        public ActionResult Edit(int id)
        {
           
            MasterArea masterArea = _masterAreaService.GetDetail(id);
            if (masterArea == null)
            {
                return HttpNotFound();
            }
            return View(masterArea);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MasterArea masterArea)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.alertMessage = "Master Area Upgraded";
            if (ModelState.IsValid)
            {
                MasterArea resultUpdate = _masterAreaService.Edit(masterArea);
                if(resultUpdate.Status == 0)
                {
                    _masterAreaService.setIncativeBranchInvolved(resultUpdate.MasterAreaId);
                }
                else if (resultUpdate.Status == 1)
                {
                    _masterAreaService.setActiveBranchInvolved(resultUpdate.MasterAreaId);
                }
                return RedirectToAction("Index");
            }
            return View(masterArea);
        }

        // GET: Admin/MasterArea/Delete/5
        public ActionResult Delete(int id)
        {

            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            MasterArea masterArea = _masterAreaService.GetDetail(id);
            if (masterArea == null)
            {
                return HttpNotFound();
            }
            return View(masterArea);
        }

        // POST: Admin/MasterArea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            ViewBag.alertMessage = "Master Area Has Been Deleted";
            MasterArea masterArea = _masterAreaService.GetDetail(id);
            masterArea.Status = 2;
            _masterAreaService.Edit(masterArea);
            _masterAreaService.DeleteBranchInvolved(id);
            return RedirectToAction("Index");
        }
    }
}
