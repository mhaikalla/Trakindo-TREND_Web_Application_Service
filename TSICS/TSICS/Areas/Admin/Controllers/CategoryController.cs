using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Web.Mvc;
using PagedList;
using TSICS.Helper;
using System.IO;
using System.Collections.Generic;
using System.Linq;


namespace TSICS.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ArticleCategoryBusinessService _ArticleCategoryBs = Factory.Create<ArticleCategoryBusinessService>("ArticleCategory", ClassType.clsTypeBusinessService);
        // GET: Admin/Category
        
        public ActionResult Index(String SearchString = "")
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            
            //int pageSize = 100;
            //int pageNumber = (page ?? 1);

            ViewBag.Domain = WebConfigure.GetDomain();
            if (String.IsNullOrWhiteSpace(SearchString))
            {
                ViewBag.ListCategoryChild = _ArticleCategoryBs.GetListCategoryMostUsedSort(0);
                
            }
            else
            {
                ViewBag.ListCategoryChild = _ArticleCategoryBs.GetList(SearchString);
            }
            return View(/*_ArticleCategoryBs.GetList().ToPagedList(pageNumber, pageSize)*/);

        }

        // GET: Admin/Category/Details/5
        public ActionResult Details(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ArticleCategory category = _ArticleCategoryBs.GetDetail(id);
            ViewBag.ParentName = category.Parent == 0 ? "MAIN CATEGORY" : _ArticleCategoryBs.GetDetail(category.Parent) == null ? "Parent Category Has Been Deleted" : _ArticleCategoryBs.GetDetail(category.Parent).Name;
            return View(category);
        }

        // GET: Admin/Category/Create
        public ActionResult Create(int parent = 0)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ViewBag.Parent = parent == 0 ? "Main Category" : _ArticleCategoryBs.GetDetail(parent).Name;
            ViewBag.ParentId = parent;
            return View();
        }

        // POST: Admin/Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection)
        {
            if (Common.CheckAdmin()) { return RedirectToAction("Login", "Default");  }

                ArticleCategory Category = new ArticleCategory();
                Category.Name = collection["Name"];
                Category.CreatedAt = DateTime.Now;
                Category.Parent = Convert.ToInt32(collection["Parent"]);
                Category.Status = Convert.ToInt32(collection["Status"]);
                Category.Position = 0;
                if (Request.Files.Count > 0)
                {
                    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    {
                        var file = Request.Files[i];
                        string response = Common.ValidateFileUpload(file);

                        if (response.Equals("true"))
                        {
                            if (file != null)
                            {
                                String Name = Category.Name.Replace(" ", "_");
                                var fileName = DateTime.Now.ToString("ddMMyyyy") + Category.ArticleCategoryId + "-" + Name + Path.GetExtension(file.FileName);

                                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Icons/"), fileName);
                                file.SaveAs(path);

                                Category.Icon = fileName;
                            }
                        }
                    }
                }
                else { Category.Icon =null; }

            var Result = _ArticleCategoryBs.Add(Category);
                return RedirectToAction("Index");
          
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ArticleCategory category = _ArticleCategoryBs.GetDetail(id);
            ViewBag.ParentData = _ArticleCategoryBs.GetListParentforEdit(id);
            ViewBag.Category = category;
            ViewBag.OrderPosition = _ArticleCategoryBs.CountData(category.Parent);
            ViewBag.ListCategoryChild = _ArticleCategoryBs.GetListCategoryMostUsedSort(0);
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collectionData)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            
            if (ModelState.IsValid)
            {
                ArticleCategory category = _ArticleCategoryBs.GetDetail(Convert.ToInt32(collectionData["ArticleCategoryId"]));
                category.Name = collectionData["Name"];
                category.Parent = Convert.ToInt32(collectionData["Parent"]);
                category.Status = Convert.ToInt32(collectionData["Status"]);
       
                if (Request.Files.Count > 0)
                    {
                        for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                        {
                            var file = Request.Files[i];
                            string response = Common.ValidateFileUpload(file);

                            if (response.Equals("true"))
                            {
                                if (file != null)
                                {
                                    String Name = category.Name.Replace(" ", "_");
                                    var fileName = DateTime.Now.ToString("ddMMyyyy") + category.ArticleCategoryId + "-" + Name + Path.GetExtension(file.FileName);

                                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Icons/"), fileName);
                                    file.SaveAs(path);

                                    category.Icon = fileName;
                                }
                            }
                        }
                    }
                    else { category.Icon = null; }
              
                //var SimilarPosition = _ArticleCategoryBs.GetDetailByPosition(Convert.ToInt32(collectionData["Position"]), category.Parent);
                //if (category.Position != 0 && SimilarPosition != null)
                //{
                //    _ArticleCategoryBs.SwapPosition(SimilarPosition, category);
                //}
                category.Position = Convert.ToInt32(collectionData["Position"]);
                var Result = _ArticleCategoryBs.Edit(category);
                
                return RedirectToAction("Index", "Category");
            }
            ViewBag.ListCategoryChild = _ArticleCategoryBs.GetListCategoryMostUsedSort(0);
            return RedirectToAction("Index","Category");
        }

        // GET: Admin/Category/Delete/5
        public ActionResult Delete(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ArticleCategory category = _ArticleCategoryBs.GetDetail(id);
            ViewBag.ParentName = category.Parent == 0 ? "MAIN CATEGORY" : _ArticleCategoryBs.GetDetail(category.Parent).Name;
            return View(category);
        }

        // POST: Admin/Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            ArticleCategory Category = _ArticleCategoryBs.GetDetail(id);
            Category.Status = 2;
            ArticleCategory category = _ArticleCategoryBs.Edit(Category);
            
            return RedirectToAction("Index");
            
        }
        [HttpGet]
        public ActionResult SubCategory(int parent = 0)
        {
            #region Category 
            ViewBag.Domain = WebConfigure.GetDomain();
            ViewBag.ListCategoryChild = _ArticleCategoryBs.GetListCategoryMostUsedSort(parent);
            #endregion
            return View();
        }
        [HttpGet]
        public ActionResult DeleteIcon(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ArticleCategory category = _ArticleCategoryBs.GetDetail(id);
            if (System.IO.File.Exists(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Icons/"), category.Icon)))
            {
                System.IO.File.Delete(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Article/Icons/"), category.Icon));
            }
            category.Icon = null;
            var result = _ArticleCategoryBs.Edit(category);
            return RedirectToAction("Edit/"+result.ArticleCategoryId, "Category");
        }
    }
}
