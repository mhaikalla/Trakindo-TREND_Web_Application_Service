using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Com.Trakindo.TSICS.Data.Model;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using TSICS.Helper;
using PagedList;
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo

namespace TSICS.Areas.Admin.Controllers
{
    public class MasterBranchController : Controller
    {
       private readonly MasterAreaBusinessService _masterAreaService = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
        private readonly MasterBranchBusinessService _masterBranchService = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        // GET: Admin/MasterBranch
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {

            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            //ViewBag.alertMessage = null;
            //int pageSize = 10;
            //int pageNumber = (page ?? 1);

            //List<MasterBranch> ListMasterBranch = new List<MasterBranch>();
            //ListMasterBranch = MasterBranchService.GetList();

            //return View(ListMasterBranch.ToPagedList(pageNumber, pageSize));


            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "IdDesc" : "";
            ViewBag.TitleSortParm = sortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.StatusSortParm = sortOrder == "AreaId" ? "AreaIdDesc" : "AreaId";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var masterBranch = _masterBranchService.GetQueryableList();
            if (!String.IsNullOrEmpty(searchString))
            {
                masterBranch = masterBranch.Where(branch =>
                    branch.Name.Contains(searchString)
                );
            }
            switch (sortOrder)
            {
                case "IdDesc":
                    masterBranch = masterBranch.OrderByDescending(t => t.MasterBranchId);
                    break;
                case "Name":
                    masterBranch = masterBranch.OrderBy(t => t.Name);
                    break;
                case "NameDesc":
                    masterBranch = masterBranch.OrderByDescending(t => t.Name);
                    break;
                case "AreaId":
                    masterBranch = masterBranch.OrderBy(t => t.MasterAreaId);
                    break;
                case "AreaIdDesc":
                    masterBranch = masterBranch.OrderByDescending(t => t.MasterAreaId);
                    break;
                default:
                    masterBranch = masterBranch.OrderBy(s => s.MasterBranchId);
                    break;
            }
            
            List<MasterBranch> masterBranchList = new List<MasterBranch>();
            if (masterBranch != null)
            {
                foreach (var branch in masterBranch)
                {
                    if (_masterAreaService.GetDetail(branch.MasterAreaId) != null)
                    {
                        MasterBranch masterBranchData = new MasterBranch()
                        {
                            MasterBranchId = branch.MasterBranchId,
                            Name = _masterAreaService.GetDetail(branch.MasterAreaId).Name
                        };
                        masterBranchList.Add(masterBranchData);
                    }
                    else
                    {
                        MasterBranch masterBranchData = new MasterBranch()
                        {
                            MasterBranchId = branch.MasterBranchId,
                            Name = ""
                        };
                        masterBranchList.Add(masterBranchData);
                    }
                }
            }
            ViewBag.MasterArea = masterBranchList;

            int pageSize = 999999999;
            int pageNumber = (page ?? 1);
            return View(masterBranch.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/MasterBranch/Details/5
        public ActionResult Details(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            MasterBranch masterBranch = _masterBranchService.GetDetail(id);
            MasterArea masterArea =  _masterAreaService.GetDetail(Convert.ToInt32(masterBranch.MasterAreaId));
            ViewBag.MasterAreaMapping = masterArea.Name;
            return View(masterBranch);
        }

        // GET: Admin/MasterBranch/Create
        public ActionResult Create()
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.ListMasterArea = _masterAreaService.GetListActive();

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MasterBranchId,MasterAreaId,Name,Status")] MasterBranch masterBranch)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.alertMessage = "Master Branch Data Has Been Created";
            if (ModelState.IsValid)
            {
                _masterBranchService.Add(masterBranch);

                return RedirectToAction("Index");
            }

            return View(masterBranch);
        }

        // GET: Admin/MasterBranch/Edit/5
        public ActionResult Edit(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.ListMasterBranch = _masterBranchService.GetList();
            MasterBranch masterBranch = _masterBranchService.GetDetail(id);
            ViewBag.ListMasterArea = _masterAreaService.GetListActive();
            if (masterBranch == null)
            {
                return HttpNotFound();
            }

            return View(masterBranch);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MasterBranch masterBranch)
        {
            ViewBag.alertMessage = "Master Branch Data Has Been Upgraded";
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            if (ModelState.IsValid)
            {
                _masterBranchService.Edit(masterBranch);
                return RedirectToAction("Index");
            }
            return View(masterBranch);
        }

        // GET: Admin/MasterBranch/Delete/5
        public ActionResult Delete(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            
            MasterBranch masterBranch = _masterBranchService.GetDetail(id);
            if (masterBranch == null)
            {
                return HttpNotFound();
            }
            return View(masterBranch);
        }

        // POST: Admin/MasterBranch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.alertMessage = "Master Branch Deleted";
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            MasterBranch masterBranch = _masterBranchService.GetDetail(id);
            masterBranch.Status = 2;

            _masterBranchService.Edit(masterBranch);
            return RedirectToAction("Index");
        }


    }
}
